namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x(null)]
internal readonly struct MapViewport
{
	internal readonly float Zoom;

	internal readonly float SpanU;

	internal readonly float SpanV;

	internal readonly float DestinationX;

	internal readonly float DestinationY;

	internal readonly float DestinationWidth;

	internal readonly float DestinationHeight;

	internal bool IsContained
	{
		get
		{
			if (!(DestinationWidth < 0.9999f))
			{
				return DestinationHeight < 0.9999f;
			}
			return true;
		}
	}

	internal MapViewport(float zoom, float spanU, float spanV, float destinationX, float destinationY, float destinationWidth, float destinationHeight)
	{
		Zoom = zoom;
		SpanU = spanU;
		SpanV = spanV;
		DestinationX = destinationX;
		DestinationY = destinationY;
		DestinationWidth = destinationWidth;
		DestinationHeight = destinationHeight;
	}
}
