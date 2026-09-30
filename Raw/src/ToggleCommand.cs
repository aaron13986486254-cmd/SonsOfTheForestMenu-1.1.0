namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("command switch")]
internal struct ToggleCommand(string label, string command, bool danger = false)
{
	public string Label = label;

	public string Command = command;

	public bool Danger = danger;
}
