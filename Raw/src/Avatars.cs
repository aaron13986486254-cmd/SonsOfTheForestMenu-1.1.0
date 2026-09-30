// ═══════════════════════════════════════════════════════════════════════════
//  Avatars.cs · Steam avatar fetch and disk cache
//  Sons of the Forest Menu — cyberfox1337x
// ═══════════════════════════════════════════════════════════════════════════

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using UnityEngine;

namespace Cyberfox1337x.SonsOfTheForest
{
    /// <summary>
    /// Player avatar (Steam profile picture) fetching for the roster.
    ///
    /// Flow:
    ///   Ensure(id)      — queue a fetch if the avatar isn't cached (memory or disk)
    ///   (background)    — call ISteamUser/GetPlayerSummaries, download the
    ///                     avatarfull image, write it to the disk cache, enqueue
    ///                     the bytes for the main thread
    ///   Tick()          — called every frame from the menu behaviour: converts
    ///                     downloaded bytes into Texture2D via ImageConversion
    ///                     (textures can only be created on the main thread)
    ///   Get(id)         — the Texture2D, or null (placeholder drawn in its place)
    ///
    /// Disk cache lives in BepInEx/config/sonsofthedead_avatars/{steamid}.jpg so
    /// avatars survive restarts without re-fetching. The API key is the user's
    /// own Steam Web API key (read from config; menu-only use, never sent to the
    /// game server).
    /// </summary>
    [Cyberfox1337x("avatar cache")]
    internal static class Avatars
    {
        static string _apiKey;
        static string _cacheDir;
        static bool _ready;

        static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        // id -> fetched bytes waiting to become a Texture2D on the main thread
        static readonly ConcurrentQueue<KeyValuePair<ulong, byte[]>> _incoming =
            new ConcurrentQueue<KeyValuePair<ulong, byte[]>>();

        // id -> finished Texture2D
        static readonly Dictionary<ulong, Texture2D> _cache =
            new Dictionary<ulong, Texture2D>();

        // ids currently being fetched (dedupe)
        static readonly HashSet<ulong> _inflight = new HashSet<ulong>();

        // ids already probed with no disk avatar — avoids a File.Exists probe
        // every frame for every uncached player (private profiles, busy queues)
        static readonly HashSet<ulong> _diskMiss = new HashSet<ulong>();

        static Texture2D _placeholder;

        public static void Init(string apiKey)
        {
            _apiKey = apiKey == null ? "" : apiKey.Trim();
            _cacheDir = System.IO.Path.Combine(Plugin.DataRoot,
                                               "sonsofthedead_avatars");
            try { Directory.CreateDirectory(_cacheDir); }
            catch (Exception e)
            {
                // Rare (permissions / read-only config dir) but fatal to the
                // whole feature: without the dir, nothing caches. Surface it.
                Diag.Warn("avatar cache dir: " + e.Message);
            }
            try
            {
                _http.DefaultRequestHeaders.UserAgent.ParseAdd("SonsOfTheForestMenu/1.0.0");
            }
            catch (Exception e)
            {
                // Some WebAPI builds reject a custom UA header. Harmless — the
                // default UA still works — but worth one log line at init.
                Diag.Warn("avatar user-agent: " + e.Message);
            }
            _ready = true;
        }

        /// <summary>
        /// Hot-reload a new API key at runtime (from the ONLINE tab).
        /// Clears in-flight and disk-miss state so every player gets re-fetched
        /// with the new key; already-cached avatars are kept.
        /// </summary>
        public static void SetApiKey(string key)
        {
            _apiKey = key == null ? "" : key.Trim();
            lock (_inflight)
            {
                _inflight.Clear();
                _diskMiss.Clear();
            }
            Diag.Info("Steam API key updated");
        }

        /// <summary>Queue a fetch for this Steam id unless it's cached or in flight.</summary>
        public static void Ensure(ulong steamId)
        {
            if (!_ready || steamId == 0) return;
            lock (_inflight)
            {
                if (_cache.ContainsKey(steamId) || _inflight.Contains(steamId)) return;
                _inflight.Add(steamId);
            }
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem(_ => Fetch(steamId));
            }
            catch
            {
                lock (_inflight) _inflight.Remove(steamId);
            }
        }

        /// <summary>Textures must be created on the main thread; call every frame.</summary>
        public static void Tick()
        {
            KeyValuePair<ulong, byte[]> item;
            while (_incoming.TryDequeue(out item))
            {
                var tex = BytesToTexture(item.Value);
                if (tex != null)
                {
                    lock (_inflight) { _cache[item.Key] = tex; _diskMiss.Remove(item.Key); }
                }
                lock (_inflight) _inflight.Remove(item.Key);
            }
        }

        public static Texture2D Get(ulong steamId)
        {
            if (steamId == 0) return null;
            // disk cache on first touch (memory miss → disk → miss → null)
            Texture2D tex;
            lock (_inflight)
            {
                if (_cache.TryGetValue(steamId, out tex)) return tex;
                // seen before and there was nothing on disk — don't re-probe
                // the filesystem every frame for the same id
                if (_diskMiss.Contains(steamId)) return null;
            }
            var disk = LoadFromDisk(steamId);
            if (disk != null)
            {
                lock (_inflight) { _cache[steamId] = disk; _diskMiss.Remove(steamId); }
                return disk;
            }
            lock (_inflight) _diskMiss.Add(steamId);
            return null;
        }

        public static Texture2D Placeholder
        {
            get
            {
                if (_placeholder == null)
                    _placeholder = MakePlaceholder();
                return _placeholder;
            }
        }

        // ── background fetch ────────────────────────────────────────────
        static void Fetch(ulong steamId)
        {
            try
            {
                string url = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/"
                             + "?key=" + Uri.EscapeDataString(_apiKey)
                             + "&steamids=" + steamId;
                string json = _http.GetStringAsync(url).GetAwaiter().GetResult();
                string avatarUrl = ParseAvatarUrl(json, steamId);
                if (string.IsNullOrEmpty(avatarUrl)) return; // private profile

                byte[] bytes = _http.GetByteArrayAsync(avatarUrl).GetAwaiter().GetResult();
                if (bytes == null || bytes.Length < 8) return;

                SaveToDisk(steamId, bytes);
                _incoming.Enqueue(new KeyValuePair<ulong, byte[]>(steamId, bytes));
            }
            catch (Exception e)
            {
                Diag.Warn("avatar fetch " + steamId + ": " + e.Message);
            }
            finally
            {
                // drop the in-flight marker if Tick never saw the bytes
                // (failure or private profile). Tick also removes it on success.
                lock (_inflight) _inflight.Remove(steamId);
            }
        }

        static string ParseAvatarUrl(string json, ulong steamId)
        {
            try
            {
                using (var doc = JsonDocument.Parse(json))
                {
                    var players = doc.RootElement
                        .GetProperty("response").GetProperty("players");
                    foreach (var p in players.EnumerateArray())
                    {
                        if (!p.TryGetProperty("steamid", out var sid)) continue;
                        ulong id;
                        if (!ulong.TryParse(sid.GetString(), out id)) continue;
                        if (id != steamId) continue;
                        if (p.TryGetProperty("avatarfull", out var av))
                            return av.GetString();
                        return null; // profile exists but avatar is private/empty
                    }
                }
            }
            catch (Exception e)
            {
                Diag.Warn("avatar parse: " + e.Message);
            }
            return null;
        }

        // ── disk cache ──────────────────────────────────────────────────
        static string PathFor(ulong steamId)
        {
            return System.IO.Path.Combine(_cacheDir, steamId + ".jpg");
        }

        static Texture2D LoadFromDisk(ulong steamId)
        {
            try
            {
                string p = PathFor(steamId);
                if (!File.Exists(p)) return null;
                return BytesToTexture(File.ReadAllBytes(p));
            }
            catch (Exception e)
            {
                // One-shot per steam id (negative-cached after); a corrupt or
                // unreadable file should be visible rather than silently
                // degrading every player to a placeholder.
                Diag.Warn("avatar disk read " + steamId + ": " + e.Message);
                return null;
            }
        }

        static void SaveToDisk(ulong steamId, byte[] bytes)
        {
            try { File.WriteAllBytes(PathFor(steamId), bytes); }
            catch (Exception e)
            {
                // One-shot per avatar; disk full / permissions. The in-memory
                // cache still works for this session, so log and move on.
                Diag.Warn("avatar disk write " + steamId + ": " + e.Message);
            }
        }

        // ── texture helpers ─────────────────────────────────────────────
        static Texture2D BytesToTexture(byte[] bytes)
        {
            try
            {
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (UnityEngine.ImageConversion.LoadImage(tex, bytes))
                {
                    tex.hideFlags = HideFlags.HideAndDontSave;
                    tex.filterMode = FilterMode.Bilinear;
                    return tex;
                }
            }
            catch (Exception e)
            {
                Diag.Warn("avatar texture: " + e.Message);
            }
            return null;
        }

        static Texture2D MakePlaceholder()
        {
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var c = Theme.Dead;
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 2; y++)
                    tex.SetPixel(x, y, c);
            tex.Apply();
            tex.hideFlags = HideFlags.HideAndDontSave;
            tex.filterMode = FilterMode.Point;
            return tex;
        }
    }
}
