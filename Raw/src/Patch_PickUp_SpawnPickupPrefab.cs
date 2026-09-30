using HarmonyLib;
using Sons.Gameplay;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(PickUp), "SpawnPickupPrefab")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_PickUp_SpawnPickupPrefab
{
	private static void Postfix(PickUp __instance)
	{
		ItemEsp.ReRegisterPickup(__instance);
	}
}
