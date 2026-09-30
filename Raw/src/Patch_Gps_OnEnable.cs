using HarmonyLib;
using Sons.Gameplay.GPS;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(GPSLocator), "OnEnable")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_Gps_OnEnable
{
	private static void Postfix(GPSLocator __instance)
	{
		ItemEsp.RegisterGps(__instance);
	}
}
