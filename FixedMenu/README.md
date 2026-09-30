# Sons of the Forest Menu — RedLoader install package

This branch contains exactly three package files:

- `README.md` — these installation instructions and build provenance.
- `manifest.json` — the mod manifest, also embedded in the DLL.
- `SonsOfTheDead.dll` — the compiled RedLoader mod.

No source tree, game interop assemblies, loader/runtime files, or save data are included in the branch's current file tree. Source and build instructions remain on `main` and `redloader-port`.

## Build provenance

- Author: **cyberfox1337x**
- Mod/assembly version: **1.2.1 / 1.2.1.0**
- GUID: `com.stormfest.sonsoftheforestmenu`
- Target game build: **0.11.3**
- Built against uploaded RedLoader **0.8.6** and game interop assemblies; targets **.NET 6**.
- Source commit: `4ac140f1df254461df3c9f5ff2f1634c414bfde3` — Separate named teleports from the map tab.
- Output: `RedLoaderPort/bin/Release/net6.0/SonsOfTheDead.dll`.
- DLL SHA-256: `6e30e7ef6410cbf31344ceda2bb6a11b466e5cf64acba9ea9fc7a2eb45f9e044`.

This package includes the separate TELEPORT tab and the menu hotkey/close changes present in that source commit. The implementation version remains 1.2.1.

## Install

1. Install RedLoader in Sons of the Forest first, then close the game.
2. Back up the old mod DLL, mod settings, and saved teleport positions.
3. Copy `SonsOfTheDead.dll` into your game's `Mods` directory, for example:
   `D:\SteamLibrary\steamapps\common\Sons Of The Forest\Mods\`
4. The manifest is embedded in the DLL. The separate `manifest.json` is supplied for inspection; no separate manifest installation is required.
5. Do not leave duplicate copies of the menu DLL or load the original BepInEx menu alongside it.
6. Launch with RedLoader, inspect its startup logs, and test opening and closing the menu with backquote (`) or your configured menu key.

Do not replace the `_Redloader` folder, game saves, or saved teleport data to install this package.

## Verification and limitations

A fresh Release build of the source commit above passed with **0 errors and 0 warnings**. All **48 existing metadata and pure-logic checks passed**, including the embedded manifest reader, 17 uniquely resolved Harmony targets, and screen/map rules. Patches were not applied during those checks.

**In-game startup and functionality remain unverified.** The Linux workspace cannot launch the Windows game. The checks do not test live menu input, saved-spot rendering, every teleport destination, multiplayer behavior, or persistence in the game. This is a package cleanup, not a fix for the issues identified in the source review.

## Branch purpose

This is an install/download handoff, not a source replacement for `main`. A PR from this three-file branch against the full-source `main` will show the other repository files as deletions; do not merge it as a routine binary update. Earlier source files also remain in Git history even though they are absent from the current tree. Download the current branch snapshot for the three-file package rather than distributing its Git history.
