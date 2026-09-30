using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class Theme
{
	public static readonly Color Ink = Hex(10, 10, 8);

	public static readonly Color Paper = Hex(216, 209, 189);

	public static readonly Color Bone = Hex(238, 232, 216);

	public static readonly Color Ash = Hex(170, 164, 147);

	public static readonly Color Rust = Hex(181, 52, 35);

	public static readonly Color RustBright = Hex(224, 70, 47);

	public static readonly Color Olive = Hex(129, 131, 106);

	public static readonly Color Live = Hex(142, 162, 86);

	public static readonly Color Dead = Hex(92, 89, 80);

	public static readonly Color Panel = new Color(0.047f, 0.047f, 0.039f, 0.94f);

	public static readonly Color Line = new Color(0.882f, 0.847f, 0.745f, 0.2f);

	public static readonly Color RailActive = new Color(0.71f, 0.2f, 0.14f, 0.22f);

	public static readonly Color RailPressed = new Color(0.71f, 0.2f, 0.14f, 0.38f);

	public static readonly Color RowHover = new Color(0.847f, 0.82f, 0.741f, 0.06f);

	public static readonly Color LiveWash = new Color(0.557f, 0.635f, 0.337f, 0.07f);

	public static readonly Color RustWash = new Color(0.71f, 0.2f, 0.14f, 0.12f);

	public static readonly Color FieldBg = new Color(0f, 0f, 0f, 0.45f);

	public static readonly Color IconBone = Hex(238, 232, 216);

	public static readonly Color IconAsh = Hex(170, 164, 147);

	public static readonly Color IconSlate = Hex(110, 106, 96);

	public static readonly Color IconRust = Hex(224, 70, 47);

	public static readonly Color IconEmber = Hex(158, 44, 28);

	public static readonly Color IconOlive = Hex(142, 162, 86);

	public static readonly Color IconMoss = Hex(90, 107, 53);

	public static readonly Color IconGold = Hex(224, 176, 47);

	public static readonly Color IconAmber = Hex(168, 122, 28);

	public static readonly Color IconSteel = Hex(108, 147, 168);

	public static readonly Color IconSky = Hex(166, 201, 218);

	public static readonly Color IconBlood = Hex(160, 58, 70);

	private static readonly Color RampRed = Hex(224, 70, 47);

	private static readonly Color RampYellow = Hex(224, 192, 47);

	private static readonly Color RampGreen = Hex(111, 191, 74);

	public static Texture2D TexPanel;

	public static Texture2D TexLine;

	public static Texture2D TexRail;

	public static Texture2D TexRailPressed;

	public static Texture2D TexHover;

	public static Texture2D TexLiveWash;

	public static Texture2D TexRustWash;

	public static Texture2D TexField;

	public static Texture2D TexLive;

	public static Texture2D TexDead;

	public static Texture2D TexTransparent;

	public static Texture2D TexWhite;

	public static Texture2D TexInk;

	public static Texture2D TexRoundFill;

	public static Texture2D TexRoundStroke;

	public static Texture2D TexRoundFillMirror;

	public static Texture2D TexRoundStrokeMirror;

	public static Texture2D TexRoundFillBot;

	public static Texture2D TexRoundStrokeBot;

	public static Texture2D TexRoundFillMirrorBot;

	public static Texture2D TexRoundStrokeMirrorBot;

	public static Texture2D TexBtn;

	public static Texture2D TexBtnHover;

	public static Texture2D TexBtnRustWash;

	public static Texture2D TexBtnInk;

	public static Texture2D TexBtnRail;

	public static Texture2D TexBtnRailPressed;

	public static GUIStyle Wordmark;

	public static GUIStyle Build;

	public static GUIStyle GroupLabel;

	public static GUIStyle Lede;

	public static GUIStyle TabOff;

	public static GUIStyle TabOn;

	public static GUIStyle SwitchOff;

	public static GUIStyle SwitchOn;

	public static GUIStyle SwitchDanger;

	public static GUIStyle StateOff;

	public static GUIStyle StateOn;

	public static GUIStyle Btn;

	public static GUIStyle BtnPrimary;

	public static GUIStyle ChipOff;

	public static GUIStyle ChipOn;

	public static GUIStyle RowStyle;

	public static GUIStyle RowId;

	public static GUIStyle RowTag;

	public static GUIStyle DialLabel;

	public static GUIStyle DialValue;

	public static GUIStyle Field;

	public static GUIStyle FieldHint;

	public static GUIStyle StatusText;

	public static GUIStyle Kbd;

	public static GUIStyle Watermark;

	public static GUIStyle EspLabel;

	public static GUIStyle DropdownRow;

	private static bool _built;

	private static Font _display;

	private static Font _body;

	private static Font _mono;

	private const int BtnTex = 16;

	private const int BtnRadius = 6;

	private const int BtnBorder = 8;

	public static int RoundR = 10;

	private const int RoundT = 2;

	public static int RoundS => RoundR + 2;

	private static Color Hex(int r, int g, int b)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, 1f);
	}

	public static Color Ramp(float t)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		t = Mathf.Clamp01(t);
		if (!(t < 0.5f))
		{
			return Color.Lerp(RampYellow, RampGreen, (t - 0.5f) * 2f);
		}
		return Color.Lerp(RampRed, RampYellow, t * 2f);
	}

	public static void Build_()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected Obj, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected Obj, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected Obj, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected Obj, but got Unknown
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected Obj, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected Obj, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected Obj, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected Obj, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected Obj, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Expected Obj, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected Obj, but got Unknown
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected Obj, but got Unknown
		//IL_04f7: Expected Obj, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected Obj, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected Obj, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Expected Obj, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected Obj, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected Obj, but got Unknown
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Expected Obj, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected Obj, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Expected Obj, but got Unknown
		//IL_07fb: Expected Obj, but got Unknown
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Expected Obj, but got Unknown
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Expected Obj, but got Unknown
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		if (!_built)
		{
			TexPanel = Solid(Panel);
			TexLine = Solid(Line);
			TexRail = Solid(RailActive);
			TexRailPressed = Solid(RailPressed);
			TexHover = Solid(RowHover);
			TexLiveWash = Solid(LiveWash);
			TexRustWash = Solid(RustWash);
			TexField = Solid(FieldBg);
			TexLive = Solid(Live);
			TexDead = Solid(Dead);
			TexInk = Solid(Ink);
			TexWhite = Solid(Color.white);
			TexTransparent = Solid(new Color(0f, 0f, 0f, 0f));
			RebuildRoundTextures();
			TexBtn = RoundedButton(Line);
			TexBtnHover = RoundedButton(RowHover);
			TexBtnRustWash = RoundedButton(RustWash);
			TexBtnInk = RoundedButton(Ink);
			TexBtnRail = RoundedButton(RailActive);
			TexBtnRailPressed = RoundedButton(RailPressed);
			_display = OsFont(new string[3] { "Haettenschweiler", "Arial Narrow", "Impact" }, 22);
			_body = OsFont(new string[2] { "Georgia", "Times New Roman" }, 14);
			_mono = OsFont(new string[2] { "Consolas", "Courier New" }, 12);
			Wordmark = new GUIStyle
			{
				font = _display,
				fontSize = 30,
				alignment = (TextAnchor)3,
				richText = true,
				wordWrap = false
			};
			Wordmark.normal.textColor = Paper;
			Build = Label(_mono, 11, Ash);
			Watermark = Label(_display, 18, Paper);
			Lede = Label(_body, 13, Ash);
			Lede.wordWrap = true;
			Lede.padding = new RectOffset(0, 0, 0, 8);
			GroupLabel = Label(_display, 15, Olive);
			GroupLabel.padding = new RectOffset(0, 0, 8, 4);
			TabOff = new GUIStyle
			{
				font = _display,
				fontSize = 18,
				alignment = (TextAnchor)4,
				padding = new RectOffset(0, 0, 6, 6),
				wordWrap = false
			};
			TabOff.normal.textColor = Ash;
			TabOff.hover.textColor = Bone;
			TabOff.hover.background = TexHover;
			TabOn = new GUIStyle(TabOff);
			TabOn.normal.textColor = Paper;
			TabOn.normal.background = TexRail;
			TabOn.hover.textColor = Paper;
			TabOn.hover.background = TexRail;
			SwitchOff = new GUIStyle
			{
				font = _body,
				fontSize = 13,
				alignment = (TextAnchor)3,
				padding = new RectOffset(10, 8, 6, 6),
				wordWrap = false
			};
			SwitchOff.normal.textColor = Ash;
			SwitchOff.normal.background = TexTransparent;
			SwitchOff.hover.textColor = Bone;
			SwitchOff.hover.background = TexHover;
			SwitchOn = new GUIStyle(SwitchOff);
			SwitchOn.normal.textColor = Bone;
			SwitchOn.normal.background = TexLiveWash;
			SwitchOn.hover.textColor = Bone;
			SwitchOn.hover.background = TexLiveWash;
			SwitchDanger = new GUIStyle(SwitchOn);
			SwitchDanger.normal.background = TexRustWash;
			SwitchDanger.hover.background = TexRustWash;
			StateOff = Label(_mono, 9, Dead);
			StateOff.alignment = (TextAnchor)5;
			StateOn = Label(_mono, 9, Live);
			StateOn.alignment = (TextAnchor)5;
			Btn = new GUIStyle
			{
				font = _display,
				fontSize = 16,
				alignment = (TextAnchor)4,
				padding = new RectOffset(12, 12, 5, 5),
				border = new RectOffset(8, 8, 8, 8)
			};
			Btn.normal.textColor = Bone;
			Btn.normal.background = TexBtn;
			Btn.hover.textColor = Paper;
			Btn.hover.background = TexBtnRail;
			Btn.active.textColor = Bone;
			Btn.active.background = TexBtnRailPressed;
			BtnPrimary = new GUIStyle(Btn);
			ChipOff = new GUIStyle(Btn)
			{
				fontSize = 15
			};
			ChipOff.normal.background = TexBtnInk;
			ChipOff.normal.textColor = Ash;
			ChipOff.hover.background = TexBtnHover;
			ChipOff.hover.textColor = Bone;
			ChipOn = new GUIStyle(ChipOff);
			ChipOn.normal.background = TexBtnRustWash;
			ChipOn.normal.textColor = Paper;
			RowStyle = new GUIStyle
			{
				font = _body,
				fontSize = 13,
				alignment = (TextAnchor)3,
				padding = new RectOffset(10, 8, 4, 4),
				wordWrap = false
			};
			RowStyle.normal.textColor = Bone;
			RowStyle.hover.background = TexRail;
			RowStyle.hover.textColor = Paper;
			RowStyle.onNormal.background = TexRailPressed;
			RowStyle.onNormal.textColor = Paper;
			RowStyle.onHover.background = TexRailPressed;
			RowStyle.onHover.textColor = Paper;
			DropdownRow = new GUIStyle
			{
				font = _body,
				fontSize = 13,
				alignment = (TextAnchor)3,
				padding = new RectOffset(10, 8, 3, 3),
				wordWrap = false
			};
			DropdownRow.normal.textColor = Paper;
			DropdownRow.hover.textColor = Paper;
			DropdownRow.hover.background = TexRail;
			RowId = Label(_mono, 11, Ash);
			RowTag = Label(_mono, 9, Dead);
			RowTag.alignment = (TextAnchor)5;
			DialLabel = Label(_body, 13, Bone);
			DialValue = Label(_mono, 12, Paper);
			DialValue.alignment = (TextAnchor)5;
			Field = new GUIStyle
			{
				font = _mono,
				fontSize = 12,
				alignment = (TextAnchor)3,
				padding = new RectOffset(7, 7, 4, 4)
			};
			Field.normal.textColor = Paper;
			Field.normal.background = TexField;
			Field.focused.textColor = Paper;
			Field.focused.background = TexField;
			FieldHint = new GUIStyle(Field);
			FieldHint.normal.background = null;
			FieldHint.focused.background = null;
			FieldHint.normal.textColor = Dead;
			FieldHint.focused.textColor = Dead;
			StatusText = Label(_mono, 10, Dead);
			Kbd = Label(_mono, 12, Paper);
			Kbd.alignment = (TextAnchor)4;
			Kbd.normal.background = TexLine;
			Kbd.padding = new RectOffset(5, 5, 2, 2);
			EspLabel = Label(_mono, 11, Bone);
			EspLabel.alignment = (TextAnchor)4;
			_built = true;
		}
	}

	private static GUIStyle Label(Font f, int size, Color c)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected Obj, but got Unknown
		GUIStyle val = new GUIStyle
		{
			font = f,
			fontSize = size,
			alignment = (TextAnchor)3,
			wordWrap = false
		};
		val.normal.textColor = c;
		return val;
	}

	private static Font OsFont(string[] names, int size)
	{
		for (int i = 0; i < names.Length; i++)
		{
			try
			{
				Font f = Font.CreateDynamicFontFromOSFont(names[i], size);
				if ((Object)(object)f != (Object)null)
				{
					return f;
				}
			}
			catch
			{
			}
		}
		return null;
	}

	private static Texture2D RoundedButton(Color c)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		Texture2D tex = new Texture2D(16, 16, (TextureFormat)4, false);
		float r = 6f;
		for (int y = 0; y < 16; y++)
		{
			for (int x = 0; x < 16; x++)
			{
				float px = (float)x + 0.5f;
				float py = (float)(16 - y) - 0.5f;
				float cx = Mathf.Min(px, 16f - px);
				float cy = Mathf.Min(py, 16f - py);
				float a = 1f;
				if (cx < r && cy < r)
				{
					float num = cx - r;
					float dy = cy - r;
					float dist = Mathf.Sqrt(num * num + dy * dy);
					a = Mathf.Clamp01(r - dist + 0.5f);
				}
				tex.SetPixel(x, y, new Color(c.r, c.g, c.b, c.a * a));
			}
		}
		((Texture)tex).filterMode = (FilterMode)1;
		((Texture)tex).wrapMode = (TextureWrapMode)1;
		((Object)tex).hideFlags = (HideFlags)61;
		tex.Apply();
		return tex;
	}

	private static Texture2D Solid(Color c)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected Obj, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Texture2D t = new Texture2D(2, 2, (TextureFormat)4, false);
		for (int x = 0; x < 2; x++)
		{
			for (int y = 0; y < 2; y++)
			{
				t.SetPixel(x, y, c);
			}
		}
		((Texture)t).filterMode = (FilterMode)0;
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		return t;
	}

	public static void Rule(Rect r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Color color = GUI.color;
		GUI.color = Line;
		GUI.DrawTexture(r, (Texture)(object)TexWhite);
		GUI.color = color;
	}

	public static void Fill(Rect r, Color c)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Color color = GUI.color;
		GUI.color = c;
		GUI.DrawTexture(r, (Texture)(object)TexWhite);
		GUI.color = color;
	}

	public static void Box(Rect r, Color c, float w = 1f)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		Fill(new Rect(r.x, r.y, r.width, w), c);
		Fill(new Rect(r.x, r.yMax - w, r.width, w), c);
		Fill(new Rect(r.x, r.y, w, r.height), c);
		Fill(new Rect(r.xMax - w, r.y, w, r.height), c);
	}

	public static void RebuildRoundTextures()
	{
		TexRoundFill = RoundTexture(stroke: false, mirror: false, bottom: false);
		TexRoundFillMirror = RoundTexture(stroke: false, mirror: true, bottom: false);
		TexRoundStroke = RoundTexture(stroke: true, mirror: false, bottom: false);
		TexRoundStrokeMirror = RoundTexture(stroke: true, mirror: true, bottom: false);
		TexRoundFillBot = RoundTexture(stroke: false, mirror: false, bottom: true);
		TexRoundFillMirrorBot = RoundTexture(stroke: false, mirror: true, bottom: true);
		TexRoundStrokeBot = RoundTexture(stroke: true, mirror: false, bottom: true);
		TexRoundStrokeMirrorBot = RoundTexture(stroke: true, mirror: true, bottom: true);
	}

	public static void RoundedFill(Rect r, Color c)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		if (r.width < (float)(RoundS * 2) || r.height < (float)(RoundS * 2))
		{
			Fill(r, c);
			return;
		}
		float s = RoundS;
		Color color = GUI.color;
		GUI.color = c;
		GUI.DrawTexture(new Rect(r.x, r.y, s, s), (Texture)(object)TexRoundFill);
		GUI.DrawTexture(new Rect(r.xMax - s, r.y, s, s), (Texture)(object)TexRoundFillMirror);
		GUI.DrawTexture(new Rect(r.x, r.yMax - s, s, s), (Texture)(object)TexRoundFillBot);
		GUI.DrawTexture(new Rect(r.xMax - s, r.yMax - s, s, s), (Texture)(object)TexRoundFillMirrorBot);
		GUI.color = color;
		Fill(new Rect(r.x + s, r.y, r.width - 2f * s, s), c);
		Fill(new Rect(r.x + s, r.yMax - s, r.width - 2f * s, s), c);
		Fill(new Rect(r.x, r.y + s, s, r.height - 2f * s), c);
		Fill(new Rect(r.xMax - s, r.y + s, s, r.height - 2f * s), c);
		Fill(new Rect(r.x + s, r.y + s, r.width - 2f * s, r.height - 2f * s), c);
	}

	public static void RoundedBox(Rect r, Color c)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		if (r.width < (float)(RoundS * 2) || r.height < (float)(RoundS * 2))
		{
			Box(r, c, 2f);
			return;
		}
		float s = RoundS;
		float w = 2f;
		Color color = GUI.color;
		GUI.color = c;
		GUI.DrawTexture(new Rect(r.x, r.y, s, s), (Texture)(object)TexRoundStroke);
		GUI.DrawTexture(new Rect(r.xMax - s, r.y, s, s), (Texture)(object)TexRoundStrokeMirror);
		GUI.DrawTexture(new Rect(r.x, r.yMax - s, s, s), (Texture)(object)TexRoundStrokeBot);
		GUI.DrawTexture(new Rect(r.xMax - s, r.yMax - s, s, s), (Texture)(object)TexRoundStrokeMirrorBot);
		GUI.color = color;
		Fill(new Rect(r.x + s, r.y, r.width - 2f * s, w), c);
		Fill(new Rect(r.x + s, r.yMax - w, r.width - 2f * s, w), c);
		Fill(new Rect(r.x, r.y + s, w, r.height - 2f * s), c);
		Fill(new Rect(r.xMax - w, r.y + s, w, r.height - 2f * s), c);
	}

	private static Texture2D RoundTexture(bool stroke, bool mirror, bool bottom)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected Obj, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		Texture2D tex = new Texture2D(RoundS, RoundS, (TextureFormat)4, false);
		for (int y = 0; y < RoundS; y++)
		{
			for (int x = 0; x < RoundS; x++)
			{
				float px = (float)(mirror ? (RoundS - 1 - x) : x) + 0.5f;
				float py = (bottom ? ((float)y + 0.5f) : ((float)(RoundS - y) - 0.5f));
				float a = (stroke ? RoundStrokeCoverage(px, py) : RoundFillCoverage(px, py));
				tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
			}
		}
		((Texture)tex).filterMode = (FilterMode)1;
		((Texture)tex).wrapMode = (TextureWrapMode)1;
		((Object)tex).hideFlags = (HideFlags)61;
		tex.Apply();
		return tex;
	}

	private static float RoundFillCoverage(float px, float py)
	{
		if (px >= (float)RoundR || py >= (float)RoundR)
		{
			return 1f;
		}
		float num = px - (float)RoundR;
		float dy = py - (float)RoundR;
		float dist = Mathf.Sqrt(num * num + dy * dy);
		return Mathf.Clamp01((float)RoundR - dist + 0.5f);
	}

	private static float RoundStrokeCoverage(float px, float py)
	{
		float band;
		if (!(px <= (float)RoundR) || !(py <= (float)RoundR))
		{
			if (px >= (float)RoundR && py < (float)RoundR)
			{
				band = Mathf.Min(py, 2f - py);
			}
			else
			{
				band = ((!(py >= (float)RoundR) || !(px < (float)RoundR)) ? (-1f) : Mathf.Min(px, 2f - px));
			}
		}
		else
		{
			float num = px - (float)RoundR;
			float dy = py - (float)RoundR;
			float dist = Mathf.Sqrt(num * num + dy * dy);
			band = Mathf.Min((float)RoundR - dist, dist - (float)(RoundR - 2));
		}
		return Mathf.Clamp01(band + 0.5f);
	}

	static Theme()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
	}
}
