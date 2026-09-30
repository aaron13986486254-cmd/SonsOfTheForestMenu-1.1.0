using HarmonyLib;
using Sons.Gameplay.Artifact;

namespace Cyberfox1337x.SonsOfTheForest;

[HarmonyPatch(typeof(ArtifactStructure), "OnEnable")]
[Cyberfox1337x("ESP registry hook", Bound = "Harmony target; Postfix matched by name")]
internal static class Patch_Artifact_OnEnable
{
	private static void Postfix(ArtifactStructure __instance)
	{
		ItemEsp.RegisterArtifact(__instance);
	}
}
