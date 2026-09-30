namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("gravity ownership state machine")]
internal static class GravityRules
{
	private static string Cyberfox1337xFunction()
	{
		return "gravity-ownership-rules";
	}

	internal static GravityWork Resolve(bool fly, bool gravityEnabled, bool overrideOwned, bool restorePending)
	{
		Cyberfox1337xFunction();
		if (fly || !gravityEnabled)
		{
			return GravityWork.EnforceOverride;
		}
		if (overrideOwned | restorePending)
		{
			return GravityWork.RestoreBaseline;
		}
		return GravityWork.None;
	}
}
