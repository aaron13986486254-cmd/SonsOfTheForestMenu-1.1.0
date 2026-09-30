# Sons of the Forest Menu — RedLoader port

RedLoader compatibility port of cyberfox1337x's original menu, targeting implementation 1.2.1 and game build 0.11.3. GUID: `com.stormfest.sonsoftheforestmenu`.

The original BepInEx source remains untouched under `SonsOfTheForestMenu-1.1.0-source/`. The newer decompiled map, teleport and screen-scaling code remains in the port; this is not a simplified replacement menu. The third tab is labelled MAP in the 1.2.1 implementation and contains the newer teleport experience.

## Build

Use .NET SDK 8 or newer (the source uses C# 12 primary constructors). The mod still targets the **.NET 6 runtime** used by RedLoader.

From `RedLoaderPort` on Windows:

```powershell
dotnet build ".\SonsOfTheDead.csproj" -c Release /p:GameDir="D:\SteamLibrary\steamapps\common\Sons Of The Forest"
```

When building against the uploaded `_Redloader` folder, set `GameDir` to the repository root instead. Use your installed game's generated `Game` interop assemblies, not reconstructed/stub assemblies or the separate `unity-libs` reference DLLs.

Output: `bin/Release/net6.0/SonsOfTheDead.dll`. `manifest.json` is both embedded in the DLL for RedLoader's manifest reader and copied to the output directory.

Install the DLL in the game's RedLoader `Mods` directory. Do not distribute the game's interop DLLs or the uploaded loader/runtime folder as part of the mod.

## Verified in the cloud workspace

- Release build against the uploaded game interop DLLs and RedLoader 0.8.6: **0 errors, 0 warnings**.
- Compiled assembly version: 1.2.1.0.
- No compiled BepInEx reference; `RedLoader`, `SonsSdk` and Harmony dependencies retained.
- Embedded manifest read successfully by the uploaded SDK's actual `ManifestReader`.
- All 17 attributed Harmony targets exist and resolve uniquely in the uploaded assemblies. Patches were **not applied** in this check.
- Pure screen-fitting rules checked at 640x480 through 7680x4320, automatic and manual scaling.
- Pure map fit, projection/unprojection, zoom, panning and bunker-label checks.

Repeat the non-game checks from the repository root:

```text
dotnet build tools/inspect/inspect.csproj -c Release
dotnet tools/inspect/bin/Release/net8.0/inspect.dll RedLoaderPort/bin/Release/net6.0/SonsOfTheDead.dll --verify
```

Repeat the source-based audit regressions from the repository root:

```text
dotnet run --project tools/audit/audit.csproj -c Release
```

The audit runner uses Roslyn bundled with .NET SDK 8 to compile the actual ESP drawing and Rapid Fire methods against test doubles. Its 20 tests cover first-frame health labels/icons, initialization retry, per-controller delay restoration, inactive/remote/input safety gates, world exit, failed writes, and destroyed controllers. It also repeats 10,000 randomized map/layout cases, all 4,096 Rapid Fire gate combinations, and all 16 gravity ownership states. These checks run in CI but do not execute Unity/IL2CPP or apply Harmony patches.

## Runtime verification still required

This Linux workspace has reference/interop assemblies but no running Windows game. Compilation and metadata checks do not establish behavioral parity or prove Unity/IL2CPP initialization works.

The uploaded `ErrorLog.log` only contained Steam minidump initialization lines, not a startup test of this newly built DLL.

A game-side test must inspect RedLoader logs, open the menu with backquote, and exercise:

- Settings, hotkey rebinding, automatic/manual scaling and saved configuration.
- PLAYER, MOVEMENT, COMBAT, SPAWN, GEAR, WORLD, GAME SETUP and CHEATS.
- MAP: actual native surface texture, panning, zoom, GPS/bunker/player markers, coordinate alignment, click-to-teleport, saved spots and reload persistence.
- VISION: entity/item ESP, dead-entity display and radar.
- ONLINE: names, Steam IDs, avatar fetching and authority-gated teleport/bring/kill/revive.
- CONSOLE commands; title screen, loading screens, scene changes and multiplayer transitions.

Keep startup and feature errors visible and investigate failures rather than treating a successful compile as a runtime pass.
