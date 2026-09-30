using System;

namespace Cyberfox1337x.SonsOfTheForest;

internal static class RapidFireRules
{
	private const float MinimumDelay = 0.01f;

	static RapidFireRules()
	{
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "held-fire-safety-and-cadence";
	}

	internal static RapidFireGate FirstBlockingGate(bool enabled, bool inputHeld, bool active, bool local, bool initialized, bool blocked, bool reloading, bool charged, bool transitioning, bool aimingRequired, bool aiming, bool hasAmmo)
	{
		if (!enabled)
		{
			return RapidFireGate.Disabled;
		}
		if (!inputHeld)
		{
			return RapidFireGate.InputReleased;
		}
		if (!active)
		{
			return RapidFireGate.Inactive;
		}
		if (!local)
		{
			return RapidFireGate.Remote;
		}
		if (!initialized)
		{
			return RapidFireGate.Uninitialized;
		}
		if (blocked)
		{
			return RapidFireGate.Blocked;
		}
		if (reloading)
		{
			return RapidFireGate.Reloading;
		}
		if (charged)
		{
			return RapidFireGate.Charged;
		}
		if (transitioning)
		{
			return RapidFireGate.Transitioning;
		}
		if (aimingRequired && !aiming)
		{
			return RapidFireGate.AimRequired;
		}
		if (!hasAmmo)
		{
			return RapidFireGate.Empty;
		}
		return RapidFireGate.Ready;
	}

	internal static bool MayAutoFire(bool enabled, bool inputHeld, bool active, bool local, bool initialized, bool blocked, bool reloading, bool charged, bool transitioning, bool aimingRequired, bool aiming, bool hasAmmo)
	{
		return FirstBlockingGate(enabled, inputHeld, active, local, initialized, blocked, reloading, charged, transitioning, aimingRequired, aiming, hasAmmo) == RapidFireGate.Ready;
	}

	internal static bool IsCadenceReady(float elapsed, float requestedDelay)
	{
		if (float.IsNaN(elapsed) || float.IsInfinity(elapsed) || elapsed < 0f || float.IsNaN(requestedDelay) || float.IsInfinity(requestedDelay))
		{
			return false;
		}
		return elapsed > Math.Max(requestedDelay, 0.01f);
	}
}
