using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using Sons.Ai.Vail;
using Sons.Atmosphere;
using Sons.Gameplay.Achievements;
using Sons.Settings;
using Sons.Wearable.Armour;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class EngineCheats
{
	internal static readonly System.Collections.Generic.List<string> AchievementNames = new System.Collections.Generic.List<string>();

	private static Cheats Setup => Safe.Get("cheats.setup", () => Cheats.Setup);

	internal static bool CheatGod
	{
		get
		{
			return Read("cheats.god.read", (Cheats c) => c.GodMode);
		}
		set
		{
			Write("cheats.god.write", $"Cheats.GodMode {value}", (Cheats c) =>
			{
				c.GodMode = value;
			});
		}
	}

	internal static bool CheatEnergy
	{
		get
		{
			return Read("cheats.energy.read", (Cheats c) => c.InfiniteEnergy);
		}
		set
		{
			Write("cheats.energy.write", $"Infinite Energy {value}", (Cheats c) =>
			{
				c.InfiniteEnergy = value;
			});
		}
	}

	internal static bool CheatNoSurvival
	{
		get
		{
			return Read("cheats.nosurvival.read", (Cheats c) => c.NoSurvival);
		}
		set
		{
			Write("cheats.nosurvival.write", $"No Survival {value}", (Cheats c) =>
			{
				c.NoSurvival = value;
			});
		}
	}

	internal static bool CheatOneHitTrees
	{
		get
		{
			return Read("cheats.onehittrees.read", (Cheats c) => c.OneHitTreeCutting);
		}
		set
		{
			Write("cheats.onehittrees.write", $"One-Hit Trees {value}", (Cheats c) =>
			{
				c.OneHitTreeCutting = value;
			});
		}
	}

	internal static int AchievementTotal { get; private set; }

	internal static int AchievementUnlocked { get; private set; }

	private static AchievementManager AchievementManager => Safe.Get("achievements.manager", () => Object.FindObjectOfType<AchievementManager>(), null, Noise.Warn);

	internal static string ArmourStatus { get; private set; } = "unknown";

	private static void Ok(string what)
	{
		Features.LastResult = what;
	}

	private static bool Read(string site, Func<Cheats, bool> get)
	{
		Cheats cheats = Setup;
		if (cheats != null)
		{
			return Safe.Get(site, cheats, get, fallback: false);
		}
		return false;
	}

	private static void Write(string site, string label, Action<Cheats> set)
	{
		Cheats cheats = Setup;
		if (cheats == null)
		{
			Features.LastResult = "Needs a loaded world";
		}
		else if (Safe.Run(site, cheats, set, Noise.Warn))
		{
			Ok(label);
		}
	}

	internal static void ResetCheats()
	{
		if (Safe.Run("cheats.reset", (Action)Cheats.Reset, Noise.Warn))
		{
			Ok("Cheats reset");
		}
	}

	private static int ForEachAchievement(string site, Action<Achievement> visit)
	{
		AchievementManager manager = AchievementManager;
		if ((Object)(object)manager == (Object)null)
		{
			return -1;
		}
		Il2CppSystem.Collections.Generic.List<Achievement> list = Safe.Get("achievements.list", manager, (AchievementManager m) => m._achievements, null, Noise.Warn);
		if (list == null)
		{
			return -1;
		}
		int visited = 0;
		int count = Safe.Get("achievements.count", list, (Il2CppSystem.Collections.Generic.List<Achievement> l) => l.Count, 0);
		for (int index = 0; index < count; index++)
		{
			if (Safe.Run(site, (list, index, visit), ((Il2CppSystem.Collections.Generic.List<Achievement> list, int index, Action<Achievement> visit) state) =>
			{
				Achievement val = state.list[state.index];
				if (!((Object)(object)val == (Object)null))
				{
					state.visit(val);
				}
			}))
			{
				visited++;
			}
		}
		return visited;
	}

	internal static void RefreshAchievements()
	{
		AchievementNames.Clear();
		AchievementTotal = 0;
		AchievementUnlocked = 0;
		Ok((ForEachAchievement("achievements.read", (Achievement achievement) =>
		{
			bool flag = achievement.IsUnlocked();
			AchievementNames.Add((flag ? "[x]" : "[ ]") + " " + achievement.ApiName);
			AchievementTotal++;
			if (flag)
			{
				AchievementUnlocked++;
			}
		}) < 0) ? "No achievement manager" : $"{AchievementUnlocked} / {AchievementTotal} unlocked");
	}

	internal static void UnlockAllAchievements()
	{
		int unlocked = 0;
		if (ForEachAchievement("achievements.unlock", (Achievement achievement) =>
		{
			if (!achievement.IsUnlocked())
			{
				achievement.Unlock();
				unlocked++;
			}
		}) < 0)
		{
			Ok("No achievement manager");
			return;
		}
		Ok($"Unlocked {unlocked} achievement(s)");
		RefreshAchievements();
	}

	internal static void ResetAllAchievements()
	{
		int reset = 0;
		if (ForEachAchievement("achievements.reset", (Achievement achievement) =>
		{
			achievement.Reset();
			reset++;
		}) < 0)
		{
			Ok("No achievement manager");
			return;
		}
		Ok($"Reset {reset} achievement(s)");
		RefreshAchievements();
	}

	internal static void RefreshArmour()
	{
		PlayerStats stats = Game.Stats;
		PlayerArmourSystem armour = (((Object)(object)stats == (Object)null) ? null : Safe.Get("armour.system", stats, (PlayerStats s) => s.ArmourSystem));
		if ((Object)(object)armour == (Object)null)
		{
			ArmourStatus = "no armour system";
			return;
		}
		ArmourStatus = Safe.Get("armour.status", armour, (PlayerArmourSystem system) =>
		{
			if (!system.IsWearingAnyArmour())
			{
				return "no armour equipped";
			}
			return (!system.IsWearingGoldenArmour()) ? "wearing armour" : "wearing golden armour";
		}, "unknown", Noise.Warn);
	}

	internal static void ToggleFreezeWater()
	{
		FreezeWater water = Safe.Get("water.freeze-component", () => Object.FindObjectOfType<FreezeWater>(), null, Noise.Warn);
		if ((Object)(object)water == (Object)null)
		{
			Ok("No water body loaded");
		}
		else if (Safe.Run("water.toggle-freeze", water, (FreezeWater w) =>
		{
			w.ToggleFreeze();
		}, Noise.Warn))
		{
			Ok("Water freeze toggled");
		}
	}

	internal static int SimActorCount()
	{
		VailWorldSimulation simulation = Safe.Get("sim.instance", () => VailWorldSimulation._instance);
		if (!((Object)(object)simulation == (Object)null))
		{
			return Safe.Get("sim.actor-count", simulation, (VailWorldSimulation s) => s.ActorCount, -1);
		}
		return -1;
	}
}
