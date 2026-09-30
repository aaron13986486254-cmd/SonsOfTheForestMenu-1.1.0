namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("command action")]
internal struct ActionCommand(string label, string command, bool danger = false)
{
	public string Label = label;

	public string Command = command;

	public bool Danger = danger;
}
