using HarmonyLib;
using Sons.Inventory;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(StructureStorage), "Awake")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_Storage_Awake
{
	private static void Postfix(StructureStorage __instance)
	{
		ItemEsp.RegisterStorage(__instance);
	}
}
