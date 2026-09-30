using HarmonyLib;
using Sons.Gameplay;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(PickUp), "OnEnable")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_PickUp_OnEnable
{
	private static void Postfix(PickUp __instance)
	{
		ItemEsp.RegisterPickup(__instance);
	}
}
