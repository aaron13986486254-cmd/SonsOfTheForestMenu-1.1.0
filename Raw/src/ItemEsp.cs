using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Sons.Gameplay;
using Sons.Gameplay.Artifact;
using Sons.Gameplay.GPS;
using Sons.Inventory;
using Sons.Items.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("item ESP")]
internal static class ItemEsp
{
	private struct Entry
	{
		public Vector3 Pos;

		public bool Collected;
	}

	private struct Candidate
	{
		public Vector3 Pos;

		public string Label;

		public Color Color;

		public float Dist;
	}

	public static bool Enabled;

	public static bool ShowItems;

	public static bool ShowContainers;

	public static bool ShowCollectibles;

	public static bool ShowWaypoints;

	public static bool ShowDead = false;

	public static bool Radar;

	public static float RadarRange = 150f;

	public static int LastDrawn;

	private static readonly HashSet<PickUp> _pickups = new HashSet<PickUp>();

	private static readonly HashSet<DroppedInventoryItemsPickup> _bags = new HashSet<DroppedInventoryItemsPickup>();

	private static readonly HashSet<ContainerItemSpawner> _crates = new HashSet<ContainerItemSpawner>();

	private static readonly HashSet<InWorldAutoStorage> _autoStorage = new HashSet<InWorldAutoStorage>();

	private static readonly HashSet<StructureStorage> _storage = new HashSet<StructureStorage>();

	private static readonly HashSet<ArtifactStructure> _artifacts = new HashSet<ArtifactStructure>();

	private static readonly HashSet<GPSLocator> _gps = new HashSet<GPSLocator>();

	private static readonly Dictionary<int, Entry> _entries = new Dictionary<int, Entry>();

	private static readonly Dictionary<int, string> _names = new Dictionary<int, string>();

	private static bool _bootstrapped;

	private static float _nextScan;

	private static readonly GUIContent _gc = new GUIContent();

	private static readonly List<Candidate> _cands = new List<Candidate>(64);

	private static Texture2D _circleTex;

	private static int Id(Component c)
	{
		if (!((Object)(object)c != (Object)null) || !((Object)(object)c.gameObject != (Object)null))
		{
			return 0;
		}
		return ((Object)c.gameObject).GetInstanceID();
	}

	public static void RegisterPickup(PickUp p)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)p == (Object)null))
		{
			DroppedInventoryItemsPickup bag = (DroppedInventoryItemsPickup)(object)((p is DroppedInventoryItemsPickup) ? p : null);
			if ((Object)(object)bag != (Object)null)
			{
				_bags.Add(bag);
			}
			else
			{
				_pickups.Add(p);
			}
			int id = Id((Component)(object)p);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)p),
					Collected = p._isCollected
				};
			}
		}
	}

	public static void RegisterContainer(ContainerItemSpawner c)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)c == (Object)null))
		{
			_crates.Add(c);
			int id = Id((Component)(object)c);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)c)
				};
			}
		}
	}

	public static void RegisterAutoStorage(InWorldAutoStorage s)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)s == (Object)null))
		{
			_autoStorage.Add(s);
			int id = Id((Component)(object)s);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)s)
				};
			}
		}
	}

	public static void RegisterStorage(StructureStorage s)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)s == (Object)null))
		{
			_storage.Add(s);
			int id = Id((Component)(object)s);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)s)
				};
			}
		}
	}

	public static void RegisterArtifact(ArtifactStructure a)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)a == (Object)null))
		{
			_artifacts.Add(a);
			int id = Id((Component)(object)a);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)a)
				};
			}
		}
	}

	public static void CollectGps(List<Vector3> pos, List<string> name)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		foreach (GPSLocator g in _gps)
		{
			if (!((Object)(object)g == (Object)null))
			{
				pos.Add(SafePos((Component)(object)g));
				name.Add(GpsName(g));
			}
		}
	}

	public static void RegisterGps(GPSLocator g)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)g == (Object)null))
		{
			_gps.Add(g);
			int id = Id((Component)(object)g);
			if (id != 0)
			{
				_entries[id] = new Entry
				{
					Pos = SafePos((Component)(object)g)
				};
			}
		}
	}

	public static void ReRegisterPickup(PickUp p)
	{
		if (!((Object)(object)p == (Object)null))
		{
			int id = Id((Component)(object)p);
			if (id != 0 && !_entries.ContainsKey(id))
			{
				RegisterPickup(p);
			}
		}
	}

	public static void ForgetCollected(PickUp p)
	{
		if (!((Object)(object)p == (Object)null))
		{
			int id = Id((Component)(object)p);
			if (id != 0 && _entries.TryGetValue(id, out var e))
			{
				e.Collected = true;
				_entries[id] = e;
			}
		}
	}

	public static void UnregisterPickup(PickUp p)
	{
		if (!((Object)(object)p == (Object)null))
		{
			DroppedInventoryItemsPickup bag = (DroppedInventoryItemsPickup)(object)((p is DroppedInventoryItemsPickup) ? p : null);
			if ((Object)(object)bag != (Object)null)
			{
				_bags.Remove(bag);
			}
			else
			{
				_pickups.Remove(p);
			}
			int id = Id((Component)(object)p);
			if (id != 0)
			{
				_entries.Remove(id);
			}
		}
	}

	private static Vector3 SafePos(Component c)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Transform t = c.transform;
			return ((Object)(object)t != (Object)null) ? t.position : Vector3.zero;
		}
		catch
		{
			return Vector3.zero;
		}
	}

	private static void EnsureBootstrapped()
	{
		if (_bootstrapped)
		{
			return;
		}
		_bootstrapped = true;
		Il2CppArrayBase<PickUp> ps = Object.FindObjectsOfType<PickUp>();
		if (ps != null)
		{
			for (int i = 0; i < ps.Length; i++)
			{
				RegisterPickup(ps[i]);
			}
		}
		Il2CppArrayBase<ContainerItemSpawner> cs = Object.FindObjectsOfType<ContainerItemSpawner>();
		if (cs != null)
		{
			for (int j = 0; j < cs.Length; j++)
			{
				RegisterContainer(cs[j]);
			}
		}
		Il2CppArrayBase<InWorldAutoStorage> as_ = Object.FindObjectsOfType<InWorldAutoStorage>();
		if (as_ != null)
		{
			for (int k = 0; k < as_.Length; k++)
			{
				RegisterAutoStorage(as_[k]);
			}
		}
		Il2CppArrayBase<StructureStorage> st = Object.FindObjectsOfType<StructureStorage>();
		if (st != null)
		{
			for (int l = 0; l < st.Length; l++)
			{
				RegisterStorage(st[l]);
			}
		}
		Il2CppArrayBase<ArtifactStructure> ar = Object.FindObjectsOfType<ArtifactStructure>();
		if (ar != null)
		{
			for (int m = 0; m < ar.Length; m++)
			{
				RegisterArtifact(ar[m]);
			}
		}
		Il2CppArrayBase<GPSLocator> gs = Object.FindObjectsOfType<GPSLocator>();
		if (gs != null)
		{
			for (int n = 0; n < gs.Length; n++)
			{
				RegisterGps(gs[n]);
			}
		}
	}

	private static string ItemName(int itemId)
	{
		if (_names.TryGetValue(itemId, out var cached))
		{
			return cached;
		}
		string name = null;
		try
		{
			ItemData d = ItemDatabaseManager.ItemById(itemId);
			if ((Object)(object)d != (Object)null)
			{
				ItemUiData ui = d._uiData;
				if (ui != null)
				{
					name = ui.GetTitleLocalized();
				}
				if (string.IsNullOrEmpty(name))
				{
					name = d.Name;
				}
			}
		}
		catch
		{
		}
		if (string.IsNullOrEmpty(name))
		{
			name = "item " + itemId;
		}
		_names[itemId] = name;
		return name;
	}

	public static void Draw()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		LastDrawn = 0;
		if (!Enabled || !Game.InWorld || (Event.current != null && (int)Event.current.type != 7))
		{
			return;
		}
		EnsureBootstrapped();
		if (Time.unscaledTime > _nextScan)
		{
			_nextScan = Time.unscaledTime + 3f;
			Prune();
		}
		Camera cam = Game.Cam;
		if ((Object)(object)cam == (Object)null)
		{
			return;
		}
		Transform self = Game.PlayerTransform;
		Vector3 origin = (((Object)(object)self != (Object)null) ? self.position : ((Component)cam).transform.position);
		_cands.Clear();
		float max = Esp.MaxDistance;
		if (ShowItems)
		{
			foreach (PickUp p in _pickups)
			{
				if ((Object)(object)p != (Object)null)
				{
					AddCandidate(_cands, ((Component)p).transform.position, ItemName(p._itemId), Theme.Bone, max, origin);
				}
			}
		}
		if (ShowCollectibles)
		{
			foreach (DroppedInventoryItemsPickup b in _bags)
			{
				if ((Object)(object)b != (Object)null)
				{
					AddCandidate(_cands, ((Component)b).transform.position, ItemName(((PickUp)b)._itemId), Theme.Live, max, origin);
				}
			}
			foreach (ArtifactStructure a in _artifacts)
			{
				if ((Object)(object)a != (Object)null)
				{
					AddCandidate(_cands, ((Component)a).transform.position, "Artifact", Theme.RustBright, max, origin);
				}
			}
		}
		if (ShowContainers)
		{
			foreach (ContainerItemSpawner c in _crates)
			{
				if ((Object)(object)c != (Object)null)
				{
					AddCandidate(_cands, ((Component)c).transform.position, "Crate", Theme.Olive, max, origin);
				}
			}
			foreach (InWorldAutoStorage s in _autoStorage)
			{
				if ((Object)(object)s != (Object)null)
				{
					AddCandidate(_cands, ((Component)s).transform.position, "Auto Storage", Theme.Olive, max, origin);
				}
			}
			foreach (StructureStorage s2 in _storage)
			{
				if ((Object)(object)s2 != (Object)null)
				{
					AddCandidate(_cands, ((Component)s2).transform.position, "Storage", Theme.Olive, max, origin);
				}
			}
		}
		if (ShowWaypoints)
		{
			foreach (GPSLocator g in _gps)
			{
				if (!((Object)(object)g == (Object)null))
				{
					string label = GpsName(g);
					AddCandidate(_cands, ((Component)g).transform.position, label, Theme.RustBright, max, origin);
				}
			}
		}
		_cands.Sort((Candidate candidate, Candidate candidate2) => candidate.Dist.CompareTo(candidate2.Dist));
		int budget = Mathf.Min(_cands.Count, 80);
		for (int i = 0; i < budget; i++)
		{
			DrawPoint(_cands[i], cam);
		}
		if (Radar)
		{
			DrawRadar(origin, ((Object)(object)self != (Object)null) ? self.eulerAngles.y : 0f);
		}
	}

	private static void AddCandidate(List<Candidate> list, Vector3 pos, string label, Color color, float max, Vector3 origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		float d = Vector3.Distance(origin, pos);
		if (!(d > max))
		{
			list.Add(new Candidate
			{
				Pos = pos,
				Label = label,
				Color = color,
				Dist = d
			});
		}
	}

	private static string GpsName(GPSLocator g)
	{
		try
		{
			string poi = g._pointOfInterestName;
			if (!string.IsNullOrEmpty(poi))
			{
				return poi;
			}
		}
		catch
		{
		}
		try
		{
			string text = g.Text;
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		catch
		{
		}
		return "Waypoint";
	}

	private static void DrawPoint(Candidate c, Camera cam)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Vector3 top = cam.WorldToScreenPoint(c.Pos + Vector3.up * 1.2f);
			if (top.z <= 0f)
			{
				return;
			}
			float sx = top.x;
			float sy = (float)Screen.height - top.y;
			float size = 7f;
			Rect r = new Rect(sx - size * 0.5f, sy - size * 0.5f, size, size);
			Theme.Fill(r, c.Color);
			Theme.Fill(new Rect(r.x - 1f, r.y - 1f, r.width + 2f, 1f), Theme.Ink);
			Theme.Fill(new Rect(r.x - 1f, r.yMax, r.width + 2f, 1f), Theme.Ink);
			if (c.Label != null)
			{
				string label = c.Label;
				if (c.Dist >= 100f)
				{
					label = label + "  " + (int)c.Dist + "m";
				}
				GUIStyle style = Theme.EspLabel;
				_gc.text = label;
				Vector2 sz = style.CalcSize(_gc);
				Rect lr = new Rect(sx - sz.x * 0.5f, sy + 4f, sz.x, sz.y);
				lr.x = Mathf.Clamp(lr.x, 4f, (float)Screen.width - sz.x - 4f);
				lr.y = Mathf.Clamp(lr.y, 4f, (float)Screen.height - sz.y - 4f);
				Color prev = style.normal.textColor;
				try
				{
					style.normal.textColor = c.Color;
					GUI.Label(lr, _gc, style);
				}
				finally
				{
					style.normal.textColor = prev;
				}
			}
			LastDrawn++;
		}
		catch
		{
		}
	}

	private static void Prune()
	{
		try
		{
			_pickups.RemoveWhere((PickUp p) => (Object)(object)p == (Object)null);
			_bags.RemoveWhere((DroppedInventoryItemsPickup b) => (Object)(object)b == (Object)null);
			_crates.RemoveWhere((ContainerItemSpawner c) => (Object)(object)c == (Object)null);
			_autoStorage.RemoveWhere((InWorldAutoStorage s) => (Object)(object)s == (Object)null);
			_storage.RemoveWhere((StructureStorage s) => (Object)(object)s == (Object)null);
			_artifacts.RemoveWhere((ArtifactStructure a) => (Object)(object)a == (Object)null);
			_gps.RemoveWhere((GPSLocator g) => (Object)(object)g == (Object)null);
		}
		catch
		{
		}
	}

	private static Texture2D CircleTex()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected Obj, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_circleTex != (Object)null)
		{
			return _circleTex;
		}
		Texture2D t = new Texture2D(128, 128, (TextureFormat)4, false);
		Vector2 center = new Vector2(64f, 64f);
		float radius = 63f;
		for (int y = 0; y < 128; y++)
		{
			for (int x = 0; x < 128; x++)
			{
				float a = ((Vector2.Distance(new Vector2((float)x + 0.5f, (float)y + 0.5f), center) <= radius) ? 1f : 0f);
				t.SetPixel(x, y, new Color(0.02f, 0.02f, 0.015f, 0.55f * a));
			}
		}
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		_circleTex = t;
		return t;
	}

	private static void DrawRadar(Vector3 origin, float yawDeg)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 corner = new Vector2((float)Screen.width - 170f - 14f, (float)Screen.height - 170f - 14f);
		Rect rect = new Rect(corner.x, corner.y, 170f, 170f);
		GUI.DrawTexture(rect, (Texture)(object)CircleTex());
		Theme.Box(new Rect(rect.x - 1f, rect.y - 1f, rect.width + 2f, rect.height + 2f), Theme.Line);
		Theme.Fill(new Rect(corner.x + 85f - 1f, corner.y - 2f, 2f, 5f), Theme.RustBright);
		float range = Mathf.Max(RadarRange, 10f);
		float cs = Mathf.Cos((0f - yawDeg) * ((float)Math.PI / 180f));
		float sn = Mathf.Sin((0f - yawDeg) * ((float)Math.PI / 180f));
		float half = 80f;
		BlipSet<PickUp>(corner, origin, cs, sn, half, range, _pickups, Theme.Bone);
		BlipSet<DroppedInventoryItemsPickup>(corner, origin, cs, sn, half, range, _bags, Theme.Live);
		BlipSet<ContainerItemSpawner>(corner, origin, cs, sn, half, range, _crates, Theme.Olive);
		BlipSet<InWorldAutoStorage>(corner, origin, cs, sn, half, range, _autoStorage, Theme.Olive);
		BlipSet<StructureStorage>(corner, origin, cs, sn, half, range, _storage, Theme.Olive);
		BlipSet<ArtifactStructure>(corner, origin, cs, sn, half, range, _artifacts, Theme.RustBright);
		BlipSet<GPSLocator>(corner, origin, cs, sn, half, range, _gps, Theme.RustBright);
	}

	private static void BlipSet<T>(Vector2 corner, Vector3 origin, float cs, float sn, float half, float range, HashSet<T> set, Color color) where T : Component
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		foreach (T c in set)
		{
			if ((Object)(object)c == (Object)null)
			{
				continue;
			}
			Vector3 pos;
			try
			{
				pos = ((Component)c).transform.position;
			}
			catch
			{
				continue;
			}
			Vector3 rel = pos - origin;
			if (!(Mathf.Abs(rel.x) > range) && !(Mathf.Abs(rel.z) > range))
			{
				float rx = rel.x * cs - rel.z * sn;
				float rz = rel.x * sn + rel.z * cs;
				float cx = corner.x + half + rx / range * half;
				float cy = corner.y + half - rz / range * half;
				if (!(cx < corner.x + 3f) && !(cx > corner.x + 168f) && !(cy < corner.y + 3f) && !(cy > corner.y + 168f))
				{
					Theme.Fill(new Rect(cx - 1.5f, cy - 1.5f, 3f, 3f), color);
				}
			}
		}
	}

	static ItemEsp()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected Obj, but got Unknown
	}
}
