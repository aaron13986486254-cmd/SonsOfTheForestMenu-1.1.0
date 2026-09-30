using System;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("guarded interop seam")]
internal static class Safe
{
	internal static bool Run(string site, Action work, Noise noise = Noise.Transient)
	{
		if (work == null)
		{
			return false;
		}
		try
		{
			work();
			return true;
		}
		catch (Exception error)
		{
			Report(site, error, noise);
			return false;
		}
	}

	internal static bool Run<TState>(string site, TState state, Action<TState> work, Noise noise = Noise.Transient)
	{
		if (work == null)
		{
			return false;
		}
		try
		{
			work(state);
			return true;
		}
		catch (Exception error)
		{
			Report(site, error, noise);
			return false;
		}
	}

	internal static TResult Get<TResult>(string site, Func<TResult> read, TResult fallback = default(TResult), Noise noise = Noise.Transient)
	{
		if (read == null)
		{
			return fallback;
		}
		try
		{
			return read();
		}
		catch (Exception error)
		{
			Report(site, error, noise);
			return fallback;
		}
	}

	internal static TResult Get<TState, TResult>(string site, TState state, Func<TState, TResult> read, TResult fallback = default(TResult), Noise noise = Noise.Transient)
	{
		if (read == null)
		{
			return fallback;
		}
		try
		{
			return read(state);
		}
		catch (Exception error)
		{
			Report(site, error, noise);
			return fallback;
		}
	}

	private static void Report(string site, Exception error, Noise noise)
	{
		string detail = Diag.Describe(error);
		switch (noise)
		{
		case Noise.Fatal:
			Diag.Error("[" + site + "] " + detail);
			break;
		case Noise.Warn:
			Diag.Warn("[" + site + "] " + detail);
			break;
		default:
			Diag.Once(site, "[" + site + "] " + detail);
			break;
		}
	}
}
