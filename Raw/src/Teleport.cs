using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TheForest.Utils;
using UnityEngine;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("saved spots")]
internal static class Teleport
{
    const string StoreFileName = "sonsofthedead_spots.json";
    internal struct Spot { internal string Name; internal Vector3 Pos; internal float R,G,B; }
    internal static readonly List<Spot> Spots = new();
    static string _path;
    static string StorePath => _path ??= Path.Combine(Plugin.DataRoot, StoreFileName);

    internal static void Load()
    {
        if (!File.Exists(StorePath)) return;
        var records = Safe.Get("spots.load", StorePath,
            path => JsonSerializer.Deserialize<List<SpotRecord>>(File.ReadAllText(path)),
            fallback: null, noise: Noise.Warn);
        if (records is null) return;
        Spots.Clear();
        foreach (var r in records)
            Spots.Add(new Spot { Name=r.Name, Pos=new Vector3(r.X,r.Y,r.Z), R=r.R,G=r.G,B=r.B });
        Diag.Info($"Loaded {Spots.Count} teleport spot(s).");
    }

    internal static void Save()
    {
        var records = new List<SpotRecord>(Spots.Count);
        foreach (var s in Spots)
            records.Add(new SpotRecord { Name=s.Name,X=s.Pos.x,Y=s.Pos.y,Z=s.Pos.z,R=s.R,G=s.G,B=s.B });
        Safe.Run("spots.save", () => File.WriteAllText(StorePath, JsonSerializer.Serialize(records)), Noise.Warn);
    }

    internal static void SaveCurrent(string name)
    {
        Transform player = Game.PlayerTransform;
        if (player == null) { Features.LastResult="Needs a loaded world"; return; }
        if (string.IsNullOrWhiteSpace(name)) name=$"Spot {Spots.Count+1}";
        Spots.Add(new Spot { Name=name, Pos=player.position, R=1f,G=1f,B=1f });
        Save();
        Features.LastResult=$"Saved spot \"{name}\"";
    }

    internal static void TeleportTo(int index)
    {
        if (!TryGet(index,out var spot)) return;
        bool moved=Safe.Run("spots.goto",spot.Pos,p=>LocalPlayer._instance.Goto(p),Noise.Warn);
        Features.LastResult=moved?$"Teleported to \"{spot.Name}\"":"Teleport failed";
    }

    internal static void RemoveAt(int index) { if(!TryGet(index,out _)) return; Spots.RemoveAt(index); Save(); }

    static bool TryGet(int index,out Spot spot)
    {
        if(index>=0 && index<Spots.Count){spot=Spots[index];return true;}
        spot=default; return false;
    }

    sealed class SpotRecord
    {
        public string Name {get;set;}
        public float X {get;set;}
        public float Y {get;set;}
        public float Z {get;set;}
        public float R {get;set;}
        public float G {get;set;}
        public float B {get;set;}
    }
}