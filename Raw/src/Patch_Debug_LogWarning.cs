using Il2CppSystem;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("boot-noise filter", Bound = "Harmony prefix bound by reflection in Plugin")]
internal static class Patch_Debug_LogWarning
{
	private static bool Prefix(Il2CppSystem.Object message)
	{
		if (message == null)
		{
			return true;
		}
		string text = message.ToString();
		if (!text.StartsWith("Duplicate FpsLimiter"))
		{
			return !text.StartsWith("Already set to the same number of voices");
		}
		return false;
	}
}
