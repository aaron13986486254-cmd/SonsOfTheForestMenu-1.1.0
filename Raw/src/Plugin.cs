using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using RedLoader;
using SonsSdk;
using UnityEngine;

namespace Cyberfox1337x.SonsOfTheForest;

public sealed class Plugin : SonsMod
{
    const float OpacityMin = .3f, OpacityMax = 1f, OpacityDefault = .94f;
    const float RadiusMin = 4f, RadiusMax = 20f, RadiusDefault = 10f;
    const float ScrollMin = .25f, ScrollMax = 3f, ScrollDefault = 1f;

    internal static HarmonyLib.Harmony Harmony { get; private set; }
    internal static string DataRoot { get; private set; }

    internal static KeyCode ToggleKey { get; private set; } = KeyCode.BackQuote;
    internal static string ToggleKeyGlyph { get; private set; } = "`";

    static ConfigCategory _category;
    static ConfigEntry<KeyCode> _toggle;
    static ConfigEntry<float> _posX, _posY, _opacity, _cornerRadius, _scrollSensitivity, _menuScale;
    static ConfigEntry<bool> _automaticMenuScale;
    static ConfigEntry<string> _apiKey;

    internal static float PosX { get => _posX?.Value ?? float.NaN; set { if (_posX != null) _posX.Value = value; } }
    internal static float PosY { get => _posY?.Value ?? float.NaN; set { if (_posY != null) _posY.Value = value; } }
    internal static string SteamApiKey { get => _apiKey?.Value ?? ""; set { if (_apiKey != null) _apiKey.Value = value ?? ""; } }
    internal static float MenuOpacity { get => _opacity == null ? OpacityDefault : Mathf.Clamp(_opacity.Value, OpacityMin, OpacityMax); set { if (_opacity != null) _opacity.Value = Mathf.Clamp(value, OpacityMin, OpacityMax); } }
    internal static float CornerRadius { get => _cornerRadius == null ? RadiusDefault : Mathf.Clamp(_cornerRadius.Value, RadiusMin, RadiusMax); set { if (_cornerRadius != null) _cornerRadius.Value = Mathf.Clamp(value, RadiusMin, RadiusMax); } }
    internal static float ScrollSensitivity { get => _scrollSensitivity == null ? ScrollDefault : Mathf.Clamp(_scrollSensitivity.Value, ScrollMin, ScrollMax); set { if (_scrollSensitivity != null) _scrollSensitivity.Value = Mathf.Clamp(value, ScrollMin, ScrollMax); } }
    internal static bool AutomaticMenuScale { get => _automaticMenuScale?.Value ?? true; set { if (_automaticMenuScale != null) _automaticMenuScale.Value = value; } }
    internal static float MenuScale { get => MenuScaleRules.ClampScale(_menuScale?.Value ?? 1f); set { if (_menuScale != null) _menuScale.Value = MenuScaleRules.ClampScale(value); } }

    public Plugin()
    {
        HarmonyPatchAll = true;
    }

    protected override void OnInitializeMod()
    {
        DataRoot = DataPath.Path;
        Directory.CreateDirectory(DataRoot);
        BindConfiguration();
        Diag.Info(Signature.BootLine(ToggleKey.ToString()));
    }

    protected override void OnSdkInitialized()
    {
        try
        {
            Avatars.Init(SteamApiKey);
            Teleport.Load();

            ClassInjector.RegisterTypeInIl2Cpp<MenuWindow>();

            Harmony = new HarmonyLib.Harmony(Signature.Guid);
            ApplyLogFilters();

            var host = new GameObject("Cyberfox1337x.SonsOfTheForest.Menu")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            UnityEngine.Object.DontDestroyOnLoad(host);
            host.AddComponent<MenuWindow>();

            Diag.Info("RedLoader port initialized.");
        }
        catch (Exception ex)
        {
            Diag.Error("RedLoader initialization failed: " + Diag.Describe(ex));
        }
    }

    internal static void SaveConfig()
    {
        try { _category?.SaveToFile(false); }
        catch (Exception ex) { Diag.Warn("config.save: " + Diag.Describe(ex)); }
    }

    internal static bool TrySetToggleKey(KeyCode key, out string reason)
    {
        if (_toggle == null) { reason = "Configuration is not loaded yet."; return false; }
        if (key == KeyCode.None) { reason = "That key could not be read — try another."; return false; }
        if (IsModifier(key)) { reason = $"{key} is a modifier. Pick a key that can be pressed on its own."; return false; }

        _toggle.Value = key;
        ToggleKey = key;
        ToggleKeyGlyph = GlyphFor(key);
        SaveConfig();
        Diag.Info($"Menu hotkey rebound to {key}.");
        reason = null;
        return true;
    }

    static void BindConfiguration()
    {
        _category = ConfigSystem.CreateFileCategory(Signature.Guid, Signature.Name, "SonsOfTheForestMenu.cfg");

        _toggle = _category.CreateEntry("Hotkeys.Toggle", KeyCode.BackQuote, "Toggle",
            "Toggles menu visibility.");
        _posX = _category.CreateEntry("Window.PosX", float.NaN, "Panel X",
            "Panel X. NaN centres it.");
        _posY = _category.CreateEntry("Window.PosY", float.NaN, "Panel Y",
            "Panel Y. NaN centres it.");
        _apiKey = _category.CreateEntry("Steam.ApiKey", "", "Steam Web API key",
            "Steam Web API key used to fetch player avatars.");
        _opacity = _category.CreateEntry("Menu.Opacity", OpacityDefault, "Opacity",
            "Panel background opacity.");
        _cornerRadius = _category.CreateEntry("Menu.CornerRadius", RadiusDefault, "Corner radius",
            "Panel corner radius in pixels.");
        _scrollSensitivity = _category.CreateEntry("Menu.ScrollSensitivity", ScrollDefault, "Scroll sensitivity",
            "Mouse-wheel speed multiplier.");
        _automaticMenuScale = _category.CreateEntry("Menu.AutomaticScale", true, "Automatic scale",
            "Scale the whole menu for the game resolution.");
        _menuScale = _category.CreateEntry("Menu.Scale", 1f, "Manual scale",
            "Manual menu scale from 0.75 to 3.0 when automatic scaling is disabled.");

        ToggleKey = _toggle.Value;
        ToggleKeyGlyph = GlyphFor(ToggleKey);
    }

    static void ApplyLogFilters()
    {
        try
        {
            PatchDebugOverload(nameof(Debug.LogError), typeof(Patch_Debug_LogError));
            PatchDebugOverload(nameof(Debug.LogWarning), typeof(Patch_Debug_LogWarning));
        }
        catch (Exception ex)
        {
            Diag.Warn("Boot-noise filters could not be applied: " + Diag.Describe(ex));
        }
    }

    static void PatchDebugOverload(string methodName, Type prefixHolder)
    {
        MethodBase target = AccessTools.Method(typeof(Debug), methodName, new[] { typeof(Il2CppSystem.Object) });
        if (target == null)
            throw new InvalidOperationException($"{methodName}(Il2CppSystem.Object) not found on UnityEngine.Debug");
        Harmony.Patch(target, prefix: new HarmonyMethod(prefixHolder, "Prefix"));
    }

    static bool IsModifier(KeyCode key) => key switch
    {
        KeyCode.LeftShift or KeyCode.RightShift => true,
        KeyCode.LeftControl or KeyCode.RightControl => true,
        KeyCode.LeftAlt or KeyCode.RightAlt or KeyCode.AltGr => true,
        KeyCode.LeftCommand or KeyCode.RightCommand => true,
        KeyCode.LeftWindows or KeyCode.RightWindows => true,
        _ => false,
    };

    static string GlyphFor(KeyCode key) => key switch
    {
        KeyCode.BackQuote => "`",
        KeyCode.Tab => "TAB",
        KeyCode.Insert => "INS",
        KeyCode.Delete => "DEL",
        KeyCode.Home => "HOME",
        _ => key.ToString(),
    };
}