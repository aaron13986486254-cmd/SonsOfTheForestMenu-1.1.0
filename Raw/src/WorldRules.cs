using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("world control rules")]
internal static class WorldRules
{
	internal static readonly WorldChoice[] TimeOfDay;

	internal static readonly WorldChoice[] CurrentDay;

	internal static readonly WorldChoice[] LightingOverride;

	internal static readonly WorldChoice[] JumpTime;

	internal static readonly WorldChoice[] TimeSpeed;

	internal static readonly WorldChoice[] Wind;

	internal static readonly WorldChoice[] Clouds;

	internal static readonly WorldChoice[] Forecast;

	internal static readonly WorldChoice[] Difficulty;

	internal static readonly WorldChoice[] GameMode;

	internal static readonly WorldChoice[] Season;

	static WorldRules()
	{
		TimeOfDay = BuildTimes();
		CurrentDay = BuildDays();
		LightingOverride = new WorldChoice[5]
		{
			new WorldChoice("Automatic · Actual Time", -1f, "off"),
			new WorldChoice("Morning · 6:00 AM", 6f, "morning"),
			new WorldChoice("Noon · 12:00 PM", 12f, "noon"),
			new WorldChoice("Sunset · 6:00 PM", 18f, "sunset"),
			new WorldChoice("Night · 12:00 AM", 0f, "night")
		};
		JumpTime = new WorldChoice[6]
		{
			new WorldChoice("1 hour", 1f),
			new WorldChoice("3 hours", 3f),
			new WorldChoice("6 hours", 6f),
			new WorldChoice("12 hours", 12f),
			new WorldChoice("24 hours", 24f),
			new WorldChoice("48 hours", 48f)
		};
		TimeSpeed = new WorldChoice[7]
		{
			new WorldChoice("Paused · 0x", 0f),
			new WorldChoice("Quarter · 0.25x", 0.25f),
			new WorldChoice("Half · 0.5x", 0.5f),
			new WorldChoice("Normal · 1x", 1f),
			new WorldChoice("Fast · 2x", 2f),
			new WorldChoice("Very Fast · 5x", 5f),
			new WorldChoice("Maximum · 10x", 10f)
		};
		Wind = new WorldChoice[6]
		{
			new WorldChoice("Automatic", -1f),
			new WorldChoice("Calm · 0%", 0f),
			new WorldChoice("Light · 25%", 0.25f),
			new WorldChoice("Moderate · 50%", 0.5f),
			new WorldChoice("Strong · 75%", 0.75f),
			new WorldChoice("Maximum · 100%", 1f)
		};
		Clouds = new WorldChoice[6]
		{
			new WorldChoice("Automatic", -1f),
			new WorldChoice("Clear · 0%", 0f),
			new WorldChoice("Light Clouds · 25%", 0.25f),
			new WorldChoice("Partly Cloudy · 50%", 0.5f),
			new WorldChoice("Mostly Cloudy · 75%", 0.75f),
			new WorldChoice("Full Cover · 100%", 1f)
		};
		Forecast = new WorldChoice[5]
		{
			new WorldChoice("Sunny", 0f, "Sunny"),
			new WorldChoice("Cloudy", 0f, "Cloudy"),
			new WorldChoice("Light Rain", 0f, "Light"),
			new WorldChoice("Medium Rain", 0f, "Medium"),
			new WorldChoice("Heavy Rain", 0f, "Heavy")
		};
		Difficulty = new WorldChoice[4]
		{
			new WorldChoice("Normal", 0f, "Normal"),
			new WorldChoice("Hard", 0f, "Hard"),
			new WorldChoice("Hard Survival", 0f, "HardSurvival"),
			new WorldChoice("Peaceful", 0f, "Peaceful")
		};
		GameMode = new WorldChoice[3]
		{
			new WorldChoice("Standard", 0f, "Standard"),
			new WorldChoice("Creative", 0f, "Creative"),
			new WorldChoice("Mod", 0f, "Mod")
		};
		Season = new WorldChoice[4]
		{
			new WorldChoice("Spring", 0f, "Spring"),
			new WorldChoice("Summer", 0f, "Summer"),
			new WorldChoice("Autumn", 0f, "Fall"),
			new WorldChoice("Winter", 0f, "Winter")
		};
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "bounded-world-controls";
	}

	internal static WorldControlAvailability ResolveAvailability(bool inWorld, bool serverOrNotRunning)
	{
		if (!inWorld)
		{
			return WorldControlAvailability.NeedsWorld;
		}
		if (!serverOrNotRunning)
		{
			return WorldControlAvailability.HostRequired;
		}
		return WorldControlAvailability.Ready;
	}

	internal static bool CanApply(WorldControlAvailability availability)
	{
		return availability == WorldControlAvailability.Ready;
	}

	internal static string ApplyLabel(WorldControlAvailability availability)
	{
		return availability switch
		{
			WorldControlAvailability.Ready => "APPLY", 
			WorldControlAvailability.SinglePlayerOnly => "SP ONLY", 
			WorldControlAvailability.HostRequired => "HOST ONLY", 
			_ => "LOAD WORLD", 
		};
	}

	internal static string AvailabilityNotice(WorldControlAvailability availability)
	{
		return availability switch
		{
			WorldControlAvailability.Ready => "HOST CONTROL - choose a preset, then APPLY. Changes replicate from this session.", 
			WorldControlAvailability.HostRequired => "HOST CONTROLLED - this multiplayer client cannot change server time, weather, or rules.", 
			_ => "LOAD A WORLD - controls are available in standalone play or to the co-op host.", 
		};
	}

	internal static string DenialMessage(WorldControlAvailability availability)
	{
		return availability switch
		{
			WorldControlAvailability.HostRequired => "Host or server-side control required for world changes", 
			WorldControlAvailability.SinglePlayerOnly => "Local time presets require offline single-player", 
			_ => "Needs a loaded world", 
		};
	}

	internal static string FormatTime12(int hour, int minute)
	{
		hour = (hour % 24 + 24) % 24;
		minute = Math.Clamp(minute, 0, 59);
		string period = ((hour < 12) ? "AM" : "PM");
		int displayHour = hour % 12;
		if (displayHour == 0)
		{
			displayHour = 12;
		}
		return $"{displayHour}:{minute:00} {period}";
	}

	internal static string FormatHour12(int hour)
	{
		return FormatTime12(hour, 0);
	}

	internal static int IndexOfValue(WorldChoice[] choices, float value)
	{
		if (choices == null || choices.Length == 0 || float.IsNaN(value) || float.IsInfinity(value))
		{
			return 0;
		}
		int nearest = 0;
		float nearestDistance = Math.Abs(choices[0].Value - value);
		for (int index = 1; index < choices.Length; index++)
		{
			float distance = Math.Abs(choices[index].Value - value);
			if (!(distance >= nearestDistance))
			{
				nearest = index;
				nearestDistance = distance;
			}
		}
		return nearest;
	}

	internal static int IndexOfToken(WorldChoice[] choices, string token)
	{
		if (choices == null || choices.Length == 0 || string.IsNullOrWhiteSpace(token))
		{
			return 0;
		}
		for (int index = 0; index < choices.Length; index++)
		{
			if (choices[index].Token.Equals(token, StringComparison.OrdinalIgnoreCase))
			{
				return index;
			}
		}
		return 0;
	}

	private static WorldChoice[] BuildTimes()
	{
		WorldChoice[] choices = new WorldChoice[48];
		for (int index = 0; index < choices.Length; index++)
		{
			int hour = index / 2;
			int minute = index % 2 * 30;
			choices[index] = new WorldChoice(FormatTime12(hour, minute), (float)hour + (float)minute / 60f);
		}
		return choices;
	}

	private static WorldChoice[] BuildDays()
	{
		WorldChoice[] choices = new WorldChoice[100];
		for (int day = 1; day <= choices.Length; day++)
		{
			choices[day - 1] = new WorldChoice($"Day {day}", day);
		}
		return choices;
	}
}
