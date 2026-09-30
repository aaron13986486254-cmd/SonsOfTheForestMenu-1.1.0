using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("roster identity rules")]
internal static class RosterRules
{
	static RosterRules()
	{
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "roster-identity-rules";
	}

	internal static bool IsRemoteCandidate(ulong localSteamId, ulong candidateSteamId, bool sameEntity)
	{
		if (sameEntity)
		{
			return false;
		}
		if (localSteamId == 0L || candidateSteamId == 0L)
		{
			return true;
		}
		return localSteamId != candidateSteamId;
	}

	internal static bool HasIdentity(ulong steamId, bool hasEntity)
	{
		return (steamId != 0) | hasEntity;
	}

	internal static bool IsUsableRemoteEntity(bool mappingFound, bool hasEntity, bool isSelf)
	{
		if (mappingFound & hasEntity)
		{
			return !isSelf;
		}
		return false;
	}

	internal static string DisplayName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return "(unnamed)";
		}
		string trimmed = name.Trim();
		char[] normalized = new char[trimmed.Length];
		int length = 0;
		bool previousWasSpace = false;
		foreach (char character in trimmed)
		{
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
		return new string(normalized, 0, length).TrimEnd();
	}

	internal static string PlayerCountText(int count)
	{
		int safeCount = Math.Max(0, count);
		if (safeCount != 1)
		{
			return $"{safeCount} PLAYERS";
		}
		return "1 PLAYER";
	}

	internal static string SourceSummary(int serverStateAdded, int connectedUiAdded, int knownCacheAdded)
	{
		string summary = ((serverStateAdded > 0) ? "server-state" : "");
		if (connectedUiAdded > 0)
		{
			summary = AppendSource(summary, "connected-ui");
		}
		if (knownCacheAdded > 0)
		{
			summary = AppendSource(summary, "known-cache");
		}
		if (summary.Length != 0)
		{
			return summary;
		}
		return "local-only";
	}

	private static string AppendSource(string current, string source)
	{
		if (current.Length != 0)
		{
			return current + "+" + source;
		}
		return source;
	}

	internal static bool TryNormalizeHealth(float current, float maximum, out float normalizedCurrent, out float normalizedMaximum)
	{
		normalizedCurrent = 0f;
		normalizedMaximum = 0f;
		if (float.IsNaN(current) || float.IsInfinity(current) || float.IsNaN(maximum) || float.IsInfinity(maximum) || maximum <= 0f)
		{
			return false;
		}
		normalizedMaximum = maximum;
		normalizedCurrent = Math.Clamp(current, 0f, maximum);
		return true;
	}

	internal static float HealthFraction(float current, float maximum)
	{
		if (!TryNormalizeHealth(current, maximum, out var normalizedCurrent, out var normalizedMaximum))
		{
			return 0f;
		}
		return normalizedCurrent / normalizedMaximum;
	}

	internal static string HealthState(bool hasHealth, bool isAlive)
	{
		if (hasHealth)
		{
			if (!isAlive)
			{
				return "DEAD";
			}
			return "ALIVE";
		}
		return "SYNCING";
	}

	internal static string StatusTag(bool isLocal, bool isAdmin, bool hasHealth, bool isAlive)
	{
		string state = HealthState(hasHealth, isAlive);
		if (isLocal)
		{
			return "YOU · " + state;
		}
		if (!isAdmin)
		{
			return state;
		}
		return "ADMIN · " + state;
	}

	internal static string HealthText(bool hasHealth, float current, float maximum)
	{
		if (!hasHealth || !TryNormalizeHealth(current, maximum, out var normalizedCurrent, out var normalizedMaximum))
		{
			return "-- / -- HP";
		}
		return $"{MathF.Round(normalizedCurrent):0} / {MathF.Round(normalizedMaximum):0} HP";
	}
}
