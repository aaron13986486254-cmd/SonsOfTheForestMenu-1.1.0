using HarmonyLib;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(Vitals), "TriggerDeath")]
[Cyberfox1337x("god mode", Bound = "Harmony target Vitals.TriggerDeath; Prefix matched by name")]
internal static class Patch_Vitals_TriggerDeath
{
	private static bool Prefix()
	{
		if (!Features.GodMode)
		{
			return true;
		}
		Diag.Once("godmode.death-suppressed", "god mode: death suppressed");
		return false;
	}
}
