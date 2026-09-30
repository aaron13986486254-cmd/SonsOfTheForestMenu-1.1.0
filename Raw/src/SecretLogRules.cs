namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("secret log status")]
internal static class SecretLogRules
{
	private static string Cyberfox1337xFunction()
	{
		return "secret-log-status";
	}

	internal static string Status(string secret)
	{
		Cyberfox1337xFunction();
		if (!string.IsNullOrEmpty(secret))
		{
			return "(set)";
		}
		return "(unset)";
	}
}
