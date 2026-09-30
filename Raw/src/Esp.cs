using Il2CppSystem.Collections.Generic;
using Sons.Ai.Vail;
using Sons.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("actor ESP")]
internal static class Esp
{
	private enum EspCategory
	{
		Enemy,
		Animal,
		Friendly
	}

	public static bool Enabled;

	public static bool Enemies = true;

	public static bool Animals;

	public static bool Friendly;

	public static bool ShowDead;

	public static float MaxDistance = 300f;

	private static readonly GUIContent _gc = new GUIContent();

	private static GUIStyle _espLabel;

	private static GUIStyle _espShadow;

	private static GUIStyle _hpLabel;

	private static GUIStyle _hpShadow;

	private static GUIStyle _catLabel;

	private static GUIStyle _catShadow;

	private static Texture2D _skullIcon;

	private static Texture2D _pawIcon;

	private static Texture2D _figureIcon;

	private static Texture2D _heartIcon;

	private const int IconN = 14;

	public static int LastDrawn { get; private set; }

	public static int ActiveCount()
	{
		if (!Enabled)
		{
			return 0;
		}
		int n = 1;
		if (Enemies)
		{
			n++;
		}
		if (Animals)
		{
			n++;
		}
		if (Friendly)
		{
			n++;
		}
		return n;
	}

	private static EspCategory Classify(VailActor a)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		try
		{
			VailActorClassId classId = a.ClassId;
			if ((int)classId - 1 <= 1)
			{
				return EspCategory.Enemy;
			}
			if ((int)classId == 3)
			{
				return EspCategory.Animal;
			}
		}
		catch
		{
		}
		try
		{
			VailActorTypeId typeId = a.TypeId;
			if ((int)typeId - 8 <= 2 || (int)typeId == 90 || (int)typeId == 100)
			{
				return EspCategory.Friendly;
			}
			return EspCategory.Enemy;
		}
		catch
		{
			return EspCategory.Enemy;
		}
	}

	private static bool Want(EspCategory c)
	{
		return c switch
		{
			EspCategory.Animal => Animals, 
			EspCategory.Friendly => Friendly, 
			_ => Enemies, 
		};
	}

	private static string CategoryLabel(EspCategory c)
	{
		return c switch
		{
			EspCategory.Animal => "ANIMAL", 
			EspCategory.Friendly => "FRIENDLY", 
			_ => "ENEMY", 
		};
	}

	public static void Draw()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Expected Obj, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected Obj, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected Obj, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected Obj, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected Obj, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected Obj, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		LastDrawn = 0;
		if (!Enabled || !Game.InWorld || (Event.current != null && (int)Event.current.type != 7))
		{
			return;
		}
		Camera cam = Game.Cam;
		if ((Object)(object)cam == (Object)null)
		{
			return;
		}
		Transform self = Game.PlayerTransform;
		Vector3 origin = (((Object)(object)self != (Object)null) ? self.position : ((Component)cam).transform.position);
		VailActorManager mgr;
		try
		{
			mgr = VailActorManager._instance;
		}
		catch
		{
			return;
		}
		if ((Object)(object)mgr == (Object)null)
		{
			return;
		}
		Il2CppSystem.Collections.Generic.List<VailActor> actors;
		try
		{
			actors = mgr._activeActors;
		}
		catch
		{
			return;
		}
		if (actors == null)
		{
			return;
		}
		int count;
		try
		{
			count = actors.Count;
		}
		catch
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			VailActor a;
			try
			{
				a = actors[i];
			}
			catch
			{
				continue;
			}
			if ((Object)(object)a == (Object)null)
			{
				continue;
			}
			try
			{
				if (a._isDead && !ShowDead)
				{
					continue;
				}
				EspCategory category = Classify(a);
				if (!Want(category))
				{
					continue;
				}
				Transform tr = ((Component)a).transform;
				if ((Object)(object)tr == (Object)null)
				{
					continue;
				}
				Vector3 pos = tr.position;
				float dist = Vector3.Distance(origin, pos);
				if (dist > MaxDistance)
				{
					continue;
				}
				Vector3 top = cam.WorldToScreenPoint(pos + Vector3.up * 2f);
				Vector3 bottom = cam.WorldToScreenPoint(pos);
				if (top.z <= 0f || bottom.z <= 0f)
				{
					continue;
				}
				float h = Mathf.Abs(top.y - bottom.y);
				if (h < 4f)
				{
					continue;
				}
				float w = h * 0.55f;
				float sx = bottom.x - w * 0.5f;
				float sy = (float)Screen.height - top.y;
				Rect box = new Rect(sx, sy, w, h);
				Color col = (a._isDead ? Theme.Dead : ColorFor(category));
				Theme.Fill(box, new Color(0f, 0f, 0f, 0.18f));
				Theme.Box(box, col, 2f);
				// Health styles and icons must exist before the first health-bearing actor.
				if (_espLabel == null)
				{
					GUIStyle espLabel = new GUIStyle(Theme.EspLabel);
					espLabel.alignment = (TextAnchor)4;
					_espShadow = new GUIStyle(espLabel);
					_espShadow.normal.textColor = new Color(0f, 0f, 0f, 0.85f);
					_hpLabel = new GUIStyle(espLabel);
					_hpLabel.fontSize = 11;
					_hpLabel.alignment = (TextAnchor)5;
					_hpShadow = new GUIStyle(_hpLabel);
					_hpShadow.normal.textColor = new Color(0f, 0f, 0f, 0.85f);
					_catLabel = new GUIStyle(Theme.EspLabel);
					_catLabel.fontSize = 9;
					_catLabel.alignment = (TextAnchor)4;
					_catShadow = new GUIStyle(_catLabel);
					_catShadow.normal.textColor = new Color(0f, 0f, 0f, 0.85f);
					_skullIcon ??= MakeSkullIcon();
					_pawIcon ??= MakePawIcon();
					_figureIcon ??= MakeFigureIcon();
					_heartIcon ??= MakeHeartIcon();
					// Publish the initialization sentinel last so failures can be retried.
					_espLabel = espLabel;
				}
				float hpFrac = -1f;
				float hpCur = 0f;
				float hpMax = 0f;
				if (!a._isDead)
				{
					hpFrac = HealthOf(a, out hpCur, out hpMax);
				}
				float barShift = 0f;
				if (hpFrac >= 0f)
				{
					float barY = box.y - 6f - 2f;
					_gc.text = RosterRules.HealthText(hasHealth: true, hpCur, hpMax);
					Vector2 hpSize = _hpLabel.CalcSize(_gc);
					hpSize.x += 6f;
					hpSize.y += 2f;
					float hpW = Mathf.Min(hpSize.x, box.width * 0.55f);
					Rect heartRect = new Rect(box.x, barY + -3f, 12f, 12f);
					GUI.DrawTexture(new Rect(heartRect.x + 1f, heartRect.y + 1f, 12f, 12f), (Texture)(object)_heartIcon, (ScaleMode)0, true, 0f, new Color(0f, 0f, 0f, 0.85f), 0f, 0f);
					Color color = GUI.color;
					GUI.color = Theme.RustBright;
					GUI.DrawTexture(heartRect, (Texture)(object)_heartIcon);
					GUI.color = color;
					float barRight = Mathf.Max(heartRect.xMax + 3f + 8f, box.xMax - hpW - 3f);
					Rect barBg = new Rect(heartRect.xMax + 3f, barY, barRight - heartRect.xMax - 3f, 6f);
					Theme.Fill(barBg, new Color(0f, 0f, 0f, 0.65f));
					if (hpFrac > 0f)
					{
						Theme.Fill(new Rect(barBg.x + 1f, barBg.y + 1f, (barBg.width - 2f) * hpFrac, 4f), HpColor(hpFrac));
					}
					Theme.Box(barBg, col);
					Rect hpRect = new Rect(box.xMax - hpW, barY - (hpSize.y - 6f) * 0.5f, hpW, hpSize.y);
					GUI.Label(new Rect(hpRect.x + 1f, hpRect.y + 1f, hpRect.width, hpRect.height), _gc, _hpShadow);
					GUI.Label(hpRect, _gc, _hpLabel);
					barShift = 8f;
				}
				float labelY = box.y - barShift;
				string catText = CategoryLabel(category);
				_gc.text = catText;
				Vector2 catSize = _catLabel.CalcSize(_gc);
				catSize.x += 6f;
				catSize.y += 2f;
				float catH = catSize.y;
				float catFullW = 15f + catSize.x;
				Rect catRect = new Rect(box.x + box.width * 0.5f - catFullW * 0.5f, labelY - catH + 2f, catFullW, catSize.y);
				Texture2D icon = category switch
				{
					EspCategory.Friendly => _figureIcon, 
					EspCategory.Animal => _pawIcon, 
					_ => _skullIcon, 
				};
				Rect iconRect = new Rect(catRect.x, catRect.y + 1f, 12f, 12f);
				GUI.DrawTexture(new Rect(iconRect.x + 1f, iconRect.y + 1f, 12f, 12f), (Texture)(object)icon, (ScaleMode)0, true, 0f, new Color(0f, 0f, 0f, 0.85f), 0f, 0f);
				Color color2 = GUI.color;
				GUI.color = col;
				GUI.DrawTexture(iconRect, (Texture)(object)icon);
				GUI.color = color2;
				Rect textRect = new Rect(catRect.x + 12f + 3f, catRect.y, catSize.x, catSize.y);
				GUI.Label(new Rect(textRect.x + 1f, textRect.y + 1f, textRect.width, textRect.height), _gc, _catShadow);
				Color prevCatColor = _catLabel.normal.textColor;
				_catLabel.normal.textColor = col;
				GUI.Label(textRect, _gc, _catLabel);
				_catLabel.normal.textColor = prevCatColor;
				string labelText = dist.ToString("0") + "m";
				_gc.text = labelText;
				Vector2 labelSize = _espLabel.CalcSize(_gc);
				labelSize.x += 8f;
				labelSize.y += 4f;
				Rect labelRect = new Rect(box.x + box.width * 0.5f - labelSize.x * 0.5f, labelY - labelSize.y - catH + 4f, labelSize.x, labelSize.y);
				GUI.Label(new Rect(labelRect.x + 1f, labelRect.y + 1f, labelRect.width, labelRect.height), _gc, _espShadow);
				Color prevColor = _espLabel.normal.textColor;
				_espLabel.normal.textColor = Color.white;
				GUI.Label(labelRect, _gc, _espLabel);
				_espLabel.normal.textColor = prevColor;
				LastDrawn++;
			}
			catch
			{
			}
		}
	}

	private static Color ColorFor(EspCategory c)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return c switch
		{
			EspCategory.Animal => Theme.Live, 
			EspCategory.Friendly => Theme.Bone, 
			_ => Theme.RustBright, 
		};
	}

	private static float HealthOf(VailActor a, out float cur, out float max)
	{
		cur = 0f;
		max = 0f;
		try
		{
			DamageController dc = a._damageController;
			if ((Object)(object)dc == (Object)null)
			{
				return -1f;
			}
			Il2CppSystem.Collections.Generic.List<DamageNode> nodes = dc._damageNodes;
			if (nodes == null)
			{
				return -1f;
			}
			int n = nodes.Count;
			if (n == 0)
			{
				return -1f;
			}
			float totalMax = 0f;
			float totalDmg = 0f;
			for (int i = 0; i < n; i++)
			{
				DamageNode node = nodes[i];
				if (!((Object)(object)node == (Object)null) && node._useBreak)
				{
					float m = node._breakAmount;
					if (!(m <= 0f))
					{
						totalMax += m;
						totalDmg += Mathf.Clamp(node._currentAppliedNodeDamage, 0f, m);
					}
				}
			}
			if (totalMax <= 0f)
			{
				return -1f;
			}
			max = totalMax;
			cur = totalMax - totalDmg;
			return Mathf.Clamp01(1f - totalDmg / totalMax);
		}
		catch
		{
			return -1f;
		}
	}

	private static Color HpColor(float hp)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Color amber = new Color(0.88f, 0.75f, 0.18f);
		if (hp < 0.5f)
		{
			return Color.Lerp(Theme.RustBright, amber, hp * 2f);
		}
		return Color.Lerp(amber, Theme.Live, (hp - 0.5f) * 2f);
	}

	private static Texture2D MakeSkullIcon()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		Texture2D t = new Texture2D(14, 14, (TextureFormat)4, false);
		for (int y = 0; y < 14; y++)
		{
			for (int x = 0; x < 14; x++)
			{
				float fx = (float)x + 0.5f;
				float fy = (float)y + 0.5f;
				float a = 0f;
				float dist1 = Mathf.Abs(fy - fx) / 1.414f;
				bool num = fx + fy > 4f && fx + fy < 24f && dist1 < 1.6f;
				float dist2 = Mathf.Abs(fy - (14f - fx)) / 1.414f;
				bool bone2 = fx - fy > -10f && fx - fy < 10f && dist2 < 1.6f;
				bool knobs = tip(2.5f, 2.5f) || tip(11.5f, 11.5f) || tip(11.5f, 2.5f) || tip(2.5f, 11.5f);
				bool num2 = num | bone2 | knobs;
				float num3 = (fx - 7f) / 5f;
				float cy = (fy - 4.5f) / 4.2f;
				bool num4 = num3 * num3 + cy * cy < 1f && fy < 9f;
				bool cheek = fy >= 6.5f && fy <= 9.5f && fx >= 1.5f && fx <= 12.5f;
				if (fy >= 8.5f && fy <= 12.5f)
				{
					float jawLerp = (fy - 8.5f) / 4f;
					float jawL = 3f + jawLerp * 1.2f;
					float jawR = 11f - jawLerp * 1.2f;
					if (fx >= jawL && fx <= jawR)
					{
						cheek = true;
					}
				}
				if (num4 | cheek)
				{
					bool num5 = x >= 3 && x <= 5 && y >= 3 && y <= 5;
					bool rightEye = x >= 8 && x <= 10 && y >= 3 && y <= 5;
					bool nose = (y == 6 && (x == 6 || x == 7)) || (y == 7 && x == 6);
					bool tooth = y >= 9 && y <= 11 && x % 2 == 1;
					if (!num5 && !rightEye && !nose && !tooth)
					{
						a = 1f;
					}
				}
				if (num2)
				{
					a = Mathf.Max(a, 0.45f);
				}
				if (knobs && a > 0.45f)
				{
					a = 0.55f;
				}
				t.SetPixel(x, y, new Color(1f, 1f, 1f, a));
				bool tip(float tx, float ty)
				{
					return Mathf.Sqrt((fx - tx) * (fx - tx) + (fy - ty) * (fy - ty)) < 2f;
				}
			}
		}
		((Texture)t).filterMode = (FilterMode)1;
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		return t;
	}

	private static Texture2D MakePawIcon()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		Texture2D t = new Texture2D(14, 14, (TextureFormat)4, false);
		for (int y = 0; y < 14; y++)
		{
			for (int x = 0; x < 14; x++)
			{
				float fx = (float)x + 0.5f;
				float fy = (float)y + 0.5f;
				float a = 0f;
				float num = (fx - 7f) / 3.8f;
				float pdy = (fy - 9.5f) / 3.2f;
				bool flag = num * num + pdy * pdy < 1f;
				bool toes = Toe(4f, 5.8f, 2.2f) || Toe(7f, 4f, 2.2f) || Toe(10f, 5.8f, 2.2f);
				bool claws = Claw(4f, 1.5f) || Claw(7f, 0.5f) || Claw(10f, 1.5f);
				float num2 = Mathf.Sqrt((fx - 2f) * (fx - 2f) + (fy - 11.5f) * (fy - 11.5f));
				float d2 = Mathf.Sqrt((fx - 12f) * (fx - 12f) + (fy - 11.5f) * (fy - 11.5f));
				bool dewclaws = num2 < 1.5f || d2 < 1.5f;
				if (flag | toes | claws | dewclaws)
				{
					a = 1f;
				}
				t.SetPixel(x, y, new Color(1f, 1f, 1f, a));
				bool Claw(float cx, float topY)
				{
					if (fx >= cx - 0.5f && fx <= cx + 0.5f && fy >= topY)
					{
						return fy <= topY + 2.5f;
					}
					return false;
				}
				bool Toe(float tx, float ty, float r)
				{
					float num3 = (fx - tx) / r;
					float dy = (fy - ty) / r;
					return num3 * num3 + dy * dy < 1f;
				}
			}
		}
		((Texture)t).filterMode = (FilterMode)1;
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		return t;
	}

	private static Texture2D MakeFigureIcon()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		Texture2D t = new Texture2D(14, 14, (TextureFormat)4, false);
		for (int y = 0; y < 14; y++)
		{
			for (int x = 0; x < 14; x++)
			{
				float fx = (float)x + 0.5f;
				float fy = (float)y + 0.5f;
				float a = 0f;
				bool flag = Mathf.Sqrt((fx - 7f) * (fx - 7f) + (fy - 2f) * (fy - 2f)) < 2.8f;
				bool neck = fx >= 6f && fx <= 8f && fy >= 4.5f && fy <= 5.5f;
				bool shoulders = fy >= 5.5f && fy <= 6.5f && fx >= 2f && fx <= 12f;
				if (fy >= 6.5f && fy <= 10f)
				{
					float armLerp = (fy - 6.5f) / 3.5f;
					float armOut = 3f + armLerp * 2.5f;
					float armIn = 1.5f + armLerp * 2f;
					bool num = fx >= 7f - armOut && fx <= 7f - armIn;
					bool rightArm = fx >= 7f + armIn && fx <= 7f + armOut;
					if (num | rightArm)
					{
						a = 1f;
					}
				}
				if (fy >= 6f && fy <= 10.5f && fx >= 5.5f && fx <= 8.5f)
				{
					a = 1f;
				}
				if (fy > 10.5f && fy <= 13.5f)
				{
					bool num2 = fx >= 4.5f && fx <= 6f;
					bool rightLeg = fx >= 8f && fx <= 9.5f;
					if (num2 | rightLeg)
					{
						a = 1f;
					}
				}
				if (fy > 12.5f && fy <= 13.5f && ((fx >= 3.5f && fx <= 7f) || (fx >= 7f && fx <= 10.5f)))
				{
					a = 1f;
				}
				if (flag | neck | shoulders)
				{
					a = 1f;
				}
				t.SetPixel(x, y, new Color(1f, 1f, 1f, a));
			}
		}
		((Texture)t).filterMode = (FilterMode)1;
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		return t;
	}

	private static Texture2D MakeHeartIcon()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		Texture2D t = new Texture2D(14, 14, (TextureFormat)4, false);
		for (int y = 0; y < 14; y++)
		{
			for (int x = 0; x < 14; x++)
			{
				float fx = (float)x + 0.5f;
				float fy = (float)y + 0.5f;
				float a = 0f;
				float num = Mathf.Sqrt((fx - 4.6f) * (fx - 4.6f) + (fy - 4.8f) * (fy - 4.8f));
				float d2 = Mathf.Sqrt((fx - 9.4f) * (fx - 9.4f) + (fy - 4.8f) * (fy - 4.8f));
				bool num2 = num < 3f || d2 < 3f;
				float halfW = 4f - (fy - 4.8f) * 0.5263158f;
				bool wedge = fy > 4.8f && fy <= 12.4f && fx >= 7f - halfW && fx <= 7f + halfW;
				if (num2 | wedge)
				{
					a = 1f;
				}
				t.SetPixel(x, y, new Color(1f, 1f, 1f, a));
			}
		}
		((Texture)t).filterMode = (FilterMode)1;
		((Texture)t).wrapMode = (TextureWrapMode)1;
		((Object)t).hideFlags = (HideFlags)61;
		t.Apply();
		return t;
	}

	static Esp()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
	}
}
