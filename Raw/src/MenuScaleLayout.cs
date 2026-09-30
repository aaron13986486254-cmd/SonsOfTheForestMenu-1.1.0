namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal readonly struct MenuScaleLayout
{
	internal readonly float Scale;

	internal readonly float Width;

	internal readonly float Height;

	internal float PixelWidth => Width * Scale;

	internal float PixelHeight => Height * Scale;

	internal MenuScaleLayout(float scale, float width, float height)
	{
		Scale = scale;
		Width = width;
		Height = height;
	}
}
