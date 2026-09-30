using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class WorldRowLayout
{
	internal const float Gap = 6f;

	internal const float PreferredLabelWidth = 190f;

	internal const float MinimumLabelWidth = 104f;

	internal const float MinimumSelectorWidth = 150f;

	internal const float MinimumActionWidth = 116f;

	private const float MinimumOptionWidth = 80f;

	static WorldRowLayout()
	{
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "responsive-world-row-layout";
	}

	internal static WorldRowGeometry Calculate(float availableWidth)
	{
		availableWidth = FiniteNonNegative(availableWidth);
		float gap = Math.Min(6f, availableWidth);
		float maximumActionWidth = Math.Max(0f, availableWidth - 104f - 150f - gap);
		float actionWidth = ((maximumActionWidth >= 116f) ? 116f : maximumActionWidth);
		float remainingWidth = Math.Max(0f, availableWidth - actionWidth - gap);
		float labelWidth = ((!(remainingWidth >= 254f)) ? (remainingWidth * 0.5f) : Math.Min(190f, remainingWidth - 150f));
		float selectorWidth = Math.Max(0f, remainingWidth - labelWidth);
		return new WorldRowGeometry(labelWidth, selectorWidth, actionWidth, gap);
	}

	internal static int OptionColumns(int choiceCount, float selectorWidth)
	{
		if (choiceCount <= 0)
		{
			return 0;
		}
		int val;
		if (choiceCount >= 80)
		{
			val = 5;
		}
		else if (choiceCount >= 40)
		{
			val = 4;
		}
		else
		{
			val = ((choiceCount >= 12) ? 3 : 2);
		}
		int widthColumns = Math.Max(1, (int)Math.Floor((FiniteNonNegative(selectorWidth) + 6f) / 86f));
		return Math.Min(val, widthColumns);
	}

	private static float FiniteNonNegative(float value)
	{
		if (!float.IsNaN(value) && !float.IsInfinity(value) && !(value < 0f))
		{
			return value;
		}
		return 0f;
	}
}
