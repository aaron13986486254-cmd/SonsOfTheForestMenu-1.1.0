extern alias SonsGame;

using System;
using GameServerInfo = SonsGame::CoopServerInfo;
using Bolt;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;
using Sons.Multiplayer;
using Sons.Multiplayer.Gui;
using Sons.MultiplayerLegacy;
using Sons.UI;
using Steamworks;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class Roster
{
	public static readonly System.Collections.Generic.List<RemotePlayer> Players;

	public static string Status;

	private static ulong _cachedMyId;

	private static float _cachedMyIdTime;

	private static string _lastSourceDiagnostic;

	static Roster()
	{
		Players = new System.Collections.Generic.List<RemotePlayer>();
		Status = "";
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "multiplayer-roster";
	}

	public static ulong MySteamId()
	{
		if (_cachedMyId != 0L && Time.time - _cachedMyIdTime < 15f)
		{
			return _cachedMyId;
		}
		Refresh();
		for (int i = 0; i < Players.Count; i++)
		{
			if (Players[i].IsLocal && Players[i].SteamId != 0L)
			{
				_cachedMyId = Players[i].SteamId;
				_cachedMyIdTime = Time.time;
				return _cachedMyId;
			}
		}
		return 0uL;
	}

	public static void Refresh()
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		Players.Clear();
		Status = "";
		try
		{
			LocalPlayer local = LocalPlayer._instance;
			if ((Object)(object)local == (Object)null)
			{
				Status = "not in a world";
				return;
			}
			BoltEntity localEntity = ((Component)local).GetComponent<BoltEntity>();
			Vitals lv = LocalPlayer.Vitals;
			MultiplayerPlayerRoles roles = Object.FindObjectOfType<MultiplayerPlayerRoles>();
			Safe.Run("roster.update-known-players", (Action)PlayerListBase.UpdateKnownPlayers, Noise.Warn);
			Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known = PlayerListBase._knownPlayers;
			ulong localSteamId = 0uL;
			try
			{
				CSteamID csid = CoopSteamServer.SteamId;
				if (csid.IsValid())
				{
					localSteamId = (ulong)csid;
				}
			}
			catch (Exception ex)
			{
				Diag.Warn("coop steam id: " + ex.Message);
			}
			if (localSteamId == 0L)
			{
				try
				{
					CSteamID sid = SteamUser.GetSteamID();
					if (sid.IsValid())
					{
						localSteamId = (ulong)sid;
					}
				}
				catch (Exception ex2)
				{
					Diag.Warn("steam id: " + ex2.Message);
				}
			}
			if (localSteamId == 0L && known != null && (Object)(object)localEntity != (Object)null)
			{
				PlayerListBase.KnownPlayerData ld = FindData(known, localEntity);
				if (ld != null)
				{
					localSteamId = ld.SteamId;
				}
			}
			string localName = null;
			try
			{
				localName = SteamFriends.GetPersonaName();
			}
			catch (Exception ex3)
			{
				Diag.Warn("steam name: " + ex3.Message);
			}
			if (string.IsNullOrEmpty(localName))
			{
				localName = "You";
			}
			bool localHasHealth = TryReadHealth(lv, out var localHealth, out var localMaxHealth);
			bool localIsAlive = (Object)(object)lv != (Object)null && Safe.Get("roster.local.alive", lv, (Vitals vitals) => vitals.IsAlive(), fallback: false);
			Players.Add(new RemotePlayer
			{
				Name = RosterRules.DisplayName(localName),
				SteamId = localSteamId,
				Position = ((Component)local).transform.position,
				Health = localHealth,
				MaxHealth = localMaxHealth,
				HasHealth = localHasHealth,
				IsAlive = localIsAlive,
				IsLocal = true,
				IsAdmin = true,
				Entity = localEntity
			});
			System.Collections.Generic.HashSet<ulong> seenSteamIds = new System.Collections.Generic.HashSet<ulong>();
			System.Collections.Generic.HashSet<BoltEntity> seenEntities = new System.Collections.Generic.HashSet<BoltEntity>();
			if (localSteamId != 0L)
			{
				seenSteamIds.Add(localSteamId);
			}
			if ((Object)(object)localEntity != (Object)null)
			{
				seenEntities.Add(localEntity);
			}
			int serverRecords = AddServerStatePlayers(localSteamId, localEntity, known, roles, seenSteamIds, seenEntities, out var serverAdded);
			int uiRecords = AddConnectedPlayerRows(localSteamId, localEntity, known, roles, seenSteamIds, seenEntities, out var uiAdded);
			int knownRecords = AddKnownPlayers(localSteamId, localEntity, known, roles, seenSteamIds, seenEntities, out var knownAdded);
			Status = RosterRules.PlayerCountText(Players.Count);
			ReportSources(serverRecords, serverAdded, uiRecords, uiAdded, knownRecords, knownAdded);
		}
		catch (Exception ex4)
		{
			Status = "roster unavailable";
			Diag.Warn("roster refresh: " + ex4.Message);
		}
		for (int i = 0; i < Players.Count; i++)
		{
			if (Players[i].SteamId != 0L)
			{
				Avatars.Ensure(Players[i].SteamId);
			}
		}
	}

	private static int AddServerStatePlayers(ulong localSteamId, BoltEntity localEntity, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, MultiplayerPlayerRoles roles, System.Collections.Generic.HashSet<ulong> seenSteamIds, System.Collections.Generic.HashSet<BoltEntity> seenEntities, out int added)
	{
		added = 0;
		NetworkArray_ProtocolToken steamIds = Safe.Get("roster.server-state.steam-ids", (Func<NetworkArray_ProtocolToken>)GameServerInfo.GetSteamIds, (NetworkArray_ProtocolToken)null, Noise.Warn);
		if (steamIds == null)
		{
			return 0;
		}
		int length = Safe.Get("roster.server-state.length", steamIds, (NetworkArray_ProtocolToken values) => ((NetworkArray_Values<IProtocolToken>)(object)values).Length, 0, Noise.Warn);
		int records = 0;
		int index;
		for (index = 0; index < length; index++)
		{
			IProtocolToken token = Safe.Get("roster.server-state.token", () => ((NetworkArray_Values<IProtocolToken>)(object)steamIds)[index]);
			if (token == null)
			{
				continue;
			}
			SteamIdToken steamToken = Safe.Get("roster.server-state.cast", token, (IProtocolToken value) => ((Il2CppObjectBase)value).TryCast<SteamIdToken>());
			if (steamToken == null)
			{
				continue;
			}
			ulong steamId = Safe.Get("roster.server-state.steam-id", steamToken, (SteamIdToken value) => value.SteamId, 0uL);
			if (steamId != 0L)
			{
				records++;
				if (TryAddRemotePlayer(localSteamId, localEntity, steamId, null, known, roles, seenSteamIds, seenEntities))
				{
					added++;
				}
			}
		}
		return records;
	}

	private static int AddConnectedPlayerRows(ulong localSteamId, BoltEntity localEntity, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, MultiplayerPlayerRoles roles, System.Collections.Generic.HashSet<ulong> seenSteamIds, System.Collections.Generic.HashSet<BoltEntity> seenEntities, out int added)
	{
		added = 0;
		Il2CppArrayBase<ActivePlayerList> lists = Safe.Get("roster.connected-ui.lists", () => Resources.FindObjectsOfTypeAll<ActivePlayerList>());
		if (lists == null)
		{
			return 0;
		}
		int records = 0;
		for (int listIndex = 0; listIndex < lists.Length; listIndex++)
		{
			ActivePlayerList list = lists[listIndex];
			if ((Object)(object)list == (Object)null)
			{
				continue;
			}
			Il2CppSystem.Collections.Generic.List<PlayerListElementView> elements = Safe.Get("roster.connected-ui.elements", list, (ActivePlayerList value) => ((PlayerListBase)value)._playerListElements);
			if (elements == null)
			{
				continue;
			}
			int count = Safe.Get("roster.connected-ui.count", elements, (Il2CppSystem.Collections.Generic.List<PlayerListElementView> values) => values.Count, 0);
			int elementIndex;
			for (elementIndex = 0; elementIndex < count; elementIndex++)
			{
				PlayerListElementView element = Safe.Get("roster.connected-ui.element", () => elements[elementIndex]);
				if ((Object)(object)element == (Object)null)
				{
					continue;
				}
				ulong steamId = Safe.Get("roster.connected-ui.steam-id", element, (PlayerListElementView value) => value._steamId, 0uL);
				if (steamId != 0L)
				{
					records++;
					if (TryAddRemotePlayer(localSteamId, localEntity, steamId, null, known, roles, seenSteamIds, seenEntities))
					{
						added++;
					}
				}
			}
		}
		return records;
	}

	private static int AddKnownPlayers(ulong localSteamId, BoltEntity localEntity, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, MultiplayerPlayerRoles roles, System.Collections.Generic.HashSet<ulong> seenSteamIds, System.Collections.Generic.HashSet<BoltEntity> seenEntities, out int added)
	{
		added = 0;
		if (known == null)
		{
			return 0;
		}
		int records = 0;
		Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData>.Enumerator iterator = known.GetEnumerator();
		while (iterator.MoveNext())
		{
			PlayerListBase.KnownPlayerData knownData = iterator.Current.Value;
			if (knownData != null)
			{
				records++;
				BoltEntity entity = Safe.Get("roster.known.entity", knownData, (PlayerListBase.KnownPlayerData player) => player.BoltEntity);
				ulong steamId = Safe.Get("roster.known.steam-id", knownData, (PlayerListBase.KnownPlayerData player) => player.SteamId, 0uL);
				if (TryAddRemotePlayer(localSteamId, localEntity, steamId, entity, known, roles, seenSteamIds, seenEntities))
				{
					added++;
				}
			}
		}
		return records;
	}

	private static BoltEntity FindEntityForSteamId(ulong steamId, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known)
	{
		if (steamId == 0L)
		{
			return null;
		}
		BoltEntity entity = FindServerStateEntity(steamId, out var mappedToSelf);
		if (mappedToSelf)
		{
			return null;
		}
		if ((Object)(object)entity != (Object)null)
		{
			return entity;
		}
		entity = Safe.Get("roster.entity-from-active-list", steamId, (ulong id) => ActivePlayerList.GetEntityFromSteamId(id));
		if ((Object)(object)entity != (Object)null)
		{
			return entity;
		}
		PlayerName[] names = Safe.Get("roster.player-names", () => Object.FindObjectsOfType<PlayerName>());
		if (names == null)
		{
			return null;
		}
		foreach (PlayerName nameTag in names)
		{
			if ((Object)(object)nameTag == (Object)null)
			{
				continue;
			}
			BoltEntity candidate = Safe.Get("roster.player-name.entity", nameTag, (PlayerName value) => ((Component)value).GetComponentInParent<BoltEntity>());
			if (!((Object)(object)candidate == (Object)null))
			{
				PlayerListBase.KnownPlayerData data = FindData(known, candidate);
				if (data != null && Safe.Get("roster.player-name.steam-id", data, (PlayerListBase.KnownPlayerData value) => value.SteamId, 0uL) == steamId)
				{
					return candidate;
				}
			}
		}
		return null;
	}

	private static BoltEntity FindServerStateEntity(ulong steamId, out bool mappedToSelf)
	{
		(bool, BoltEntity, bool) mapping = Safe.Get("roster.server-state.entity", steamId, (ulong id) =>
		{
			BoltEntity item = default;
			bool item2 = default;
			return (Found: GameServerInfo.TryGetPlayerEntityFromSteamId(id, out item, out item2), Entity: item, IsSelf: item2);
		}, (false, null, false), Noise.Warn);
		mappedToSelf = mapping.Item1 && mapping.Item3;
		if (!RosterRules.IsUsableRemoteEntity(mapping.Item1, (Object)(object)mapping.Item2 != (Object)null, mapping.Item3))
		{
			return null;
		}
		return mapping.Item2;
	}

	private static Vitals ReadRemoteVitals(BoltEntity entity, ulong steamId, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, out BoltEntity liveEntity)
	{
		liveEntity = entity;
		Vitals vitals = ReadVitals(entity);
		if ((Object)(object)vitals != (Object)null)
		{
			return vitals;
		}
		if (steamId == 0L)
		{
			return null;
		}
		BoltEntity fresh = FindEntityForSteamId(steamId, known);
		if ((Object)(object)fresh != (Object)null)
		{
			liveEntity = fresh;
			vitals = ReadVitals(fresh);
			if ((Object)(object)vitals != (Object)null)
			{
				return vitals;
			}
		}
		return null;
	}

	private static Vitals ReadVitals(BoltEntity entity)
	{
		if ((Object)(object)entity == (Object)null)
		{
			return null;
		}
		return Safe.Get("roster.remote.vitals", entity, (BoltEntity value) => ((Component)value).GetComponentInChildren<Vitals>());
	}

	private static bool TryAddRemotePlayer(ulong localSteamId, BoltEntity localEntity, ulong steamId, BoltEntity entity, Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, MultiplayerPlayerRoles roles, System.Collections.Generic.HashSet<ulong> seenSteamIds, System.Collections.Generic.HashSet<BoltEntity> seenEntities)
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)entity == (Object)null && steamId != 0L)
		{
			entity = FindEntityForSteamId(steamId, known);
		}
		if (!RosterRules.HasIdentity(steamId, (Object)(object)entity != (Object)null))
		{
			return false;
		}
		bool sameEntity = (Object)(object)localEntity != (Object)null && (Object)(object)entity == (Object)(object)localEntity;
		if (!RosterRules.IsRemoteCandidate(localSteamId, steamId, sameEntity))
		{
			return false;
		}
		if (steamId != 0L && seenSteamIds.Contains(steamId))
		{
			return false;
		}
		if ((Object)(object)entity != (Object)null && seenEntities.Contains(entity))
		{
			return false;
		}
		if (steamId != 0L)
		{
			seenSteamIds.Add(steamId);
		}
		if ((Object)(object)entity != (Object)null)
		{
			seenEntities.Add(entity);
		}
		PlayerListBase.KnownPlayerData knownData = FindData(known, steamId);
		string name = ((knownData != null) ? Safe.Get("roster.known.name", knownData, (PlayerListBase.KnownPlayerData player) => player.Name) : null);
		if (string.IsNullOrWhiteSpace(name) && steamId != 0L)
		{
			name = Safe.Get("roster.steam.name", steamId, (ulong id) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return SteamFriends.GetFriendPersonaName(new CSteamID(id));
			});
		}
		Vitals vitals = ReadRemoteVitals(entity, steamId, known, out var liveEntity);
		if ((Object)(object)liveEntity != (Object)null)
		{
			entity = liveEntity;
		}
		Transform transform = (((Object)(object)entity != (Object)null) ? Safe.Get("roster.remote.transform", entity, (BoltEntity value) => ((Component)value).transform) : null);
		Vector3 position = (((Object)(object)transform != (Object)null) ? Safe.Get("roster.remote.position", transform, (Transform value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.position;
		}, Vector3.zero) : Vector3.zero);
		bool hasHealth = TryReadHealth(vitals, out var health, out var maxHealth);
		bool isAlive = (Object)(object)vitals != (Object)null && Safe.Get("roster.remote.alive", vitals, (Vitals value) => value.IsAlive(), fallback: false);
		bool isAdmin = (Object)(object)roles != (Object)null && steamId != 0L && Safe.Get("roster.remote.admin", (roles, steamId), ((MultiplayerPlayerRoles roles, ulong steamId) state) => state.roles.IsAdmin(state.steamId) || state.roles.IsOwner(state.steamId), fallback: false);
		Players.Add(new RemotePlayer
		{
			Name = RosterRules.DisplayName(name),
			SteamId = steamId,
			Position = position,
			Health = health,
			MaxHealth = maxHealth,
			HasHealth = hasHealth,
			IsAlive = isAlive,
			IsLocal = false,
			IsAdmin = isAdmin,
			Entity = entity
		});
		return true;
	}

	private static void ReportSources(int serverRecords, int serverAdded, int uiRecords, int uiAdded, int knownRecords, int knownAdded)
	{
		string sources = RosterRules.SourceSummary(serverAdded, uiAdded, knownAdded);
		string diagnostic = $"source={sources}; server={serverRecords}/{serverAdded}; ui={uiRecords}/{uiAdded}; known={knownRecords}/{knownAdded}; visible={Players.Count}";
		if (!(diagnostic == _lastSourceDiagnostic))
		{
			_lastSourceDiagnostic = diagnostic;
			Diag.Info("Roster membership: " + diagnostic);
		}
	}

	private static bool TryReadHealth(Vitals vitals, out float current, out float maximum)
	{
		current = 0f;
		maximum = 0f;
		if ((Object)(object)vitals == (Object)null)
		{
			return false;
		}
		if (!Safe.Get("roster.vitals.has-health", vitals, (Vitals value) => value.HasHealth(), fallback: false))
		{
			return false;
		}
		float current2 = Safe.Get("roster.vitals.current", vitals, (Vitals value) => value.GetHealth(), 0f);
		float max = Safe.Get("roster.vitals.maximum", vitals, (Vitals value) => value.GetMaxHealth(), 0f);
		return RosterRules.TryNormalizeHealth(current2, max, out current, out maximum);
	}

	private static PlayerListBase.KnownPlayerData FindData(Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, BoltEntity entity)
	{
		if (known == null || (Object)(object)entity == (Object)null)
		{
			return null;
		}
		Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData>.Enumerator it = known.GetEnumerator();
		while (it.MoveNext())
		{
			PlayerListBase.KnownPlayerData d = it.Current.Value;
			if (d != null && (Object)(object)Safe.Get("roster.known.lookup-entity", d, (PlayerListBase.KnownPlayerData player) => player.BoltEntity) == (Object)(object)entity)
			{
				return d;
			}
		}
		return null;
	}

	private static PlayerListBase.KnownPlayerData FindData(Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData> known, ulong steamId)
	{
		if (known == null || steamId == 0L)
		{
			return null;
		}
		Il2CppSystem.Collections.Generic.Dictionary<BoltConnection, PlayerListBase.KnownPlayerData>.Enumerator iterator = known.GetEnumerator();
		while (iterator.MoveNext())
		{
			PlayerListBase.KnownPlayerData data = iterator.Current.Value;
			if (data != null && Safe.Get("roster.known.lookup-steam-id", data, (PlayerListBase.KnownPlayerData player) => player.SteamId, 0uL) == steamId)
			{
				return data;
			}
		}
		return null;
	}

	public static void Prefetch()
	{
		try
		{
			Refresh();
		}
		catch (Exception ex)
		{
			Diag.Warn("avatar prefetch: " + ex.Message);
		}
	}

	public static bool TryGet(int index, out RemotePlayer p)
	{
		if (index >= 0 && index < Players.Count)
		{
			p = Players[index];
			return true;
		}
		p = default;
		return false;
	}

	public static void TeleportTo(int index)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGet(index, out var p) || p.IsLocal)
		{
			return;
		}
		if (!TryGetLivePosition(p, out var target))
		{
			Features.LastResult = p.Name + " is connected but still loading — try TP again";
			return;
		}
		LocalPlayer local = Safe.Get("roster.teleport.local", () => LocalPlayer._instance);
		if ((Object)(object)local == (Object)null)
		{
			Features.LastResult = "Needs a loaded world";
			return;
		}
		Features.LastResult = (Safe.Run("roster.teleport.goto", (local, target), ((LocalPlayer local, Vector3 target) state) =>
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			state.local.Goto(state.target);
		}, Noise.Warn) ? ("Teleported to " + p.Name) : "Teleport failed — try again");
	}

	private static bool TryGetLivePosition(RemotePlayer p, out Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		position = Vector3.zero;
		BoltEntity entity = p.Entity;
		if ((Object)(object)entity == (Object)null && p.SteamId != 0L)
		{
			entity = FindEntityForSteamId(p.SteamId, null);
		}
		if ((Object)(object)entity == (Object)null)
		{
			return false;
		}
		(bool Found, Vector3 Position) result = Safe.Get("roster.live.position", entity, (BoltEntity value) =>
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return (Found: true, Position: ((Component)value).transform.position);
		}, (Found: false, Position: Vector3.zero));
		position = result.Position;
		return result.Found;
	}

	public static void Bring(int index)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGet(index, out var p) || p.IsLocal || (Object)(object)p.Entity == (Object)null)
		{
			return;
		}
		try
		{
			LocalPlayer me = LocalPlayer._instance;
			((Component)p.Entity).transform.position = ((Component)me).transform.position + Vector3.up * 1.5f;
			Features.LastResult = "Brought " + p.Name;
		}
		catch (Exception ex)
		{
			Features.LastResult = "Bring failed";
			Diag.Warn(ex.Message);
		}
	}

	public static void Kill(int index)
	{
		if (!TryGet(index, out var p) || p.IsLocal || (Object)(object)p.Entity == (Object)null)
		{
			return;
		}
		try
		{
			Vitals v = ((Component)p.Entity).GetComponentInChildren<Vitals>();
			if ((Object)(object)v != (Object)null)
			{
				v.TriggerDeath();
				Features.LastResult = "Killed " + p.Name;
			}
			else
			{
				Features.LastResult = "No vitals on " + p.Name;
			}
		}
		catch (Exception ex)
		{
			Features.LastResult = "Kill failed";
			Diag.Warn(ex.Message);
		}
	}

	public static void Revive(int index)
	{
		if (!TryGet(index, out var p) || p.IsLocal || (Object)(object)p.Entity == (Object)null)
		{
			return;
		}
		try
		{
			Vitals v = ((Component)p.Entity).GetComponentInChildren<Vitals>();
			if ((Object)(object)v != (Object)null)
			{
				v.SetFullHealth();
				Features.LastResult = "Revived " + p.Name;
			}
			else
			{
				Features.LastResult = "No vitals on " + p.Name;
			}
		}
		catch (Exception ex)
		{
			Features.LastResult = "Revive failed";
			Diag.Warn(ex.Message);
		}
	}
}
