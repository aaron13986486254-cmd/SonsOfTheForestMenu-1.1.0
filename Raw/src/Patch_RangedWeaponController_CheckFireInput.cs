using HarmonyLib;
using Sons.Weapon;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(RangedWeaponController), "CheckFireInput")]
[Cyberfox1337x("rapid fire", Bound = "Harmony target RangedWeaponController.CheckFireInput")]
internal static class Patch_RangedWeaponController_CheckFireInput
{
	private static void Postfix(RangedWeaponController __instance)
	{
		Features.TryRapidFire(__instance);
	}
}
