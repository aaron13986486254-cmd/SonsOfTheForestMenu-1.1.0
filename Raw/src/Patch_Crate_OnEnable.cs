using HarmonyLib;
using Sons.Gameplay;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(ContainerItemSpawner), "OnEnable")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_Crate_OnEnable
{
	private static void Postfix(ContainerItemSpawner __instance)
	{
		ItemEsp.RegisterContainer(__instance);
	}
}
