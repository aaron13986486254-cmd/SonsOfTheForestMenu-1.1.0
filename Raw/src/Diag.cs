using System;
using System.Collections.Concurrent;
using RedLoader;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x]
internal static class Diag
{
    static readonly ConcurrentDictionary<string,bool> _seen = new();

    internal static void Bind(object ignored = null) { }

    internal static void Info(string message) => RLog.Msg(message);
    internal static void Warn(string message) => RLog.Warning(message);
    internal static void Error(string message) => RLog.Error(message);

    internal static void InfoOnce(string site, string message) => Once(site, message);

    internal static void Once(string site, string message)
    {
        if (_seen.TryAdd(site, true))
            RLog.Warning($"{message}  (further reports from '{site}' suppressed)");
    }

    internal static string Describe(Exception error) =>
        error is null ? "(no detail)" : $"{error.GetType().Name}: {error.Message}";

    internal static string Redact(string secret)
    {
        if (string.IsNullOrEmpty(secret)) return "(unset)";
        return secret.Length <= 4
            ? $"(set, {secret.Length} chars)"
            : $"(set, {secret.Length} chars, ends {secret[^4..]})";
    }
}