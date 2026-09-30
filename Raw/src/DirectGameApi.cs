using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Construction;
using Endnight.Types;
using Sons.Ai.Vail;
using Sons.Atmosphere;
using Sons.Environment;
using Sons.Gameplay;
using Sons.Multiplayer;
using Sons.Weapon;
using TheForest;
using TheForest.Commons.Enums;
using TheForest.Utils;
using TheForest.World;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class DirectGameApi
{
	[CompilerGenerated]
	private static string Result__BackingField;

	private static readonly Dictionary<string, Action<PlayerStats>> StatusEffects;

	private static bool _ghost;

	private static bool _ownsLocalTimeOverride;

	[CompilerGenerated]
	private static float GliderForward__BackingField;

	[CompilerGenerated]
	private static float GliderPitchUp__BackingField;

	[CompilerGenerated]
	private static float GliderDown__BackingField;

	[CompilerGenerated]
	private static float KnightVSpeed__BackingField;

	[CompilerGenerated]
	private static float FlashlightIntensity__BackingField;

	[CompilerGenerated]
	private static float RebreatherLight__BackingField;

	internal static string Result
	{
		[CompilerGenerated]
		get
		{
			return Result__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Result__BackingField = value;
		}
	}

	internal static bool ShowPlayerNames => Safe.Get("names.preference.read", () => PlayerPreferences.ShowPlayerNamesMP, fallback: false);

	internal static bool Ghost
	{
		get
		{
			return _ghost;
		}
		set
		{
			_ghost = value;
			Attempt("enemies.ghost", "Ghost player " + (value ? "on" : "off"), () =>
			{
				VailActorManager.SetGhostPlayer(value);
			});
		}
	}

	internal static float GliderForward
	{
		[CompilerGenerated]
		get
		{
			return GliderForward__BackingField;
		}
		[CompilerGenerated]
		set
		{
			GliderForward__BackingField = value;
		}
	}

	internal static float GliderPitchUp
	{
		[CompilerGenerated]
		get
		{
			return GliderPitchUp__BackingField;
		}
		[CompilerGenerated]
		set
		{
			GliderPitchUp__BackingField = value;
		}
	}

	internal static float GliderDown
	{
		[CompilerGenerated]
		get
		{
			return GliderDown__BackingField;
		}
		[CompilerGenerated]
		set
		{
			GliderDown__BackingField = value;
		}
	}

	internal static float KnightVSpeed
	{
		[CompilerGenerated]
		get
		{
			return KnightVSpeed__BackingField;
		}
		[CompilerGenerated]
		set
		{
			KnightVSpeed__BackingField = value;
		}
	}

	internal static float FlashlightIntensity
	{
		[CompilerGenerated]
		get
		{
			return FlashlightIntensity__BackingField;
		}
		[CompilerGenerated]
		set
		{
			FlashlightIntensity__BackingField = value;
		}
	}

	internal static float FlashlightDrain { get; set; }

	internal static float RebreatherAir { get; set; }

	internal static float RebreatherLight
	{
		[CompilerGenerated]
		get
		{
			return RebreatherLight__BackingField;
		}
		[CompilerGenerated]
		set
		{
			RebreatherLight__BackingField = value;
		}
	}

	static DirectGameApi()
	{
		Result__BackingField = string.Empty;
		StatusEffects = new Dictionary<string, Action<PlayerStats>>
		{
			["StopBurning"] = (PlayerStats stats) =>
			{
				stats.StopBurning();
			},
			["GotClean"] = (PlayerStats stats) =>
			{
				stats.GotCleanReal();
			},
			["GotMud"] = (PlayerStats stats) =>
			{
				stats.GotMud();
			},
			["GotBloody"] = (PlayerStats stats) =>
			{
				stats.GotBloody();
			},
			["HitPoison"] = (PlayerStats stats) =>
			{
				stats.HitPoison();
			},
			["AdrenalineRush"] = (PlayerStats stats) =>
			{
				stats.AdrenalineRush();
			},
			["ClearSkunkSmell"] = (PlayerStats stats) =>
			{
				stats.ClearSkunkSmell();
			}
		};
		GliderForward__BackingField = 12f;
		GliderPitchUp__BackingField = 5f;
		GliderDown__BackingField = 3f;
		KnightVSpeed__BackingField = 18f;
		FlashlightIntensity__BackingField = 4f;
		RebreatherLight__BackingField = 4f;
		Cyberfox1337xFunction();
	}

	private static void Cyberfox1337xFunction()
	{
	}

	private static void Ok(string what)
	{
		Result = what;
		Features.LastResult = what;
	}

	private static void Fail(string what)
	{
		Result = what + " failed";
		Features.LastResult = Result;
	}

	private static void Attempt(string site, string what, Action work)
	{
		if (Safe.Run(site, work, Noise.Warn))
		{
			Ok(what);
		}
		else
		{
			Fail(what);
		}
	}

	internal static void SetShowPlayerNames(bool show)
	{
		if (Safe.Run("names.preference.write", show, (bool next) =>
		{
			PlayerPreferences.ShowPlayerNamesMP = next;
		}, Noise.Warn))
		{
			Ok("Native teammate names " + (show ? "on" : "off"));
		}
		else
		{
			Fail("Native teammate names");
		}
	}

	internal static void StatusEffect(string which)
	{
		PlayerStats stats = Game.Stats;
		Action<PlayerStats> apply;
		if ((Object)(object)stats == (Object)null)
		{
			Features.LastResult = "Needs a loaded world";
		}
		else if (!StatusEffects.TryGetValue(which, out apply))
		{
			Diag.Warn("unknown status effect requested: '" + which + "'");
		}
		else if (Safe.Run("status." + which, stats, apply, Noise.Warn))
		{
			Ok(which);
		}
		else
		{
			Fail(which);
		}
	}

	internal static void KillAllActors()
	{
		VailActorManager manager = Safe.Get("enemies.actor-manager", () => VailActorManager._instance, null, Noise.Warn);
		if ((Object)(object)manager == (Object)null)
		{
			Features.LastResult = "No actor manager";
			return;
		}
		Il2CppSystem.Collections.Generic.List<VailActor> actors = Safe.Get("enemies.actor-list", manager, (VailActorManager m) => m._activeActors, null, Noise.Warn);
		if (actors == null)
		{
			Features.LastResult = "No actors";
			return;
		}
		int killed = 0;
		int count = Safe.Get("enemies.actor-count", actors, (Il2CppSystem.Collections.Generic.List<VailActor> list) => list.Count, 0);
		for (int index = 0; index < count; index++)
		{
			if (Safe.Run("enemies.force-death", (actors, index), ((Il2CppSystem.Collections.Generic.List<VailActor> actors, int index) state) =>
			{
				VailActor val = state.actors[state.index];
				if (!((Object)(object)val == (Object)null) && !val._isDead)
				{
					val.ForceDeath();
				}
			}))
			{
				killed++;
			}
		}
		Ok($"Killed {killed} actor(s)");
	}

	internal static WorldControlAvailability GetWorldControlAvailability()
	{
		if (!Game.InWorld)
		{
			return WorldRules.ResolveAvailability(inWorld: false, serverOrNotRunning: false);
		}
		bool hasAuthority = Safe.Get("world.authority", () => BoltNetwork.isServerOrNotRunning, fallback: false);
		WorldControlAvailability worldControlAvailability = WorldRules.ResolveAvailability(inWorld: true, hasAuthority);
		if (worldControlAvailability == WorldControlAvailability.HostRequired)
		{
			Diag.InfoOnce("world.authority.client-readonly", "World controls are read-only: this game session is a multiplayer client; the host or dedicated server owns replicated world state.");
		}
		return worldControlAvailability;
	}

	private static bool CanChangeWorld()
	{
		WorldControlAvailability availability = GetWorldControlAvailability();
		if (availability == WorldControlAvailability.Ready)
		{
			return true;
		}
		Result = WorldRules.DenialMessage(availability);
		Features.LastResult = Result;
		return false;
	}

	private static void WorldResult(bool verified, string success, string failure)
	{
		if (verified)
		{
			Ok(success);
		}
		else
		{
			Fail(failure);
		}
	}

	private static bool IsOfflineSinglePlayer()
	{
		return Safe.Get("world.local-time.single-player", () => GameSetup.IsSinglePlayer && !GameSetup.IsMultiplayer && !BoltNetwork.isRunning && !GameServerManager.IsDedicatedClient, fallback: false);
	}

	internal static WorldControlAvailability GetLocalTimeOverrideAvailability(bool clearing)
	{
		if (!Game.InWorld)
		{
			return WorldControlAvailability.NeedsWorld;
		}
		if (clearing)
		{
			return WorldControlAvailability.Ready;
		}
		if (!IsOfflineSinglePlayer())
		{
			return WorldControlAvailability.SinglePlayerOnly;
		}
		return WorldControlAvailability.Ready;
	}

	internal static void EnforceLocalTimeOverrideSafety(bool inWorld)
	{
		if (_ownsLocalTimeOverride && (!inWorld || !IsOfflineSinglePlayer()) && Safe.Get("world.local-time.auto-clear", () =>
		{
			TimeOfDayHolder.UseOverrideTime(false);
			TimeOfDayHolder val = default;
			return !SingletonBoltBehaviour<TimeOfDayHolder>.TryGetInstance(out val) || (Object)(object)val == (Object)null || !val._usingOverrideTime;
		}, fallback: false, Noise.Warn))
		{
			_ownsLocalTimeOverride = false;
			Features.LastResult = "Local time override cleared for session safety";
		}
	}

	internal static void SetTimeOfDay(int hour, int minute)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
		{
			Features.LastResult = "Invalid time selection";
			return;
		}
		WorldResult(Safe.Get("world.time.set", (hour, minute), ((int Hour, int Minute) value) =>
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			bool flag = TimeOfDayHolder.SetDirect(-1, value.Hour, value.Minute, 0, 0, true, -1f, (Object)null, true);
			TimeOfDay timeOfDay = TimeOfDayHolder.GetTimeOfDay();
			return flag && timeOfDay.Hours == value.Hour && timeOfDay.Minutes == value.Minute;
		}, fallback: false, Noise.Warn), "Time set to " + WorldRules.FormatTime12(hour, minute), "Time change");
	}

	internal static void SetLightingOverride(string token, float hour)
	{
		if (!Game.InWorld)
		{
			Features.LastResult = "Needs a loaded world";
			return;
		}
		if (string.Equals(token, "off", StringComparison.Ordinal))
		{
			bool flag = Safe.Get("world.local-time.clear", () =>
			{
				TimeOfDayHolder.UseOverrideTime(false);
				TimeOfDayHolder val = default;
				return !SingletonBoltBehaviour<TimeOfDayHolder>.TryGetInstance(out val) || (Object)(object)val == (Object)null || !val._usingOverrideTime;
			}, fallback: false, Noise.Warn);
			if (flag)
			{
				_ownsLocalTimeOverride = false;
			}
			WorldResult(flag, "Lighting follows the live world clock", "Local time override reset");
			return;
		}
		if (!IsOfflineSinglePlayer())
		{
			Features.LastResult = "Local time presets require offline single-player";
			return;
		}
		bool flag2;
		switch (token)
		{
		case "morning":
		case "noon":
		case "sunset":
		case "night":
			flag2 = true;
			break;
		default:
			flag2 = false;
			break;
		}
		bool flag3 = flag2;
		int selectedHour = Mathf.RoundToInt(hour);
		if (!flag3 || selectedHour < 0 || selectedHour > 23)
		{
			Features.LastResult = "Invalid lighting preset";
			return;
		}
		bool applied = Safe.Get("world.local-time.set", selectedHour, (int value) =>
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			TimeOfDayHolder val = default;
			if (!SingletonBoltBehaviour<TimeOfDayHolder>.TryGetInstance(out val) || (Object)(object)val == (Object)null)
			{
				return false;
			}
			TimeOfDayHolder.UseOverrideTime(new TimeOfDay((float)value * 3600f));
			if (val._usingOverrideTime)
			{
				TimeOfDay overrideTimeOfDay = val._overrideTimeOfDay;
				return overrideTimeOfDay.Hours == value;
			}
			return false;
		}, fallback: false, Noise.Warn);
		if (applied)
		{
			_ownsLocalTimeOverride = true;
		}
		WorldResult(applied, "Local time held at " + token switch
		{
			"morning" => "Morning", 
			"noon" => "Noon", 
			"sunset" => "Sunset", 
			_ => "Night", 
		}, "Local time override");
	}

	internal static void SetCurrentDay(int day)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (day < 1 || day > 100)
		{
			Features.LastResult = "Invalid day selection";
			return;
		}
		WorldResult(Safe.Get("world.day.set", day, (int value) =>
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			TimeOfDayHolder.SetDay(value);
			TimeOfDay timeOfDay = TimeOfDayHolder.GetTimeOfDay();
			return timeOfDay.Days == value;
		}, fallback: false, Noise.Warn), $"Current day set to {day}", "Day change");
	}

	internal static void JumpTime(float hours)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (hours <= 0f || hours > 48f)
		{
			Features.LastResult = "Invalid time jump";
			return;
		}
		WorldResult(Safe.Run("world.time.jump", hours, (float value) =>
		{
			TimeOfDayHolder.JumpTime(value * 3600f, 0f);
		}, Noise.Warn), $"Time jumped forward {hours:0} hour(s)", "Time jump");
	}

	internal static void SetTimeSpeed(float speed)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (speed < 0f || speed > 10f)
		{
			Features.LastResult = "Invalid time speed";
			return;
		}
		WorldResult(Safe.Get("world.time-speed.set", speed, (float value) =>
		{
			TimeOfDayHolder.SetBaseTimeSpeed(value);
			return Approximately(TimeOfDayHolder.GetBaseSpeedMultiplier(), value);
		}, fallback: false, Noise.Warn), $"Time speed set to {speed:0.##}x", "Time speed change");
	}

	internal static void SetWind(float intensity)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (intensity < 0f)
		{
			WorldResult(Safe.Get("world.wind.unlock", () =>
			{
				WindManager instance = WindManager._instance;
				if ((Object)(object)instance == (Object)null)
				{
					return false;
				}
				WindManager.Unlock();
				return !instance._lock;
			}, fallback: false, Noise.Warn), "Wind returned to automatic", "Wind unlock");
			return;
		}
		intensity = Mathf.Clamp01(intensity);
		WorldResult(Safe.Get("world.wind.set", intensity, (float value) =>
		{
			WindManager instance = WindManager._instance;
			if ((Object)(object)instance == (Object)null)
			{
				return false;
			}
			WindManager.SetAndLockIntensity(value);
			return instance._lock;
		}, fallback: false, Noise.Warn), $"Wind set to {intensity * 100f:0}%", "Wind change");
	}

	internal static void SetClouds(float factor)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		if (factor < -1f || factor > 1f)
		{
			Features.LastResult = "Invalid cloud selection";
			return;
		}
		bool verified = Safe.Get("world.clouds.set", factor, (float value) =>
		{
			DebugConsole instance = DebugConsole.Instance;
			if ((Object)(object)instance == (Object)null)
			{
				return false;
			}
			instance._cloudFactor(value.ToString("0.##", CultureInfo.InvariantCulture));
			CloudManager val = default;
			if (!CloudManager.TryGetInstance(out val) || (Object)(object)val == (Object)null)
			{
				return false;
			}
			if (value < 0f)
			{
				return val._lockedCloudStormFactor < 0f;
			}
			return Approximately(val._lockedCloudStormFactor, value) || Approximately(val.GetCurrentCloudStormFactorTarget(), value);
		}, fallback: false, Noise.Warn);
		string label = ((factor < 0f) ? "automatic" : $"{factor * 100f:0}%");
		WorldResult(verified, "Cloud cover set to " + label, "Cloud change");
	}

	internal static void SetForecast(string forecast)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		bool verified = Safe.Get("world.forecast.set", forecast, (string value) =>
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Invalid comparison between Unknown and I4
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Invalid comparison between Unknown and I4
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Invalid comparison between Unknown and I4
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Invalid comparison between Unknown and I4
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Invalid comparison between Unknown and I4
			WeatherSystem instance = WeatherSystem.Instance;
			if ((Object)(object)instance == (Object)null)
			{
				return false;
			}
			switch (value)
			{
			case "Sunny":
				instance.StopRaining();
				instance.SetTargetStormFactor(0f);
				return (int)instance.CurrentRainType == 0;
			case "Cloudy":
				instance.StopRaining();
				instance.SetTargetStormFactor(0.25f);
				return (int)instance.CurrentRainType == 0;
			case "Light":
				instance.StartRaining(WeatherSystem.RainType.Light);
				return (int)instance.CurrentRainType == 1;
			case "Medium":
				instance.StartRaining(WeatherSystem.RainType.Medium);
				return (int)instance.CurrentRainType == 2;
			case "Heavy":
				instance.StartRaining(WeatherSystem.RainType.Heavy);
				return (int)instance.CurrentRainType == 3;
			default:
				return false;
			}
		}, fallback: false, Noise.Warn);
		WorldResult(verified, "Forecast set to " + forecast switch
		{
			"Light" => "Light rain", 
			"Medium" => "Medium rain", 
			"Heavy" => "Heavy rain", 
			_ => forecast, 
		}, "Forecast change");
	}

	internal static void SetSeasonDirect(string season)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		bool verified = Safe.Get("world.season.set", season, (string value) =>
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			SeasonsManager instance = SeasonsManager.Instance;
			if ((Object)(object)instance == (Object)null || !TrySeason(value, out var season2))
			{
				return false;
			}
			SeasonsManager.SetSeasonSwitchInstant(true);
			instance.SetSeason(season2);
			return SeasonsManager.ActiveSeason == season2;
		}, fallback: false, Noise.Warn);
		string label = ((season == "Fall") ? "Autumn" : season);
		WorldResult(verified, "Season set to " + label, "Season change");
	}

	internal static void SetDifficulty(string token)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		WorldResult(Safe.Get("world.difficulty.set", token, (string value) =>
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (!TryDifficulty(value, out var mode))
			{
				return false;
			}
			GameSetup.SetDifficulty(mode);
			return GameSetup.Difficulty == mode;
		}, fallback: false, Noise.Warn), "Difficulty set to " + ReadableMode(token), "Difficulty");
	}

	internal static void SetGameMode(string token)
	{
		if (!CanChangeWorld())
		{
			return;
		}
		WorldResult(Safe.Get("world.game-type.set", token, (string value) =>
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGameType(value, out var game))
			{
				return false;
			}
			GameSetup.SetGameType(game);
			return GameSetup.Game == game;
		}, fallback: false, Noise.Warn), "Game type set to " + token, "Game type");
	}

	private static bool TrySeason(string value, out SeasonsManager.Season season)
	{
		switch (value)
		{
		case "Spring":
			season = (SeasonsManager.Season)0;
			return true;
		case "Summer":
			season = (SeasonsManager.Season)1;
			return true;
		case "Fall":
			season = (SeasonsManager.Season)2;
			return true;
		case "Winter":
			season = (SeasonsManager.Season)3;
			return true;
		default:
			season = (SeasonsManager.Season)0;
			return false;
		}
	}

	private static bool TryDifficulty(string value, out DifficultyModes mode)
	{
		switch (value)
		{
		case "Normal":
			mode = (DifficultyModes)1;
			return true;
		case "Hard":
			mode = (DifficultyModes)2;
			return true;
		case "HardSurvival":
			mode = (DifficultyModes)3;
			return true;
		case "Peaceful":
			mode = (DifficultyModes)0;
			return true;
		default:
			mode = (DifficultyModes)0;
			return false;
		}
	}

	private static bool TryGameType(string value, out GameTypes game)
	{
		switch (value)
		{
		case "Standard":
			game = (GameTypes)0;
			return true;
		case "Creative":
			game = (GameTypes)2;
			return true;
		case "Mod":
			game = (GameTypes)1;
			return true;
		default:
			game = (GameTypes)0;
			return false;
		}
	}

	private static string ReadableMode(string value)
	{
		if (value == "HardSurvival")
		{
			return "Hard Survival";
		}
		return value;
	}

	private static bool Approximately(float first, float second)
	{
		return Mathf.Abs(first - second) < 0.001f;
	}

	internal static void ClearAllStructures()
	{
		RuntimeStructureDatabase database = Safe.Get("world.structure-database", () => Object.FindObjectOfType<RuntimeStructureDatabase>(), null, Noise.Warn);
		if ((Object)(object)database == (Object)null)
		{
			Features.LastResult = "No structure database";
			return;
		}
		Attempt("world.clear-structures", "All structures cleared", () =>
		{
			database.ClearAllStructures();
		});
	}

	internal static void ApplyGlider()
	{
		PlayerHangGliderAction action = Safe.Get("gear.glider-action", () => Object.FindObjectOfType<PlayerHangGliderAction>());
		HangGliderSettings settings = (((Object)(object)action == (Object)null) ? null : Safe.Get("gear.glider-settings", action, (PlayerHangGliderAction a) => a._hangGliderSettings));
		if ((Object)(object)settings == (Object)null)
		{
			Features.LastResult = "No glider in world";
			return;
		}
		Attempt("gear.glider-apply", "Glider tuned", () =>
		{
			settings.ConstantForwardForce = GliderForward;
			settings.UpPitchDownForce = GliderPitchUp;
			settings.BaseDownPitchForce = GliderDown;
		});
	}

	internal static void ApplyKnightV()
	{
		PlayerKnightVAction action = Safe.Get("gear.knightv-action", () => Object.FindObjectOfType<PlayerKnightVAction>());
		KnightVControlDefinition definition = (((Object)(object)action == (Object)null) ? null : Safe.Get("gear.knightv-definition", action, (PlayerKnightVAction a) => a._controlDefinition));
		if ((Object)(object)definition == (Object)null)
		{
			Features.LastResult = "No Knight V in world";
			return;
		}
		Attempt("gear.knightv-apply", "Knight V tuned", () =>
		{
			definition.MaxVelocity = KnightVSpeed;
		});
	}

	internal static void ApplyFlashlight()
	{
		FlashlightController flashlight = Safe.Get("gear.flashlight", () => Object.FindObjectOfType<FlashlightController>());
		if ((Object)(object)flashlight == (Object)null)
		{
			Features.LastResult = "No flashlight in world";
			return;
		}
		Attempt("gear.flashlight-apply", "Flashlight tuned", () =>
		{
			flashlight._maxLightIntensity = FlashlightIntensity;
			flashlight._powerDrainRate = FlashlightDrain;
		});
	}

	internal static void ApplyRebreather()
	{
		RebreatherController rebreather = Safe.Get("gear.rebreather", () => Object.FindObjectOfType<RebreatherController>());
		if ((Object)(object)rebreather == (Object)null)
		{
			Features.LastResult = "No rebreather in world";
			return;
		}
		Attempt("gear.rebreather-apply", "Rebreather tuned", () =>
		{
			rebreather._airConsumptionRate = RebreatherAir;
			rebreather._maxLightIntensity = RebreatherLight;
		});
	}
}
