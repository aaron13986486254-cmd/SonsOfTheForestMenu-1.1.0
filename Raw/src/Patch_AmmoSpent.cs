using HarmonyLib;
using Sons.Weapon;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(RangedWeaponController), "OnAmmoSpent")]
[Cyberfox1337x("infinite ammo", Bound = "Harmony target RangedWeaponController.OnAmmoSpent")]
internal static class Patch_AmmoSpent
{
	private static bool Prefix()
	{
		return !Features.InfAmmo;
	}
}
