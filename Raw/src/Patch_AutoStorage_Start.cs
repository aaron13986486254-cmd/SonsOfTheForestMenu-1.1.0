using HarmonyLib;
using Sons.Inventory;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(InWorldAutoStorage), "Start")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_AutoStorage_Start
{
	private static void Postfix(InWorldAutoStorage __instance)
	{
		ItemEsp.RegisterAutoStorage(__instance);
	}
}
