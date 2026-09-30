using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Sons.Input;
using Sons.Items.Core;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("menu shell", Bound = "IL2CPP-injected MonoBehaviour; Unity matches Update/OnGUI by name")]
public class MenuWindow(IntPtr ptr) : MonoBehaviour(ptr)
{
	private readonly struct Category
	{
		internal string Name { get; }

		internal string Hint { get; }

		internal Category(string name, string hint)
		{
			Name = name;
			Hint = hint;
		}
	}

	private const float RAIL = 150f;

	private const float HEAD = 58f;

	private const float FOOT = 24f;

	private const float PAD = 16f;

	private const float ROW = 26f;

	private const float SCROLLW = 18f;

	private const float RowGap = 6f;

	private const float RAIL_ROW = 26f;

	private const float RAIL_ROW_MIN = 18f;

	private const float RAIL_TOP_PAD = 8f;

	private const float RailLabelInset = 12f;

	private const float ContentTopPad = 10f;

	private const float MapShare = 0.76f;

	private const float MinimumMapContentHeight = 120f;

	private static readonly Category[] Categories = new Category[14]
	{
		new Category("PLAYER", "Survival · Needs · Status · Cheats"),
		new Category("MOVEMENT", "On Foot · Speed · Flight"),
		new Category("MAP", "Surface GPS · Markers · Click Travel"),
		new Category("TELEPORT", "Named Locations · Saved Spots"),
		new Category("COMBAT", "Weapons · Tuning · Enemies · Area"),
		new Category("SPAWN", "Items · Characters · Animals · Props"),
		new Category("GEAR", "Glider · Knight V · Light · Rebreather"),
		new Category("WORLD", "Build · Season · Time · Weather"),
		new Category("GAME SETUP", "World rules · Survival rules"),
		new Category("VISION", "ESP · Item ESP · Radar · Overlays"),
		new Category("ONLINE", "Roster · Session · Network"),
		new Category("CHEATS", "Engine · Achievements · Armour"),
		new Category("CONSOLE", "Search · Run · Recent"),
		new Category("SETTINGS", "Menu size · Hotkey · Appearance")
	};

	private const int TabMapIndex = 2;

	private const int TabTeleportIndex = 3;

	private const float RosterRefreshInterval = 2f;

	private static readonly GUIContent _chipGc = new GUIContent();

	private int _tab;

	private bool _open;

	private Vector2 _scroll;

	private Rect _panel;

	private MenuScaleLayout _scaleLayout;

	private bool _centerOnNextLayout;

	private Vector2 _pos;

	private bool _placed;

	private bool _dragging;

	private bool _nudged;

	private Vector2 _grabOffset;

	private CursorLockMode _cursorLockBeforeOpen;

	private bool _cursorVisibleBeforeOpen;

	private bool _hasCursorSnapshot;

	private bool _ownsMenuInputState;

	private bool _rosterDirty = true;

	private float _nextRosterRefresh;

	private string _steamKey = "";

	private bool _rebindArmed;

	private string _rebindStatus;

	private bool _rebindOk;

	private int _lastToggleFrame = -1;

	private bool _locOpen;

	private int _locSel = -1;

	private Vector2 _locScroll;

	private float _contentViewportH;

	private float _contentViewportTop;

	private Rect _locBoxInPane;

	private static GUIStyle _statusInWorld;

	private static GUIStyle _statusNoWorld;

	private static GUIStyle _statusLive;

	private static GUIStyle _tagAdmin;

	private static GUIStyle _tagBoss;

	private static GUIStyle _keyOk;

	private static GUIStyle _keyBad;

	private string _itemId = "392";

	private string _itemQty = "1";

	private string _search = "";

	private string _mutQty = "1";

	private string _aniQty = "1";

	private string _compQty = "1";

	private int _selMutant = -1;

	private int _selAnimal = -1;

	private int _selCompanion = -1;

	private string _spotName = "";

	private string _keyStatus;

	private bool _keyStatusOk;

	private static Texture2D _rosterHeartIcon;

	private string _worldDropdown = string.Empty;

	private int _timeChoice = 24;

	private int _lightingChoice;

	private int _dayChoice;

	private int _jumpChoice;

	private int _timeSpeedChoice = 3;

	private int _windChoice = 5;

	private int _cloudChoice = 5;

	private int _forecastChoice;

	private int _difficultyChoice;

	private int _gameModeChoice;

	private int _worldSeasonChoice = 1;

	private string _cutsceneName = "";

	private string _cmdSearch = "";

	private string _cmdLine = "";

	private readonly List<string> _history = new List<string>();

	private static readonly (string Key, string Label, bool Boss)[] Companions = new (string, string, bool)[6]
	{
		("robby", "Kelvin", false),
		("virginia", "Virginia", false),
		("timmy", "Timmy", false),
		("helicopter", "Helicopter", false),
		("mrpuffton", "Mr. Puffton", true),
		("mrspuffton", "Mrs. Puffton", true)
	};

	private static readonly (string Key, string Label, bool Boss)[] Mutants = new (string, string, bool)[45]
	{
		("fingers", "Fingers", false),
		("twins", "Twins", false),
		("creepyvirginia", "Creepy Virginia", false),
		("armsy", "Armsy", true),
		("demonboss", "Demon Boss", true),
		("bossmutant", "Mutant Boss", true),
		("demon", "Demon", false),
		("cannibal goldmask", "Gold Mask", false),
		("cannibal carl", "Carl", false),
		("cannibal billy", "Billy", false),
		("cannibal danny", "Danny", false),
		("cannibal andy", "Andy", false),
		("cannibal greg", "Greg (effigies)", false),
		("cannibal eddy", "Eddy (spears)", false),
		("cannibal lgor", "Igor (charges)", false),
		("cannibalnight", "Night Cannibal", false),
		("painted male", "Painted Cannibal", false),
		("femalecannibal angel", "Angel", false),
		("femalecannibal crystal", "Crystal", false),
		("femalecannibal destiny", "Destiny", false),
		("femalecannibal brandy", "Brandy", false),
		("femalecannibal Elise", "Elise", false),
		("male", "Male Cannibal", false),
		("female", "Female Cannibal", false),
		("family", "Assorted Cannibals", false),
		("heavy", "Heavy Cannibal", false),
		("fat", "Red Fat Cannibal", false),
		("malefat", "Fat Male", false),
		("femalefat", "Fat Female", false),
		("muddymale", "Muddy Male", false),
		("muddyfemale", "Muddy Female", false),
		("muddy", "Muddy (grass pile)", false),
		("fire", "Fire Cannibal", false),
		("holey", "Holey", false),
		("legsy", "Legsy", false),
		("slug", "Sluggy", false),
		("john2", "John 2.0", false),
		("mrpuffy", "Mr. Puffy", false),
		("misspuffy", "Mrs. Puffy", false),
		("puffybossmale", "Puffy Boss Male", false),
		("puffybossfemale", "Puffy Boss Female", false),
		("spotted male", "Spotted Male", false),
		("spotted female", "Spotted Female", false),
		("baby", "Baby", false),
		("frank", "Frank", false)
	};

	private static readonly (string Key, string Label, bool Hostile)[] Animals = new (string, string, bool)[17]
	{
		("deer", "Deer", false),
		("moose", "Moose", false),
		("rabbit", "Rabbit", false),
		("squirrel", "Squirrel", false),
		("raccoon", "Raccoon", false),
		("shark", "Shark", true),
		("killerwhale", "Orca", true),
		("eagle", "Eagle", false),
		("seagull", "Seagull", false),
		("turtle", "Turtle", false),
		("landturtle", "Tortoise", false),
		("bat", "Bat", false),
		("duck", "Duck", false),
		("gull", "Gull", false),
		("skunk", "Skunk", false),
		("bluebird", "Bluebird", false),
		("hummingbird", "Hummingbird", false)
	};

	private static readonly string[] Seasons = new string[4] { "Spring", "Summer", "Autumn", "Winter" };

	private static Texture2D[] _seasonIcons;

	private static readonly float[] MenuScalePresets = new float[6] { 1f, 1.25f, 1.5f, 2f, 2.5f, 3f };

	private static GUIStyle _worldLockedActionStyle;

	private readonly Dictionary<string, string> _fieldValues = new Dictionary<string, string>();

	private static readonly Dictionary<string, Action<Color32[]>> SectionIcons = new Dictionary<string, Action<Color32[]>>
	{
		["SURVIVAL"] = HeartIcon,
		["NEEDS"] = AppleIcon,
		["STATUS EFFECTS"] = LightningIcon,
		["GAME CHEATS"] = GamepadIcon,
		["ACTIONS"] = PlayIcon,
		["VALUES"] = HashIcon,
		["WEAPONS"] = BladesIcon,
		["COMBAT TUNING"] = SlidersIcon,
		["ENEMIES"] = SkullIcon,
		["AREA EFFECTS"] = BlastIcon,
		["ENEMY TUNING"] = GaugeIcon,
		["CLEANUP"] = BroomIcon,
		["CONSTRUCTION"] = HammerIcon,
		["SEASON"] = LeafIcon,
		["WORLD CHEATS"] = GlobeBoltIcon,
		["TIME & WEATHER"] = ClockIcon,
		["GAME SETUP — WORLD"] = GearIcon,
		["GAME SETUP — SURVIVAL"] = FlameIcon,
		["WORLD ACTIONS"] = GlobeIcon,
		["ENGINE CHEATS"] = CogRustIcon,
		["WATER"] = DropletIcon,
		["ARMOUR"] = ShieldIcon,
		["ACHIEVEMENTS"] = TrophyIcon,
		["WORLD SIMULATION"] = AtomIcon,
		["HANG GLIDER"] = GliderIcon,
		["KNIGHT V"] = BuggyIcon,
		["FLASHLIGHT"] = TorchIcon,
		["REBREATHER"] = MaskIcon,
		["ACTORS"] = PersonIcon,
		["ITEM ESP"] = EyeCrateIcon,
		["RANGE & RADAR"] = RadarIcon,
		["OVERLAYS"] = LayersIcon,
		["ON FOOT"] = BootIcon,
		["GAME MOVEMENT CHEATS"] = SprintIcon,
		["FLIGHT"] = UpdraftIcon,
		["SAVED SPOTS"] = BookmarkIcon,
		["ITEMS"] = CrateIcon,
		["COMPANIONS & STORY"] = CompanionsIcon,
		["MUTANTS & CANNIBALS"] = BiohazardIcon,
		["ANIMALS"] = PawIcon,
		["PREFABS (BOLT)"] = CubeIcon,
		["CUTSCENES"] = ClapperIcon,
		["ROSTER"] = RosterIcon,
		["SESSION"] = LinkIcon,
		["NETWORK DEBUG"] = AntennaIcon,
		["COMMON (WIKI)"] = BookIcon,
		["RUN"] = TerminalIcon,
		["RECENT"] = HistoryIcon,
		["ALL COMMANDS"] = ListIcon,
		["STEAM API KEY"] = KeyIcon,
		["APPEARANCE"] = PaletteIcon,
		["SCROLLING"] = MouseIcon
	};

	private const int IconRes = 48;

	private const int IconS = 16;

	private const int IconSub = 3;

	private const float SecHeaderH = 26f;

	private const float SecIconH = 18f;

	private const float SecIconYOff = 2f;

	private const float SecIconGap = 7f;

	private const float SecLineGap = 10f;

	private static Dictionary<string, Texture2D> _sectionTex;

	private const float TrackW = 36f;

	private const float TrackH = 18f;

	private const float ThumbD = 14f;

	private const float ThumbPad = 2f;

	private static Texture2D _circleTex;

	private static Texture2D _pillTex;

	private static readonly Dictionary<string, float> _toggleAnim = new Dictionary<string, float>();

	private const float ToggleSpeed = 8f;

	private float RailRowHeight => Mathf.Clamp(Mathf.Floor((H - 58f - 24f - 8f) / (float)Categories.Length), 18f, 26f);

	private float W => _scaleLayout.Width;

	private float H => _scaleLayout.Height;

	private static void Cyberfox1337xMapFunction()
	{
	}

	private static bool NeedsRoster(int tab)
	{
		return tab == 10;
	}

	private static GUIStyle StatusStyle(bool inWorld)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected Obj, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (_statusInWorld == null)
		{
			_statusInWorld = new GUIStyle(Theme.Build);
			_statusInWorld.normal.textColor = Theme.Live;
			_statusNoWorld = new GUIStyle(Theme.Build);
			_statusNoWorld.normal.textColor = Theme.Dead;
		}
		if (!inWorld)
		{
			return _statusNoWorld;
		}
		return _statusInWorld;
	}

	private static GUIStyle LiveStyle()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected Obj, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (_statusLive == null)
		{
			_statusLive = new GUIStyle(Theme.StatusText);
			_statusLive.normal.textColor = Theme.Live;
		}
		return _statusLive;
	}

	private static GUIStyle AdminTagStyle()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected Obj, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (_tagAdmin == null)
		{
			_tagAdmin = new GUIStyle(Theme.RowTag);
			_tagAdmin.normal.textColor = Theme.RustBright;
		}
		return _tagAdmin;
	}

	private static GUIStyle BossTagStyle()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected Obj, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (_tagBoss == null)
		{
			_tagBoss = new GUIStyle(Theme.RowTag);
			_tagBoss.normal.textColor = Theme.RustBright;
		}
		return _tagBoss;
	}

	private static GUIStyle KeyStatusStyle(bool ok)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected Obj, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (_keyOk == null)
		{
			_keyOk = new GUIStyle(Theme.Lede);
			_keyOk.normal.textColor = Theme.Live;
			_keyBad = new GUIStyle(Theme.Lede);
			_keyBad.normal.textColor = Theme.RustBright;
		}
		if (!ok)
		{
			return _keyBad;
		}
		return _keyOk;
	}

	private void Update()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Cyberfox1337xMapFunction();
			if (!_rebindArmed && Input.GetKeyDown(Plugin.ToggleKey))
			{
				ToggleMenu();
			}
			Features.MenuOpen = _open;

			if (_open)
			{
				NudgeWithKeys();
			}
			Avatars.Tick();
			Features.Tick();
			Map.Tick(_open && _tab == TabMapIndex);
		}
		catch (Exception ex)
		{
			Diag.Error("Update: " + ex);
		}
	}

	private void ToggleMenu()
	{
		int frame = Time.frameCount;
		if (_lastToggleFrame == frame)
		{
			return;
		}
		_lastToggleFrame = frame;
		_open = !_open;
		Features.MenuOpen = _open;
		if (_open)
		{
			OnOpen();
		}
		else
		{
			OnClose();
		}
	}

	private void OnOpen()
	{
		_cursorLockBeforeOpen = Cursor.lockState;
		_cursorVisibleBeforeOpen = Cursor.visible;
		_hasCursorSnapshot = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SetPlayerFrozen(frozen: true);
		if (string.IsNullOrEmpty(_steamKey))
		{
			_steamKey = Plugin.SteamApiKey;
		}
	}

	private void OnClose()
	{
		_open = false;
		Features.MenuOpen = false;
		CancelRebind();
		_dragging = false;
		try
		{
			SetPlayerFrozen(frozen: false);
		}
		finally
		{
			if (_hasCursorSnapshot)
			{
				Cursor.lockState = _cursorLockBeforeOpen;
				Cursor.visible = _cursorVisibleBeforeOpen;
				_hasCursorSnapshot = false;
			}
		}
		// Saving must not prevent input or cursor restoration.
		Safe.Run("menu.close.save-position", SavePos, Noise.Warn);
	}

	private void OnDisable()
	{
		if (_open || _hasCursorSnapshot || _ownsMenuInputState)
		{
			OnClose();
		}
	}

	private void SetPlayerFrozen(bool frozen)
	{
		try
		{
			if (frozen)
			{
				// Do not clear a Menu state that was already owned by the game.
				if (!InputSystem.GetState(InputState.Menu))
				{
					_ownsMenuInputState = true;
					InputSystem.SetState(InputState.Menu, true);
				}
			}
			else if (_ownsMenuInputState)
			{
				InputSystem.SetState(InputState.Menu, false);
				_ownsMenuInputState = false;
			}
		}
		catch (Exception ex)
		{
			Diag.Warn("input freeze failed: " + ex.Message);
		}
	}

	private void OnGUI()
	{
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Theme.Build_();
			Event toggleEvent = Event.current;
			if (_rebindArmed)
			{
				HandleRebindCapture();
			}
			else if (toggleEvent != null && toggleEvent.type == EventType.KeyDown && toggleEvent.keyCode == Plugin.ToggleKey)
			{
				// Consume the hotkey before any TextField handles it, then drop focus.
				toggleEvent.Use();
				GUIUtility.keyboardControl = 0;
				ToggleMenu();
			}
			int wantRadius = (int)Plugin.CornerRadius;
			if (Theme.RoundR != wantRadius)
			{
				Theme.RoundR = wantRadius;
				Theme.RebuildRoundTextures();
			}
			Esp.Draw();
			ItemEsp.Draw();
			if (!_open)
			{
				Watermark();
				return;
			}
			Cursor.lockState = (CursorLockMode)0;
			Cursor.visible = true;
			RefreshMenuLayout();
			float w = W;
			float h = H;
			float scale = _scaleLayout.Scale;
			float pixelWidth = _scaleLayout.PixelWidth;
			float pixelHeight = _scaleLayout.PixelHeight;
			if (!_placed)
			{
				float sx = Plugin.PosX;
				float sy = Plugin.PosY;
				_pos = ((!MenuScaleRules.IsFinite(sx) || !MenuScaleRules.IsFinite(sy)) ? new Vector2(((float)Screen.width - pixelWidth) * 0.5f, ((float)Screen.height - pixelHeight) * 0.5f) : new Vector2(sx, sy));
				_placed = true;
			}
			ClampMenuPosition();
			HandleDrag(new Rect(_pos.x, _pos.y, pixelWidth, 58f * scale));
			ClampMenuPosition();
			_panel = new Rect(_pos.x / scale, _pos.y / scale, w, h);
			Matrix4x4 previousMatrix = GUI.matrix;
			try
			{
				GUI.matrix = previousMatrix * Matrix4x4.Scale(new Vector3(scale, scale, 1f));
				Color panelColor = Theme.Panel;
				panelColor.a = Plugin.MenuOpacity;
				Theme.RoundedFill(_panel, panelColor);
				Theme.RoundedBox(_panel, Theme.Line);
				DrawHeader();
				DrawRail();
				DrawPane();
				DrawStatus();
			}
			finally
			{
				GUI.matrix = previousMatrix;
			}
		}
		catch (Exception ex)
		{
			Diag.Error("OnGUI: " + ex);
		}
	}

	private void Watermark()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		GUI.Label(new Rect(14f, 10f, 320f, 26f), "SONS OF THE FOREST", Theme.Watermark);
		GUI.Label(new Rect(14f, 32f, 320f, 16f), "[" + ((object)Plugin.ToggleKey/*cast due to constrained. prefix*/).ToString() + "] menu", Theme.Build);
	}

	private void RefreshMenuLayout()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		if (!(_scaleLayout.Scale > 0f) || (int)Event.current.type == 8)
		{
			MenuScaleLayout next = MenuScaleRules.Resolve(Screen.width, Screen.height, Plugin.AutomaticMenuScale, Plugin.MenuScale);
			if (_centerOnNextLayout)
			{
				_pos = new Vector2(((float)Screen.width - next.PixelWidth) * 0.5f, ((float)Screen.height - next.PixelHeight) * 0.5f);
				_placed = true;
				_centerOnNextLayout = false;
			}
			else if (_placed && _scaleLayout.Scale > 0f)
			{
				_pos += new Vector2((_scaleLayout.PixelWidth - next.PixelWidth) * 0.5f, (_scaleLayout.PixelHeight - next.PixelHeight) * 0.5f);
			}
			_scaleLayout = next;
		}
	}

	private void ClampMenuPosition()
	{
		_pos.x = MenuScaleRules.ClampPosition(_pos.x, _scaleLayout.PixelWidth, Screen.width);
		_pos.y = MenuScaleRules.ClampPosition(_pos.y, _scaleLayout.PixelHeight, Screen.height);
	}

	private void ResetMenuScale()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Plugin.AutomaticMenuScale = true;
		Plugin.MenuScale = 1f;
		Plugin.PosX = (Plugin.PosY = float.NaN);
		_centerOnNextLayout = true;
		_dragging = false;
		_scroll = Vector2.zero;
		Plugin.SaveConfig();
	}

	private void NudgeWithKeys()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (!Input.GetKey((KeyCode)304) && !Input.GetKey((KeyCode)303))
		{
			if (_nudged)
			{
				_nudged = false;
				SavePos();
			}
			return;
		}
		bool control = Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305);
		if (control && Input.GetKeyDown((KeyCode)278))
		{
			ResetMenuScale();
			_nudged = false;
			return;
		}
		float step = (control ? 1400f : 450f) * Time.deltaTime;
		Vector2 before = _pos;
		if (Input.GetKey((KeyCode)276))
		{
			_pos.x -= step;
		}
		if (Input.GetKey((KeyCode)275))
		{
			_pos.x += step;
		}
		if (Input.GetKey((KeyCode)273))
		{
			_pos.y -= step;
		}
		if (Input.GetKey((KeyCode)274))
		{
			_pos.y += step;
		}
		if (_pos != before)
		{
			_nudged = true;
		}
		if (Input.GetKeyDown((KeyCode)278))
		{
			ResetPosition();
			_nudged = false;
		}
	}

	private void SavePos()
	{
		Plugin.PosX = _pos.x;
		Plugin.PosY = _pos.y;
	}

	private void HandleDrag(Rect grip)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Event e = Event.current;
		if (e == null)
		{
			return;
		}
		if ((int)e.type == 0 && e.button == 0 && grip.Contains(e.mousePosition))
		{
			if (e.clickCount >= 2)
			{
				ResetPosition();
				e.Use();
			}
			else
			{
				_dragging = true;
				_grabOffset = e.mousePosition - _pos;
				e.Use();
			}
		}
		else if ((int)e.type == 3 && _dragging)
		{
			_pos = e.mousePosition - _grabOffset;
			e.Use();
		}
		else if (_dragging && ((int)e.type == 1 || (int)e.type == 11))
		{
			_dragging = false;
			Plugin.PosX = _pos.x;
			Plugin.PosY = _pos.y;
			if ((int)e.type == 1)
			{
				e.Use();
			}
		}
	}

	private void CancelRebind()
	{
		if (_rebindArmed)
		{
			_rebindArmed = false;
			_rebindOk = false;
			_rebindStatus = null;
		}
	}

	private void HandleRebindCapture()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (!_rebindArmed)
		{
			return;
		}
		Event key = Event.current;
		if (key == null || (int)key.type != 4)
		{
			return;
		}
		KeyCode pressed = key.keyCode;
		key.Use();
		_lastToggleFrame = Time.frameCount;
		if ((int)pressed != 0)
		{
			string reason;
			if ((int)pressed == 27)
			{
				_rebindArmed = false;
				_rebindOk = false;
				_rebindStatus = "Cancelled — still " + Plugin.ToggleKeyGlyph + ".";
			}
			else if (Plugin.TrySetToggleKey(pressed, out reason))
			{
				_rebindArmed = false;
				_rebindOk = true;
				_rebindStatus = "Menu now opens with " + Plugin.ToggleKeyGlyph + ".";
			}
			else
			{
				_rebindOk = false;
				_rebindStatus = reason;
			}
		}
	}

	private void ResetPosition()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		MenuScaleLayout layout = ((_scaleLayout.Scale > 0f) ? _scaleLayout : MenuScaleRules.Resolve(Screen.width, Screen.height, Plugin.AutomaticMenuScale, Plugin.MenuScale));
		_pos = new Vector2(((float)Screen.width - layout.PixelWidth) * 0.5f, ((float)Screen.height - layout.PixelHeight) * 0.5f);
		_dragging = false;
		Plugin.PosX = _pos.x;
		Plugin.PosY = _pos.y;
	}

	private void DrawHeader()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected Obj, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected Obj, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected Obj, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected Obj, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected Obj, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		Rect h = new Rect(_panel.x, _panel.y, _panel.width, 58f);
		Color gripCol = (_dragging ? Theme.RustBright : Theme.Line);
		for (int i = 0; i < 3; i++)
		{
			Theme.Fill(new Rect(h.center.x - 14f, h.y + 4f + (float)i * 3f, 28f, 1f), gripCol);
		}
		GUIContent markText = new GUIContent("SONS OF THE FOREST");
		GUIContent buildText = new GUIContent("SOTF UTIL  ·  v1.2.1  ·  GAME 0.11.3");
		float markW = Theme.Wordmark.CalcSize(markText).x;
		float buildW = Theme.Build.CalcSize(buildText).x;
		float titleW = Mathf.Max(markW, buildW);
		GUI.Label(new Rect(h.x + 16f, h.y + 10f, markW, 30f), markText, Theme.Wordmark);
		GUI.Label(new Rect(h.x + 16f, h.y + 38f, buildW, 14f), buildText, Theme.Build);
		float num = h.xMax - 16f;
		GUIContent togText = new GUIContent("TOGGLE");
		float togW = Theme.Build.CalcSize(togText).x;
		float num2 = num - togW;
		GUI.Label(new Rect(num2, h.y + 21f, togW, 16f), togText, Theme.Build);
		GUIContent keyText = new GUIContent(Plugin.ToggleKeyGlyph);
		float keyW = Mathf.Max(20f, Theme.Kbd.CalcSize(keyText).x + 10f);
		float num3 = num2 - (keyW + 7f);
		GUI.Label(new Rect(num3, h.y + 18f, keyW, 20f), keyText, Theme.Kbd);
		bool inWorld = Game.InWorld;
		Color statusCol = (inWorld ? Theme.Live : Theme.Dead);
		GUIStyle st = StatusStyle(inWorld);
		GUIContent stText = new GUIContent(inWorld ? "IN WORLD" : "NO WORLD");
		float stW = st.CalcSize(stText).x;
		float num4 = num3 - (stW + 20f);
		GUI.Label(new Rect(num4, h.y + 21f, stW, 16f), stText, st);
		Theme.Fill(new Rect(num4 - 12f, h.y + 25f, 7f, 7f), statusCol);
		Category current = Categories[Mathf.Clamp(_tab, 0, Categories.Length - 1)];
		_chipGc.text = current.Name + "  ·  " + current.Hint;
		float crumbLeft = h.x + 16f + titleW + 12f;
		float crumbRight = num4 - 12f;
		float crumbMaxW = Mathf.Max(0f, crumbRight - crumbLeft);
		if (crumbMaxW > 30f)
		{
			GUIStyle crumbStyle = Theme.GroupLabel;
			float crumbW = Mathf.Min(crumbStyle.CalcSize(_chipGc).x, crumbMaxW);
			float num5 = Mathf.Clamp(h.center.x - crumbW * 0.5f, crumbLeft, crumbRight - crumbW);
			GUI.Label(new Rect(num5, h.y + 17f, crumbW, 22f), _chipGc, crumbStyle);
			float barW = Mathf.Min(crumbW, 160f);
			float num6 = num5 + (crumbW - barW) * 0.5f;
			float barY = h.y + 42f;
			float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 1.8f);
			Color glow = Theme.RustBright;
			glow.a = 0.08f + 0.05f * pulse;
			Theme.Fill(new Rect(num6 - 4f, barY - 3f, barW + 8f, 8f), glow);
			Color bar = Theme.RustBright;
			bar.a = 0.4f + 0.4f * pulse;
			Theme.Fill(new Rect(num6, barY, barW, 2f), bar);
		}
		Theme.Rule(new Rect(h.x, h.yMax, h.width, 1f));
	}

	private void DrawRail()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		float y = _panel.y + 58f + 8f;
		float rowHeight = RailRowHeight;
		for (int index = 0; index < Categories.Length; index++)
		{
			Rect row = new Rect(_panel.x, y, 150f, rowHeight);
			bool selected = index == _tab;
			if (selected)
			{
				Theme.Fill(row, Theme.RailActive);
				Theme.Fill(new Rect(row.x, row.y, 3f, row.height), Theme.RustBright);
			}
			if (GUI.Button(new Rect(row.x + 12f, row.y, row.width - 12f, row.height), Categories[index].Name, selected ? Theme.TabOn : Theme.TabOff))
			{
				Select(index);
			}
			y += rowHeight;
		}
		Theme.Rule(new Rect(_panel.x + 150f, _panel.y + 58f, 1f, _panel.height - 58f - 24f));
	}

	private void Select(int index)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (index >= 0 && index < Categories.Length)
		{
			_tab = index;
			_scroll = Vector2.zero;
			_locOpen = false;
			_locBoxInPane = default;
			GUIUtility.keyboardControl = 0;
			CancelRebind();
			if (NeedsRoster(index))
			{
				_rosterDirty = true;
				_nextRosterRefresh = 0f;
			}
		}
	}

	private void DrawPane()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		Rect pane = new Rect(_panel.x + 150f + 1f, _panel.y + 58f + 1f, _panel.width - 150f - 2f, _panel.height - 58f - 24f - 2f);
		GUI.BeginGroup(pane);
		float mapH = 0f;
		if (_tab == TabMapIndex)
		{
			float mapHeightByContent = Mathf.Max(0f, pane.height - 10f - 120f);
			mapH = Mathf.Round(Mathf.Min(Mathf.Min(pane.height * 0.76f, pane.width - 32f), mapHeightByContent));
			TabMapCanvas(new Rect(0f, 0f, pane.width, mapH));
			Theme.Rule(new Rect(16f, mapH, pane.width - 32f, 1f));
		}
		float top = mapH + 10f;
		Rect inner = new Rect(0f, top, pane.width, pane.height - top);
		Event scrollEvent = Event.current;
		if ((int)scrollEvent.type == 6)
		{
			if (_tab == TabTeleportIndex && _locOpen && _locBoxInPane.Contains(scrollEvent.mousePosition))
			{
				// The named-location list consumes its own wheel while hovered.
			}
			else
			{
				Rect wheel = new Rect(16f, top, inner.width - 32f, inner.height);
				if (wheel.Contains(scrollEvent.mousePosition))
				{
					_scroll.y += scrollEvent.delta.y * 10f * Plugin.ScrollSensitivity;
					scrollEvent.Use();
				}
			}
		}
		float areaH = inner.height;
		_contentViewportH = areaH;
		_contentViewportTop = top;
		GUILayout.BeginArea(new Rect(16f, top, inner.width - 32f, areaH));
		_scroll = GUILayout.BeginScrollView(_scroll, GUIStyle.none, GUIStyle.none, Array.Empty<GUILayoutOption>());
		switch (_tab)
		{
		case 0:
			TabPlayer();
			break;
		case 1:
			TabMovement();
			break;
		case TabMapIndex:
			TabMap();
			break;
		case TabTeleportIndex:
			TabTeleport();
			break;
		case 4:
			TabCombat();
			break;
		case 5:
			TabSpawn();
			break;
		case 6:
			TabGear();
			break;
		case 7:
			TabWorld();
			break;
		case 8:
			TabGameSetup();
			break;
		case 9:
			TabVision();
			break;
		case 10:
			TabOnline();
			break;
		case 11:
			TabCheats();
			break;
		case 12:
			TabConsole();
			break;
		case 13:
			TabSettings();
			break;
		}
		Rect last = GUILayoutUtility.GetLastRect();
		GUILayout.Space(80f);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		float contentH = last.yMax + 80f;
		if (contentH > areaH + 1f)
		{
			float maxScroll = contentH - areaH;
			float frac = Mathf.Clamp01(_scroll.y / maxScroll);
			Rect track = new Rect(inner.width - 10f, top + 4f, 3f, areaH - 8f);
			Theme.Fill(track, Theme.Line);
			float thumbH = Mathf.Max(24f, areaH * areaH / contentH);
			Theme.Fill(new Rect(track.x, track.y + frac * (track.height - thumbH), track.width, thumbH), new Color(0.878f, 0.275f, 0.184f, 0.85f));
		}
		GUI.EndGroup();
	}

	private void TabMapCanvas(Rect pane)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Map.Draw(new Rect(0f, 0f, pane.width, pane.height));
	}

	private void RefreshRosterIfDue()
	{
		if (_rosterDirty || !(Time.unscaledTime < _nextRosterRefresh))
		{
			Roster.Refresh();
			_rosterDirty = false;
			_nextRosterRefresh = Time.unscaledTime + 2f;
		}
	}

	private void DrawStatus()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		Rect f = new Rect(_panel.x, _panel.yMax - 24f, _panel.width, 24f);
		float s = Theme.RoundS;
		Theme.Rule(new Rect(f.x + s, f.y, f.width - 2f * s, 1f));
		float x = f.x + 16f;
		x += Seg("ACTIVE", Theme.StatusText, f, x) + 8f;
		GUIStyle countStyle = LiveStyle();
		countStyle.normal.textColor = Theme.Ramp((float)Features.ActiveCount() / 10f);
		x += Seg(Features.ActiveCount().ToString(), countStyle, f, x) + 20f;
		if (Esp.Enabled)
		{
			x += Seg("ESP TRACKING " + Esp.LastDrawn, Theme.StatusText, f, x) + 20f;
		}
		if (!string.IsNullOrEmpty(Features.LastResult))
		{
			float room = f.xMax - 16f - x;
			if (room > 60f)
			{
				GUI.Label(new Rect(x, f.y + 4f, room, 16f), Features.LastResult.ToUpper(), Theme.StatusText);
			}
		}
	}

	private static float Seg(string text, GUIStyle style, Rect f, float x)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float w = style.CalcSize(new GUIContent(text)).x;
		if (x + w > f.xMax - 16f)
		{
			return 0f;
		}
		GUI.Label(new Rect(x, f.y + 4f, w, 16f), text, style);
		return w;
	}

	private static void Cyberfox1337xRosterHealthFunction()
	{
	}

	private static Texture2D SeasonIcon(int i)
	{
		if (_seasonIcons == null)
		{
			_seasonIcons = new Texture2D[4];
			_seasonIcons[0] = BuildIcon(FlowerShape);
			_seasonIcons[1] = BuildIcon(SunShape);
			_seasonIcons[2] = BuildIcon(LeafShape);
			_seasonIcons[3] = BuildIcon(SnowShape);
		}
		return _seasonIcons[i];
	}

	private static void FlowerShape(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 24f, 26f, 22f, 45f, 2.6f, Theme.IconMoss);
		Poly(p, new float[8] { 22f, 36f, 12f, 32f, 11f, 39f, 21f, 41f }, Theme.IconOlive);
		for (int k = 0; k < 5; k++)
		{
			float a = (float)k * (float)Math.PI * 2f / 5f - (float)Math.PI / 2f;
			Disc(p, 24f + Mathf.Cos(a) * 11f, 20f + Mathf.Sin(a) * 11f, 7.5f, Theme.IconBone);
		}
		Disc(p, 24f, 20f, 6f, Theme.IconGold);
		Disc(p, 22f, 18f, 2.4f, Theme.IconAmber);
	}

	private static void SunShape(Color32[] p)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		for (int k = 0; k < 8; k++)
		{
			float num = (float)k * (float)Math.PI / 4f;
			float cs = Mathf.Cos(num);
			float sn = Mathf.Sin(num);
			float len = ((k % 2 == 0) ? 21f : 18f);
			Stroke(p, 24f + cs * 13f, 24f + sn * 13f, 24f + cs * len, 24f + sn * len, 3.4f, Theme.IconAmber);
		}
		Disc(p, 24f, 24f, 12f, Theme.IconGold);
		Disc(p, 20f, 20f, 4f, Lift(Theme.IconGold, 0.5f));
	}

	private static void LeafShape(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[14]
		{
			39f, 8f, 41f, 21f, 33f, 35f, 17f, 41f, 9f, 32f,
			13f, 18f, 26f, 10f
		}, Theme.IconRust);
		Poly(p, new float[8] { 39f, 8f, 41f, 21f, 33f, 35f, 26f, 38f }, Theme.IconGold);
		Stroke(p, 38f, 9f, 13f, 40f, 2.2f, Theme.IconEmber);
		Stroke(p, 29f, 19f, 33f, 26f, 1.6f, Theme.IconEmber);
		Stroke(p, 21f, 28f, 25f, 35f, 1.6f, Theme.IconEmber);
	}

	private static void SnowShape(Color32[] p)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		for (int arm = 0; arm < 6; arm++)
		{
			float num = (float)arm * (float)Math.PI / 3f;
			float cs = Mathf.Cos(num);
			float sn = Mathf.Sin(num);
			Stroke(p, 24f, 24f, 24f + cs * 20f, 24f + sn * 20f, 2.8f, Theme.IconSky);
			float bx = 24f + cs * 13f;
			float by = 24f + sn * 13f;
			Stroke(p, bx, by, bx + (cs * 0.5f - sn * 0.866f) * 7f, by + (sn * 0.5f + cs * 0.866f) * 7f, 2.2f, Theme.IconSteel);
			Stroke(p, bx, by, bx + (cs * 0.5f + sn * 0.866f) * 7f, by + (sn * 0.5f - cs * 0.866f) * 7f, 2.2f, Theme.IconSteel);
		}
		Disc(p, 24f, 24f, 4.5f, Theme.IconBone);
	}

	private void TabPlayer()
	{
		Lede("Vitals are held at ceiling while enabled. Needs a loaded save.");
		Group("Survival");
		Features.GodMode = Switch("God Mode", Features.GodMode, danger: true);
		Features.InfStamina = Switch("Infinite Stamina", Features.InfStamina);
		Features.MaxStrength = Switch("Max Strength", Features.MaxStrength);
		Features.NoFallDamage = Switch("No Fall Damage", Features.NoFallDamage);
		Group("Needs");
		Features.NoHunger = Switch("No Hunger", Features.NoHunger);
		Features.NoThirst = Switch("No Thirst", Features.NoThirst);
		Features.NoCold = Switch("No Cold", Features.NoCold);
		Features.AlwaysRested = Switch("Always Rested", Features.AlwaysRested);
		Features.InfLung = Switch("Infinite Lung Capacity", Features.InfLung);
		Group("Game Cheats");
		SwitchRows(Actions.PlayerSwitches);
		Group("Actions");
		ActionRows(Actions.PlayerActions);
		Group("Values");
		ValueRows(Actions.PlayerValues, "pval");
		Group("Status Effects");
		float efW = ColW();
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("STOP BURNING", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("StopBurning");
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("GET CLEAN", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("GotClean");
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("ADRENALINE", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("AdrenalineRush");
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("CLEAR SKUNK", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("ClearSkunkSmell");
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("GET MUDDY", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("GotMud");
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("GET BLOODY", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(efW)
		}))
		{
			DirectGameApi.StatusEffect("GotBloody");
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		if (GUILayout.Button("POISON ME", Theme.BtnPrimary, new GUILayoutOption[1] { GUILayout.Height(26f) }))
		{
			DirectGameApi.StatusEffect("HitPoison");
		}
	}

	private void TabOnline()
	{
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected Obj, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		Lede("Connected-player status and health. Session actions apply only when you are the host.");
		Group("Roster");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("REFRESH", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(24f),
			GUILayout.Width(110f)
		}))
		{
			_rosterDirty = true;
		}
		GUILayout.Space(8f);
		GUILayout.Label(Roster.Status, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
		RefreshRosterIfDue();
		for (int i = 0; i < Roster.Players.Count; i++)
		{
			RemotePlayer p = Roster.Players[i];
			GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(42f) });
			GUIStyle tagStyle = (p.IsAdmin ? AdminTagStyle() : Theme.RowTag);
			string tag = RosterRules.StatusTag(p.IsLocal, p.IsAdmin, p.HasHealth, p.IsAlive);
			if (GUILayout.Button("", Theme.RowStyle, new GUILayoutOption[1] { GUILayout.Height(42f) }))
			{
				_rosterDirty = true;
			}
			Rect r = GUILayoutUtility.GetLastRect();
			Texture2D av = Avatars.Get(p.SteamId);
			Rect avatarRect = new Rect(r.x + 4f, r.y + 7f, 28f, 28f);
			if ((Object)(object)av == (Object)null)
			{
				Theme.Fill(avatarRect, Theme.FieldBg);
				Theme.Box(avatarRect, Theme.Dead);
			}
			else
			{
				GUI.DrawTexture(avatarRect, (Texture)(object)av);
				Theme.Box(avatarRect, Theme.Dead);
			}
			float tagW = tagStyle.CalcSize(new GUIContent(tag)).x + 10f;
			float tagX = r.xMax - tagW - 6f;
			GUI.Label(new Rect(r.x + 37f, r.y + 1f, Mathf.Max(20f, tagX - r.x - 43f), 18f), p.Name, Theme.RowStyle);
			GUI.Label(new Rect(tagX, r.y + 1f, tagW, 18f), tag, tagStyle);
			DrawRosterHealth(r, p);
			if (p.IsLocal)
			{
				GUILayout.Space(236f);
			}
			else
			{
				if (RosterAction("TP", Theme.Btn, 56f, 42f))
				{
					Roster.TeleportTo(i);
				}
				GUILayout.Space(4f);
				if (RosterAction("BRING", Theme.Btn, 56f, 42f))
				{
					Roster.Bring(i);
				}
				GUILayout.Space(4f);
				if (RosterAction("KILL", Theme.BtnPrimary, 56f, 42f))
				{
					Roster.Kill(i);
				}
				GUILayout.Space(4f);
				if (RosterAction("REVIVE", Theme.Btn, 56f, 42f))
				{
					Roster.Revive(i);
				}
			}
			GUILayout.EndHorizontal();
		}
		if (!_rosterDirty && Roster.Players.Count == 0)
		{
			GUILayout.Label((Roster.Status == "not in a world") ? "Load or join a world to populate the connected-player roster." : "Roster data is unavailable. Use REFRESH after the session loads.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		else if (!_rosterDirty && Roster.Players.Count == 1)
		{
			GUILayout.Label("No remote players are connected.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		Group("Native Co-op Nameplates");
		GUILayout.Label("Uses Sons of the Forest's own nameplates for visible teammates. The game controls their presentation.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		bool showPlayerNames = DirectGameApi.ShowPlayerNames;
		bool nextShowPlayerNames = Switch("Show Teammate Names", showPlayerNames);
		if (nextShowPlayerNames != showPlayerNames)
		{
			DirectGameApi.SetShowPlayerNames(nextShowPlayerNames);
		}
		Group("Session");
		ActionRows(Actions.MultiplayerActions);
		Group("Network Debug");
		SwitchRows(Actions.MultiplayerSwitches);
	}

	private static void DrawRosterHealth(Rect row, RemotePlayer player)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected Obj, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		Cyberfox1337xRosterHealthFunction();
		if (_rosterHeartIcon == null)
		{
			_rosterHeartIcon = BuildIcon(HeartIcon);
		}
		float y = row.y + 23f;
		Rect heartRect = new Rect(row.x + 38f, y, 14f, 14f);
		Color color = GUI.color;
		GUI.color = (player.HasHealth ? Color.white : Theme.Dead);
		GUI.DrawTexture(heartRect, (Texture)(object)_rosterHeartIcon);
		GUI.color = color;
		string hp = RosterRules.HealthText(player.HasHealth, player.Health, player.MaxHealth);
		float hpWidth = Theme.RowId.CalcSize(new GUIContent(hp)).x + 4f;
		float hpX = row.xMax - hpWidth - 7f;
		Rect rail = new Rect(heartRect.xMax + 6f, y + 3f, Mathf.Max(18f, hpX - heartRect.xMax - 12f), 8f);
		Theme.Fill(rail, Theme.FieldBg);
		Theme.Box(rail, Theme.Dead);
		if (player.HasHealth)
		{
			float fraction = RosterRules.HealthFraction(player.Health, player.MaxHealth);
			Theme.Fill(new Rect(rail.x + 1f, rail.y + 1f, Mathf.Max(0f, (rail.width - 2f) * fraction), rail.height - 2f), Theme.Ramp(fraction));
		}
		GUI.Label(new Rect(hpX, y - 2f, hpWidth, 18f), hp, Theme.RowId);
	}

	private static bool RosterAction(string label, GUIStyle style, float width, float rowHeight)
	{
		GUILayout.BeginVertical(new GUILayoutOption[2]
		{
			GUILayout.Width(width),
			GUILayout.Height(rowHeight)
		});
		GUILayout.Space((rowHeight - 24f) * 0.5f);
		bool result = GUILayout.Button(label, style, new GUILayoutOption[2]
		{
			GUILayout.Width(width),
			GUILayout.Height(24f)
		});
		GUILayout.EndVertical();
		return result;
	}

	private void TabSettings()
	{
		DrawMenuSizeSettings();
		Lede("Mod preferences. The Steam Web API key powers player avatars on the ONLINE tab.");
		Group("Steam API Key");
		GUILayout.Label("Paste your Steam Web API key to show player avatars. Get one free at steamcommunity.com/dev/apikey", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		_steamKey = HintField("steamkey", _steamKey, "Paste your 32-character Steam Web API key…");
		GUILayout.Space(6f);
		if (GUILayout.Button("SAVE KEY", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Width(100f),
			GUILayout.Height(24f)
		}))
		{
			SaveSteamKey();
		}
		GUILayout.EndHorizontal();
		if (_keyStatus != null)
		{
			GUILayout.Label(_keyStatus, KeyStatusStyle(_keyStatusOk), (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		GUILayout.Space(6f);
		Group("Menu Hotkey");
		Lede("Click REBIND, then press the key you want. Escape cancels, and the new key is saved to disk immediately.");
		GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(26f) });
		GUILayout.Label("Open / close menu", Theme.DialLabel, new GUILayoutOption[1] { GUILayout.Width(190f) });
		GUILayout.Label(Plugin.ToggleKeyGlyph, Theme.Kbd, new GUILayoutOption[2]
		{
			GUILayout.Width(64f),
			GUILayout.Height(24f)
		});
		GUILayout.Space(6f);
		if (GUILayout.Button(_rebindArmed ? "PRESS A KEY…" : "REBIND", _rebindArmed ? Theme.BtnPrimary : Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(140f),
			GUILayout.Height(24f)
		}))
		{
			_rebindArmed = !_rebindArmed;
			_rebindOk = false;
			_rebindStatus = (_rebindArmed ? "Listening — press any key, or Escape to cancel." : null);
		}
		GUILayout.EndHorizontal();
		if (!string.IsNullOrEmpty(_rebindStatus))
		{
			GUILayout.Label(_rebindStatus, KeyStatusStyle(_rebindOk), (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		GUILayout.Space(6f);
		Group("Appearance");
		Lede("Live preview — applies immediately and persists on exit.");
		Plugin.MenuOpacity = Dial("Menu Opacity", Plugin.MenuOpacity, 0.3f, 1f);
		Plugin.CornerRadius = Dial("Corner Radius", Plugin.CornerRadius, 4f, 20f, "0");
		Group("Scrolling");
		Plugin.ScrollSensitivity = Dial("Scroll Sensitivity", Plugin.ScrollSensitivity, 0.25f, 3f);
	}

	private void DrawMenuSizeSettings()
	{
		Group("Menu Size");
		Lede("Enlarge the window, text, buttons, and map together.");
		bool automatic = Switch("Automatic (match resolution)", Plugin.AutomaticMenuScale);
		if (automatic != Plugin.AutomaticMenuScale)
		{
			if (!automatic)
			{
				Plugin.MenuScale = _scaleLayout.Scale;
			}
			Plugin.AutomaticMenuScale = automatic;
			Plugin.SaveConfig();
		}
		float requested = (Plugin.AutomaticMenuScale ? MenuScaleRules.AutomaticScale(Screen.width, Screen.height) : Plugin.MenuScale);
		MenuWindow menuWindow = this;
		string text = $"Active size: {_scaleLayout.Scale * 100f:0}%";
		string text2;
		if (requested > _scaleLayout.Scale + 0.01f)
		{
			text2 = $". {requested * 100f:0}% requested; limited to fit this window.";
		}
		else
		{
			text2 = (Plugin.AutomaticMenuScale ? " (automatic)." : " (manual, saved).");
		}
		menuWindow.Lede(text + text2);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("- 25%", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(76f),
			GUILayout.Height(26f)
		}))
		{
			SetManualMenuScale((float)Math.Round(_scaleLayout.Scale * 4f) / 4f - 0.25f);
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("+ 25%", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(76f),
			GUILayout.Height(26f)
		}))
		{
			SetManualMenuScale((float)Math.Round(_scaleLayout.Scale * 4f) / 4f + 0.25f);
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("RESET AUTO", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(120f),
			GUILayout.Height(26f)
		}))
		{
			ResetMenuScale();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		for (int i = 0; i < MenuScalePresets.Length; i++)
		{
			if (i > 0)
			{
				GUILayout.Space(6f);
			}
			float preset = MenuScalePresets[i];
			bool selected = !Plugin.AutomaticMenuScale && Mathf.Abs(Plugin.MenuScale - preset) < 0.01f;
			if (GUILayout.Button($"{preset * 100f:0}%", selected ? Theme.BtnPrimary : Theme.Btn, new GUILayoutOption[2]
			{
				GUILayout.Width(50f),
				GUILayout.Height(26f)
			}))
			{
				SetManualMenuScale(preset);
			}
		}
		GUILayout.EndHorizontal();
		Lede("Manual range: 75% to 300%. Ctrl + Shift + Home restores automatic size and centres the menu while it is open.");
		GUILayout.Space(6f);
	}

	private void SetManualMenuScale(float scale)
	{
		Plugin.MenuScale = scale;
		Plugin.AutomaticMenuScale = false;
		Plugin.SaveConfig();
	}

	private void SaveSteamKey()
	{
		string k = (Plugin.SteamApiKey = _steamKey.Trim());
		Plugin.SaveConfig();
		Avatars.SetApiKey(k);
		_rosterDirty = true;
		Features.LastResult = (string.IsNullOrEmpty(k) ? "Steam key cleared — avatars disabled" : "Steam key saved — avatars will load");
		if (string.IsNullOrEmpty(k))
		{
			_keyStatusOk = false;
			_keyStatus = "Key cleared — avatars disabled until you save a key.";
		}
		else if (!LooksLikeSteamKey(k))
		{
			_keyStatusOk = false;
			_keyStatus = "Saved, but that doesn't look like a Steam key (32 hex characters) — avatars will likely fail to load.";
		}
		else
		{
			_keyStatusOk = true;
			_keyStatus = "Saved to com.stormfest.sonsoftheforestmenu.cfg — avatars will load in-world.";
		}
	}

	private static bool LooksLikeSteamKey(string k)
	{
		if (k.Length != 32)
		{
			return false;
		}
		foreach (char c in k)
		{
			if ((c < '0' || c > '9') && (c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
			{
				return false;
			}
		}
		return true;
	}

	private void TabCombat()
	{
		Lede("Radius commands act on everything around you at the given distance.");
		Group("Weapons");
		Features.InfAmmo = Switch("Infinite Ammo", Features.InfAmmo);
		Features.RapidFire = Switch("Rapid Fire", Features.RapidFire);
		if (Features.RapidFire)
		{
			Features.FireDelay = Dial("Fire Delay", Features.FireDelay, 0.02f, 0.5f);
		}
		Group("Combat Tuning");
		Features.AiPause = Switch("Pause AI Simulation", Features.AiPause, danger: true);
		Features.DamageTakenMult = Dial("Damage Taken", Features.DamageTakenMult, 0f, 10f);
		Group("Enemies");
		bool g = DirectGameApi.Ghost;
		bool g2 = Switch("Ghost Player (enemies ignore you)", g);
		if (g2 != g)
		{
			DirectGameApi.Ghost = g2;
		}
		SwitchRows(Actions.CombatSwitches);
		GUILayout.Space(4f);
		if (FullButton("Kill All Loaded Actors"))
		{
			DirectGameApi.KillAllActors();
		}
		Group("Area Effects");
		ValueRows(Actions.RadiusValues, "rad");
		Group("Enemy Tuning");
		ValueRows(Actions.AiValues, "aival");
		Group("Cleanup");
		ActionRows(Actions.CombatActions);
	}

	private void TabWorld()
	{
		WorldControlAvailability worldAccess = DirectGameApi.GetWorldControlAvailability();
		WorldControlAvailability lightingAccess = DirectGameApi.GetLocalTimeOverrideAvailability(_lightingChoice == 0);
		Lede("Construction cheats are local. Time, weather, and world rules belong to standalone play or the co-op host; clients stay read-only.");
		Group("Construction");
		Features.InstantBuild = Switch("Instant Build", Features.InstantBuild);
		Features.InfiniteBuild = Switch("Infinite Build (free ingredients)", Features.InfiniteBuild);
		Features.InfiniteLogs = Switch("Infinite Held Items / Logs", Features.InfiniteLogs);
		Features.InfRope = Switch("Infinite Rope Length", Features.InfRope);
		if (Features.InfRope)
		{
			Features.RopeLength = Dial("Rope Length", Features.RopeLength, 50f, 1000f, "0");
		}
		Group("World Cheats");
		SwitchRows(Actions.WorldSwitches);
		Group("Time & Weather");
		Lede(WorldRules.AvailabilityNotice(worldAccess));
		WorldChoiceRow("time", "Time of Day", WorldRules.TimeOfDay, worldAccess, ref _timeChoice);
		WorldChoiceRow("lighting", "Local Time Override", WorldRules.LightingOverride, lightingAccess, ref _lightingChoice);
		Lede("Local Time Override holds this game's effective time without changing the saved/server clock. Presets are offline single-player only; Automatic clears it.");
		WorldChoiceRow("day", "Current Day", WorldRules.CurrentDay, worldAccess, ref _dayChoice);
		WorldChoiceRow("jump", "Jump Forward", WorldRules.JumpTime, worldAccess, ref _jumpChoice);
		WorldChoiceRow("speed", "Time Speed", WorldRules.TimeSpeed, worldAccess, ref _timeSpeedChoice);
		WorldChoiceRow("wind", "Wind Intensity", WorldRules.Wind, worldAccess, ref _windChoice);
		WorldChoiceRow("clouds", "Cloud Cover", WorldRules.Clouds, worldAccess, ref _cloudChoice);
		WorldChoiceRow("forecast", "Forecast", WorldRules.Forecast, worldAccess, ref _forecastChoice);
		Group("World Rules");
		WorldChoiceRow("difficulty", "Difficulty", WorldRules.Difficulty, worldAccess, ref _difficultyChoice);
		WorldChoiceRow("mode", "Game Type", WorldRules.GameMode, worldAccess, ref _gameModeChoice);
		WorldChoiceRow("season", "Season", WorldRules.Season, worldAccess, ref _worldSeasonChoice);
		Group("World Actions");
		ActionRows(Actions.WorldActions);
		GUILayout.Space(4f);
		if (FullButton("Clear All Structures", Theme.BtnPrimary))
		{
			DirectGameApi.ClearAllStructures();
		}
	}

	private void TabGameSetup()
	{
		Lede("The options you normally pick when creating a world, applied live.");
		Group("Game Setup — World");
		Lede("The custom-game options you normally pick at world creation, applied live via setGameSetupSetting.");
		SetupRows(Wiki.GameSetupWorld, "gsw");
		Group("Game Setup — Survival");
		SetupRows(Wiki.GameSetupSurvival, "gss");
	}

	private void TabCheats()
	{
		Lede("The game's own cheat flags, read back live from Sons.Settings.Cheats — these persist independently of the console switches.");
		Group("Engine Cheats");
		bool a = EngineCheats.CheatGod;
		bool b = Switch("God Mode (engine flag)", a, danger: true);
		if (b != a)
		{
			EngineCheats.CheatGod = b;
		}
		a = EngineCheats.CheatEnergy;
		b = Switch("Infinite Energy", a);
		if (b != a)
		{
			EngineCheats.CheatEnergy = b;
		}
		a = EngineCheats.CheatNoSurvival;
		b = Switch("No Survival Drain", a);
		if (b != a)
		{
			EngineCheats.CheatNoSurvival = b;
		}
		a = EngineCheats.CheatOneHitTrees;
		b = Switch("One-Hit Tree Cutting", a);
		if (b != a)
		{
			EngineCheats.CheatOneHitTrees = b;
		}
		GUILayout.Space(6f);
		if (FullButton("Reset All Cheat Flags", Theme.Btn))
		{
			EngineCheats.ResetCheats();
		}
		Group("Water");
		if (FullButton("Toggle Freeze Ocean", Theme.Btn))
		{
			EngineCheats.ToggleFreezeWater();
		}
		Group("Armour");
		GUILayout.Label("Status: " + EngineCheats.ArmourStatus, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		if (FullButton("Refresh Armour Status", Theme.Btn))
		{
			EngineCheats.RefreshArmour();
		}
		Group("Achievements");
		GUILayout.Label((EngineCheats.AchievementTotal > 0) ? (EngineCheats.AchievementUnlocked + " of " + EngineCheats.AchievementTotal + " unlocked") : "Not loaded yet — refresh to read them.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		float achGap = 10f;
		float achW = ColW(3, achGap);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button("REFRESH", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(achW)
		}))
		{
			EngineCheats.RefreshAchievements();
		}
		GUILayout.Space(achGap);
		if (GUILayout.Button("UNLOCK ALL", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(achW)
		}))
		{
			EngineCheats.UnlockAllAchievements();
		}
		GUILayout.Space(achGap);
		if (GUILayout.Button("RESET ALL", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.Width(achW)
		}))
		{
			EngineCheats.ResetAllAchievements();
		}
		GUILayout.EndHorizontal();
		if (EngineCheats.AchievementNames.Count > 0)
		{
			GUILayout.Space(4f);
			int max = Mathf.Min(EngineCheats.AchievementNames.Count, 40);
			for (int i = 0; i < max; i++)
			{
				GUILayout.Label(EngineCheats.AchievementNames[i], Theme.RowId, (Il2CppReferenceArray<GUILayoutOption>)null);
			}
			if (EngineCheats.AchievementNames.Count > max)
			{
				GUILayout.Label("… and " + (EngineCheats.AchievementNames.Count - max) + " more", Theme.Build, (Il2CppReferenceArray<GUILayoutOption>)null);
			}
		}
		Group("World Simulation");
		int n = EngineCheats.SimActorCount();
		GUILayout.Label((n >= 0) ? ("Simulated actors: " + n) : "World simulation not available.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
	}

	private void TabGear()
	{
		Lede("Tuning for equipment and vehicles. Each needs the item to exist in the loaded world.");
		Group("Hang Glider");
		DirectGameApi.GliderForward = Dial("Forward Force", DirectGameApi.GliderForward, 1f, 80f, "0.0");
		DirectGameApi.GliderPitchUp = Dial("Pitch Up Force", DirectGameApi.GliderPitchUp, 0f, 40f, "0.0");
		DirectGameApi.GliderDown = Dial("Down Pitch", DirectGameApi.GliderDown, 0f, 40f, "0.0");
		GearApply("Apply Glider", () =>
		{
			DirectGameApi.ApplyGlider();
		});
		Group("Knight V");
		DirectGameApi.KnightVSpeed = Dial("Max Speed", DirectGameApi.KnightVSpeed, 5f, 120f, "0.0");
		GearApply("Apply Knight V", () =>
		{
			DirectGameApi.ApplyKnightV();
		});
		Group("Flashlight");
		DirectGameApi.FlashlightIntensity = Dial("Intensity", DirectGameApi.FlashlightIntensity, 0f, 30f, "0.0");
		DirectGameApi.FlashlightDrain = Dial("Drain Rate", DirectGameApi.FlashlightDrain, 0f, 5f);
		GearApply("Apply Flashlight", () =>
		{
			DirectGameApi.ApplyFlashlight();
		});
		Group("Rebreather");
		DirectGameApi.RebreatherAir = Dial("Air Consumption", DirectGameApi.RebreatherAir, 0f, 5f);
		DirectGameApi.RebreatherLight = Dial("Light Intensity", DirectGameApi.RebreatherLight, 0f, 30f, "0.0");
		GearApply("Apply Rebreather", () =>
		{
			DirectGameApi.ApplyRebreather();
		});
	}

	private void TabVision()
	{
		Lede("Overlays drawn on top of the world. Range is shared by actor and item ESP.");
		Group("Actors");
		Esp.Enabled = Switch("Enable Actor ESP", Esp.Enabled, danger: true);
		Esp.Enemies = Switch("Enemies", Esp.Enemies);
		Esp.Animals = Switch("Animals", Esp.Animals);
		Esp.Friendly = Switch("Friendly", Esp.Friendly);
		Esp.ShowDead = Switch("Show Dead", Esp.ShowDead);
		Group("Item ESP");
		ItemEsp.Enabled = Switch("Enable Item ESP", ItemEsp.Enabled, danger: true);
		ItemEsp.ShowItems = Switch("Items", ItemEsp.ShowItems);
		ItemEsp.ShowContainers = Switch("Containers (crates / storage)", ItemEsp.ShowContainers);
		ItemEsp.ShowCollectibles = Switch("Collectibles (bags / artifacts)", ItemEsp.ShowCollectibles);
		ItemEsp.ShowWaypoints = Switch("Waypoints (GPS)", ItemEsp.ShowWaypoints);
		Group("Range & Radar");
		Esp.MaxDistance = Dial("Max Distance", Esp.MaxDistance, 25f, 600f, "0");
		ItemEsp.Radar = Switch("Radar", ItemEsp.Radar);
		if (ItemEsp.Radar)
		{
			ItemEsp.RadarRange = Dial("Radar Range", ItemEsp.RadarRange, 25f, 400f, "0");
		}
		GUILayout.Label("Tracking " + Esp.LastDrawn + " actor(s), " + ItemEsp.LastDrawn + " item(s).", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		Group("Overlays");
		SwitchRows(Actions.DisplaySwitches);
		Group("Actions");
		ActionRows(Actions.DisplayActions);
		Group("Values");
		ValueRows(Actions.DisplayValues, "dval");
	}

	private void TabMovement()
	{
		Lede("Multipliers apply live. Undo restores the values captured on load.");
		Group("On Foot");
		Features.RunSpeed = Dial("Run Speed", Features.RunSpeed, 0.5f, 40f);
		Features.JumpMult = Dial("Jump Multiplier", Features.JumpMult, 0.5f, 12f);
		Features.SwimSpeed = Dial("Swim Speed", Features.SwimSpeed, 0.5f, 30f);
		Group("Gravity & Movement");
		bool gravityEnabled = Features.GravityEnabled;
		bool nextGravity = Switch("Gravity  (OFF = ZERO-G)", gravityEnabled);
		if (nextGravity != gravityEnabled)
		{
			Features.SetGravityEnabled(nextGravity);
		}
		SwitchRows(Actions.MovementSwitches);
		Group("Flight");
		Features.Fly = Switch("Fly  (WASD / Space / C)", Features.Fly);
		Features.FlySpeed = Dial("Fly Speed", Features.FlySpeed, 2f, 60f, "0.0");
		Group("Actions");
		ActionRows(Actions.MovementActions);
		GUILayout.Space(6f);
		if (GUILayout.Button("UNDO ALL", Theme.Btn, new GUILayoutOption[1] { GUILayout.Height(26f) }))
		{
			Features.RestoreBaseline();
			Features.LastResult = "Movement restored";
		}
	}

	private void TabMap()
	{
		Lede("Explore the island map above. Click a marker to travel to it, or click open terrain to teleport there. Use click-drag to pan and the wheel to zoom.");
		Group("Map Markers");
		GUILayout.Label("GPS locations, bunkers, saved spots and connected players are shown on the map. For named destinations and saved teleport points, open the TELEPORT tab.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
	}

	private void TabTeleport()
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		Group("Saved Spots");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		_spotName = HintField("spotname", _spotName, "Name this spot…", GUILayout.Width(170f), GUILayout.Height(24f));
		GUILayout.Space(6f);
		if (GUILayout.Button("SAVE HERE", Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(110f),
			GUILayout.Height(24f)
		}))
		{
			Teleport.SaveCurrent(_spotName.Trim());
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
		int deleteSpot = -1;
		for (int i = 0; i < Teleport.Spots.Count && i < 30; i++)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(22f) });
			if (GUILayout.Button("TP", Theme.Btn, new GUILayoutOption[2]
			{
				GUILayout.Width(40f),
				GUILayout.Height(22f)
			}))
			{
				Teleport.TeleportTo(i);
			}
			if (GUILayout.Button("✕", Theme.BtnPrimary, new GUILayoutOption[2]
			{
				GUILayout.Width(28f),
				GUILayout.Height(22f)
			}))
			{
				deleteSpot = i;
			}
			// Reserve the remaining row width rather than using the delete button's bounds.
			GUILayout.Label(Teleport.Spots[i].Name, Theme.RowStyle, new GUILayoutOption[3]
			{
				GUILayout.MinWidth(0f),
				GUILayout.ExpandWidth(true),
				GUILayout.Height(22f)
			});
			GUILayout.EndHorizontal();
		}
		if (deleteSpot >= 0)
		{
			Teleport.RemoveAt(deleteSpot);
		}
		if (Teleport.Spots.Count == 0)
		{
			GUILayout.Label("No spots saved yet — name one above and SAVE HERE.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}

		Group("Named Locations");
		Lede("Choose a bunker, cave, landmark or loot location, then teleport.");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		string locationLabel = (_locOpen ? "▴ " : "▾ ") + (_locSel >= 0
			? Wiki.Locations[_locSel].Label
			: "SELECT A LOCATION (" + Wiki.Locations.Length + ")");
		if (GUILayout.Button(locationLabel, Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.ExpandWidth(true),
			GUILayout.Height(26f)
		}))
		{
			_locOpen = !_locOpen;
		}
		GUILayout.Space(6f);
		bool wasEnabled = GUI.enabled;
		bool teleportSelected;
		try
		{
			GUI.enabled = wasEnabled && _locSel >= 0;
			teleportSelected = GUILayout.Button("TP", Theme.BtnPrimary, new GUILayoutOption[2]
			{
				GUILayout.Width(48f),
				GUILayout.Height(26f)
			});
		}
		finally
		{
			GUI.enabled = wasEnabled;
		}
		GUILayout.EndHorizontal();
		if (teleportSelected && _locSel >= 0 && _locSel < Wiki.Locations.Length)
		{
			Features.RunCmd("goto " + Wiki.Locations[_locSel].Target);
		}

		if (_locOpen)
		{
			_locBoxInPane = default;
			const float rowHeight = 22f;
			const int maxRows = 10;
			const int minRows = 3;
			const float keepVisible = 48f;
			int fittingRows = Mathf.FloorToInt((_contentViewportH - keepVisible) / rowHeight);
			int rows = Mathf.Clamp(fittingRows, minRows, Mathf.Min(maxRows, Wiki.Locations.Length));
			float boxHeight = rowHeight * rows;
			Rect box = GUILayoutUtility.GetRect(0f, boxHeight, new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
			if (box.width > 1f && box.height > 1f)
			{
				_locBoxInPane = new Rect(16f + box.x, _contentViewportTop + box.y - _scroll.y, box.width, box.height);
			}
			float maxScroll = Mathf.Max(0f, Wiki.Locations.Length * rowHeight - boxHeight);
			Event wheel = Event.current;
			if ((int)wheel.type == 6 && box.Contains(wheel.mousePosition))
			{
				_locScroll.y += wheel.delta.y * 10f * Plugin.ScrollSensitivity;
				wheel.Use();
			}
			_locScroll.y = Mathf.Clamp(_locScroll.y, 0f, maxScroll);
			Theme.Fill(box, Theme.Ink);
			Theme.Box(box, Theme.Line);
			int first = (int)(_locScroll.y / rowHeight);
			GUI.BeginClip(box);
			for (int index = first; index < Wiki.Locations.Length; index++)
			{
				float y = index * rowHeight - _locScroll.y;
				if (y > boxHeight)
				{
					break;
				}
				bool selected = index == _locSel;
				Rect row = new Rect(0f, y, box.width, rowHeight);
				if (selected)
				{
					Theme.Fill(row, Theme.RustWash);
				}
				if (GUI.Button(row, (selected ? "● " : "   ") + Wiki.Locations[index].Label, Theme.DropdownRow))
				{
					_locSel = index;
					_locOpen = false;
					_locScroll.y = 0f;
				}
			}
			GUI.EndClip();
			GUILayout.Space(4f);
		}
	}

	private void TabSpawn()
	{
		Lede("Spawns route through the game's debug console and land at your feet.");
		Group("Items");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Label("ID", Theme.Build, new GUILayoutOption[1] { GUILayout.Width(18f) });
		_itemId = GUILayout.TextField(_itemId, Theme.Field, new GUILayoutOption[1] { GUILayout.Width(70f) });
		GUILayout.Space(8f);
		GUILayout.Label("QTY", Theme.Build, new GUILayoutOption[1] { GUILayout.Width(28f) });
		_itemQty = GUILayout.TextField(_itemQty, Theme.Field, new GUILayoutOption[1] { GUILayout.Width(50f) });
		GUILayout.Space(8f);
		if (GUILayout.Button("ADD", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Width(70f),
			GUILayout.Height(24f)
		}))
		{
			if (int.TryParse(_itemId, out var id))
			{
				if (!int.TryParse(_itemQty, out var qty))
				{
					qty = 1;
				}
				Features.SpawnItem(id, qty);
			}
			else
			{
				Features.LastResult = "Item ID must be a number";
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		ActionRows(Actions.ItemActions);
		ValueRows(Actions.ItemValues, "ival");
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Label("SEARCH", Theme.Build, new GUILayoutOption[1] { GUILayout.Width(50f) });
		_search = HintField("itemsearch", _search, "Search items…");
		GUILayout.EndHorizontal();
		DrawItemList();
		Group("Companions & Story");
		SpawnList(2);
		Group("Mutants & Cannibals");
		SpawnList(0);
		Group("Animals");
		SpawnList(1);
		Group("Prefabs (Bolt)");
		Lede("Props spawned through the game's Bolt prefab table — spawns a few metres in front of you.");
		float pfW = ColW();
		for (int i = 0; i < Scene.Prefabs.Length; i += 2)
		{
			GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
			for (int k = i; k < i + 2 && k < Scene.Prefabs.Length; k++)
			{
				if (GUILayout.Button(Scene.Prefabs[k].Label.ToUpper(), Theme.Btn, new GUILayoutOption[2]
				{
					GUILayout.Height(26f),
					GUILayout.Width(pfW)
				}))
				{
					Scene.SpawnPrefab(k);
				}
				if (k < i + 1 && k < Scene.Prefabs.Length - 1)
				{
					GUILayout.Space(6f);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
		Group("Cutscenes");
		Lede("Played through CutsceneManager — these are the wiki-listed ones that work. Any other cutscene name can be typed below.");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		for (int j = 0; j < Scene.Cutscenes.Length; j++)
		{
			if (j > 0)
			{
				GUILayout.Space(6f);
			}
			if (GUILayout.Button(Scene.Cutscenes[j].Label.ToUpper(), Theme.Btn, new GUILayoutOption[1] { GUILayout.Height(24f) }))
			{
				Scene.PlayCutscene(Scene.Cutscenes[j].Name);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		_cutsceneName = HintField("cutname", _cutsceneName, "Cutscene name…", GUILayout.Width(180f));
		GUILayout.Space(6f);
		if (GUILayout.Button("PLAY NAME", Theme.BtnPrimary, new GUILayoutOption[1] { GUILayout.Height(24f) }))
		{
			Scene.PlayCutscene(_cutsceneName.Trim());
		}
		GUILayout.Space(6f);
		if (GUILayout.Button("SKIP", Theme.Btn, new GUILayoutOption[1] { GUILayout.Height(24f) }))
		{
			Scene.SkipCutscene();
		}
		GUILayout.EndHorizontal();
		string active = Scene.ActiveCutscene();
		if (active != null)
		{
			GUILayout.Label(active, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
	}

	private void TabConsole()
	{
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Invalid comparison between Unknown and I4
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Invalid comparison between Unknown and I4
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Invalid comparison between Unknown and I4
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		Lede("Every command the game's own debug console exposes — " + Commands.All.Length + " of them. Type one, or pick from the list.");
		Group("Common (wiki)");
		float cmnGap = 10f;
		float cmnW = ColW(4, cmnGap);
		string[] rowA = new string[4] { "ADD ALL ITEMS", "CAVE LIGHT", "INVISIBLE", "CREEPY VILLAGE" };
		string[] rowB = new string[4] { "KILL RADIUS 50", "SUPER JUMP", "SPEEDY RUN", "ENERGY HACK" };
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button(rowA[0], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("addAllItems");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowA[1], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("caveLight on");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowA[2], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("invisible on");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowA[3], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("creepyVillage");
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		if (GUILayout.Button(rowB[0], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("killRadius 50");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowB[1], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("superJump on");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowB[2], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("speedyrun on");
		}
		GUILayout.Space(cmnGap);
		if (GUILayout.Button(rowB[3], Theme.Btn, new GUILayoutOption[2]
		{
			GUILayout.Width(cmnW),
			GUILayout.Height(26f)
		}))
		{
			Features.RunCmd("energyhack on");
		}
		GUILayout.EndHorizontal();
		Group("Run");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUI.SetNextControlName("cmdline");
		_cmdLine = HintField("cmdline", _cmdLine, "Type a console command…");
		GUILayout.Space(6f);
		bool run = GUILayout.Button("RUN", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Width(70f),
			GUILayout.Height(24f)
		});
		GUILayout.EndHorizontal();
		if (_cmdLine.Trim().Length > 0 && Wiki.Desc.TryGetValue(_cmdLine.Trim().ToLowerInvariant(), out var desc))
		{
			GUILayout.Label(desc, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		if (!run && (int)Event.current.type == 4 && ((int)Event.current.keyCode == 13 || (int)Event.current.keyCode == 271) && GUI.GetNameOfFocusedControl() == "cmdline")
		{
			run = true;
			Event.current.Use();
		}
		if (run && _cmdLine.Trim().Length > 0)
		{
			string c = _cmdLine.Trim();
			Features.RunCmd(c);
			_history.Insert(0, c);
			if (_history.Count > 12)
			{
				_history.RemoveAt(_history.Count - 1);
			}
			_cmdLine = "";
		}
		if (_history.Count > 0)
		{
			Group("Recent");
			for (int i = 0; i < _history.Count; i++)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(20f) });
				if (GUILayout.Button("", Theme.RowStyle, new GUILayoutOption[1] { GUILayout.Height(20f) }))
				{
					_cmdLine = _history[i];
				}
				Rect r = GUILayoutUtility.GetLastRect();
				GUI.Label(new Rect(r.x + 8f, r.y, r.width - 60f, r.height), _history[i], Theme.RowId);
				GUILayout.EndHorizontal();
			}
		}
		Group("All Commands");
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Label("SEARCH", Theme.Build, new GUILayoutOption[1] { GUILayout.Width(50f) });
		_cmdSearch = HintField("cmdsearch", _cmdSearch, "Search commands…");
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		string q = _cmdSearch.Trim().ToLowerInvariant();
		int shown = 0;
		for (int j = 0; j < Commands.All.Length; j++)
		{
			if (shown >= 60)
			{
				break;
			}
			string c2 = Commands.All[j];
			if (q.Length <= 0 || Commands.Lower[j].IndexOf(q, StringComparison.Ordinal) >= 0)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(20f) });
				if (GUILayout.Button("", Theme.RowStyle, new GUILayoutOption[1] { GUILayout.Height(20f) }))
				{
					_cmdLine = c2;
				}
				Rect r2 = GUILayoutUtility.GetLastRect();
				GUI.Label(new Rect(r2.x + 8f, r2.y, r2.width - 70f, r2.height), c2, Theme.RowStyle);
				GUILayout.EndHorizontal();
				shown++;
				if (shown == 1 && q.Length > 0 && Wiki.Desc.TryGetValue(c2.ToLowerInvariant(), out var first))
				{
					GUILayout.Label(first, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
				}
			}
		}
		if (shown == 0)
		{
			GUILayout.Label("No command matches \"" + _cmdSearch + "\".", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		else if (shown >= 60)
		{
			GUILayout.Label("Showing first 60 — refine the search.", Theme.Build, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
	}

	private static int ParseQty(string s)
	{
		if (!int.TryParse(s, out var q) || q <= 0)
		{
			return 1;
		}
		return q;
	}

	private void DrawItemList()
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		Il2CppSystem.Collections.Generic.List<ItemData> items = Game.Items;
		if (items == null)
		{
			GUILayout.Label("Item database is empty — load a save to populate it.", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
			return;
		}
		int shown = 0;
		int total;
		try
		{
			total = items.Count;
		}
		catch
		{
			return;
		}
		string q = _search.Trim().ToLowerInvariant();
		for (int i = 0; i < total && shown < 40; i++)
		{
			string name;
			int id;
			try
			{
				ItemData d = items[i];
				if ((Object)(object)d == (Object)null)
				{
					continue;
				}
				name = d._name;
				id = d._id;
				goto IL_0078;
			}
			catch
			{
			}
			continue;
			IL_0078:
			if (!string.IsNullOrEmpty(name) && (q.Length <= 0 || name.ToLowerInvariant().IndexOf(q, StringComparison.Ordinal) >= 0 || id.ToString().IndexOf(q, StringComparison.Ordinal) >= 0))
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(22f) });
				if (GUILayout.Button("", Theme.RowStyle, new GUILayoutOption[1] { GUILayout.Height(22f) }))
				{
					_itemId = id.ToString();
				}
				Rect r = GUILayoutUtility.GetLastRect();
				GUI.Label(new Rect(r.x + 8f, r.y, 46f, r.height), id.ToString(), Theme.RowId);
				GUI.Label(new Rect(r.x + 58f, r.y, r.width - 70f, r.height), name, Theme.RowStyle);
				GUILayout.EndHorizontal();
				shown++;
			}
		}
		if (shown == 0)
		{
			GUILayout.Label("No item matches \"" + _search + "\".", Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
		else if (shown >= 40)
		{
			GUILayout.Label("Showing first 40 matches — refine the search.", Theme.Build, (Il2CppReferenceArray<GUILayoutOption>)null);
		}
	}

	private void SpawnList(int which)
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		int count = which switch
		{
			1 => Animals.Length, 
			0 => Mutants.Length, 
			_ => Companions.Length, 
		};
		int selected = which switch
		{
			1 => _selAnimal, 
			0 => _selMutant, 
			_ => _selCompanion, 
		};
		string qty = which switch
		{
			1 => _aniQty, 
			0 => _mutQty, 
			_ => _compQty, 
		};
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Label("COUNT", Theme.Build, new GUILayoutOption[1] { GUILayout.Width(44f) });
		qty = GUILayout.TextField(qty, Theme.Field, new GUILayoutOption[1] { GUILayout.Width(46f) });
		GUILayout.Space(8f);
		bool go = GUILayout.Button("SPAWN SELECTED", Theme.BtnPrimary, new GUILayoutOption[2]
		{
			GUILayout.Width(160f),
			GUILayout.Height(24f)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		for (int i = 0; i < count; i++)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(22f) });
			bool sel = selected == i;
			GUIStyle st = (sel ? Theme.SwitchOn : Theme.RowStyle);
			if (GUILayout.Button("", st, new GUILayoutOption[1] { GUILayout.Height(22f) }))
			{
				selected = i;
			}
			Rect r = GUILayoutUtility.GetLastRect();
			if (sel)
			{
				Theme.Fill(new Rect(r.x, r.y, 2f, r.height), Theme.RustBright);
			}
			string label = which switch
			{
				1 => Animals[i].Label, 
				0 => Mutants[i].Label, 
				_ => Companions[i].Label, 
			};
			bool flag = which switch
			{
				1 => Animals[i].Hostile, 
				0 => Mutants[i].Boss, 
				_ => Companions[i].Boss, 
			};
			GUI.Label(new Rect(r.x + 10f, r.y, r.width - 80f, r.height), label, Theme.RowStyle);
			if (flag)
			{
				GUI.Label(new Rect(r.xMax - 60f, r.y, 54f, r.height), "BOSS", BossTagStyle());
			}
			GUILayout.EndHorizontal();
		}
		switch (which)
		{
		case 0:
			_selMutant = selected;
			_mutQty = qty;
			break;
		case 1:
			_selAnimal = selected;
			_aniQty = qty;
			break;
		default:
			_selCompanion = selected;
			_compQty = qty;
			break;
		}
		if (go)
		{
			if (selected >= 0 && selected < count)
			{
				Features.SpawnCharacter(which switch
				{
					1 => Animals[selected].Key, 
					0 => Mutants[selected].Key, 
					_ => Companions[selected].Key, 
				}, ParseQty(qty));
			}
			else
			{
				Features.LastResult = "Select a target first";
			}
		}
	}

	private static string Cyberfox1337xWidgetFunction()
	{
		return "menu-widget-icons";
	}

	private static Color Lift(Color c, float t)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Color.Lerp(c, Theme.IconBone, t);
	}

	private static Color Shade(Color c, float t)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Color.Lerp(c, Theme.Ink, t);
	}

	private static void Plot(Color32[] p, int x, int y, Color c, float a)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (!(a <= 0f) && (uint)x < 48u && (uint)y < 48u)
		{
			if (a > 1f)
			{
				a = 1f;
			}
			int i = y * 48 + x;
			Color32 dst = p[i];
			float da = (float)(int)dst.a / 255f;
			float outA = a + da * (1f - a);
			if (outA <= 0.0001f)
			{
				p[i] = new Color32((byte)0, (byte)0, (byte)0, (byte)0);
				return;
			}
			float r = (c.r * a + (float)(int)dst.r / 255f * da * (1f - a)) / outA;
			float g = (c.g * a + (float)(int)dst.g / 255f * da * (1f - a)) / outA;
			float b = (c.b * a + (float)(int)dst.b / 255f * da * (1f - a)) / outA;
			p[i] = new Color32((byte)(Mathf.Clamp01(r) * 255f), (byte)(Mathf.Clamp01(g) * 255f), (byte)(Mathf.Clamp01(b) * 255f), (byte)(Mathf.Clamp01(outA) * 255f));
		}
	}

	private static void Raster(Color32[] p, float x0, float y0, float x1, float y1, Func<float, float, bool> inside, Color c)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		int ix0 = Mathf.Max(0, Mathf.FloorToInt(x0));
		int num = Mathf.Max(0, Mathf.FloorToInt(y0));
		int ix1 = Mathf.Min(47, Mathf.CeilToInt(x1));
		int iy1 = Mathf.Min(47, Mathf.CeilToInt(y1));
		for (int i = num; i <= iy1; i++)
		{
			for (int j = ix0; j <= ix1; j++)
			{
				int hits = 0;
				for (int sy = 0; sy < 3; sy++)
				{
					for (int sx = 0; sx < 3; sx++)
					{
						if (inside((float)j + (float)sx * (1f / 3f) + 1f / 6f, (float)i + (float)sy * (1f / 3f) + 1f / 6f))
						{
							hits++;
						}
					}
				}
				if (hits > 0)
				{
					Plot(p, j, i, c, (float)hits / 9f);
				}
			}
		}
	}

	private static void Disc(Color32[] p, float cx, float cy, float r, Color c)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, cx - r - 1f, cy - r - 1f, cx + r + 1f, cy + r + 1f, (float x, float y) => (x - cx) * (x - cx) + (y - cy) * (y - cy) <= r * r, c);
	}

	private static void Ring(Color32[] p, float cx, float cy, float ro, float ri, Color c)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, cx - ro - 1f, cy - ro - 1f, cx + ro + 1f, cy + ro + 1f, (float x, float y) =>
		{
			float num = (x - cx) * (x - cx) + (y - cy) * (y - cy);
			return num <= ro * ro && num >= ri * ri;
		}, c);
	}

	private static void Arc(Color32[] p, float cx, float cy, float ro, float ri, float from, float to, Color c)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, cx - ro - 1f, cy - ro - 1f, cx + ro + 1f, cy + ro + 1f, (float x, float y) =>
		{
			float num = x - cx;
			float num2 = y - cy;
			float num3 = num * num + num2 * num2;
			if (num3 > ro * ro || num3 < ri * ri)
			{
				return false;
			}
			float num4 = Mathf.Atan2(num2, num) * 57.29578f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			if (!(from <= to))
			{
				if (!(num4 >= from))
				{
					return num4 <= to;
				}
				return true;
			}
			return num4 >= from && num4 <= to;
		}, c);
	}

	private static void Box(Color32[] p, float x0, float y0, float x1, float y1, Color c)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, x0, y0, x1, y1, (float num, float num2) => num >= x0 && num <= x1 && num2 >= y0 && num2 <= y1, c);
	}

	private static void RBox(Color32[] p, float x0, float y0, float x1, float y1, float rad, Color c)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, x0, y0, x1, y1, (float num, float num2) =>
		{
			if (num < x0 || num > x1 || num2 < y0 || num2 > y1)
			{
				return false;
			}
			float num3 = Mathf.Max(Mathf.Max(x0 + rad - num, 0f), Mathf.Max(num - (x1 - rad), 0f));
			float num4 = Mathf.Max(Mathf.Max(y0 + rad - num2, 0f), Mathf.Max(num2 - (y1 - rad), 0f));
			return num3 * num3 + num4 * num4 <= rad * rad;
		}, c);
	}

	private static void Stroke(Color32[] p, float ax, float ay, float bx, float by, float thick, Color c)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		float h = thick * 0.5f;
		float minX = Mathf.Min(ax, bx) - h - 1f;
		float maxX = Mathf.Max(ax, bx) + h + 1f;
		float minY = Mathf.Min(ay, by) - h - 1f;
		float maxY = Mathf.Max(ay, by) + h + 1f;
		float vx = bx - ax;
		float vy = by - ay;
		float len2 = vx * vx + vy * vy;
		Raster(p, minX, minY, maxX, maxY, (float x, float y) =>
		{
			float num = ((len2 <= 0.0001f) ? 0f : Mathf.Clamp01(((x - ax) * vx + (y - ay) * vy) / len2));
			float num2 = x - (ax + vx * num);
			float num3 = y - (ay + vy * num);
			return num2 * num2 + num3 * num3 <= h * h;
		}, c);
	}

	private static void Tri(Color32[] p, float ax, float ay, float bx, float by, float cx, float cy, Color col)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[6] { ax, ay, bx, by, cx, cy }, col);
	}

	private static void Poly(Color32[] p, float[] pts, Color c)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		int n = pts.Length / 2;
		if (n < 3)
		{
			return;
		}
		float minX = pts[0];
		float maxX = pts[0];
		float minY = pts[1];
		float maxY = pts[1];
		for (int i = 1; i < n; i++)
		{
			minX = Mathf.Min(minX, pts[i * 2]);
			maxX = Mathf.Max(maxX, pts[i * 2]);
			minY = Mathf.Min(minY, pts[i * 2 + 1]);
			maxY = Mathf.Max(maxY, pts[i * 2 + 1]);
		}
		Raster(p, minX - 1f, minY - 1f, maxX + 1f, maxY + 1f, (float x, float y) =>
		{
			bool flag = false;
			int num = 0;
			int num2 = n - 1;
			while (num < n)
			{
				float num3 = pts[num * 2];
				float num4 = pts[num * 2 + 1];
				float num5 = pts[num2 * 2];
				float num6 = pts[num2 * 2 + 1];
				if (num4 > y != num6 > y && x < (num5 - num3) * (y - num4) / (num6 - num4) + num3)
				{
					flag = !flag;
				}
				num2 = num++;
			}
			return flag;
		}, c);
	}

	private static Texture2D BuildIcon(Action<Color32[]> draw)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected Obj, but got Unknown
		Color32[] px = new Color32[2304];
		draw(px);
		Color32[] flipped = new Color32[px.Length];
		for (int y = 0; y < 48; y++)
		{
			Array.Copy(px, y * 48, flipped, (47 - y) * 48, 48);
		}
		Texture2D val = new Texture2D(48, 48, (TextureFormat)4, false)
		{
			filterMode = (FilterMode)1,
			wrapMode = (TextureWrapMode)1,
			hideFlags = (HideFlags)52
		};
		val.SetPixels32(new Il2CppStructArray<Color32>(flipped));
		val.Apply();
		return val;
	}

	private static void HeartIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 16f, 18f, 9.5f, Theme.IconRust);
		Disc(p, 32f, 18f, 9.5f, Theme.IconRust);
		Tri(p, 6.8f, 20f, 41.2f, 20f, 24f, 42f, Theme.IconRust);
		Tri(p, 24f, 27f, 41f, 22f, 24f, 42f, Shade(Theme.IconRust, 0.22f));
		Disc(p, 15f, 14f, 3.4f, Lift(Theme.IconRust, 0.55f));
	}

	private static void AppleIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 17f, 28f, 11f, Theme.IconRust);
		Disc(p, 31f, 28f, 11f, Theme.IconRust);
		Box(p, 17f, 20f, 31f, 39f, Theme.IconRust);
		Disc(p, 33f, 31f, 9f, Shade(Theme.IconRust, 0.2f));
		Stroke(p, 24f, 19f, 26f, 8f, 2.6f, Theme.IconAmber);
		Poly(p, new float[8] { 27f, 12f, 38f, 6f, 39f, 14f, 29f, 17f }, Theme.IconOlive);
		Disc(p, 16f, 22f, 3.2f, Lift(Theme.IconRust, 0.6f));
	}

	private static void GamepadIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 4f, 17f, 44f, 36f, 9f, Theme.IconSlate);
		Disc(p, 12f, 34f, 8f, Theme.IconSlate);
		Disc(p, 36f, 34f, 8f, Theme.IconSlate);
		Box(p, 4f, 29f, 44f, 36f, Shade(Theme.IconSlate, 0.25f));
		Box(p, 9f, 24f, 20f, 27f, Theme.IconBone);
		Box(p, 13f, 20f, 16f, 31f, Theme.IconBone);
		Disc(p, 33f, 22f, 3.2f, Theme.IconRust);
		Disc(p, 39f, 26f, 3.2f, Theme.IconGold);
		Disc(p, 33f, 30f, 3.2f, Theme.IconOlive);
		Disc(p, 27f, 26f, 3.2f, Theme.IconSteel);
	}

	private static void PlayIcon(Color32[] p)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Tri(p, 14f, 8f, 14f, 40f, 40f, 24f, Theme.IconOlive);
		Tri(p, 14f, 24f, 14f, 40f, 40f, 24f, Shade(Theme.IconOlive, 0.22f));
	}

	private static void HashIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 17f, 7f, 13f, 41f, 4.2f, Theme.IconGold);
		Stroke(p, 33f, 7f, 29f, 41f, 4.2f, Theme.IconGold);
		Stroke(p, 8f, 18f, 40f, 18f, 4.2f, Theme.IconAmber);
		Stroke(p, 7f, 31f, 39f, 31f, 4.2f, Theme.IconAmber);
	}

	private static void LightningIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[12]
		{
			28f, 4f, 13f, 27f, 22f, 27f, 18f, 44f, 35f, 20f,
			26f, 20f
		}, Theme.IconGold);
		Poly(p, new float[8] { 28f, 4f, 13f, 27f, 22f, 27f, 22f, 20f }, Lift(Theme.IconGold, 0.4f));
	}

	private static void BladesIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 10f, 38f, 36f, 10f, 5f, Theme.IconBone);
		Stroke(p, 38f, 38f, 12f, 10f, 5f, Lift(Theme.IconAsh, 0.15f));
		Stroke(p, 7f, 41f, 15f, 33f, 5.5f, Theme.IconAmber);
		Stroke(p, 41f, 41f, 33f, 33f, 5.5f, Theme.IconAmber);
	}

	private static void SlidersIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 7f, 13f, 41f, 13f, 3.2f, Theme.IconSlate);
		Stroke(p, 7f, 24f, 41f, 24f, 3.2f, Theme.IconSlate);
		Stroke(p, 7f, 35f, 41f, 35f, 3.2f, Theme.IconSlate);
		Disc(p, 31f, 13f, 6f, Theme.IconSteel);
		Disc(p, 16f, 24f, 6f, Theme.IconSteel);
		Disc(p, 35f, 35f, 6f, Theme.IconSteel);
		Disc(p, 29.5f, 11.5f, 2.2f, Theme.IconSky);
		Disc(p, 14.5f, 22.5f, 2.2f, Theme.IconSky);
		Disc(p, 33.5f, 33.5f, 2.2f, Theme.IconSky);
	}

	private static void SkullIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 20f, 15f, Theme.IconBone);
		Box(p, 15f, 20f, 33f, 32f, Theme.IconBone);
		Disc(p, 15f, 30f, 6f, Theme.IconBone);
		Disc(p, 33f, 30f, 6f, Theme.IconBone);
		Box(p, 18f, 33f, 30f, 41f, Theme.IconBone);
		Disc(p, 24f, 41f, 6f, Theme.IconBone);
		Disc(p, 17f, 20f, 5.4f, Shade(Theme.IconSlate, 0.62f));
		Disc(p, 31f, 20f, 5.4f, Shade(Theme.IconSlate, 0.62f));
		Tri(p, 24f, 26f, 20.5f, 33f, 27.5f, 33f, Shade(Theme.IconSlate, 0.62f));
		Box(p, 20f, 37f, 21.5f, 44f, Shade(Theme.IconSlate, 0.5f));
		Box(p, 26.5f, 37f, 28f, 44f, Shade(Theme.IconSlate, 0.5f));
	}

	private static void BlastIcon(Color32[] p)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Ring(p, 24f, 24f, 21f, 17.5f, Shade(Theme.IconRust, 0.35f));
		Ring(p, 24f, 24f, 14f, 10.5f, Theme.IconRust);
		Disc(p, 24f, 24f, 6.5f, Theme.IconGold);
		Disc(p, 22f, 22f, 2.6f, Lift(Theme.IconGold, 0.55f));
	}

	private static void GaugeIcon(Color32[] p)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Arc(p, 24f, 30f, 19f, 13f, 180f, 360f, Theme.IconSlate);
		Arc(p, 24f, 30f, 19f, 13f, 290f, 360f, Theme.IconRust);
		Stroke(p, 24f, 30f, 35f, 18f, 3.4f, Theme.IconBone);
		Disc(p, 24f, 30f, 4.2f, Theme.IconBone);
	}

	private static void BroomIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 31f, 5f, 20f, 25f, 3.6f, Theme.IconAmber);
		Poly(p, new float[8] { 13f, 26f, 27f, 26f, 33f, 43f, 8f, 43f }, Theme.IconOlive);
		Box(p, 12.5f, 26f, 27.5f, 31f, Theme.IconAmber);
		Stroke(p, 17f, 32f, 14f, 42f, 1.6f, Shade(Theme.IconOlive, 0.35f));
		Stroke(p, 22f, 32f, 21f, 42f, 1.6f, Shade(Theme.IconOlive, 0.35f));
		Stroke(p, 27f, 32f, 28f, 42f, 1.6f, Shade(Theme.IconOlive, 0.35f));
	}

	private static void HammerIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 26f, 18f, 13f, 42f, 4.4f, Theme.IconAmber);
		Poly(p, new float[8] { 20f, 6f, 42f, 12f, 39f, 22f, 17f, 16f }, Theme.IconAsh);
		Poly(p, new float[8] { 20f, 6f, 42f, 12f, 41f, 15f, 19f, 10f }, Lift(Theme.IconAsh, 0.35f));
	}

	private static void LeafIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[14]
		{
			40f, 7f, 41f, 22f, 31f, 37f, 15f, 41f, 8f, 33f,
			12f, 18f, 26f, 9f
		}, Theme.IconOlive);
		Poly(p, new float[8] { 40f, 7f, 41f, 22f, 31f, 37f, 24f, 40f }, Theme.IconGold);
		Stroke(p, 39f, 8f, 12f, 40f, 2.2f, Shade(Theme.IconMoss, 0.25f));
		Stroke(p, 30f, 18f, 34f, 25f, 1.6f, Shade(Theme.IconMoss, 0.15f));
		Stroke(p, 22f, 27f, 26f, 34f, 1.6f, Shade(Theme.IconMoss, 0.15f));
	}

	private static void GlobeIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 24f, 19f, Theme.IconSteel);
		Ring(p, 24f, 24f, 19f, 16.5f, Theme.IconSky);
		Stroke(p, 5.5f, 24f, 42.5f, 24f, 2.4f, Theme.IconSky);
		Ring(p, 24f, 24f, 9.5f, 7.1f, Theme.IconSky);
		Stroke(p, 24f, 5.5f, 24f, 42.5f, 2.4f, Theme.IconSky);
	}

	private static void GlobeBoltIcon(Color32[] p)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		GlobeIcon(p);
		Poly(p, new float[12]
		{
			30f, 8f, 19f, 26f, 26f, 26f, 20f, 42f, 36f, 21f,
			28f, 21f
		}, Theme.IconRust);
	}

	private static void ClockIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 24f, 19f, Theme.IconBone);
		Ring(p, 24f, 24f, 19f, 15.5f, Theme.IconAsh);
		Stroke(p, 24f, 24f, 24f, 12f, 3f, Theme.IconRust);
		Stroke(p, 24f, 24f, 33f, 28f, 3f, Theme.IconEmber);
		Disc(p, 24f, 24f, 2.6f, Theme.IconSlate);
	}

	private static void GearIcon(Color32[] p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Cog(p, Theme.IconSteel, Theme.IconSky);
	}

	private static void CogRustIcon(Color32[] p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Cog(p, Theme.IconRust, Theme.IconGold);
	}

	private static void Cog(Color32[] p, Color body, Color core)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 8; i++)
		{
			float num = (float)i * (float)Math.PI / 4f;
			float cs = Mathf.Cos(num);
			float sn = Mathf.Sin(num);
			Stroke(p, 24f + cs * 12f, 24f + sn * 12f, 24f + cs * 20f, 24f + sn * 20f, 7f, body);
		}
		Disc(p, 24f, 24f, 15f, body);
		Disc(p, 24f, 24f, 7f, Shade(body, 0.55f));
		Ring(p, 24f, 24f, 7f, 5f, core);
	}

	private static void FlameIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[20]
		{
			24f, 4f, 34f, 18f, 33f, 27f, 38f, 24f, 39f, 34f,
			24f, 44f, 9f, 34f, 12f, 21f, 17f, 25f, 16f, 15f
		}, Theme.IconRust);
		Poly(p, new float[10] { 24f, 17f, 31f, 29f, 28f, 40f, 18f, 39f, 16f, 29f }, Theme.IconGold);
		Disc(p, 24f, 35f, 4f, Lift(Theme.IconGold, 0.5f));
	}

	private static void DropletIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[12]
		{
			24f, 4f, 36f, 24f, 38f, 31f, 24f, 44f, 10f, 31f,
			12f, 24f
		}, Theme.IconSteel);
		Disc(p, 24f, 31f, 12f, Theme.IconSteel);
		Disc(p, 19f, 30f, 4.2f, Theme.IconSky);
		Stroke(p, 16f, 24f, 15f, 30f, 2.4f, Theme.IconSky);
	}

	private static void ShieldIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[16]
		{
			24f, 4f, 41f, 10f, 41f, 24f, 36f, 34f, 24f, 44f,
			12f, 34f, 7f, 24f, 7f, 10f
		}, Theme.IconBone);
		Poly(p, new float[10] { 24f, 4f, 41f, 10f, 41f, 24f, 36f, 34f, 24f, 44f }, Shade(Theme.IconBone, 0.18f));
		Poly(p, new float[12]
		{
			24f, 13f, 34f, 20f, 34f, 26f, 24f, 19f, 14f, 26f,
			14f, 20f
		}, Theme.IconRust);
	}

	private static void TrophyIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[10] { 14f, 6f, 34f, 6f, 32f, 24f, 24f, 30f, 16f, 24f }, Theme.IconGold);
		Ring(p, 11f, 13f, 6.5f, 4.2f, Theme.IconAmber);
		Ring(p, 37f, 13f, 6.5f, 4.2f, Theme.IconAmber);
		Box(p, 21.5f, 29f, 26.5f, 36f, Theme.IconAmber);
		RBox(p, 13f, 36f, 35f, 42f, 2f, Theme.IconGold);
		Stroke(p, 19f, 9f, 19f, 22f, 2.6f, Lift(Theme.IconGold, 0.5f));
	}

	private static void AtomIcon(Color32[] p)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		Ring(p, 24f, 24f, 19f, 16f, Theme.IconSteel);
		Raster(p, 4f, 4f, 44f, 44f, (float x, float y) =>
		{
			float num = x - 24f;
			float num2 = y - 24f;
			float num3 = (num + num2) * 0.7071f;
			float num4 = (num - num2) * 0.7071f;
			float num5 = num3 * num3 / 361f + num4 * num4 / 64f;
			float num6 = num4 * num4 / 361f + num3 * num3 / 64f;
			return (num5 <= 1f && num5 >= 0.55f) || (num6 <= 1f && num6 >= 0.55f);
		}, Theme.IconSky);
		Disc(p, 24f, 24f, 5f, Theme.IconRust);
	}

	private static void GliderIcon(Color32[] p)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[6] { 24f, 10f, 45f, 26f, 26f, 24f }, Theme.IconBone);
		Poly(p, new float[6] { 24f, 10f, 3f, 26f, 22f, 24f }, Shade(Theme.IconBone, 0.2f));
		Stroke(p, 24f, 10f, 24f, 30f, 2f, Theme.IconAsh);
		Disc(p, 24f, 34f, 5f, Theme.IconRust);
		Stroke(p, 24f, 30f, 24f, 31f, 2f, Theme.IconAsh);
	}

	private static void BuggyIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[14]
		{
			8f, 30f, 14f, 18f, 30f, 18f, 38f, 27f, 42f, 30f,
			42f, 34f, 8f, 34f
		}, Theme.IconRust);
		Poly(p, new float[8] { 16f, 20f, 29f, 20f, 34f, 27f, 16f, 27f }, Shade(Theme.IconSteel, 0.25f));
		Ring(p, 15f, 36f, 7f, 3.2f, Theme.IconSlate);
		Ring(p, 35f, 36f, 7f, 3.2f, Theme.IconSlate);
		Disc(p, 41f, 28f, 2.6f, Theme.IconGold);
	}

	private static void TorchIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 8f, 17f, 22f, 31f, 3f, Theme.IconSlate);
		Box(p, 22f, 19f, 27f, 29f, Theme.IconAsh);
		Poly(p, new float[8] { 27f, 12f, 44f, 6f, 44f, 42f, 27f, 36f }, Theme.IconGold);
		Poly(p, new float[8] { 27f, 17f, 44f, 14f, 44f, 34f, 27f, 31f }, Lift(Theme.IconGold, 0.45f));
		Disc(p, 12f, 24f, 2.4f, Theme.IconRust);
	}

	private static void MaskIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 9f, 12f, 39f, 34f, 11f, Theme.IconSteel);
		Disc(p, 18f, 22f, 5.4f, Shade(Theme.IconSlate, 0.55f));
		Disc(p, 30f, 22f, 5.4f, Shade(Theme.IconSlate, 0.55f));
		Disc(p, 16.5f, 20.5f, 2f, Theme.IconSky);
		Disc(p, 28.5f, 20.5f, 2f, Theme.IconSky);
		RBox(p, 18f, 34f, 30f, 43f, 4f, Theme.IconSlate);
		Stroke(p, 24f, 34f, 24f, 43f, 2f, Shade(Theme.IconSlate, 0.4f));
	}

	private static void PersonIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 13f, 7.5f, Theme.IconBone);
		Poly(p, new float[10] { 24f, 22f, 37f, 33f, 37f, 43f, 11f, 43f, 11f, 33f }, Theme.IconAsh);
	}

	private static void EyeCrateIcon(Color32[] p)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, 2f, 8f, 46f, 32f, (float x, float y) =>
		{
			float num = (x - 23f) / 21f;
			float num2 = (y - 20f) / 11f;
			return num * num + num2 * num2 <= 1f;
		}, Theme.IconBone);
		Disc(p, 23f, 20f, 8f, Theme.IconOlive);
		Disc(p, 23f, 20f, 4f, Shade(Theme.IconMoss, 0.7f));
		Disc(p, 20.5f, 17.5f, 2.2f, Theme.IconBone);
		RBox(p, 27f, 30f, 45f, 45f, 2f, Theme.IconAmber);
		Stroke(p, 27f, 35f, 45f, 35f, 2f, Shade(Theme.IconAmber, 0.4f));
		Stroke(p, 36f, 30f, 36f, 45f, 2f, Shade(Theme.IconAmber, 0.4f));
	}

	private static void RadarIcon(Color32[] p)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		Ring(p, 24f, 24f, 20f, 17.5f, Theme.IconMoss);
		Ring(p, 24f, 24f, 12f, 10f, Theme.IconMoss);
		Arc(p, 24f, 24f, 17.5f, 0f, 270f, 340f, Shade(Theme.IconOlive, 0.45f));
		Stroke(p, 24f, 24f, 24f, 6f, 2.4f, Theme.IconOlive);
		Stroke(p, 24f, 24f, 39f, 15f, 2.4f, Theme.IconOlive);
		Disc(p, 32f, 31f, 3.2f, Theme.IconRust);
		Disc(p, 16f, 30f, 2.6f, Theme.IconGold);
	}

	private static void LayersIcon(Color32[] p)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[8] { 24f, 30f, 43f, 38f, 24f, 45f, 5f, 38f }, Shade(Theme.IconSteel, 0.4f));
		Poly(p, new float[8] { 24f, 19f, 43f, 27f, 24f, 34f, 5f, 27f }, Theme.IconSteel);
		Poly(p, new float[8] { 24f, 8f, 43f, 16f, 24f, 23f, 5f, 16f }, Theme.IconSky);
	}

	private static void BootIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[12]
		{
			13f, 6f, 25f, 6f, 27f, 26f, 41f, 32f, 41f, 40f,
			13f, 40f
		}, Theme.IconAsh);
		Box(p, 12f, 39f, 42f, 44f, Theme.IconSlate);
		Stroke(p, 14f, 13f, 24f, 13f, 2f, Shade(Theme.IconAsh, 0.35f));
		Stroke(p, 14f, 20f, 25f, 20f, 2f, Shade(Theme.IconAsh, 0.35f));
	}

	private static void SprintIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 30f, 10f, 5.5f, Theme.IconGold);
		Stroke(p, 28f, 17f, 22f, 28f, 4.4f, Theme.IconGold);
		Stroke(p, 22f, 28f, 28f, 41f, 4.2f, Theme.IconGold);
		Stroke(p, 25f, 26f, 15f, 34f, 4.2f, Theme.IconGold);
		Stroke(p, 27f, 20f, 38f, 24f, 4f, Theme.IconAmber);
		Stroke(p, 3f, 16f, 15f, 16f, 2.6f, Theme.IconAmber);
		Stroke(p, 5f, 24f, 14f, 24f, 2.6f, Theme.IconAmber);
	}

	private static void UpdraftIcon(Color32[] p)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[8] { 44f, 12f, 30f, 30f, 8f, 30f, 22f, 12f }, Theme.IconSky);
		Poly(p, new float[6] { 44f, 12f, 30f, 30f, 20f, 30f }, Theme.IconSteel);
		Stroke(p, 12f, 38f, 24f, 38f, 2.6f, Theme.IconSteel);
		Stroke(p, 20f, 44f, 34f, 44f, 2.6f, Theme.IconSteel);
	}

	private static void BookmarkIcon(Color32[] p)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[10] { 11f, 4f, 37f, 4f, 37f, 44f, 24f, 33f, 11f, 44f }, Theme.IconGold);
		Poly(p, new float[8] { 24f, 4f, 37f, 4f, 37f, 44f, 24f, 33f }, Shade(Theme.IconGold, 0.25f));
	}

	private static void CrateIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 6f, 12f, 42f, 42f, 2.5f, Theme.IconAmber);
		Box(p, 6f, 12f, 42f, 19f, Theme.IconGold);
		Stroke(p, 8f, 22f, 40f, 40f, 3f, Shade(Theme.IconAmber, 0.4f));
		Stroke(p, 40f, 22f, 8f, 40f, 3f, Shade(Theme.IconAmber, 0.4f));
	}

	private static void CompanionsIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 32f, 16f, 7f, Theme.IconBone);
		Poly(p, new float[10] { 32f, 24f, 45f, 33f, 45f, 43f, 19f, 43f, 19f, 33f }, Shade(Theme.IconBone, 0.2f));
		Disc(p, 16f, 15f, 8f, Theme.IconOlive);
		Poly(p, new float[10] { 16f, 24f, 30f, 34f, 30f, 44f, 2f, 44f, 2f, 34f }, Theme.IconMoss);
	}

	private static void BiohazardIcon(Color32[] p)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 3; i++)
		{
			float a = -(float)Math.PI / 2f + (float)i * 2f * (float)Math.PI / 3f;
			float cx = 24f + Mathf.Cos(a) * 11f;
			float cy = 24f + Mathf.Sin(a) * 11f;
			Ring(p, cx, cy, 10.5f, 6f, Theme.IconBlood);
		}
		Disc(p, 24f, 24f, 8.5f, Theme.Ink);
		Disc(p, 24f, 24f, 5.5f, Theme.IconBlood);
	}

	private static void PawIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 32f, 11f, Theme.IconOlive);
		Disc(p, 12f, 20f, 5.5f, Theme.IconOlive);
		Disc(p, 20f, 12f, 5.5f, Theme.IconOlive);
		Disc(p, 29f, 12f, 5.5f, Theme.IconOlive);
		Disc(p, 37f, 20f, 5.5f, Theme.IconOlive);
		Disc(p, 21f, 29f, 4f, Shade(Theme.IconMoss, 0.2f));
	}

	private static void CubeIcon(Color32[] p)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Poly(p, new float[8] { 24f, 4f, 43f, 15f, 24f, 26f, 5f, 15f }, Theme.IconSky);
		Poly(p, new float[8] { 5f, 15f, 24f, 26f, 24f, 45f, 5f, 34f }, Theme.IconSteel);
		Poly(p, new float[8] { 43f, 15f, 24f, 26f, 24f, 45f, 43f, 34f }, Shade(Theme.IconSteel, 0.35f));
	}

	private static void ClapperIcon(Color32[] p)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		Box(p, 5f, 20f, 43f, 43f, Theme.IconSlate);
		Poly(p, new float[8] { 5f, 11f, 43f, 5f, 43f, 18f, 5f, 24f }, Theme.IconBone);
		Poly(p, new float[8] { 12f, 10f, 18f, 9f, 14f, 22f, 8f, 23f }, Shade(Theme.IconSlate, 0.3f));
		Poly(p, new float[8] { 26f, 8f, 32f, 7f, 28f, 20f, 22f, 21f }, Shade(Theme.IconSlate, 0.3f));
		Disc(p, 24f, 32f, 6f, Theme.IconGold);
		Tri(p, 22f, 28f, 22f, 36f, 29f, 32f, Theme.IconSlate);
	}

	private static void RosterIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 15f, 16f, 7f, Theme.IconBone);
		Poly(p, new float[10] { 15f, 24f, 28f, 33f, 28f, 43f, 2f, 43f, 2f, 33f }, Theme.IconAsh);
		Disc(p, 34f, 18f, 6f, Shade(Theme.IconBone, 0.15f));
		Poly(p, new float[10] { 34f, 25f, 46f, 33f, 46f, 43f, 24f, 43f, 24f, 33f }, Shade(Theme.IconAsh, 0.25f));
	}

	private static void LinkIcon(Color32[] p)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		Raster(p, 3f, 12f, 30f, 36f, (float x, float y) =>
		{
			float num = (x - 17f) / 13f;
			float num2 = (y - 24f) / 10f;
			float num3 = num * num + num2 * num2;
			return num3 <= 1f && num3 >= 0.4f;
		}, Theme.IconSteel);
		Raster(p, 18f, 12f, 45f, 36f, (float x, float y) =>
		{
			float num = (x - 31f) / 13f;
			float num2 = (y - 24f) / 10f;
			float num3 = num * num + num2 * num2;
			return num3 <= 1f && num3 >= 0.4f;
		}, Theme.IconSky);
	}

	private static void AntennaIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		Stroke(p, 24f, 20f, 24f, 44f, 3.2f, Theme.IconSlate);
		Box(p, 15f, 42f, 33f, 45f, Theme.IconSlate);
		Disc(p, 24f, 18f, 4.4f, Theme.IconRust);
		Arc(p, 24f, 18f, 12f, 9.4f, 200f, 340f, Theme.IconSteel);
		Arc(p, 24f, 18f, 19f, 16.4f, 205f, 335f, Theme.IconSky);
	}

	private static void BookIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 6f, 7f, 42f, 41f, 3f, Theme.IconOlive);
		Box(p, 10f, 10f, 42f, 38f, Theme.IconBone);
		Box(p, 6f, 7f, 12f, 41f, Theme.IconMoss);
		Stroke(p, 16f, 17f, 37f, 17f, 2.2f, Theme.IconAsh);
		Stroke(p, 16f, 24f, 37f, 24f, 2.2f, Theme.IconAsh);
		Stroke(p, 16f, 31f, 30f, 31f, 2.2f, Theme.IconAsh);
	}

	private static void TerminalIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 4f, 8f, 44f, 40f, 3f, Theme.IconSlate);
		Box(p, 7f, 15f, 41f, 37f, Theme.Ink);
		Stroke(p, 4f, 11.5f, 44f, 11.5f, 5f, Theme.IconSlate);
		Disc(p, 9f, 11.5f, 1.8f, Theme.IconRust);
		Disc(p, 15f, 11.5f, 1.8f, Theme.IconGold);
		Disc(p, 21f, 11.5f, 1.8f, Theme.IconOlive);
		Stroke(p, 12f, 21f, 18f, 26f, 2.6f, Theme.IconOlive);
		Stroke(p, 18f, 26f, 12f, 31f, 2.6f, Theme.IconOlive);
		Stroke(p, 22f, 32f, 33f, 32f, 2.6f, Theme.IconOlive);
	}

	private static void HistoryIcon(Color32[] p)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		Arc(p, 24f, 24f, 18f, 14f, 300f, 240f, Theme.IconAsh);
		Poly(p, new float[6] { 24f, 1f, 24f, 15f, 12f, 8f }, Theme.IconAsh);
		Stroke(p, 24f, 24f, 24f, 15f, 2.8f, Theme.IconRust);
		Stroke(p, 24f, 24f, 32f, 27f, 2.8f, Theme.IconRust);
		Disc(p, 24f, 24f, 2.4f, Theme.IconBone);
	}

	private static void ListIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 8f, 11f, 3.2f, Theme.IconRust);
		Disc(p, 8f, 24f, 3.2f, Theme.IconGold);
		Disc(p, 8f, 37f, 3.2f, Theme.IconOlive);
		Stroke(p, 17f, 11f, 42f, 11f, 3.4f, Theme.IconAsh);
		Stroke(p, 17f, 24f, 42f, 24f, 3.4f, Theme.IconAsh);
		Stroke(p, 17f, 37f, 42f, 37f, 3.4f, Theme.IconAsh);
	}

	private static void KeyIcon(Color32[] p)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		Ring(p, 15f, 17f, 11f, 5.5f, Theme.IconGold);
		Stroke(p, 21f, 24f, 41f, 41f, 4.6f, Theme.IconGold);
		Stroke(p, 33f, 40f, 39f, 33f, 4.2f, Theme.IconAmber);
		Stroke(p, 27f, 35f, 33f, 28f, 4.2f, Theme.IconAmber);
		Disc(p, 12f, 14f, 2.6f, Lift(Theme.IconGold, 0.5f));
	}

	private static void PaletteIcon(Color32[] p)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		Disc(p, 24f, 24f, 20f, Theme.IconBone);
		Disc(p, 33f, 32f, 6.5f, Theme.Ink);
		Disc(p, 15f, 15f, 4.2f, Theme.IconRust);
		Disc(p, 27f, 12f, 4.2f, Theme.IconGold);
		Disc(p, 36f, 20f, 4.2f, Theme.IconOlive);
		Disc(p, 12f, 27f, 4.2f, Theme.IconSteel);
		Disc(p, 18f, 36f, 4.2f, Theme.IconBlood);
	}

	private static void MouseIcon(Color32[] p)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		RBox(p, 12f, 5f, 36f, 43f, 12f, Theme.IconBone);
		Stroke(p, 24f, 6f, 24f, 20f, 2f, Theme.IconAsh);
		Stroke(p, 13f, 20f, 35f, 20f, 2f, Theme.IconAsh);
		RBox(p, 22f, 10f, 26f, 19f, 2f, Theme.IconRust);
		Stroke(p, 41f, 14f, 41f, 24f, 2.4f, Theme.IconAsh);
		Poly(p, new float[6] { 41f, 10f, 44f, 15f, 38f, 15f }, Theme.IconAsh);
		Poly(p, new float[6] { 41f, 28f, 44f, 23f, 38f, 23f }, Theme.IconAsh);
	}

	private void Group(string title)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected Obj, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		Cyberfox1337xWidgetFunction();
		GUILayout.Space(6f);
		string t = title.ToUpper();
		if (_sectionTex == null)
		{
			_sectionTex = new Dictionary<string, Texture2D>();
		}
		Texture2D icon = null;
		if (SectionIcons.TryGetValue(t, out var draw) && !_sectionTex.TryGetValue(t, out icon))
		{
			icon = BuildIcon(draw);
			_sectionTex[t] = icon;
		}
		Rect hr = GUILayoutUtility.GetRect(1f, 26f, new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
		float iconY = Mathf.Round(hr.y + (hr.height - 18f) * 0.5f + 2f);
		Rect iconRect = new Rect(hr.x + 2f, iconY, 18f, 18f);
		Color color = GUI.color;
		if ((Object)(object)icon != (Object)null)
		{
			GUI.color = Color.white;
			GUI.DrawTexture(iconRect, (Texture)(object)icon);
		}
		else if (SectionIcons.ContainsKey(t))
		{
			GUI.color = Theme.IconAsh;
			GUI.DrawTexture(new Rect(iconRect.x + 2f, iconRect.y + 2f, iconRect.width - 4f, iconRect.height - 4f), (Texture)(object)Texture2D.whiteTexture);
		}
		GUIContent titleC = new GUIContent(t);
		float titleW = Theme.GroupLabel.CalcSize(titleC).x;
		float num = iconRect.xMax + 7f;
		GUI.color = Color.white;
		GUI.Label(new Rect(num, hr.y, titleW, hr.height), titleC, Theme.GroupLabel);
		float lineX = num + titleW + 10f;
		float lineY = Mathf.Round(hr.center.y) + 1f;
		float lineW = Mathf.Max(0f, hr.xMax - lineX);
		if (lineW > 0f)
		{
			GUI.color = Theme.Line;
			GUI.DrawTexture(new Rect(lineX, lineY, lineW, 1f), (Texture)(object)Texture2D.whiteTexture);
		}
		GUI.color = color;
		GUILayout.Space(2f);
	}

	private void Lede(string text)
	{
		GUILayout.Label(text, Theme.Lede, (Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Space(4f);
	}

	private static Texture2D CircleTex()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected Obj, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_circleTex != (Object)null)
		{
			return _circleTex;
		}
		_circleTex = new Texture2D(16, 16, (TextureFormat)4, false);
		((Texture)_circleTex).filterMode = (FilterMode)1;
		Color32[] px = new Color32[256];
		for (int y = 0; y < 16; y++)
		{
			for (int x = 0; x < 16; x++)
			{
				float num = (float)x + 0.5f - 8f;
				float dy = (float)y + 0.5f - 8f;
				float dist = Mathf.Sqrt(num * num + dy * dy);
				float a = Mathf.Clamp01(8f - dist + 0.5f);
				px[y * 16 + x] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(a * 255f));
			}
		}
		_circleTex.SetPixels32(new Il2CppStructArray<Color32>(px));
		_circleTex.Apply();
		return _circleTex;
	}

	private static Texture2D PillTex()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected Obj, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_pillTex != (Object)null)
		{
			return _pillTex;
		}
		int w = 144;
		int h = 72;
		float radius = (float)h * 0.5f;
		Color32[] px = new Color32[w * h];
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				float num = (float)x + 0.5f;
				float fy = (float)y + 0.5f;
				float spineX = Mathf.Clamp(num, radius, (float)w - radius);
				float num2 = num - spineX;
				float dy = fy - radius;
				float dist = Mathf.Sqrt(num2 * num2 + dy * dy);
				float a = Mathf.Clamp01(radius - dist + 0.5f);
				px[y * w + x] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(a * 255f));
			}
		}
		_pillTex = new Texture2D(w, h, (TextureFormat)4, false);
		((Texture)_pillTex).filterMode = (FilterMode)1;
		((Texture)_pillTex).wrapMode = (TextureWrapMode)1;
		((Object)_pillTex).hideFlags = (HideFlags)52;
		_pillTex.SetPixels32(new Il2CppStructArray<Color32>(px));
		_pillTex.Apply();
		return _pillTex;
	}

	private static float EaseOut(float t)
	{
		return 1f - (1f - t) * (1f - t) * (1f - t);
	}

	private bool Switch(string label, bool value, bool danger = false)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		if (!_toggleAnim.TryGetValue(label, out var raw))
		{
			raw = (value ? 1f : 0f);
		}
		float target = (value ? 1f : 0f);
		raw = Mathf.MoveTowards(raw, target, 8f * Time.deltaTime);
		_toggleAnim[label] = raw;
		float anim = EaseOut(raw);
		Color val = new Color(0.22f, 0.22f, 0.2f, 0.7f);
		Color onCol = (danger ? Theme.RustBright : Theme.Live);
		Color trackCol = Color.Lerp(val, onCol, anim);
		GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(26f) });
		GUIStyle labelStyle;
		if (raw > 0.4f)
		{
			labelStyle = (danger ? Theme.SwitchDanger : Theme.SwitchOn);
		}
		else
		{
			labelStyle = Theme.SwitchOff;
		}
		if (GUILayout.Button(label, labelStyle, new GUILayoutOption[2]
		{
			GUILayout.Height(26f),
			GUILayout.ExpandWidth(true)
		}))
		{
			value = !value;
		}
		GUILayout.Space(2f);
		Rect toggleRect = GUILayoutUtility.GetRect(40f, 26f, new GUILayoutOption[2]
		{
			GUILayout.Width(40f),
			GUILayout.Height(26f)
		});
		float ty = toggleRect.y + (toggleRect.height - 18f) * 0.5f;
		Rect track = new Rect(toggleRect.x, ty, 36f, 18f);
		Color color = GUI.color;
		GUI.color = trackCol;
		GUI.DrawTexture(track, (Texture)(object)PillTex());
		GUI.color = color;
		float num = track.x + 2f + anim * 18f;
		float thumbScale = 1f + 0.1f * (1f - Mathf.Abs(anim - 0.5f) * 2f);
		float scaledThumbD = 14f * thumbScale;
		float scaledOffset = (scaledThumbD - 14f) * 0.5f;
		GUI.color = Theme.Bone;
		GUI.DrawTexture(new Rect(num - scaledOffset, track.y + 2f - scaledOffset, scaledThumbD, scaledThumbD), (Texture)(object)CircleTex());
		GUI.color = color;
		if ((int)Event.current.type == 0 && Event.current.button == 0 && toggleRect.Contains(Event.current.mousePosition))
		{
			value = !value;
			Event.current.Use();
		}
		GUILayout.EndHorizontal();
		return value;
	}

	[HideFromIl2Cpp]
	private void SwitchRows(ToggleCommand[] rows)
	{
		for (int i = 0; i < rows.Length; i++)
		{
			bool cur = Features.GetCmd(rows[i].Command);
			bool next = Switch(rows[i].Label, cur, rows[i].Danger);
			if (next != cur)
			{
				Features.SetCmd(rows[i].Command, next);
			}
		}
	}

	[HideFromIl2Cpp]
	private float ColW(int cols = 2, float gap = 6f)
	{
		return (W - 150f - 2f - 18f - 32f - (float)(cols - 1) * gap) / (float)cols;
	}

	private bool FullButton(string label, GUIStyle style = null, float height = 26f)
	{
		return GUILayout.Button(label.ToUpper(), style ?? Theme.BtnPrimary, new GUILayoutOption[1] { GUILayout.Height(height) });
	}

	[HideFromIl2Cpp]
	private void GearApply(string label, Action click)
	{
		GUILayout.Space(4f);
		if (GUILayout.Button(label.ToUpper(), Theme.BtnPrimary, new GUILayoutOption[1] { GUILayout.Height(26f) }))
		{
			click();
		}
	}

	[HideFromIl2Cpp]
	private void ActionRows(ActionCommand[] rows)
	{
		int perRow = 2;
		float gap = 6f;
		float eachW = ColW(perRow, gap);
		for (int i = 0; i < rows.Length; i += perRow)
		{
			GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
			for (int k = i; k < i + perRow && k < rows.Length; k++)
			{
				if (GUILayout.Button(rows[k].Label.ToUpper(), Theme.Btn, new GUILayoutOption[2]
				{
					GUILayout.Height(26f),
					GUILayout.Width(eachW)
				}))
				{
					Features.RunCmd(rows[k].Command);
				}
				if (k < i + perRow - 1 && k < rows.Length - 1)
				{
					GUILayout.Space(gap);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
		}
	}

	[HideFromIl2Cpp]
	private string HintField(string controlName, string value, string hint, params GUILayoutOption[] options)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		GUI.SetNextControlName(controlName);
		string text = GUILayout.TextField(value ?? string.Empty, Theme.Field, options);
		Rect field = GUILayoutUtility.GetLastRect();
		if (string.IsNullOrEmpty(text) && GUI.GetNameOfFocusedControl() != controlName && (int)Event.current.type == 7)
		{
			GUI.Label(new Rect(field.x, field.y, field.width, field.height), hint, Theme.FieldHint);
		}
		return text;
	}

	[HideFromIl2Cpp]
	private void ValueRows(ValueCommand[] rows, string key)
	{
		for (int i = 0; i < rows.Length; i++)
		{
			string id = key + i;
			if (!_fieldValues.TryGetValue(id, out var cur))
			{
				cur = rows[i].DefaultValue;
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(24f) });
			GUILayout.Label(rows[i].Label, Theme.DialLabel, new GUILayoutOption[1] { GUILayout.Width(190f) });
			cur = GUILayout.TextField(cur, Theme.Field, new GUILayoutOption[1] { GUILayout.Width(64f) });
			GUILayout.Space(6f);
			if (GUILayout.Button("APPLY", Theme.Btn, new GUILayoutOption[2]
			{
				GUILayout.Width(76f),
				GUILayout.Height(22f)
			}))
			{
				Features.RunCmd(rows[i].Command + " " + cur.Trim());
			}
			GUILayout.EndHorizontal();
			_fieldValues[id] = cur;
			GUILayout.Space(2f);
		}
	}

	[HideFromIl2Cpp]
	private void WorldChoiceRow(string key, string label, WorldChoice[] choices, WorldControlAvailability availability, ref int selected)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Invalid comparison between Unknown and I4
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		if (choices != null && choices.Length != 0)
		{
			selected = Mathf.Clamp(selected, 0, choices.Length - 1);
			bool open = _worldDropdown == key;
			float availableWidth = Mathf.Max(0f, W - 150f - 2f - 32f);
			WorldRowGeometry geometry = WorldRowLayout.Calculate(availableWidth);
			Rect row = GUILayoutUtility.GetRect(availableWidth, 24f, new GUILayoutOption[2]
			{
				GUILayout.Width(availableWidth),
				GUILayout.Height(24f)
			});
			Rect labelRect = new Rect(row.x, row.y, geometry.LabelWidth, 22f);
			Rect selector = new Rect(labelRect.xMax, row.y, geometry.SelectorWidth, 22f);
			Rect actionRect = new Rect(selector.xMax + geometry.Gap, row.y, geometry.ActionWidth, 22f);
			GUI.Label(labelRect, label, Theme.DialLabel);
			if ((int)Event.current.type == 7)
			{
				Theme.Fill(selector, Theme.FieldBg);
				Theme.Box(selector, open ? Theme.Rust : Theme.Line);
			}
			if (GUI.Button(selector, choices[selected].Label, Theme.DropdownRow))
			{
				open = !open;
				_worldDropdown = (open ? key : string.Empty);
			}
			GUI.Label(new Rect(selector.xMax - 24f, selector.y, 16f, selector.height), open ? "^" : "v", Theme.RowTag);
			bool previousEnabled = GUI.enabled;
			bool apply;
			try
			{
				GUI.enabled = previousEnabled && WorldRules.CanApply(availability);
				GUIStyle actionStyle = (WorldRules.CanApply(availability) ? Theme.Btn : WorldLockedActionStyle());
				apply = GUI.Button(actionRect, WorldRules.ApplyLabel(availability), actionStyle);
			}
			finally
			{
				GUI.enabled = previousEnabled;
			}
			if (apply)
			{
				ApplyWorldChoice(key, choices[selected], availability);
			}
			if (open)
			{
				WorldChoiceOptions(key, choices, geometry, availableWidth, ref selected);
			}
			GUILayout.Space(2f);
		}
	}

	private static GUIStyle WorldLockedActionStyle()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected Obj, but got Unknown
		//IL_0033: Expected Obj, but got Unknown
		if (_worldLockedActionStyle != null)
		{
			return _worldLockedActionStyle;
		}
		_worldLockedActionStyle = new GUIStyle(Theme.Btn)
		{
			fontSize = 11,
			padding = new RectOffset(6, 6, 3, 3)
		};
		return _worldLockedActionStyle;
	}

	[HideFromIl2Cpp]
	private void WorldChoiceOptions(string key, WorldChoice[] choices, WorldRowGeometry geometry, float availableWidth, ref int selected)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		int columns = WorldRowLayout.OptionColumns(choices.Length, geometry.SelectorWidth);
		if (columns == 0)
		{
			return;
		}
		float optionGapsWidth = geometry.Gap * (float)(columns - 1);
		float optionWidth = Math.Max(0f, (geometry.SelectorWidth - optionGapsWidth) / (float)columns);
		for (int first = 0; first < choices.Length; first += columns)
		{
			Rect row = GUILayoutUtility.GetRect(availableWidth, 24f, new GUILayoutOption[2]
			{
				GUILayout.Width(availableWidth),
				GUILayout.Height(24f)
			});
			float selectorX = row.x + geometry.LabelWidth;
			for (int column = 0; column < columns; column++)
			{
				int index = first + column;
				if (index >= choices.Length)
				{
					break;
				}
				GUIStyle style = ((index == selected) ? Theme.ChipOn : Theme.ChipOff);
				if (GUI.Button(new Rect(selectorX + (float)column * (optionWidth + geometry.Gap), row.y, optionWidth, 22f), choices[index].Label, style))
				{
					selected = index;
					_worldDropdown = string.Empty;
				}
			}
		}
	}

	private static void ApplyWorldChoice(string key, WorldChoice choice, WorldControlAvailability availability)
	{
		if (availability != WorldControlAvailability.Ready)
		{
			Features.LastResult = WorldRules.DenialMessage(availability);
			return;
		}
		switch (key)
		{
		case "time":
		{
			int hour = Mathf.FloorToInt(choice.Value);
			int minute = Mathf.RoundToInt((choice.Value - (float)hour) * 60f);
			DirectGameApi.SetTimeOfDay(hour, minute);
			break;
		}
		case "lighting":
			DirectGameApi.SetLightingOverride(choice.Token, choice.Value);
			break;
		case "day":
			DirectGameApi.SetCurrentDay(Mathf.RoundToInt(choice.Value));
			break;
		case "jump":
			DirectGameApi.JumpTime(choice.Value);
			break;
		case "speed":
			DirectGameApi.SetTimeSpeed(choice.Value);
			break;
		case "wind":
			DirectGameApi.SetWind(choice.Value);
			break;
		case "clouds":
			DirectGameApi.SetClouds(choice.Value);
			break;
		case "forecast":
			DirectGameApi.SetForecast(choice.Token);
			break;
		case "difficulty":
			DirectGameApi.SetDifficulty(choice.Token);
			break;
		case "mode":
			DirectGameApi.SetGameMode(choice.Token);
			break;
		case "season":
			DirectGameApi.SetSeasonDirect(choice.Token);
			break;
		default:
			Features.LastResult = "Unknown world control";
			break;
		}
	}

	[HideFromIl2Cpp]
	private void SetupRows((string Label, string Setting, string Def)[] rows, string key)
	{
		for (int i = 0; i < rows.Length; i++)
		{
			string id = key + i;
			if (!_fieldValues.TryGetValue(id, out var cur))
			{
				cur = rows[i].Def;
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[1] { GUILayout.Height(24f) });
			GUILayout.Label(rows[i].Label, Theme.DialLabel, new GUILayoutOption[1] { GUILayout.Width(190f) });
			cur = GUILayout.TextField(cur, Theme.Field, new GUILayoutOption[1] { GUILayout.Width(64f) });
			GUILayout.Space(6f);
			if (GUILayout.Button("APPLY", Theme.Btn, new GUILayoutOption[2]
			{
				GUILayout.Width(76f),
				GUILayout.Height(22f)
			}))
			{
				Features.RunCmd("setGameSetupSetting " + rows[i].Setting + " " + cur.Trim());
			}
			GUILayout.EndHorizontal();
			_fieldValues[id] = cur;
			GUILayout.Space(2f);
		}
	}

	private float Dial(string label, float value, float min, float max, string fmt = "0.00")
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Invalid comparison between Unknown and I4
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginHorizontal((Il2CppReferenceArray<GUILayoutOption>)null);
		GUILayout.Label(label, Theme.DialLabel, new GUILayoutOption[1] { GUILayout.Width(200f) });
		GUILayout.FlexibleSpace();
		Theme.DialValue.normal.textColor = Theme.Ramp((value - min) / (max - min));
		GUILayout.Label(value.ToString(fmt), Theme.DialValue, new GUILayoutOption[1] { GUILayout.Width(62f) });
		GUILayout.EndHorizontal();
		Rect r = GUILayoutUtility.GetRect(0f, 16f, new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
		float t = Mathf.Clamp01((value - min) / (max - min));
		Rect view = r;
		int id = GUIUtility.GetControlID((FocusType)2);
		Event e = Event.current;
		if ((int)e.type == 0 && e.button == 0 && view.Contains(e.mousePosition))
		{
			GUIUtility.hotControl = id;
			float p = Mathf.Clamp01((e.mousePosition.x - view.x) / view.width);
			value = Mathf.Lerp(min, max, p);
			e.Use();
		}
		if ((int)e.type == 3 && GUIUtility.hotControl == id)
		{
			float p2 = Mathf.Clamp01((e.mousePosition.x - view.x) / view.width);
			value = Mathf.Lerp(min, max, p2);
			e.Use();
		}
		if ((int)e.type == 1 && GUIUtility.hotControl == id)
		{
			GUIUtility.hotControl = 0;
			e.Use();
		}
		Theme.Fill(new Rect(r.x, r.y + 6f, r.width, 4f), Theme.Line);
		Theme.Fill(new Rect(r.x, r.y + 6f, r.width * t, 4f), Theme.Ramp(t));
		bool hot = GUIUtility.hotControl == id;
		Theme.Fill(new Rect(r.x + r.width * t - 5f, r.y + 1f, 10f, 14f), (Color)(hot ? Theme.RustBright : new Color(0.72f, 0.72f, 0.65f, 0.95f)));
		GUILayout.Space(6f);
		return value;
	}

	static MenuWindow()
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected Obj, but got Unknown
	}
}
