using System;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;
using Sons.Gameplay.GPS;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("native GPS travel map")]
internal static class Map
{
	private struct Marker
	{
		internal Vector3 Position;

		internal string Label;

		internal MapMarkerKind Kind;

		internal int TeleportIndex;

		internal Texture2D NativeIcon;

		internal Texture2D NativeOutline;

		internal Color IconColor;

		internal Color OutlineColor;
	}

	private struct Projected
	{
		internal int MarkerIndex;

		internal float X;

		internal float Y;

		internal int Count;
	}

	private struct NativeCandidate
	{
		internal Rect Uv;

		internal int Score;

		internal int Width;

		internal int Height;

		internal int TrackerIndex;

		internal int TextureInstanceId;

		internal bool IsSprite;

		internal string OwnerName;

		internal string TextureName;
	}

	private struct LiveMapDraw
	{
		internal Rect Target;

		internal Rect Uv;

		internal Rect BackdropTarget;

		internal Rect BackdropUv;

		internal bool DrawBackdrop;

		internal int TrackerIndex;

		internal int TextureInstanceId;

		internal int ExpectedWidth;

		internal int ExpectedHeight;
	}

	private const float DefaultMinX = -2100f;

	private const float DefaultMaxX = 2100f;

	private const float DefaultMinZ = -2100f;

	private const float DefaultMaxZ = 2100f;

	private const float ResolveInterval = 2f;

	private const float ResolveFailureBackoff = 10f;

	private const float SnapshotInterval = 1f;

	private const float MarkerSpacing = 22f;

	private const float MarkerHitRadius = 15f;

	private const int NoHover = int.MinValue;

	private const string TextureOwnerPrefix = "Cyberfox1337x.SonsOfTheDead.MapTexture.";

	private static readonly Color Deck;

	private static readonly Color NativeTint;

	private static readonly Color OverviewBackdropTint;

	private static readonly Color Shadow;

	private static readonly Color LabelDeck;

	private static readonly string[] ClusterText;

	private static readonly string[] ClusterCaptionText;

	private const string Controls = "CLICK TRAVEL   ·   HOLD + DRAG PAN   ·   WHEEL ZOOM";

	private static readonly Vector3[] BunkerPositions;

	private static readonly string[] BunkerNames;

	private static readonly System.Collections.Generic.List<Marker> _markers;

	private static readonly System.Collections.Generic.List<Projected> _projected;

	private static readonly System.Collections.Generic.List<Rect> _labelRects;

	private static Rect _baseUv;

	private static float _nativeAspect;

	private static int _nativeTrackerIndex;

	private static int _nativeTextureInstanceId;

	private static int _nativeExpectedWidth;

	private static int _nativeExpectedHeight;

	private static float _minX;

	private static float _maxX;

	private static float _minZ;

	private static float _maxZ;

	private static float _nextResolve;

	private static float _nextSnapshot;

	private static bool _wasInWorld;

	private static bool _nativeMapAttached;

	private static bool _hasLocal;

	private static Marker _local;

	private static float _zoom;

	private static float _centerU;

	private static float _centerV;

	private static string _zoomText;

	private static string _summary;

	private static string _source;

	private static bool _panArmed;

	private static bool _panning;

	private static int _panButton;

	private static int _panControlId;

	private static Vector2 _panStart;

	private static float _panCenterU;

	private static float _panCenterV;

	private static int _hoverKey;

	private static int _hoverClusterCount;

	private static string _hoverTitle;

	private static string _hoverDetail;

	private static GUIStyle _title;

	private static GUIStyle _status;

	private static GUIStyle _right;

	private static GUIStyle _cluster;

	private static GUIStyle _markerLabel;

	private static RawImage _gpsIconOwner;

	private static RawImage _bunkerIconOwner;

	private static RawImage _savedIconOwner;

	private static RawImage _localIconOwner;

	private static RawImage _clusterBadgeOwner;

	private static Texture2D _gpsIconTexture;

	private static Texture2D _bunkerIconTexture;

	private static Texture2D _savedIconTexture;

	private static Texture2D _localIconTexture;

	private static Texture2D _clusterBadgeTexture;

	internal static bool Ready => HasNativeMapTexture();

	internal static bool Sampling => false;

	internal static float Progress
	{
		get
		{
			if (!Ready)
			{
				return 0f;
			}
			return 1f;
		}
	}

	static Map()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		Deck = new Color(0.018f, 0.022f, 0.018f, 0.94f);
		NativeTint = new Color(0.92f, 0.96f, 0.88f, 0.96f);
		OverviewBackdropTint = new Color(0.2f, 0.24f, 0.18f, 0.28f);
		Shadow = new Color(0f, 0f, 0f, 0.78f);
		LabelDeck = new Color(0.018f, 0.022f, 0.018f, 0.86f);
		ClusterText = new string[11]
		{
			"", "", "2", "3", "4", "5", "6", "7", "8", "9",
			"9+"
		};
		ClusterCaptionText = new string[11]
		{
			"", "", "2 LOCATIONS", "3 LOCATIONS", "4 LOCATIONS", "5 LOCATIONS", "6 LOCATIONS", "7 LOCATIONS", "8 LOCATIONS", "9 LOCATIONS",
			"9+ LOCATIONS"
		};
		BunkerPositions = new Vector3[7]
		{
			new Vector3(-477.71f, 86.52f, 713.86f),
			new Vector3(-1136.85f, 280.74f, -1103.4f),
			new Vector3(1109.97f, 129.03f, 1007.63f),
			new Vector3(-1192.75f, 66.94f, 140.7f),
			new Vector3(-1015.25f, 99.56f, 1040.33f),
			new Vector3(1756.66f, 40.33f, 552.78f),
			new Vector3(1238.51f, 239.69f, -657.45f)
		};
		BunkerNames = new string[7] { "Bunker A", "Bunker B", "Bunker C", "Bunker Entertainment", "Bunker Food", "Bunker Luxury", "Bunker Residential" };
		_markers = new System.Collections.Generic.List<Marker>(64);
		_projected = new System.Collections.Generic.List<Projected>(64);
		_labelRects = new System.Collections.Generic.List<Rect>(64);
		_baseUv = new Rect(0f, 0f, 1f, 1f);
		_nativeAspect = 1f;
		_nativeTrackerIndex = -1;
		_minX = -2100f;
		_maxX = 2100f;
		_minZ = -2100f;
		_maxZ = 2100f;
		_zoom = 1f;
		_centerU = 0.5f;
		_centerV = 0.5f;
		_zoomText = "1.0X";
		_summary = "0 GPS · 0 BUNKERS · 0 SAVED";
		_source = "GPS MAP NOT INITIALIZED";
		_panButton = -1;
		_hoverKey = int.MinValue;
		_hoverTitle = string.Empty;
		_hoverDetail = string.Empty;
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "native-gps-travel-map";
	}

	internal static void Tick(bool visible)
	{
		if (!visible)
		{
			CancelGesture();
			return;
		}
		if (!Game.InWorld)
		{
			if (_wasInWorld)
			{
				ClearWorld();
			}
			_wasInWorld = false;
			return;
		}
		_wasInWorld = true;
		float now = Time.unscaledTime;
		if (!HasNativeMapTexture() && now >= _nextResolve)
		{
			ReleaseNativeMap();
			_nextResolve = now + 2f;
			ResolveNativeMap(now);
		}
		if (now >= _nextSnapshot)
		{
			RefreshMarkers();
			_nextSnapshot = now + 1f;
		}
		RefreshLocal();
	}

	internal static void Draw(Rect pane)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (!Safe.Run("map.draw", pane, (Rect value) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			DrawCore(value);
		}))
		{
			InvalidateNativeMap("NATIVE MAP LINK LOST");
		}
	}

	private static void DrawCore(Rect pane)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		EnsurePresentation();
		Rect deck = new Rect(pane.x + 8f, pane.y + 8f, Mathf.Max(100f, pane.width - 16f), Mathf.Max(78f, pane.height - 16f));
		Theme.Fill(deck, Deck);
		Theme.Box(deck, Theme.Line);
		Rect header = new Rect(deck.x + 8f, deck.y + 2f, deck.width - 16f, 25f);
		Rect footer = new Rect(deck.x + 8f, deck.yMax - 23f, deck.width - 16f, 21f);
		Rect available = new Rect(deck.x + 5f, header.yMax, deck.width - 10f, Mathf.Max(26f, footer.y - header.yMax - 2f));
		DrawHeader(header, available);
		if (!HasNativeMapTexture())
		{
			DrawWaiting(available);
			DrawFooter(footer);
			return;
		}
		MapViewport viewport = GetViewport(available);
		Rect mapRect = ViewportRect(available, viewport);
		if (!DrawNative(available, mapRect, viewport))
		{
			DrawWaiting(available);
			DrawFooter(footer);
			return;
		}
		ProjectMarkers(mapRect, viewport);
		DrawMarkers(mapRect);
		UpdateHover(mapRect);
		DrawFurniture(mapRect);
		HandleInput(mapRect, available, viewport);
		DrawFooter(footer);
	}

	private static void ResolveNativeMap(float now)
	{
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> trackers = Safe.Get("map.native.trackers", () => GPSTrackerSystem.GpsTrackers);
		int count = ((trackers != null) ? Safe.Get("map.native.tracker-count", trackers, (Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> list) => list.Count, 0) : 0);
		System.Collections.Generic.List<NativeCandidate> candidates = new System.Collections.Generic.List<NativeCandidate>(16);
		for (int index = 0; index < count; index++)
		{
			GPSTrackerSystem tracker = Safe.Get("map.native.tracker", (trackers, index), ((Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> List, int Index) state) => state.List[state.Index]);
			if (!((Object)(object)tracker == (Object)null))
			{
				RectTransform surface = Safe.Get("map.native.surface", tracker, (GPSTrackerSystem value) => value._surfaceMap);
				if (!((Object)(object)surface == (Object)null))
				{
					ReadWorldSize(tracker);
					ConsiderDirectImage(surface, index, candidates);
				}
			}
		}
		candidates.Sort((NativeCandidate first, NativeCandidate second) => second.Score.CompareTo(first.Score));
		if (candidates.Count > 0)
		{
			NativeCandidate candidate = candidates[0];
			_nativeMapAttached = true;
			_nativeTrackerIndex = candidate.TrackerIndex;
			_nativeTextureInstanceId = candidate.TextureInstanceId;
			_nativeExpectedWidth = candidate.Width;
			_nativeExpectedHeight = candidate.Height;
			_baseUv = candidate.Uv;
			_nativeAspect = (float)candidate.Width / (float)candidate.Height;
			_source = "NATIVE SURFACE MAP";
			Diag.InfoOnce("map.native.asset-ready", "Native surface map metadata linked; Repaint will reacquire the live game-owned GPS asset.");
			string owner = DiagnosticName(candidate.OwnerName);
			string texture = DiagnosticName(candidate.TextureName);
			Diag.InfoOnce("map.native.selection", $"Native GPS candidate selected: {(candidate.IsSprite ? "sprite" : "raw Texture2D")}, {candidate.Width}x{candidate.Height}, score {candidate.Score}, owner '{owner}', texture '{texture}'.");
			Diag.InfoOnce("map.native.ready", "Native GPS surface map is ready without a plugin-created texture copy.");
		}
		else
		{
			_source = "GPS MAP NOT INITIALIZED";
			_nextResolve = now + 10f;
		}
	}

	private static void ReadWorldSize(GPSTrackerSystem tracker)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 size = Safe.Get("map.native.world-size", tracker, (GPSTrackerSystem value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value._worldSize;
		}, Vector2.zero);
		if (Finite(size.x) && Finite(size.y) && !(size.x < 2500f) && !(size.y < 2500f) && !(size.x > 12000f) && !(size.y > 12000f))
		{
			_minX = size.x * -0.5f;
			_maxX = size.x * 0.5f;
			_minZ = size.y * -0.5f;
			_maxZ = size.y * 0.5f;
		}
	}

	private static void ConsiderDirectImage(RectTransform surface, int trackerIndex, System.Collections.Generic.List<NativeCandidate> candidates)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		Image image = Safe.Get("map.native.image", surface, (RectTransform root) => ((Component)root).GetComponent<Image>());
		Sprite sprite = (((Object)(object)image == (Object)null) ? null : Safe.Get("map.native.sprite", image, (Image value) => value.sprite));
		Texture2D texture = (((Object)(object)sprite == (Object)null) ? null : Safe.Get("map.native.sprite-texture", sprite, (Sprite value) => value.texture));
		Rect rect = (((Object)(object)sprite == (Object)null) ? default(Rect) : Safe.Get("map.native.sprite-rect", sprite, (Sprite value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.rect;
		}));
		if ((Object)(object)texture == (Object)null || rect.width < 256f || rect.height < 256f)
		{
			return;
		}
		float width = Safe.Get("map.native.atlas-width", texture, (Texture2D value) => ((Texture)value).width, 0f);
		float height = Safe.Get("map.native.atlas-height", texture, (Texture2D value) => ((Texture)value).height, 0f);
		if (!(width <= 0f) && !(height <= 0f))
		{
			Rect uv = new Rect(rect.x / width, rect.y / height, rect.width / width, rect.height / height);
			string ownerName = Safe.Get("map.native.image-name", image, (Image value) => ((Object)value).name, string.Empty);
			Consider(texture, uv, isSprite: true, ownerName, trackerIndex, candidates);
		}
	}

	private static void Consider(Texture2D texture, Rect uv, bool isSprite, string ownerName, int trackerIndex, System.Collections.Generic.List<NativeCandidate> candidates)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		if (!IsTextureAlive(texture))
		{
			return;
		}
		int width = Safe.Get("map.native.texture-width", texture, (Texture2D value) => ((Texture)value).width, 0);
		int num = Safe.Get("map.native.texture-height", texture, (Texture2D value) => ((Texture)value).height, 0);
		int effectiveWidth = Mathf.RoundToInt((float)width * Mathf.Abs(uv.width));
		int effectiveHeight = Mathf.RoundToInt((float)num * Mathf.Abs(uv.height));
		string textureName = Safe.Get("map.native.texture-name", texture, (Texture2D value) => ((Object)value).name, string.Empty);
		int score = MapRules.ScoreNativeCandidate(isSprite, ownerName, textureName, effectiveWidth, effectiveHeight);
		if (score != int.MinValue)
		{
			int textureInstanceId = Safe.Get("map.native.texture-instance-id", texture, (Texture2D value) => ((Object)value).GetInstanceID(), 0);
			if (textureInstanceId != 0)
			{
				candidates.Add(new NativeCandidate
				{
					Uv = uv,
					Score = score,
					Width = effectiveWidth,
					Height = effectiveHeight,
					TrackerIndex = trackerIndex,
					TextureInstanceId = textureInstanceId,
					IsSprite = isSprite,
					OwnerName = ownerName,
					TextureName = textureName
				});
			}
		}
	}

	private static string DiagnosticName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return "unnamed";
		}
		return MapRules.DisplayLabel(name).Replace("'", string.Empty);
	}

	private static bool RetainTexture(string ownerName, Texture2D texture, ref RawImage cachedOwner, ref Texture2D cachedTexture)
	{
		if (!IsTextureAlive(texture))
		{
			return false;
		}
		cachedOwner = ResolveTextureOwner(ownerName, cachedOwner, create: true);
		if (IsOwnerAlive(cachedOwner))
		{
			Safe.Run("map.texture-owner.retain", (cachedOwner, texture), ((RawImage Owner, Texture2D Texture) value) =>
			{
				value.Owner.texture = (Texture)(object)value.Texture;
			});
		}
		cachedTexture = texture;
		return true;
	}

	private static RawImage ResolveTextureOwner(string ownerName, RawImage cachedOwner, bool create)
	{
		if (IsOwnerAlive(cachedOwner))
		{
			return cachedOwner;
		}
		return Safe.Get("map.texture-owner.resolve", (ownerName, create), (Func<(string, bool), RawImage>)(((string Name, bool Create) value) =>
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected Obj, but got Unknown
			GameObject val = GameObject.Find(value.Name);
			if (val == null)
			{
				if (!value.Create)
				{
					return (RawImage)null;
				}
				val = new GameObject(value.Name);
			}
			((Object)val).hideFlags = (HideFlags)61;
			Object.DontDestroyOnLoad((Object)(object)val);
			RawImage val2 = val.GetComponent<RawImage>();
			if (val2 == null)
			{
				val2 = val.AddComponent<RawImage>();
			}
			((Behaviour)val2).enabled = false;
			((Graphic)val2).raycastTarget = false;
			((Object)val2).hideFlags = (HideFlags)61;
			return val2;
		}), (RawImage)null, Noise.Transient);
	}

	private static bool IsOwnerAlive(RawImage owner)
	{
		if (owner == null)
		{
			return false;
		}
		return Safe.Get("map.texture-owner.alive", owner, (RawImage value) => !((Il2CppObjectBase)value).WasCollected, fallback: false);
	}

	private static bool HasNativeMapTexture()
	{
		if (_nativeMapAttached && _nativeTrackerIndex >= 0 && _nativeTextureInstanceId != 0 && _nativeExpectedWidth > 0)
		{
			return _nativeExpectedHeight > 0;
		}
		return false;
	}

	private static void ClearTextureOwner(string ownerName, ref RawImage cachedOwner)
	{
		cachedOwner = ResolveTextureOwner(ownerName, cachedOwner, create: false);
		if (IsOwnerAlive(cachedOwner))
		{
			Safe.Run("map.texture-owner.clear", cachedOwner, (RawImage value) =>
			{
				value.texture = null;
			});
		}
	}

	private static void RefreshMarkers()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		_markers.Clear();
		int savedCount = 0;
		int gpsCount = 0;
		int bunkerCount = 0;
		for (int index = 0; index < Teleport.Spots.Count; index++)
		{
			Teleport.Spot spot = Teleport.Spots[index];
			_markers.Add(new Marker
			{
				Position = spot.Pos,
				Label = MapRules.DisplayLabel(spot.Name),
				Kind = MapMarkerKind.SavedSpot,
				TeleportIndex = index,
				IconColor = Theme.IconGold,
				OutlineColor = Theme.Ink
			});
			savedCount++;
		}
		Il2CppSystem.Collections.Generic.List<GPSLocator> locators = Safe.Get("map.locators.list", () => GPSLocator._activeLocators);
		int locatorCount = ((locators != null) ? Safe.Get("map.locators.count", locators, (Il2CppSystem.Collections.Generic.List<GPSLocator> list) => list.Count, 0) : 0);
		for (int index2 = 0; index2 < locatorCount; index2++)
		{
			if (TryLocator(Safe.Get("map.locators.item", (locators, index2), ((Il2CppSystem.Collections.Generic.List<GPSLocator> List, int Index) state) => state.List[state.Index]), out var marker))
			{
				_markers.Add(marker);
				if (marker.Kind == MapMarkerKind.Bunker)
				{
					bunkerCount++;
				}
				else
				{
					gpsCount++;
				}
			}
		}
		for (int index3 = 0; index3 < BunkerPositions.Length; index3++)
		{
			if (!HasNativePointNear(BunkerPositions[index3], 35f))
			{
				_markers.Add(new Marker
				{
					Position = BunkerPositions[index3],
					Label = BunkerNames[index3],
					Kind = MapMarkerKind.Bunker,
					TeleportIndex = -1,
					IconColor = Theme.IconRust,
					OutlineColor = Theme.Ink
				});
				bunkerCount++;
			}
		}
		_summary = $"{gpsCount} GPS · {bunkerCount} BUNKERS · {savedCount} SAVED";
	}

	private static bool TryLocator(GPSLocator locator, out Marker marker)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		marker = default;
		if ((Object)(object)locator == (Object)null)
		{
			return false;
		}
		bool flag = Safe.Get("map.locator.active", locator, (GPSLocator value) => value.IsActive, fallback: false);
		GPSLocator.Style style = Safe.Get("map.locator.style", locator, (GPSLocator value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.UIStyle;
		}, (GPSLocator.Style)3);
		bool actorAttached = Safe.Get("map.locator.actor", locator, (GPSLocator value) => (Object)(object)value._worldActorLocators != (Object)null, fallback: true);
		if ((!flag || (int)style == 3) | actorAttached)
		{
			return false;
		}
		Vector3 position = Safe.Get("map.locator.position", locator, (GPSLocator value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.Position();
		}, new Vector3(float.NaN, float.NaN, float.NaN));
		if (!Finite(position.x) || !Finite(position.z))
		{
			return false;
		}
		string pointOfInterestName = Safe.Get("map.locator.name", locator, (GPSLocator value) => value._pointOfInterestName);
		string objectName = Safe.Get("map.locator.object-name", locator, (GPSLocator value) => ((Object)((Component)value).gameObject).name);
		string text = Safe.Get("map.locator.text", locator, (GPSLocator value) => value.Text);
		int iconId = Safe.Get("map.locator.icon-id", locator, (GPSLocator value) => value.CurrentIconId, -1);
		GPSLocatorIcons.IconData iconData = Safe.Get("map.locator.icon-data", locator, (GPSLocator value) => value.IconData);
		Texture2D nativeIcon = ((iconData == null) ? null : Safe.Get("map.locator.icon", iconData, (GPSLocatorIcons.IconData value) => value._icon));
		string iconTextureName = (((Object)(object)nativeIcon == (Object)null) ? null : Safe.Get("map.locator.icon-name", nativeIcon, (Texture2D value) => ((Object)value).name));
		string label = MapRules.ResolveLocatorLabel(pointOfInterestName, objectName, text, iconTextureName, iconId);
		MapMarkerKind kind = ((label != null && label.IndexOf("bunker", StringComparison.OrdinalIgnoreCase) >= 0) ? MapMarkerKind.Bunker : MapMarkerKind.Locator);
		marker = new Marker
		{
			Position = position,
			Label = label,
			Kind = kind,
			TeleportIndex = -1,
			NativeIcon = nativeIcon,
			NativeOutline = ((iconData == null) ? null : Safe.Get("map.locator.outline", iconData, (GPSLocatorIcons.IconData value) => value._iconOutline)),
			IconColor = ((iconData == null) ? Theme.IconSteel : Safe.Get("map.locator.icon-color", iconData, (GPSLocatorIcons.IconData value) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return value._iconColor;
			}, Theme.IconSteel)),
			OutlineColor = ((iconData == null) ? Theme.Ink : Safe.Get("map.locator.outline-color", iconData, (GPSLocatorIcons.IconData value) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return value._iconOutlineColor;
			}, Theme.Ink))
		};
		return true;
	}

	private static bool HasNativePointNear(Vector3 position, float distance)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		float maximum = distance * distance;
		for (int index = 0; index < _markers.Count; index++)
		{
			Marker marker = _markers[index];
			if (marker.Kind == MapMarkerKind.Locator || marker.Kind == MapMarkerKind.Bunker)
			{
				float num = marker.Position.x - position.x;
				float z = marker.Position.z - position.z;
				if (num * num + z * z <= maximum)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void RefreshLocal()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		Transform player = Game.PlayerTransform;
		Vector3 position = (Vector3)(((Object)(object)player == (Object)null) ? new Vector3(float.NaN, float.NaN, float.NaN) : Safe.Get("map.local.position", player, (Transform value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.position;
		}, new Vector3(float.NaN, float.NaN, float.NaN)));
		_hasLocal = Finite(position.x) && Finite(position.z);
		if (_hasLocal)
		{
			_local = new Marker
			{
				Position = position,
				Label = "Your position",
				Kind = MapMarkerKind.LocalPlayer,
				TeleportIndex = -1,
				IconColor = Theme.IconOlive,
				OutlineColor = Theme.Ink
			};
		}
	}

	private static bool DrawNative(Rect available, Rect mapRect, MapViewport viewport)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		if (!_nativeMapAttached)
		{
			InvalidateNativeMap("NATIVE MAP LINK LOST");
			return false;
		}
		if ((int)Event.current.type != 7)
		{
			return true;
		}
		Rect uv = SourceUv(viewport, _centerU, _centerV);
		Rect backdropUv = default;
		if (viewport.IsContained)
		{
			MapViewport backdrop = MapRules.CalculateViewport(_nativeAspect, TargetAspect(available), 1f);
			float backdropCenterU = MapRules.ClampCenterToSpan(_centerU, backdrop.SpanU);
			float backdropCenterV = MapRules.ClampCenterToSpan(_centerV, backdrop.SpanV);
			backdropUv = SourceUv(backdrop, backdropCenterU, backdropCenterV);
		}
		Color previous = GUI.color;
		GUI.color = NativeTint;
		bool flag = Safe.Run("map.native.live-reacquire", new LiveMapDraw
		{
			Target = mapRect,
			Uv = uv,
			BackdropTarget = available,
			BackdropUv = backdropUv,
			DrawBackdrop = viewport.IsContained,
			TrackerIndex = _nativeTrackerIndex,
			TextureInstanceId = _nativeTextureInstanceId,
			ExpectedWidth = _nativeExpectedWidth,
			ExpectedHeight = _nativeExpectedHeight
		}, (LiveMapDraw value) =>
		{
			DrawLiveNative(value);
		});
		GUI.color = previous;
		if (!flag)
		{
			Diag.Once("map.native.live-reacquire-failed", "Native GPS link was lost during Repaint; returning to the tracker feed on the next Update.");
			InvalidateNativeMap("NATIVE MAP LINK LOST");
			return false;
		}
		Diag.InfoOnce("map.native.live-draw-ready", "Native surface map reacquired and drawn inside one Repaint callback.");
		Theme.Box(available, Theme.Dead);
		if (viewport.IsContained)
		{
			Theme.Box(mapRect, Theme.Olive);
		}
		return true;
	}

	private static Rect SourceUv(MapViewport viewport, float centerU, float centerV)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		return new Rect(_baseUv.x + (centerU - viewport.SpanU * 0.5f) * _baseUv.width, _baseUv.y + (centerV - viewport.SpanV * 0.5f) * _baseUv.height, viewport.SpanU * _baseUv.width, viewport.SpanV * _baseUv.height);
	}

	private static void DrawLiveNative(LiveMapDraw request)
	{
		if (TryDrawNativeByInstanceId(request))
		{
			return;
		}
		Diag.Once("map.native.id-fallback", "Native GPS texture instance lookup was unavailable; trying the narrow live tracker fallback.");
		Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> trackers = Safe.Get("map.native.live-trackers", () => GPSTrackerSystem.GpsTrackers);
		if (trackers == null)
		{
			throw new InvalidOperationException("The live GPS tracker list was unavailable during Repaint.");
		}
		int trackerCount = Safe.Get("map.native.live-tracker-count", trackers, (Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> value) => value.Count, -1);
		if (trackerCount <= 0)
		{
			throw new InvalidOperationException("No live GPS tracker was available during Repaint.");
		}
		int preferred = request.TrackerIndex;
		if (preferred >= 0 && preferred < trackerCount)
		{
			GPSTrackerSystem tracker = Safe.Get("map.native.live-tracker-item", (trackers, preferred), ((Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> Trackers, int Index) value) => value.Trackers[value.Index]);
			if ((Object)(object)tracker != (Object)null && TryDrawLiveTracker(tracker, request))
			{
				GC.KeepAlive(trackers);
				return;
			}
		}
		for (int index = 0; index < trackerCount; index++)
		{
			if (index != preferred)
			{
				GPSTrackerSystem tracker2 = Safe.Get("map.native.live-tracker-item", (trackers, index), ((Il2CppSystem.Collections.Generic.List<GPSTrackerSystem> Trackers, int Index) value) => value.Trackers[value.Index]);
				if ((Object)(object)tracker2 != (Object)null && TryDrawLiveTracker(tracker2, request))
				{
					GC.KeepAlive(trackers);
					return;
				}
			}
		}
		GC.KeepAlive(trackers);
		throw new InvalidOperationException("The live GPS surface Image or main texture was unavailable during Repaint.");
	}

	private static bool TryDrawNativeByInstanceId(LiveMapDraw request)
	{
		if (request.TextureInstanceId == 0)
		{
			return false;
		}
		Object nativeObject = Safe.Get("map.native.id-object", request.TextureInstanceId, (int instanceId) => Resources.InstanceIDToObject(instanceId));
		if (nativeObject == null)
		{
			return false;
		}
		Texture texture = Safe.Get("map.native.id-cast", nativeObject, (Object value) => ((Il2CppObjectBase)value).TryCast<Texture>());
		if (texture == null)
		{
			GC.KeepAlive(nativeObject);
			return false;
		}
		if (!Safe.Get("map.native.id-state", texture, (Texture value) => !((Il2CppObjectBase)value).WasCollected, fallback: false))
		{
			GC.KeepAlive(texture);
			GC.KeepAlive(nativeObject);
			return false;
		}
		(int, int) dimensions = Safe.Get("map.native.id-size", texture, (Texture value) => (value.width, value.height), (0, 0));
		if (dimensions.Item1 < request.ExpectedWidth || dimensions.Item2 < request.ExpectedHeight)
		{
			Diag.Once("map.native.id-mismatch", $"Native GPS instance ID resolved to {dimensions.Item1}x{dimensions.Item2}; expected at least {request.ExpectedWidth}x{request.ExpectedHeight}.");
			GC.KeepAlive(texture);
			GC.KeepAlive(nativeObject);
			return false;
		}
		bool flag = Safe.Run("map.native.id-draw", (request, texture), ((LiveMapDraw Request, Texture Texture) value) =>
		{
			DrawNativeSurface(value.Request, value.Texture);
		});
		GC.KeepAlive(texture);
		GC.KeepAlive(nativeObject);
		if (flag)
		{
			Diag.InfoOnce("map.native.id-draw-ready", "Native GPS texture resolved by Unity instance ID and drawn inside one Repaint callback.");
		}
		return flag;
	}

	private static bool TryDrawLiveTracker(GPSTrackerSystem tracker, LiveMapDraw request)
	{
		if (!Safe.Get("map.native.live-tracker-state", tracker, (GPSTrackerSystem value) => !((Il2CppObjectBase)value).WasCollected, fallback: false))
		{
			return false;
		}
		RectTransform surface = Safe.Get("map.native.live-surface", tracker, (GPSTrackerSystem value) => value._surfaceMap);
		if ((Object)(object)surface == (Object)null)
		{
			return false;
		}
		if (!Safe.Get("map.native.live-surface-state", surface, (RectTransform value) => !((Il2CppObjectBase)value).WasCollected, fallback: false))
		{
			return false;
		}
		Image image = Safe.Get("map.native.live-image", surface, (RectTransform value) => ((Component)value).GetComponent<Image>());
		if ((Object)(object)image == (Object)null)
		{
			return false;
		}
		if (!Safe.Get("map.native.live-image-state", image, (Image value) => !((Il2CppObjectBase)value).WasCollected, fallback: false))
		{
			return false;
		}
		Texture texture = Safe.Get("map.native.live-main-texture", image, (Image value) => ((Graphic)value).mainTexture);
		if ((Object)(object)texture == (Object)null)
		{
			return false;
		}
		if (!Safe.Get("map.native.live-texture-state", texture, (Texture value) => !((Il2CppObjectBase)value).WasCollected, fallback: false))
		{
			return false;
		}
		(int, int) dimensions = Safe.Get("map.native.live-texture-size", texture, (Texture value) => (value.width, value.height), (0, 0));
		if (dimensions.Item1 <= 0 || dimensions.Item2 <= 0)
		{
			return false;
		}
		bool result = Safe.Run("map.native.live-draw", (request, texture), ((LiveMapDraw Request, Texture Texture) value) =>
		{
			DrawNativeSurface(value.Request, value.Texture);
		});
		GC.KeepAlive(texture);
		GC.KeepAlive(image);
		GC.KeepAlive(surface);
		GC.KeepAlive(tracker);
		return result;
	}

	private static void DrawNativeSurface(LiveMapDraw request, Texture texture)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Material material = GUI.blendMaterial;
		if (request.DrawBackdrop)
		{
			Graphics.DrawTexture(request.BackdropTarget, texture, request.BackdropUv, 0, 0, 0, 0, OverviewBackdropTint, material, 0);
		}
		Graphics.DrawTexture(request.Target, texture, request.Uv, 0, 0, 0, 0, GUI.color, material, 0);
		GC.KeepAlive(material);
		GC.KeepAlive(texture);
	}

	private static void ProjectMarkers(Rect mapRect, MapViewport viewport)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_projected.Clear();
		if (_hasLocal)
		{
			AddProjection(mapRect, _local, -1, viewport.SpanU, viewport.SpanV);
		}
		for (int index = 0; index < _markers.Count; index++)
		{
			AddProjection(mapRect, _markers[index], index, viewport.SpanU, viewport.SpanV);
		}
	}

	private static void AddProjection(Rect mapRect, Marker marker, int markerIndex, float spanU, float spanV)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (!MapRules.TryProject(marker.Position.x, marker.Position.z, _minX, _maxX, _minZ, _maxZ, _centerU, _centerV, spanU, spanV, out var u, out var v))
		{
			return;
		}
		float x = mapRect.x + u * mapRect.width;
		float y = mapRect.y + (1f - v) * mapRect.height;
		for (int index = 0; index < _projected.Count; index++)
		{
			Projected current = _projected[index];
			if (MapRules.IsNear(x, y, current.X, current.Y, 22f))
			{
				current.Count++;
				_projected[index] = current;
				return;
			}
		}
		_projected.Add(new Projected
		{
			MarkerIndex = markerIndex,
			X = x,
			Y = y,
			Count = 1
		});
	}

	private static void DrawMarkers(Rect mapRect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		if ((int)Event.current.type != 7)
		{
			return;
		}
		_labelRects.Clear();
		for (int index = 0; index < _projected.Count; index++)
		{
			Projected point = _projected[index];
			Marker marker = ((point.MarkerIndex < 0) ? _local : _markers[point.MarkerIndex]);
			float size = ((marker.Kind == MapMarkerKind.LocalPlayer) ? 24f : 20f);
			Rect halo = new Rect(point.X - size * 0.5f - 2f, point.Y - size * 0.5f - 2f, size + 4f, size + 4f);
			Color previous = GUI.color;
			bool nativeIconAlive = IsTextureAlive(marker.NativeIcon);
			bool drewSilhouette = false;
			if (IsTextureAlive(marker.NativeOutline))
			{
				GUI.color = Visible(marker.OutlineColor, Shadow);
				drewSilhouette = TryDrawMarkerTexture(halo, marker.NativeOutline, "map.marker.native-outline");
			}
			if (!drewSilhouette)
			{
				GUI.color = Shadow;
				drewSilhouette = (nativeIconAlive ? TryDrawMarkerTexture(halo, marker.NativeIcon, "map.marker.native-silhouette") : DrawProceduralMarker(halo, marker.Kind));
			}
			GUI.color = Visible(marker.IconColor, Color.white);
			Rect iconRect = new Rect(point.X - size * 0.5f, point.Y - size * 0.5f, size, size);
			if ((!nativeIconAlive || !TryDrawMarkerTexture(iconRect, marker.NativeIcon, "map.marker.native-icon")) && !DrawProceduralMarker(iconRect, marker.Kind))
			{
				DrawFallbackMarker(iconRect, Visible(marker.IconColor, Theme.IconSteel));
			}
			GUI.color = previous;
			if (point.Count > 1)
			{
				int clusterIndex = Mathf.Min(point.Count, 10);
				Rect badge = new Rect(point.X + 6f, point.Y - 15f, 18f, 18f);
				GUI.color = Theme.RustBright;
				if (IsTextureAlive(_clusterBadgeTexture))
				{
					TryDrawMarkerTexture(badge, _clusterBadgeTexture, "map.marker.cluster-badge");
				}
				GUI.color = previous;
				GUI.Label(badge, ClusterText[clusterIndex], _cluster);
			}
		}
		for (int i = 0; i < _projected.Count; i++)
		{
			Projected point2 = _projected[i];
			Marker marker2 = ((point2.MarkerIndex < 0) ? _local : _markers[point2.MarkerIndex]);
			DrawMarkerLabel(mapRect, point2, marker2);
		}
	}

	private static void DrawMarkerLabel(Rect mapRect, Projected point, Marker marker)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		string caption = ((point.Count > 1) ? ClusterCaptionText[Mathf.Min(point.Count, 10)] : marker.Label);
		if (string.IsNullOrWhiteSpace(caption))
		{
			return;
		}
		float width = Mathf.Clamp(12f + (float)caption.Length * 5.4f, 48f, 136f);
		float rightOffset = ((point.Count > 1) ? 27f : 14f);
		Rect label = new Rect(point.X + rightOffset, point.Y - 8f, width, 16f);
		if (!IsLabelPlacementOpen(mapRect, label, point))
		{
			label = new Rect(point.X - width - 14f, point.Y - 8f, width, 16f);
			if (!IsLabelPlacementOpen(mapRect, label, point))
			{
				label = new Rect(point.X - width * 0.5f, point.Y - 27f, width, 16f);
				if (!IsLabelPlacementOpen(mapRect, label, point))
				{
					label = new Rect(point.X - width * 0.5f, point.Y + 12f, width, 16f);
					if (!IsLabelPlacementOpen(mapRect, label, point))
					{
						return;
					}
				}
			}
		}
		Theme.Fill(label, LabelDeck);
		Theme.Fill(new Rect(label.x, label.y + 3f, 1f, label.height - 6f), Theme.Olive);
		GUI.Label(new Rect(label.x + 4f, label.y, label.width - 6f, label.height), caption, _markerLabel);
		_labelRects.Add(label);
	}

	private static bool IsLabelPlacementOpen(Rect mapRect, Rect candidate, Projected owner)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		Rect bounds = new Rect(mapRect.x + 2f, mapRect.y + 2f, Mathf.Max(0f, mapRect.width - 4f), Mathf.Max(0f, mapRect.height - 4f));
		if (!bounds.Contains(candidate.min) || !bounds.Contains(candidate.max))
		{
			return false;
		}
		for (int index = 0; index < _labelRects.Count; index++)
		{
			Rect val = _labelRects[index];
			if (val.Overlaps(candidate))
			{
				return false;
			}
		}
		for (int i = 0; i < _projected.Count; i++)
		{
			Projected point = _projected[i];
			if (point.MarkerIndex != owner.MarkerIndex || !(Mathf.Abs(point.X - owner.X) < 0.1f) || !(Mathf.Abs(point.Y - owner.Y) < 0.1f))
			{
				Rect symbol = new Rect(point.X - 11f, point.Y - 11f, 22f, 22f);
				if (symbol.Overlaps(candidate))
				{
					return false;
				}
			}
		}
		return true;
	}

	private static bool TryDrawMarkerTexture(Rect rect, Texture2D texture, string site)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Safe.Run(site, (rect, texture), ((Rect Rect, Texture2D Texture) value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			GUI.DrawTexture(value.Rect, (Texture)(object)value.Texture, (ScaleMode)2, true);
		});
	}

	private static bool DrawProceduralMarker(Rect rect, MapMarkerKind kind)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		Texture2D texture = kind switch
		{
			MapMarkerKind.Bunker => _bunkerIconTexture, 
			MapMarkerKind.SavedSpot => _savedIconTexture, 
			MapMarkerKind.LocalPlayer => _localIconTexture, 
			_ => _gpsIconTexture, 
		};
		if (!IsTextureAlive(texture))
		{
			return false;
		}
		return Safe.Run("map.marker.procedural", (rect, texture), ((Rect Rect, Texture2D Texture) value) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			GUI.DrawTexture(value.Rect, (Texture)(object)value.Texture, (ScaleMode)2, true);
		});
	}

	private static void DrawFallbackMarker(Rect rect, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Theme.Fill(new Rect(rect.center.x - 4f, rect.center.y - 0.5f, 8f, 1f), color);
		Theme.Fill(new Rect(rect.center.x - 0.5f, rect.center.y - 4f, 1f, 8f), color);
	}

	private static void DrawHeader(Rect rect, Rect available)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		Theme.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), Theme.Line);
		GUI.Label(new Rect(rect.x, rect.y + 1f, 105f, 20f), "SURFACE GPS", _title);
		Theme.Fill(new Rect(rect.x + 105f, rect.y + 8f, 5f, 5f), HasNativeMapTexture() ? Theme.Live : Theme.RustBright);
		GUI.Label(new Rect(rect.x + 117f, rect.y + 1f, Mathf.Max(50f, rect.width - 270f), 20f), _source, _status);
		float num = rect.xMax - 90f;
		GUI.Label(new Rect(num - 53f, rect.y + 1f, 48f, 20f), _zoomText, _status);
		float targetAspect = TargetAspect(available);
		if (GUI.Button(new Rect(num, rect.y + 1f, 22f, 20f), "-", Theme.Btn))
		{
			ApplyZoom(MapRules.AdjustZoom(_zoom, 1f, _nativeAspect, targetAspect), available.center, available);
		}
		if (GUI.Button(new Rect(num + 25f, rect.y + 1f, 22f, 20f), "+", Theme.Btn))
		{
			ApplyZoom(MapRules.AdjustZoom(_zoom, -1f, _nativeAspect, targetAspect), available.center, available);
		}
		if (GUI.Button(new Rect(num + 50f, rect.y + 1f, 40f, 20f), "FIT", Theme.Btn))
		{
			FitViewport(available);
		}
	}

	private static void DrawFooter(Rect rect)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		Theme.Fill(new Rect(rect.x, rect.y, rect.width, 1f), Theme.Line);
		if (_hoverKey == int.MinValue)
		{
			GUI.Label(new Rect(rect.x, rect.y + 3f, rect.width * 0.36f, 17f), _summary, _status);
			GUI.Label(new Rect(rect.x + rect.width * 0.36f, rect.y + 3f, rect.width * 0.64f, 17f), "CLICK TRAVEL   ·   HOLD + DRAG PAN   ·   WHEEL ZOOM", _right);
		}
		else
		{
			GUI.Label(new Rect(rect.x, rect.y + 3f, rect.width * 0.45f, 17f), _hoverTitle, _title);
			GUI.Label(new Rect(rect.x + rect.width * 0.45f, rect.y + 3f, rect.width * 0.55f, 17f), _hoverDetail, _right);
		}
	}

	private static void DrawWaiting(Rect rect)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		bool linkLost = _source.Contains("LINK LOST", StringComparison.Ordinal) || _source.Contains("RECONNECT", StringComparison.Ordinal) || _source.Contains("RECOVER", StringComparison.Ordinal);
		Theme.Fill(rect, Theme.FieldBg);
		Theme.Box(rect, Theme.Dead);
		GUI.Label(new Rect(rect.x + 20f, rect.center.y - 26f, rect.width - 40f, 22f), linkLost ? "NATIVE MAP LINK LOST" : "GPS MAP NOT INITIALIZED", _title);
		GUI.Label(new Rect(rect.x + 20f, rect.center.y - 2f, rect.width - 40f, 40f), linkLost ? "REACQUIRING GAME GPS FEED  ·  KEEP GPS EQUIPPED" : "LOAD SAVE  ·  EQUIP GPS ONCE TO LINK SURFACE FEED", _status);
	}

	private static void DrawFurniture(Rect rect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		if ((int)Event.current.type == 7)
		{
			Theme.Fill(new Rect(rect.x + 9f, rect.y + 8f, 1f, 18f), Theme.Paper);
			Theme.Fill(new Rect(rect.x + 5f, rect.y + 12f, 9f, 1f), Theme.Paper);
			GUI.Label(new Rect(rect.x + 17f, rect.y + 4f, 24f, 18f), "N", _status);
			Color guide = new Color(Theme.Paper.r, Theme.Paper.g, Theme.Paper.b, 0.28f);
			Theme.Fill(new Rect(rect.center.x - 6f, rect.center.y, 12f, 1f), guide);
			Theme.Fill(new Rect(rect.center.x, rect.center.y - 6f, 1f, 12f), guide);
		}
	}

	private static void UpdateHover(Rect mapRect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mouse = Event.current.mousePosition;
		int key = int.MinValue;
		int clusterCount = 0;
		float nearest = 15f;
		for (int index = 0; index < _projected.Count; index++)
		{
			Projected point = _projected[index];
			float distance = Vector2.Distance(mouse, new Vector2(point.X, point.Y));
			if (!(distance >= nearest))
			{
				nearest = distance;
				key = point.MarkerIndex;
				clusterCount = point.Count;
			}
		}
		if (!mapRect.Contains(mouse))
		{
			key = int.MinValue;
			clusterCount = 0;
		}
		if (key != _hoverKey || clusterCount != _hoverClusterCount)
		{
			_hoverKey = key;
			_hoverClusterCount = clusterCount;
			if (key == int.MinValue)
			{
				_hoverTitle = (_hoverDetail = string.Empty);
				return;
			}
			if (clusterCount > 1)
			{
				_hoverTitle = ClusterCaptionText[Mathf.Min(clusterCount, 10)];
				_hoverDetail = "CLUSTER · CLICK TO ZOOM FOR INDIVIDUAL NAMES";
				return;
			}
			Marker marker = ((key < 0) ? _local : _markers[key]);
			_hoverTitle = marker.Label;
			_hoverDetail = $"{MapRules.CategoryLabel(marker.Kind)} · X {marker.Position.x:0} · Z {marker.Position.z:0}";
		}
	}

	private static void HandleInput(Rect mapRect, Rect available, MapViewport viewport)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		Event current = Event.current;
		if (current == null)
		{
			return;
		}
		int controlId = GUIUtility.GetControlID((FocusType)2);
		if (_panArmed && GUIUtility.hotControl != _panControlId)
		{
			CancelGesture();
		}
		if ((int)current.type == 11 || (int)current.type == 21)
		{
			CancelGesture();
		}
		else if ((int)current.type == 6 && mapRect.Contains(current.mousePosition))
		{
			CancelGesture();
			ApplyZoom(MapRules.AdjustZoom(_zoom, current.delta.y, _nativeAspect, TargetAspect(available)), current.mousePosition, available);
			current.Use();
		}
		else if ((int)current.type == 0 && (current.button == 0 || current.button == 1) && mapRect.Contains(current.mousePosition))
		{
			CancelGesture();
			_panArmed = true;
			_panning = false;
			_panButton = current.button;
			_panControlId = controlId;
			_panStart = current.mousePosition;
			_panCenterU = _centerU;
			_panCenterV = _centerV;
			GUIUtility.hotControl = controlId;
			current.Use();
		}
		else if ((int)current.type == 3 && _panArmed && current.button == _panButton)
		{
			if (!_panning)
			{
				_panning = MapRules.ExceedsPanThreshold(_panStart.x, _panStart.y, current.mousePosition.x, current.mousePosition.y);
			}
			if (_panning)
			{
				Vector2 delta = current.mousePosition - _panStart;
				MapRules.PanCenter(_panCenterU, _panCenterV, delta.x, delta.y, mapRect.width, mapRect.height, viewport.SpanU, viewport.SpanV, out _centerU, out _centerV);
			}
			current.Use();
		}
		else if ((int)current.type == 1 && _panArmed)
		{
			bool num = current.button == _panButton && _panButton == 0 && !_panning && mapRect.Contains(current.mousePosition);
			Vector2 mouse = current.mousePosition;
			CancelGesture();
			current.Use();
			if (num)
			{
				ActivateMapClick(mouse, mapRect, available, viewport);
			}
		}
	}

	private static void ActivateMapClick(Vector2 mouse, Rect mapRect, Rect available, MapViewport viewport)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		if (ActivateMarker(mouse, mapRect, available) || !MapRules.TryNormalizePoint(mouse.x, mouse.y, mapRect.x, mapRect.y, mapRect.width, mapRect.height, out var screenU, out var screenY))
		{
			return;
		}
		float screenV = 1f - screenY;
		if (MapRules.TryUnproject(screenU, screenV, _minX, _maxX, _minZ, _maxZ, _centerU, _centerV, viewport.SpanU, viewport.SpanV, out var worldX, out var worldZ))
		{
			float height = GroundHeight(worldX, worldZ);
			if (!Finite(height))
			{
				Features.LastResult = "No terrain at selected map point";
			}
			else
			{
				Goto(new Vector3(worldX, height + 1f, worldZ));
			}
		}
	}

	private static bool ActivateMarker(Vector2 mouse, Rect mapRect, Rect available)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		int selectedProjection = -1;
		float nearest = 15f;
		for (int index = 0; index < _projected.Count; index++)
		{
			Projected point = _projected[index];
			float distance = Vector2.Distance(mouse, new Vector2(point.X, point.Y));
			if (!(distance >= nearest))
			{
				nearest = distance;
				selectedProjection = index;
			}
		}
		if (selectedProjection < 0)
		{
			return false;
		}
		Projected selectedPoint = _projected[selectedProjection];
		if (selectedPoint.Count > 1)
		{
			ApplyZoom(_zoom + 1f, new Vector2(selectedPoint.X, selectedPoint.Y), available);
			Features.LastResult = "Zoomed into clustered locations";
			return true;
		}
		int selected = selectedPoint.MarkerIndex;
		if (selected < 0)
		{
			Features.LastResult = "Already at your current position";
			return true;
		}
		Marker marker = _markers[selected];
		if (marker.TeleportIndex >= 0)
		{
			Teleport.TeleportTo(marker.TeleportIndex);
		}
		else
		{
			Goto(marker.Position);
		}
		return true;
	}

	private static float GroundHeight(float x, float z)
	{
		return Safe.Get("map.ground-height", (x, z), ((float X, float Z) point) =>
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			RaycastHit val = default;
			return (!Physics.Raycast(new Ray(new Vector3(point.X, 5000f, point.Z), Vector3.down), out val, 12000f)) ? float.NaN : val.point.y;
		}, float.NaN, Noise.Warn);
	}

	private static void Goto(Vector3 position)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		LocalPlayer localPlayer = Safe.Get("map.local-player", () => LocalPlayer._instance);
		if ((Object)(object)localPlayer == (Object)null)
		{
			Features.LastResult = "Needs a loaded world";
			return;
		}
		Features.LastResult = (Safe.Run("map.goto", (localPlayer, position), ((LocalPlayer Player, Vector3 Position) state) =>
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			state.Player.Goto(state.Position);
		}, Noise.Warn) ? $"Teleported ({position.x:0}, {position.z:0})" : "Teleport failed");
	}

	private static void ApplyZoom(float next, Vector2 screenAnchor, Rect available)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		CancelGesture();
		MapViewport oldViewport = GetViewport(available);
		MapViewport newViewport = MapRules.CalculateViewport(_nativeAspect, TargetAspect(available), next);
		Rect oldRect = ViewportRect(available, oldViewport);
		Rect newRect = ViewportRect(available, newViewport);
		NormalizeClamped(screenAnchor, oldRect, out var oldAnchorU, out var oldAnchorY);
		NormalizeClamped(screenAnchor, newRect, out var newAnchorU, out var newAnchorY);
		MapRules.ZoomBetweenViews(oldViewport.SpanU, oldViewport.SpanV, newViewport.SpanU, newViewport.SpanV, oldAnchorU, 1f - oldAnchorY, newAnchorU, 1f - newAnchorY, ref _centerU, ref _centerV);
		_zoom = newViewport.Zoom;
		UpdateZoomText();
	}

	private static MapViewport GetViewport(Rect available)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		MapViewport viewport = MapRules.CalculateViewport(_nativeAspect, TargetAspect(available), _zoom);
		_zoom = viewport.Zoom;
		_centerU = MapRules.ClampCenterToSpan(_centerU, viewport.SpanU);
		_centerV = MapRules.ClampCenterToSpan(_centerV, viewport.SpanV);
		UpdateZoomText();
		return viewport;
	}

	private static float TargetAspect(Rect available)
	{
		return Mathf.Max(1f, available.width) / Mathf.Max(1f, available.height);
	}

	private static Rect ViewportRect(Rect available, MapViewport viewport)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		return new Rect(available.x + available.width * viewport.DestinationX, available.y + available.height * viewport.DestinationY, available.width * viewport.DestinationWidth, available.height * viewport.DestinationHeight);
	}

	private static void NormalizeClamped(Vector2 point, Rect rect, out float normalizedX, out float normalizedY)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		normalizedX = Mathf.Clamp01((point.x - rect.x) / Mathf.Max(1f, rect.width));
		normalizedY = Mathf.Clamp01((point.y - rect.y) / Mathf.Max(1f, rect.height));
	}

	private static void FitViewport(Rect available)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		CancelGesture();
		_zoom = MapRules.CalculateFitZoom(_nativeAspect, TargetAspect(available));
		_centerU = (_centerV = 0.5f);
		UpdateZoomText();
	}

	private static void UpdateZoomText()
	{
		_zoomText = _zoom.ToString((_zoom < 0.995f) ? "0.00" : "0.0") + "X";
	}

	private static void ResetViewport()
	{
		CancelGesture();
		_zoom = 1f;
		_centerU = (_centerV = 0.5f);
		_zoomText = "1.0X";
	}

	private static void CancelGesture()
	{
		if (_panControlId != 0 && GUIUtility.hotControl == _panControlId)
		{
			GUIUtility.hotControl = 0;
		}
		_panArmed = false;
		_panning = false;
		_panButton = -1;
		_panControlId = 0;
	}

	private static void EnsurePresentation()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected Obj, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected Obj, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected Obj, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected Obj, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected Obj, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		if (_title == null)
		{
			_title = new GUIStyle(Theme.Build)
			{
				alignment = (TextAnchor)3,
				fontStyle = (FontStyle)1
			};
			_title.normal.textColor = Theme.Bone;
			_status = new GUIStyle(Theme.Build)
			{
				alignment = (TextAnchor)3,
				clipping = (TextClipping)1
			};
			_status.normal.textColor = Theme.Ash;
			_right = new GUIStyle(Theme.Build)
			{
				alignment = (TextAnchor)5,
				clipping = (TextClipping)1
			};
			_right.normal.textColor = Theme.Olive;
			_cluster = new GUIStyle(Theme.Build)
			{
				alignment = (TextAnchor)4,
				fontStyle = (FontStyle)1
			};
			_cluster.normal.textColor = Theme.Bone;
			_markerLabel = new GUIStyle(Theme.Build)
			{
				alignment = (TextAnchor)3,
				clipping = (TextClipping)1,
				fontSize = 10
			};
			_markerLabel.normal.textColor = Theme.Bone;
			Texture2D gpsIcon = MakeIcon(MapMarkerKind.Locator);
			Texture2D bunkerIcon = MakeIcon(MapMarkerKind.Bunker);
			Texture2D savedIcon = MakeIcon(MapMarkerKind.SavedSpot);
			Texture2D localIcon = MakeIcon(MapMarkerKind.LocalPlayer);
			Texture2D clusterBadge = MakeClusterBadge();
			bool flag = RetainTexture("Cyberfox1337x.SonsOfTheDead.MapTexture.Locator", gpsIcon, ref _gpsIconOwner, ref _gpsIconTexture);
			bool bunkerRetained = RetainTexture("Cyberfox1337x.SonsOfTheDead.MapTexture.Bunker", bunkerIcon, ref _bunkerIconOwner, ref _bunkerIconTexture);
			bool savedRetained = RetainTexture("Cyberfox1337x.SonsOfTheDead.MapTexture.Saved", savedIcon, ref _savedIconOwner, ref _savedIconTexture);
			bool localRetained = RetainTexture("Cyberfox1337x.SonsOfTheDead.MapTexture.Local", localIcon, ref _localIconOwner, ref _localIconTexture);
			bool clusterRetained = RetainTexture("Cyberfox1337x.SonsOfTheDead.MapTexture.Cluster", clusterBadge, ref _clusterBadgeOwner, ref _clusterBadgeTexture);
			if (!flag || !bunkerRetained || !savedRetained || !localRetained || !clusterRetained)
			{
				Diag.Once("map.marker-owner.unavailable", "One or more procedural marker textures could not be attached to Unity owners.");
			}
		}
	}

	private static Texture2D MakeIcon(MapMarkerKind kind)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected Obj, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		Color32[] pixels = new Color32[1024];
		Color32 white = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		for (int y = 1; y < 31; y++)
		{
			for (int x = 1; x < 31; x++)
			{
				float dx = (float)x + 0.5f - 16f;
				float dy = (float)y + 0.5f - 16f;
				if (kind switch
				{
					MapMarkerKind.LocalPlayer => dy >= -11f && dy <= 12f && Mathf.Abs(dx) <= (12f - dy) * 0.43f && (!(dy < -2f) || !(Mathf.Abs(dx) < (0f - dy - 2f) * 0.22f)), 
					MapMarkerKind.Bunker => ((dy >= -9f && dy <= 5f && Mathf.Abs(dx) <= 11f) || (dy > 5f && dy <= 10f && Mathf.Abs(dx) <= 12f - (dy - 5f) * 0.35f)) && (!(dy <= 1f) || !(Mathf.Abs(dx) <= 3f)), 
					MapMarkerKind.SavedSpot => (dx >= -9f && dx <= -6f && dy >= -11f && dy <= 12f) || (dy >= 1f && dy <= 11f && dx > -6f && dx <= 9f - (dy - 1f) * 0.35f), 
					_ => (dx * dx + (dy - 4f) * (dy - 4f) <= 76f && dx * dx + (dy - 4f) * (dy - 4f) >= 19f) || (dy >= -12f && dy <= 0f && Mathf.Abs(dx) <= (dy + 12f) * 0.36f), 
				})
				{
					pixels[y * 32 + x] = white;
				}
			}
		}
		Texture2D val = new Texture2D(32, 32, (TextureFormat)4, false)
		{
			filterMode = (FilterMode)1,
			wrapMode = (TextureWrapMode)1,
			hideFlags = (HideFlags)52
		};
		val.SetPixels32(new Il2CppStructArray<Color32>(pixels));
		val.Apply(false, true);
		return val;
	}

	private static Texture2D MakeClusterBadge()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected Obj, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		Color32[] pixels = new Color32[576];
		Color32 white = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		float center = 12f;
		float num = center - 1.5f;
		float radiusSquared = num * num;
		for (int y = 0; y < 24; y++)
		{
			for (int x = 0; x < 24; x++)
			{
				float num2 = (float)x + 0.5f - center;
				float dy = (float)y + 0.5f - center;
				if (num2 * num2 + dy * dy <= radiusSquared)
				{
					pixels[y * 24 + x] = white;
				}
			}
		}
		Texture2D val = new Texture2D(24, 24, (TextureFormat)4, false)
		{
			filterMode = (FilterMode)1,
			wrapMode = (TextureWrapMode)1,
			hideFlags = (HideFlags)52
		};
		val.SetPixels32(new Il2CppStructArray<Color32>(pixels));
		val.Apply(false, true);
		return val;
	}

	private static Color Visible(Color color, Color fallback)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!(color.a <= 0.01f))
		{
			return color;
		}
		return fallback;
	}

	private static bool Finite(float value)
	{
		if (!float.IsNaN(value))
		{
			return !float.IsInfinity(value);
		}
		return false;
	}

	private static bool IsTextureAlive(Texture2D texture)
	{
		if (texture == null)
		{
			return false;
		}
		return Safe.Get("map.texture.alive", texture, (Texture2D value) => !((Il2CppObjectBase)value).WasCollected && ((Texture)value).width > 0 && ((Texture)value).height > 0, fallback: false);
	}

	private static void InvalidateNativeMap(string status)
	{
		ReleaseNativeMap();
		_source = status;
		_nextResolve = Safe.Get("map.native.retry-clock", () => Time.unscaledTime, 0f) + 2f;
	}

	private static void ReleaseNativeMap()
	{
		CancelGesture();
		_nativeMapAttached = false;
		_nativeTrackerIndex = -1;
		_nativeTextureInstanceId = 0;
		_nativeExpectedWidth = 0;
		_nativeExpectedHeight = 0;
	}

	private static void ClearWorld()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		ReleaseNativeMap();
		_baseUv = new Rect(0f, 0f, 1f, 1f);
		_nativeAspect = 1f;
		_markers.Clear();
		_projected.Clear();
		_hasLocal = false;
		_hoverKey = int.MinValue;
		_hoverClusterCount = 0;
		_hoverTitle = (_hoverDetail = string.Empty);
		_source = "GPS MAP NOT INITIALIZED";
		_summary = "0 GPS · 0 BUNKERS · 0 SAVED";
		_nextResolve = (_nextSnapshot = 0f);
		_minX = -2100f;
		_maxX = 2100f;
		_minZ = -2100f;
		_maxZ = 2100f;
		ResetViewport();
	}
}
