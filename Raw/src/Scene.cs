using System;
using Bolt;
using Sons.Cutscenes;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("prefabs and cutscenes")]
internal static class Scene
{
	internal readonly struct PrefabEntry
	{
		internal string Label { get; }

		internal Func<PrefabId> Resolve { get; }

		internal PrefabEntry(string label, Func<PrefabId> resolve)
		{
			Label = label;
			Resolve = resolve;
		}
	}

	private const float SpawnForwardOffset = 3f;

	private const float SpawnUpOffset = 1f;

	internal static readonly PrefabEntry[] Prefabs = new PrefabEntry[22]
	{
		new PrefabEntry("Golf Cart", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.GolfCart;
		}),
		new PrefabEntry("Snow Cart", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.SnowGolfCart;
		}),
		new PrefabEntry("Weapon Case Black", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.WeaponCaseBlack;
		}),
		new PrefabEntry("Weapon Case Blue", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.WeaponCaseBlue;
		}),
		new PrefabEntry("Weapon Case Green", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.WeaponCaseGreen;
		}),
		new PrefabEntry("Weapon Case Orange", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.WeaponCaseOrange;
		}),
		new PrefabEntry("Suitcase Blue", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.SuitcaseBlueContainer;
		}),
		new PrefabEntry("Suitcase Red", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.SuitcaseRedContainer;
		}),
		new PrefabEntry("Storage Crate A", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.StorageCrateA;
		}),
		new PrefabEntry("Storage Crate B", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.StorageCrateB;
		}),
		new PrefabEntry("Wooden Crate", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.WoodenCrateContainer;
		}),
		new PrefabEntry("Log Sled", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.BasicLogSledStructure;
		}),
		new PrefabEntry("Propane Burner", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.PropaneBurner;
		}),
		new PrefabEntry("Radio (bar)", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.BarRadio;
		}),
		new PrefabEntry("Radio (gym)", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.GymRadio;
		}),
		new PrefabEntry("Radio (indoors)", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.RadioIndoors;
		}),
		new PrefabEntry("Turtle Egg", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.TurtleEggPickup;
		}),
		new PrefabEntry("Dead Squirrel", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.DeadSquirrel;
		}),
		new PrefabEntry("Dead Rabbit", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.DeadRabbit;
		}),
		new PrefabEntry("Solafite Armour", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.SolafiteArmourPickup;
		}),
		new PrefabEntry("Golden Armour", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.GoldenArmourPickup;
		}),
		new PrefabEntry("Shiitake Mushrooms", () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return BoltPrefabs.ShiitakeMushroomPickup;
		})
	};

	internal static readonly (string Label, string Name)[] Cutscenes = new (string, string)[4]
	{
		("Ocean Crash", "HelicopterOceanCrashCutscene"),
		("Snow Crash", "HelicopterSnowCrashCutscene"),
		("Tree Crash", "HelicopterTreeCrashCutscene"),
		("Sleeping", "SleepingCutscene")
	};

	internal static void SpawnPrefab(int index)
	{
		if (!Game.InWorld)
		{
			Features.LastResult = "Needs a loaded world";
		}
		else
		{
			if (index < 0 || index >= Prefabs.Length)
			{
				return;
			}
			PrefabEntry entry = Prefabs[index];
			Features.LastResult = (Safe.Run("scene.spawn-prefab", entry, (PrefabEntry prefab) =>
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				LocalPlayer instance = LocalPlayer._instance;
				if (!((Object)(object)instance == (Object)null))
				{
					Transform transform = ((Component)instance).transform;
					Vector3 val = transform.position + transform.forward * 3f + Vector3.up * 1f;
					BoltNetwork.Instantiate(prefab.Resolve(), val, transform.rotation);
				}
			}, Noise.Warn) ? ("Spawned " + entry.Label) : ("Could not spawn " + entry.Label));
		}
	}

	internal static bool PlayCutscene(string name)
	{
		if (!Game.InWorld)
		{
			Features.LastResult = "Needs a loaded world";
			return false;
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		bool flag = Safe.Get("scene.queue-cutscene", name, (Func<string, bool>)CutsceneManager.QueueNextCutscene, false, Noise.Warn);
		Features.LastResult = (flag ? ("Queued cutscene " + name) : ("Unknown cutscene: " + name));
		return flag;
	}

	internal static void SkipCutscene()
	{
		Features.LastResult = (Safe.Run("scene.skip-cutscene", (Action)CutsceneManager.SkipActiveCutscene, Noise.Warn) ? "Cutscene skipped" : "Skip failed");
	}

	internal static string ActiveCutscene()
	{
		Cutscene active = Safe.Get("scene.active-cutscene", () => CutsceneManager.GetActiveCutScene());
		if (!((Object)(object)active == (Object)null))
		{
			return "Playing: " + NameOf(active);
		}
		return null;
	}

	private static string NameOf(Cutscene cutscene)
	{
		string objectName = Safe.Get("scene.cutscene-object-name", cutscene, (Cutscene c) => (!((Object)(object)((Component)c).gameObject == (Object)null)) ? ((Object)((Component)c).gameObject).name : null);
		if (!string.IsNullOrEmpty(objectName))
		{
			return objectName;
		}
		return Safe.Get("scene.cutscene-name", cutscene, (Cutscene c) => ((Object)c).name, "?");
	}
}
