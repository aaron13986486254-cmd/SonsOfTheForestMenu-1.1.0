namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("command with argument")]
internal struct ValueCommand(string label, string command, string defaultValue)
{
	public string Label = label;

	public string Command = command;

	public string DefaultValue = defaultValue;
}
