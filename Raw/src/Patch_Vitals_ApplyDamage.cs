using HarmonyLib;
using UnityEngine;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(Vitals), "ApplyDamage")]
[Cyberfox1337x("damage scaling", Bound = "Harmony target Vitals.ApplyDamage; Prefix matched by name")]
internal static class Patch_Vitals_ApplyDamage
{
	private const float MultiplierEpsilon = 0.0005f;

	private static bool Prefix(ref float value)
	{
		if (Features.GodMode)
		{
			return false;
		}
		float multiplier = Features.DamageTakenMult;
		if (Mathf.Abs(multiplier - 1f) > 0.0005f)
		{
			value *= multiplier;
		}
		return true;
	}
}
