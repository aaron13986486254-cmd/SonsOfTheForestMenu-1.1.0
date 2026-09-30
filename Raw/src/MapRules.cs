using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("map viewport rules")]
internal static class MapRules
{
	internal const float NormalZoom = 1f;

	internal const float MaximumZoom = 4f;

	internal const float ZoomStep = 0.5f;

	internal const float PanDragThreshold = 6f;

	internal const int MaximumLabelLength = 48;

	internal const int RejectedNativeCandidate = int.MinValue;

	static MapRules()
	{
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "map-viewport-rules";
	}

	internal static float CalculateFitZoom(float sourceAspect, float targetAspect)
	{
		sourceAspect = SafeAspect(sourceAspect);
		targetAspect = SafeAspect(targetAspect);
		return Math.Min(1f, Math.Min(sourceAspect / targetAspect, targetAspect / sourceAspect));
	}

	internal static float ClampZoom(float zoom, float sourceAspect, float targetAspect)
	{
		float fitZoom = CalculateFitZoom(sourceAspect, targetAspect);
		if (!IsFinite(zoom))
		{
			return 1f;
		}
		return Math.Clamp(zoom, fitZoom, 4f);
	}

	internal static float AdjustZoom(float zoom, float wheelDelta, float sourceAspect, float targetAspect)
	{
		float current = ClampZoom(zoom, sourceAspect, targetAspect);
		float direction;
		if (wheelDelta < 0f)
		{
			direction = 1f;
		}
		else
		{
			direction = ((wheelDelta > 0f) ? (-1f) : 0f);
		}
		if (direction == 0f)
		{
			return current;
		}
		float candidate = current + direction * 0.5f;
		if (current < 1f && candidate > 1f)
		{
			candidate = 1f;
		}
		if (current > 1f && candidate < 1f)
		{
			candidate = 1f;
		}
		return ClampZoom(candidate, sourceAspect, targetAspect);
	}

	internal static MapViewport CalculateViewport(float sourceAspect, float targetAspect, float zoom)
	{
		sourceAspect = SafeAspect(sourceAspect);
		targetAspect = SafeAspect(targetAspect);
		float safeZoom = ClampZoom(zoom, sourceAspect, targetAspect);
		float coverU = Math.Min(1f, targetAspect / sourceAspect);
		float coverV = Math.Min(1f, sourceAspect / targetAspect);
		float spanU = Math.Min(1f, coverU / safeZoom);
		float spanV = Math.Min(1f, coverV / safeZoom);
		float destinationWidth = Math.Min(1f, safeZoom / coverU);
		float destinationHeight = Math.Min(1f, safeZoom / coverV);
		return new MapViewport(safeZoom, spanU, spanV, (1f - destinationWidth) * 0.5f, (1f - destinationHeight) * 0.5f, destinationWidth, destinationHeight);
	}

	internal static void CalculateCoverSpans(float sourceAspect, float targetAspect, float zoom, out float spanU, out float spanV)
	{
		MapViewport viewport = CalculateViewport(sourceAspect, targetAspect, zoom);
		spanU = viewport.SpanU;
		spanV = viewport.SpanV;
	}

	internal static float ClampCenterToSpan(float center, float span)
	{
		if (!IsFinite(span))
		{
			span = 1f;
		}
		span = Math.Clamp(span, 0.0001f, 1f);
		float halfView = span * 0.5f;
		if (float.IsNaN(center) || float.IsInfinity(center))
		{
			center = 0.5f;
		}
		return Math.Clamp(center, halfView, 1f - halfView);
	}

	internal static void ZoomAround(float oldSpanU, float oldSpanV, float newSpanU, float newSpanV, float anchorU, float anchorV, ref float centerU, ref float centerV)
	{
		ZoomBetweenViews(oldSpanU, oldSpanV, newSpanU, newSpanV, anchorU, anchorV, anchorU, anchorV, ref centerU, ref centerV);
	}

	internal static void ZoomBetweenViews(float oldSpanU, float oldSpanV, float newSpanU, float newSpanV, float oldAnchorU, float oldAnchorV, float newAnchorU, float newAnchorV, ref float centerU, ref float centerV)
	{
		oldSpanU = SafeSpan(oldSpanU);
		oldSpanV = SafeSpan(oldSpanV);
		newSpanU = SafeSpan(newSpanU);
		newSpanV = SafeSpan(newSpanV);
		oldAnchorU = Math.Clamp(oldAnchorU, 0f, 1f);
		oldAnchorV = Math.Clamp(oldAnchorV, 0f, 1f);
		newAnchorU = Math.Clamp(newAnchorU, 0f, 1f);
		newAnchorV = Math.Clamp(newAnchorV, 0f, 1f);
		float sourceU = centerU + (oldAnchorU - 0.5f) * oldSpanU;
		float sourceV = centerV + (oldAnchorV - 0.5f) * oldSpanV;
		centerU = ClampCenterToSpan(sourceU - (newAnchorU - 0.5f) * newSpanU, newSpanU);
		centerV = ClampCenterToSpan(sourceV - (newAnchorV - 0.5f) * newSpanV, newSpanV);
	}

	internal static void PanCenter(float startU, float startV, float deltaX, float deltaY, float contentWidth, float contentHeight, float spanU, float spanV, out float centerU, out float centerV)
	{
		spanU = SafeSpan(spanU);
		spanV = SafeSpan(spanV);
		if (!IsFinite(contentWidth) || contentWidth <= 0f || !IsFinite(contentHeight) || contentHeight <= 0f || !IsFinite(deltaX) || !IsFinite(deltaY))
		{
			centerU = ClampCenterToSpan(startU, spanU);
			centerV = ClampCenterToSpan(startV, spanV);
		}
		else
		{
			centerU = ClampCenterToSpan(startU - deltaX / contentWidth * spanU, spanU);
			centerV = ClampCenterToSpan(startV + deltaY / contentHeight * spanV, spanV);
		}
	}

	internal static bool ExceedsPanThreshold(float startX, float startY, float currentX, float currentY)
	{
		if (!IsFinite(startX) || !IsFinite(startY) || !IsFinite(currentX) || !IsFinite(currentY))
		{
			return false;
		}
		float num = currentX - startX;
		float deltaY = currentY - startY;
		return num * num + deltaY * deltaY >= 36f;
	}

	internal static bool TryNormalizePoint(float pointX, float pointY, float left, float top, float width, float height, out float normalizedX, out float normalizedY)
	{
		normalizedX = (normalizedY = 0f);
		if (!IsFinite(pointX) || !IsFinite(pointY) || !IsFinite(left) || !IsFinite(top) || !IsFinite(width) || width <= 0f || !IsFinite(height) || height <= 0f)
		{
			return false;
		}
		normalizedX = (pointX - left) / width;
		normalizedY = (pointY - top) / height;
		if (normalizedX >= 0f && normalizedX <= 1f && normalizedY >= 0f)
		{
			return normalizedY <= 1f;
		}
		return false;
	}

	internal static bool TryProject(float worldX, float worldZ, float minimumX, float maximumX, float minimumZ, float maximumZ, float centerU, float centerV, float spanU, float spanV, out float screenU, out float screenV)
	{
		screenU = 0f;
		screenV = 0f;
		if (!IsFinite(worldX) || !IsFinite(worldZ) || !IsFinite(minimumX) || !IsFinite(maximumX) || !IsFinite(minimumZ) || !IsFinite(maximumZ) || maximumX <= minimumX || maximumZ <= minimumZ)
		{
			return false;
		}
		spanU = SafeSpan(spanU);
		spanV = SafeSpan(spanV);
		centerU = ClampCenterToSpan(centerU, spanU);
		centerV = ClampCenterToSpan(centerV, spanV);
		float sourceU = (worldX - minimumX) / (maximumX - minimumX);
		float sourceV = (worldZ - minimumZ) / (maximumZ - minimumZ);
		screenU = (sourceU - centerU) / spanU + 0.5f;
		screenV = (sourceV - centerV) / spanV + 0.5f;
		if (screenU >= 0f && screenU <= 1f && screenV >= 0f)
		{
			return screenV <= 1f;
		}
		return false;
	}

	internal static bool TryUnproject(float screenU, float screenV, float minimumX, float maximumX, float minimumZ, float maximumZ, float centerU, float centerV, float spanU, float spanV, out float worldX, out float worldZ)
	{
		worldX = 0f;
		worldZ = 0f;
		if (!IsFinite(screenU) || !IsFinite(screenV) || screenU < 0f || screenU > 1f || screenV < 0f || screenV > 1f || !IsFinite(minimumX) || !IsFinite(maximumX) || !IsFinite(minimumZ) || !IsFinite(maximumZ) || maximumX <= minimumX || maximumZ <= minimumZ)
		{
			return false;
		}
		spanU = SafeSpan(spanU);
		spanV = SafeSpan(spanV);
		centerU = ClampCenterToSpan(centerU, spanU);
		centerV = ClampCenterToSpan(centerV, spanV);
		float sourceU = centerU + (screenU - 0.5f) * spanU;
		float sourceV = centerV + (screenV - 0.5f) * spanV;
		worldX = minimumX + sourceU * (maximumX - minimumX);
		worldZ = minimumZ + sourceV * (maximumZ - minimumZ);
		return true;
	}

	internal static bool IsNear(float firstX, float firstY, float secondX, float secondY, float minimumDistance)
	{
		float num = firstX - secondX;
		float deltaY = firstY - secondY;
		float safeDistance = Math.Max(0f, minimumDistance);
		return num * num + deltaY * deltaY < safeDistance * safeDistance;
	}

	internal static string DisplayLabel(string label)
	{
		if (string.IsNullOrWhiteSpace(label))
		{
			return "GPS Point";
		}
		string trimmed = label.Trim();
		char[] normalized = new char[Math.Min(trimmed.Length, 49)];
		int length = 0;
		bool previousWasSpace = false;
		for (int index = 0; index < trimmed.Length; index++)
		{
			if (length >= normalized.Length)
			{
				break;
			}
			char character = trimmed[index];
			if (char.IsWhiteSpace(character) || char.IsControl(character))
			{
				if (!previousWasSpace)
				{
					normalized[length++] = ' ';
				}
				previousWasSpace = true;
			}
			else
			{
				normalized[length++] = character;
				previousWasSpace = false;
			}
		}
		while (length > 0 && normalized[length - 1] == ' ')
		{
			length--;
		}
		if (trimmed.Length <= 48 && length <= 48)
		{
			return new string(normalized, 0, length);
		}
		int kept = Math.Max(0, 45);
		if (length < kept)
		{
			kept = length;
		}
		return new string(normalized, 0, kept).TrimEnd() + "...";
	}

	internal static string ResolveLocatorLabel(string pointOfInterestName, string objectName, string text, string iconTextureName, int iconId)
	{
		if (IsSpecificLocatorText(pointOfInterestName))
		{
			return DisplayLabel(pointOfInterestName);
		}
		string objectLabel = NormalizeLocatorObjectName(objectName);
		if (objectLabel != null)
		{
			return objectLabel;
		}
		if (IsSpecificLocatorText(text))
		{
			return DisplayLabel(text);
		}
		string iconLabel = IconTextureLabel(iconTextureName);
		if (iconLabel != null)
		{
			return iconLabel;
		}
		iconLabel = IconIdLabel(iconId);
		if (iconLabel != null)
		{
			return iconLabel;
		}
		return "GPS Point";
	}

	internal static string NormalizeLocatorObjectName(string objectName)
	{
		if (string.IsNullOrWhiteSpace(objectName))
		{
			return null;
		}
		string source = objectName.Trim();
		source = RemoveSuffix(source, "(Clone)");
		source = RemoveNumericInstanceSuffix(source);
		if (IsGenericLocatorObjectName(source))
		{
			return null;
		}
		if (source.Equals("VirgniaGPSLocator", StringComparison.OrdinalIgnoreCase))
		{
			return "Virginia";
		}
		if (source.Equals("SuperStructureHouseIcon", StringComparison.OrdinalIgnoreCase) || source.Equals("SleepGpsLocator", StringComparison.OrdinalIgnoreCase) || source.Equals("ClassicSleepGpsLocator", StringComparison.OrdinalIgnoreCase))
		{
			return "Shelter";
		}
		source = RemoveSuffix(source, "GPSLocator");
		source = RemoveSuffix(source, "Locator");
		source = RemoveSuffix(source, "GPS");
		source = RemoveSuffix(source, "Go");
		source = RemoveSuffix(source, "Tracker");
		source = source.Trim('_', '-', ' ');
		if (source.Length == 0)
		{
			return null;
		}
		char[] expanded = new char[source.Length * 2];
		int length = 0;
		for (int index = 0; index < source.Length; index++)
		{
			char current = source[index];
			if (current == '_' || current == '-')
			{
				if (length > 0 && expanded[length - 1] != ' ')
				{
					expanded[length++] = ' ';
				}
				continue;
			}
			char previous = ((index > 0) ? source[index - 1] : '\0');
			char next = ((index + 1 < source.Length) ? source[index + 1] : '\0');
			if (length > 0 && expanded[length - 1] != ' ' && ((char.IsUpper(current) && (char.IsLower(previous) || char.IsDigit(previous) || (char.IsUpper(previous) && char.IsLower(next)))) || (char.IsDigit(current) && char.IsLetter(previous)) || (char.IsLetter(current) && char.IsDigit(previous))))
			{
				expanded[length++] = ' ';
			}
			expanded[length++] = current;
		}
		string label = DisplayLabel(new string(expanded, 0, length));
		if (!IsSpecificLocatorText(label))
		{
			return null;
		}
		return label;
	}

	private static string RemoveSuffix(string source, string suffix)
	{
		if (!source.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
		{
			return source;
		}
		int length = suffix.Length;
		return source.Substring(0, source.Length - length).TrimEnd();
	}

	private static string RemoveNumericInstanceSuffix(string source)
	{
		if (source.Length >= 3)
		{
			if (source[source.Length - 1] == ')')
			{
				int opening = source.LastIndexOf('(');
				if (opening <= 0 || opening >= source.Length - 2)
				{
					return source;
				}
				for (int index = opening + 1; index < source.Length - 1; index++)
				{
					if (!char.IsDigit(source[index]))
					{
						return source;
					}
				}
				return source.Substring(0, opening).TrimEnd();
			}
		}
		return source;
	}

	private static bool IsGenericLocatorObjectName(string source)
	{
		string compact = source.Replace("_", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
		if (!compact.Equals("GPSLocator", StringComparison.OrdinalIgnoreCase) && !compact.Equals("ClassicGPSLocator", StringComparison.OrdinalIgnoreCase) && !compact.Equals("Tracker", StringComparison.OrdinalIgnoreCase) && !compact.Equals("LocatorIcon", StringComparison.OrdinalIgnoreCase) && !compact.StartsWith("GPSLocatorHeld", StringComparison.OrdinalIgnoreCase) && !compact.StartsWith("GPSLocatorPickup", StringComparison.OrdinalIgnoreCase))
		{
			return compact.StartsWith("GPSLocatorGrabBag", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	private static bool IsSpecificLocatorText(string label)
	{
		if (string.IsNullOrWhiteSpace(label))
		{
			return false;
		}
		string trimmed = label.Trim();
		if (trimmed.Equals("Waypoint", StringComparison.OrdinalIgnoreCase) || trimmed.Equals("GPS Point", StringComparison.OrdinalIgnoreCase) || trimmed.Equals("GPS Locator", StringComparison.OrdinalIgnoreCase) || trimmed.Equals("None", StringComparison.OrdinalIgnoreCase) || trimmed.Equals("P", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		for (int index = 0; index < trimmed.Length; index++)
		{
			if (char.IsLetterOrDigit(trimmed[index]))
			{
				return true;
			}
		}
		return false;
	}

	private static string IconTextureLabel(string textureName)
	{
		if (string.IsNullOrWhiteSpace(textureName))
		{
			return null;
		}
		return textureName.Trim().ToLowerInvariant() switch
		{
			"gpstrackericon" => "GPS Tracker", 
			"crossicon" => "Cross", 
			"deericon" => "Deer", 
			"batterypackicon" => "Battery Pack", 
			"radioicon" => "Radio", 
			"hearticon" => "Heart", 
			"cave_0" => "Cave", 
			"deadtactilogo" => "Dead Tactical Marker", 
			"helicoptermapicon" => "Helicopter", 
			"mysteryicon" => "Mystery", 
			"xmark" => "X Mark", 
			"robbylogo" => "Kelvin", 
			"mapmarkericon" => "Map Marker", 
			"houseicon" => "Shelter", 
			"knightvicon" => "Knight V", 
			"hangglidericon" => "Hang Glider", 
			"logsledflagicon" => "Log Sled", 
			"wrenchicon" => "Maintenance", 
			"golfcarticon" => "Golf Cart", 
			"bunkerentertainmenticon" => "Bunker Entertainment", 
			"bunkermaintenanceicon" => "Bunker Maintenance", 
			"bunkerfoodicon" => "Bunker Food", 
			"bunkervipicon" => "Bunker Luxury", 
			"bunkerresidentialicon" => "Bunker Residential", 
			_ => null, 
		};
	}

	private static string IconIdLabel(int iconId)
	{
		switch (iconId)
		{
		case 0:
			return "GPS Tracker";
		case 1:
			return "Cross";
		case 2:
			return "Deer";
		case 3:
			return "Battery Pack";
		case 4:
			return "Radio";
		case 5:
			return "Heart";
		case 6:
			return "Cave";
		case 7:
			return "Dead Tactical Marker";
		case 8:
			return "Helicopter";
		case 9:
		case 17:
			return "Mystery";
		case 10:
			return "X Mark";
		case 11:
			return "Kelvin";
		case 12:
		case 26:
			return "Map Marker";
		case 13:
			return "Shelter";
		case 14:
			return "Knight V";
		case 15:
		case 20:
			return "Hang Glider";
		case 16:
			return "Log Sled";
		case 18:
			return "Maintenance";
		case 19:
			return "Golf Cart";
		case 21:
			return "Bunker Entertainment";
		case 22:
			return "Bunker Maintenance";
		case 23:
			return "Bunker Food";
		case 24:
			return "Bunker Luxury";
		case 25:
			return "Bunker Residential";
		default:
			return null;
		}
	}

	internal static string CategoryLabel(MapMarkerKind kind)
	{
		return kind switch
		{
			MapMarkerKind.Locator => "GPS POINT", 
			MapMarkerKind.Bunker => "BUNKER", 
			MapMarkerKind.SavedSpot => "SAVED SPOT", 
			MapMarkerKind.LocalPlayer => "YOU", 
			_ => "POINT", 
		};
	}

	internal static int ScoreNativeCandidate(bool isSprite, string ownerName, string textureName, int width, int height)
	{
		if (width < 256 || height < 256)
		{
			return int.MinValue;
		}
		float aspect = (float)width / (float)height;
		if (!IsFinite(aspect) || aspect < 0.6f || aspect > 1.7f)
		{
			return int.MinValue;
		}
		string names = ((ownerName ?? string.Empty) + " " + (textureName ?? string.Empty)).ToLowerInvariant();
		string[] rejected = new string[11]
		{
			"black", "blank", "glitch", "mask", "noise", "overlay", "procedural", "render texture", "rendertexture", "scanline",
			"static"
		};
		for (int index = 0; index < rejected.Length; index++)
		{
			if (names.Contains(rejected[index], StringComparison.Ordinal))
			{
				return int.MinValue;
			}
		}
		int score = (isSprite ? 1000000 : 100000);
		score += (int)Math.Min(100000L, (long)width * (long)height / 1024);
		if (names.Contains("surface", StringComparison.Ordinal))
		{
			score += 40000;
		}
		if (names.Contains("map", StringComparison.Ordinal))
		{
			score += 30000;
		}
		if (names.Contains("island", StringComparison.Ordinal))
		{
			score += 20000;
		}
		if (names.Contains("gps", StringComparison.Ordinal))
		{
			score += 10000;
		}
		return score + (int)(10000f * (1f - Math.Min(1f, Math.Abs(1f - aspect))));
	}

	private static bool IsFinite(float value)
	{
		if (!float.IsNaN(value))
		{
			return !float.IsInfinity(value);
		}
		return false;
	}

	private static float SafeAspect(float aspect)
	{
		if (!IsFinite(aspect) || !(aspect > 0f))
		{
			return 1f;
		}
		return aspect;
	}

	private static float SafeSpan(float span)
	{
		if (!IsFinite(span))
		{
			return 1f;
		}
		return Math.Clamp(span, 0.0001f, 1f);
	}
}
