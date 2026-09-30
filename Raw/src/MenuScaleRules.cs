using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal static class MenuScaleRules
{
	internal const float MinScale = 0.75f;

	internal const float MaxScale = 3f;

	internal const float MinWidth = 640f;

	internal const float MinHeight = 440f;

	internal const float ScreenMargin = 12f;

	private static void Cyberfox1337xFunction()
	{
	}

	internal static bool IsFinite(float value)
	{
		if (!float.IsNaN(value))
		{
			return !float.IsInfinity(value);
		}
		return false;
	}

	internal static float ClampScale(float value)
	{
		if (!IsFinite(value))
		{
			return 1f;
		}
		return Math.Clamp(value, 0.75f, 3f);
	}

	internal static float AutomaticScale(int screenWidth, int screenHeight)
	{
		return Math.Clamp(Math.Min((float)screenWidth / 1920f, (float)screenHeight / 1080f), 1f, 3f);
	}

	internal static MenuScaleLayout Resolve(int screenWidth, int screenHeight, bool automatic, float manualScale)
	{
		Cyberfox1337xFunction();
		float width = Math.Max(1, screenWidth);
		float height = Math.Max(1, screenHeight);
		float availableWidth = Math.Max(1f, width - 24f);
		float availableHeight = Math.Max(1f, height - 24f);
		float val = (automatic ? AutomaticScale(screenWidth, screenHeight) : ClampScale(manualScale));
		float fit = Math.Min(availableWidth / 640f, availableHeight / 440f);
		float scale = Math.Min(val, fit);
		float logicalWidth = Math.Min(Math.Clamp(width / scale * 0.66f, 640f, 940f), availableWidth / scale);
		float logicalHeight = Math.Min(Math.Clamp(height / scale * 0.78f, 440f, 680f), availableHeight / scale);
		return new MenuScaleLayout(scale, logicalWidth, logicalHeight);
	}

	internal static float ClampPosition(float position, float panelPixels, int screenPixels)
	{
		float remaining = Math.Max(0f, (float)screenPixels - panelPixels);
		float margin = Math.Min(12f, remaining * 0.5f);
		if (!IsFinite(position))
		{
			position = remaining * 0.5f;
		}
		return Math.Clamp(position, margin, remaining - margin);
	}
}
