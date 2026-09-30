namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("world control choice")]
internal readonly struct WorldChoice
{
	internal string Label { get; }

	internal float Value { get; }

	internal string Token { get; }

	internal WorldChoice(string label, float value, string token = "")
	{
		Label = label;
		Value = value;
		Token = token ?? string.Empty;
	}
}
