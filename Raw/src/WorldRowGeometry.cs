namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal readonly struct WorldRowGeometry
{
	internal float LabelWidth { get; }

	internal float SelectorWidth { get; }

	internal float ActionWidth { get; }

	internal float Gap { get; }

	internal float TotalWidth => LabelWidth + SelectorWidth + ActionWidth + Gap;

	internal WorldRowGeometry(float labelWidth, float selectorWidth, float actionWidth, float gap)
	{
		LabelWidth = labelWidth;
		SelectorWidth = selectorWidth;
		ActionWidth = actionWidth;
		Gap = gap;
	}
}
