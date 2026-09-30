using Il2CppSystem.Collections.Generic;
using Sons.Items.Core;
using Sons.StatSystem;
using TheForest;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class Game
{
	internal static bool InWorld => Safe.Get("game.in-world", () => (Object)(object)LocalPlayer._instance != (Object)null && LocalPlayer.IsInWorld, fallback: false);

	internal static Vitals Vitals => Safe.Get("player.vitals", () => LocalPlayer.Vitals);

	internal static FirstPersonCharacter Fp => Safe.Get("player.fp", () => LocalPlayer.FpCharacter);

	internal static PlayerStats Stats => Safe.Get("player.stats", () => LocalPlayer.Stats);

	internal static Camera Cam => Safe.Get("player.camera", () => LocalPlayer.MainCam);

	internal static Transform PlayerTransform => Safe.Get("player.transform", () => LocalPlayer.Transform);

	internal static Rigidbody Body => Safe.Get("player.rigidbody", () => LocalPlayer.Rigidbody);

	internal static Il2CppSystem.Collections.Generic.List<ItemData> Items => Safe.Get("items.database", () => ItemDatabaseManager.Items);

	internal static void Peg(Stat stat)
	{
		if (stat != null)
		{
			Safe.Run("stat.peg", stat, (Stat s) =>
			{
				s._currentValue = s._max;
			});
		}
	}

	internal static float Max(Stat stat)
	{
		if (stat != null)
		{
			return Safe.Get("stat.max", stat, (Stat s) => s._max, 0f);
		}
		return 0f;
	}

	internal static float Current(Stat stat)
	{
		if (stat != null)
		{
			return Safe.Get("stat.current", stat, (Stat s) => s._currentValue, 0f);
		}
		return 0f;
	}

	internal static void SetCurrent(Stat stat, float value)
	{
		if (stat != null)
		{
			Safe.Run("stat.set-current", (stat, value), ((Stat stat, float value) t) =>
			{
				t.stat._currentValue = t.value;
			});
		}
	}

	internal static void SetMax(Stat stat, float value)
	{
		if (stat != null)
		{
			Safe.Run("stat.set-max", (stat, value), ((Stat stat, float value) t) =>
			{
				t.stat._max = t.value;
			});
		}
	}

	internal static bool Console(string command)
	{
		if (string.IsNullOrWhiteSpace(command))
		{
			return false;
		}
		string verb = command.Split(' ')[0];
		return Safe.Get("console." + verb, command, (string cmd) =>
		{
			DebugConsole instance = DebugConsole.Instance;
			if ((Object)(object)instance == (Object)null)
			{
				return false;
			}
			instance.SendCommand(cmd);
			return true;
		}, fallback: false, Noise.Warn);
	}
}
