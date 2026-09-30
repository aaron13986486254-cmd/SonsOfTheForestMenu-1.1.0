using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Sons.Ai.Vail;
using Sons.Crafting.Structures;
using Sons.Input;
using Sons.StatSystem;
using Sons.Weapon;
using TheForest.Items.Inventory;
using TheForest.Items.Special;
using TheForest.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("feature state and tick")]
internal static class Features
{
	internal static bool GodMode;

	internal static bool InfStamina;

	internal static bool InfLung;

	internal static bool MaxStrength;

	internal static bool NoFallDamage;

	internal static bool NoHunger;

	internal static bool NoThirst;

	internal static bool NoCold;

	internal static bool AlwaysRested;

	internal static bool InfAmmo;

	private static bool _rapidFire;

	internal static bool RapidFire
	{
		get => _rapidFire;
		set
		{
			_rapidFire = value;
			if (!value)
			{
				RestoreRapidFireDelays();
			}
		}
	}

	internal static bool AiPause;

	internal static float FireDelay = 0.06f;

	internal static float DamageTakenMult = 1f;

	internal static bool InstantBuild;

	internal static bool InfiniteBuild;

	internal static bool InfiniteLogs;

	internal static bool InfRope;

	internal static float RopeLength = 300f;

	internal static bool Fly;

	internal static bool GravityEnabled = true;

	internal static float RunSpeed = 1f;

	internal static float JumpMult = 1f;

	internal static float SwimSpeed = 1f;

	internal static float FlySpeed = 12f;

	internal static bool MenuOpen;

	internal static string LastResult = string.Empty;

	private const float DialEpsilon = 0.001f;

	private const float RescanSeconds = 4f;

	private const float FlyBoostMultiplier = 3f;

	private const float AvatarScanInterval = 12f;

	private const float AvatarScanWindow = 36f;

	private static readonly Dictionary<string, bool> _commandState = new Dictionary<string, bool>();

	private static bool _baselineTaken;

	private static float _baseRun = 1f;

	private static float _baseJump = 1f;

	private static float _baseSwim = 1f;

	private static bool _baseFallDamage = true;

	private static bool _baseDisableGravity;

	private static bool _baseUseGravity = true;

	private static bool _gravityOverrideActive;

	private static bool _gravityRestorePending;

	private static bool _wasInWorld;

	private static float _nextAvatarScan;

	private static float _avatarScanDeadline;

	private static bool _aiPaused;

	private static bool _consoleGod;

	private static bool _consoleEnergy;

	private static bool _consoleInstantBuild;

	private static bool _consoleInitialised;

	private static StructureCraftingSystem _crafting;

	private static float _nextScan;

	private static RangedWeaponController[] _weapons;

	private static RopeGunController _ropeGun;

	private static readonly Dictionary<string, int> _rapidFireSuccessCounts = new Dictionary<string, int>();

	// Each controller owns its original delay until restoration succeeds or it is destroyed.
	private static readonly Dictionary<RangedWeaponController, float> _rapidFireDelays = new Dictionary<RangedWeaponController, float>();

	private const int MaxSpawnBatch = 100;

	private static StructureCraftingSystem Crafting
	{
		get
		{
			if ((Object)(object)_crafting == (Object)null)
			{
				_crafting = Safe.Get("build.crafting-system", () => Object.FindObjectOfType<StructureCraftingSystem>());
			}
			return _crafting;
		}
	}

	internal static bool GetCmd(string command)
	{
		bool on;
		return _commandState.TryGetValue(command, out on) & on;
	}

	internal static void SetCmd(string command, bool on)
	{
		_commandState[command] = on;
		string full = command + " " + (on ? "on" : "off");
		LastResult = (Game.Console(full) ? full : (command + " needs a loaded world"));
	}

	internal static void RunCmd(string command)
	{
		LastResult = (Game.Console(command) ? command : (command + " failed"));
	}

	private static int CmdActiveCount()
	{
		int active = 0;
		foreach (KeyValuePair<string, bool> item in _commandState)
		{
			if (item.Value)
			{
				active++;
			}
		}
		return active;
	}

	internal static int ActiveCount()
	{
		int active = 0;
		if (GodMode)
		{
			active++;
		}
		if (InfStamina)
		{
			active++;
		}
		if (InfLung)
		{
			active++;
		}
		if (MaxStrength)
		{
			active++;
		}
		if (NoFallDamage)
		{
			active++;
		}
		if (NoHunger)
		{
			active++;
		}
		if (NoThirst)
		{
			active++;
		}
		if (NoCold)
		{
			active++;
		}
		if (AlwaysRested)
		{
			active++;
		}
		if (InfAmmo)
		{
			active++;
		}
		if (RapidFire)
		{
			active++;
		}
		if (AiPause)
		{
			active++;
		}
		if (Mathf.Abs(DamageTakenMult - 1f) > 0.001f)
		{
			active++;
		}
		if (InstantBuild)
		{
			active++;
		}
		if (InfiniteBuild)
		{
			active++;
		}
		if (InfiniteLogs)
		{
			active++;
		}
		if (InfRope)
		{
			active++;
		}
		if (Fly)
		{
			active++;
		}
		if (!GravityEnabled)
		{
			active++;
		}
		active += Esp.ActiveCount();
		if (ItemEsp.Enabled)
		{
			active++;
		}
		if (ItemEsp.ShowItems)
		{
			active++;
		}
		if (ItemEsp.ShowContainers)
		{
			active++;
		}
		if (ItemEsp.ShowCollectibles)
		{
			active++;
		}
		if (ItemEsp.ShowWaypoints)
		{
			active++;
		}
		if (ItemEsp.Radar)
		{
			active++;
		}
		return active + CmdActiveCount();
	}

	internal static void CaptureBaseline()
	{
		if (!_baselineTaken)
		{
			FirstPersonCharacter fp = Game.Fp;
			Rigidbody body = Game.Body;
			if (!((Object)(object)fp == (Object)null) && !((Object)(object)body == (Object)null) && Safe.Run("movement.capture-baseline", (fp, body), ((FirstPersonCharacter Character, Rigidbody Body) state) =>
			{
				_baseRun = state.Character._runSpeed;
				_baseSwim = state.Character._swimSpeed;
				_baseJump = state.Character.JumpMultiplier;
				_baseFallDamage = state.Character._allowFallDamage;
				_baseDisableGravity = state.Character._disableGravity;
				_baseUseGravity = state.Body.useGravity;
			}, Noise.Warn))
			{
				RunSpeed = _baseRun;
				SwimSpeed = _baseSwim;
				JumpMult = _baseJump;
				GravityEnabled = true;
				_gravityOverrideActive = false;
				_gravityRestorePending = false;
				_baselineTaken = true;
				Diag.Info($"baseline captured  run={_baseRun:0.##} swim={_baseSwim:0.##} jump={_baseJump:0.##}");
			}
		}
	}

	internal static void RestoreBaseline()
	{
		FirstPersonCharacter fp = Game.Fp;
		Rigidbody body = Game.Body;
		if (!((Object)(object)fp == (Object)null) && !((Object)(object)body == (Object)null))
		{
			Fly = false;
			GravityEnabled = true;
			Safe.Run("movement.restore-baseline", fp, (FirstPersonCharacter character) =>
			{
				character._runSpeed = _baseRun;
				character._swimSpeed = _baseSwim;
				character.JumpMultiplier = _baseJump;
				character._allowFallDamage = _baseFallDamage;
			});
			RunSpeed = _baseRun;
			SwimSpeed = _baseSwim;
			JumpMult = _baseJump;
			NoFallDamage = false;
			ApplyGravityState(cancelMomentum: false);
		}
	}

	internal static void SetGravityEnabled(bool enabled)
	{
		if (!Game.InWorld)
		{
			LastResult = "Gravity needs a loaded world";
			return;
		}
		CaptureBaseline();
		if (!_baselineTaken)
		{
			LastResult = "Gravity controller is not ready";
			return;
		}
		bool wasEnabled = GravityEnabled;
		GravityEnabled = enabled;
		if (!ApplyGravityState(!enabled & wasEnabled))
		{
			LastResult = (enabled ? "Gravity restore pending - retrying" : "Zero-G pending - retrying");
		}
		else
		{
			string lastResult;
			if (enabled)
			{
				lastResult = (Fly ? "Gravity armed - Fly still controls movement" : "Gravity on - baseline restored");
			}
			else
			{
				lastResult = "Gravity off - zero-G active";
			}
			LastResult = lastResult;
		}
	}

	internal static void Tick()
	{
		bool inWorld = Game.InWorld;
		if (!RapidFire || !inWorld)
		{
			RestoreRapidFireDelays();
		}
		DirectGameApi.EnforceLocalTimeOverrideSafety(inWorld);
		if (!inWorld)
		{
			ResetWorldState();
			return;
		}
		PrefetchAvatarsAfterLoad();
		CaptureBaseline();
		SyncConsoleToggles();
		EnforceVitals();
		EnforceWarmth();
		EnforceMovement();
		EnforceAiPause();
		EnforceGravity();
		EnforceFly();
	}

	private static void ResetWorldState()
	{
		_baselineTaken = false;
		_wasInWorld = false;
		_nextAvatarScan = 0f;
		_avatarScanDeadline = 0f;
		GravityEnabled = true;
		Fly = false;
		_baseDisableGravity = false;
		_baseUseGravity = true;
		_gravityOverrideActive = false;
		_gravityRestorePending = false;
	}

	private static void PrefetchAvatarsAfterLoad()
	{
		if (!_wasInWorld)
		{
			_wasInWorld = true;
			_nextAvatarScan = 0f;
			_avatarScanDeadline = Time.unscaledTime + 36f;
		}
		if (Time.unscaledTime >= _nextAvatarScan && Time.unscaledTime <= _avatarScanDeadline)
		{
			_nextAvatarScan = Time.unscaledTime + 12f;
			Safe.Run("avatars.prefetch", Roster.Prefetch);
		}
	}

	private static void EnforceVitals()
	{
		Vitals vitals = Game.Vitals;
		if ((Object)(object)vitals == (Object)null)
		{
			return;
		}
		if (GodMode)
		{
			HealthStat stat = Safe.Get("vitals.health", vitals, (Vitals v) => v._health);
			float max = Game.Max((Stat)(object)stat);
			Game.SetCurrent((Stat)(object)stat, max);
			Safe.Run("vitals.target-health", (vitals, max), ((Vitals vitals, float max) t) =>
			{
				t.vitals._targetHealth = t.max;
			});
		}
		if (InfLung)
		{
			Safe.Run("vitals.lung", vitals, (Vitals v) =>
			{
				BreathingData lungBreathing = v.LungBreathing;
				if (lungBreathing != null)
				{
					lungBreathing.CurrentLungAir = lungBreathing.MaxLungAirCapacity;
				}
			});
		}
		if (InstantBuild)
		{
			SetInstantBuild(on: true);
		}
		if (InfiniteBuild)
		{
			SetInfiniteItems(on: true);
		}
		if (InfiniteLogs)
		{
			SetInfiniteHeld(on: true);
		}
		if (InfAmmo)
		{
			TopUpAmmo();
		}
		if (InfRope)
		{
			ExtendRope();
		}
		if (InfStamina)
		{
			Game.Peg((Stat)(object)Safe.Get("vitals.stamina", vitals, (Vitals v) => v._stamina));
		}
		if (MaxStrength)
		{
			Game.Peg((Stat)(object)Safe.Get("vitals.strength", vitals, (Vitals v) => v._strength));
		}
		if (NoHunger)
		{
			Game.Peg((Stat)(object)Safe.Get("vitals.fullness", vitals, (Vitals v) => v._fullness));
		}
		if (NoThirst)
		{
			Game.Peg((Stat)(object)Safe.Get("vitals.hydration", vitals, (Vitals v) => v._hydration));
		}
		if (AlwaysRested)
		{
			Game.Peg((Stat)(object)Safe.Get("vitals.rested", vitals, (Vitals v) => v._rested));
		}
	}

	private static void EnforceWarmth()
	{
		if (!NoCold)
		{
			return;
		}
		PlayerStats stats = Game.Stats;
		if (!((Object)(object)stats == (Object)null))
		{
			Safe.Run("player.interior-warmth", stats, (PlayerStats s) =>
			{
				s.InteriorSpaceWarmth = true;
			});
		}
	}

	private static void EnforceMovement()
	{
		FirstPersonCharacter fp = Game.Fp;
		if ((Object)(object)fp == (Object)null)
		{
			return;
		}
		Safe.Run("movement.enforce", fp, (FirstPersonCharacter character) =>
		{
			character._allowFallDamage = !NoFallDamage;
			if (Mathf.Abs(character._runSpeed - RunSpeed) > 0.001f)
			{
				character._runSpeed = RunSpeed;
			}
			if (Mathf.Abs(character._swimSpeed - SwimSpeed) > 0.001f)
			{
				character._swimSpeed = SwimSpeed;
			}
			if (Mathf.Abs(character.JumpMultiplier - JumpMult) > 0.001f)
			{
				character.JumpMultiplier = JumpMult;
			}
		});
	}

	private static void EnforceAiPause()
	{
		if (AiPause != _aiPaused && Safe.Run("ai.set-paused", AiPause, (Action<bool>)VailWorldSimulation.SetPaused, Noise.Warn))
		{
			_aiPaused = AiPause;
		}
	}

	private static void SyncConsoleToggles()
	{
		if (!_consoleInitialised)
		{
			_consoleGod = (_consoleEnergy = (_consoleInstantBuild = false));
			_consoleInitialised = true;
		}
		if (GodMode != _consoleGod)
		{
			Game.Console("godmode " + (GodMode ? "on" : "off"));
			_consoleGod = GodMode;
		}
		if (InfStamina != _consoleEnergy)
		{
			Game.Console("energyhack " + (InfStamina ? "on" : "off"));
			_consoleEnergy = InfStamina;
		}
		if (!InstantBuild && _consoleInstantBuild)
		{
			SetInstantBuild(on: false);
		}
		_consoleInstantBuild = InstantBuild;
	}

	private static void SetInstantBuild(bool on)
	{
		StructureCraftingSystem crafting = Crafting;
		if (!((Object)(object)crafting == (Object)null))
		{
			Safe.Run("build.instant", (crafting, on), ((StructureCraftingSystem crafting, bool on) t) =>
			{
				t.crafting.InstantBuild = t.on;
			});
		}
	}

	private static void SetInfiniteItems(bool on)
	{
		StructureCraftingSystem crafting = Crafting;
		if (!((Object)(object)crafting == (Object)null))
		{
			Safe.Run("build.infinite-items", (crafting, on), ((StructureCraftingSystem crafting, bool on) t) =>
			{
				t.crafting.InfiniteItemHack = t.on;
			});
		}
	}

	private static void SetInfiniteHeld(bool on)
	{
		Safe.Run("build.infinite-held", on, (bool enabled) =>
		{
			PlayerInventory inventory = LocalPlayer.Inventory;
			if (!((Object)(object)inventory == (Object)null))
			{
				IHeldOnlyItemController heldOnlyItemController = inventory.HeldOnlyItemController;
				if (heldOnlyItemController != null)
				{
					heldOnlyItemController.InfiniteHack = enabled;
				}
			}
		});
	}

	private static void Rescan()
	{
		if (!(Time.time < _nextScan))
		{
			_nextScan = Time.time + 4f;
			Safe.Run("weapons.rescan", () =>
			{
				_weapons = Object.FindObjectsOfType<RangedWeaponController>();
				_ropeGun = Object.FindObjectOfType<RopeGunController>();
			});
		}
	}

	private static void TopUpAmmo()
	{
		Rescan();
		if (_weapons == null)
		{
			return;
		}
		RangedWeaponController[] weapons = _weapons;
		foreach (RangedWeaponController controller in weapons)
		{
			if ((Object)(object)controller == (Object)null)
			{
				continue;
			}
			Safe.Run("weapons.top-up-ammo", controller, (RangedWeaponController weapon) =>
			{
				RangedWeapon rangedWeapon = weapon._rangedWeapon;
				if (!((Object)(object)rangedWeapon == (Object)null))
				{
					var ammo = rangedWeapon._ammo;
					if (ammo != null && ammo._currentCount < ammo._maxCount)
					{
						ammo._currentCount = ammo._maxCount;
					}
				}
			});
		}
	}

	private static void RestoreRapidFireDelay(RangedWeaponController weapon)
	{
		if (ReferenceEquals(weapon, null) || !_rapidFireDelays.TryGetValue(weapon, out float originalDelay))
		{
			return;
		}
		if ((Object)(object)weapon == (Object)null || Safe.Run("weapons.rapid-fire.restore", (weapon, originalDelay), ((RangedWeaponController Weapon, float Delay) state) =>
		{
			state.Weapon._fireDelay = state.Delay;
		}, Noise.Warn))
		{
			_rapidFireDelays.Remove(weapon);
		}
	}

	private static void RestoreRapidFireDelays()
	{
		if (_rapidFireDelays.Count == 0)
		{
			return;
		}
		// Restoration removes entries; take a snapshot and retain failed writes for retry.
		foreach (RangedWeaponController weapon in new List<RangedWeaponController>(_rapidFireDelays.Keys))
		{
			RestoreRapidFireDelay(weapon);
		}
	}

	internal static void TryRapidFire(RangedWeaponController weapon)
	{
		if (!RapidFire || !Game.InWorld || (Object)(object)weapon == (Object)null)
		{
			RestoreRapidFireDelay(weapon);
			return;
		}
		Safe.Run("weapons.rapid-fire", weapon, (RangedWeaponController controller) =>
		{
			string name = ((object)controller).GetType().Name;
			Diag.InfoOnce("rapid-fire.observed." + name, "Rapid Fire observed active controller '" + name + "'.");
			bool inputHeld = !MenuOpen && Input.GetMouseButton(0);
			if (!IsFirearmController(controller))
			{
				RestoreRapidFireDelay(controller);
				ReportRapidFireGate(name, inputHeld, "not-firearm");
			}
			else
			{
				RangedWeapon rangedWeapon = controller._rangedWeapon;
				var val = ((rangedWeapon != null) ? rangedWeapon._ammo : null);
				RapidFireGate rapidFireGate = RapidFireRules.FirstBlockingGate(RapidFire, inputHeld, ((Behaviour)controller).isActiveAndEnabled, (Object)(object)controller._remotePlayer == (Object)null, initialized: (Object)(object)rangedWeapon != (Object)null, controller._blockFiring, controller._isReloading || controller._reloadQueued, controller._isChargedWeapon || controller._isCharging, ((HeldControllerBase)controller)._isUnequipping || ((HeldControllerBase)controller)._isStashing || ((HeldControllerBase)controller)._isDropping || ((HeldControllerBase)controller).IsFirstLookRunning, controller._mustAimToFire, (Object)(object)rangedWeapon != (Object)null && rangedWeapon.IsAiming, val != null && val._currentCount > 0);
				if (rapidFireGate != RapidFireGate.Ready)
				{
					RestoreRapidFireDelay(controller);
					ReportRapidFireGate(name, inputHeld, rapidFireGate.ToString());
					return;
				}
				if (float.IsNaN(FireDelay) || float.IsInfinity(FireDelay) || FireDelay <= 0f)
				{
					RestoreRapidFireDelay(controller);
					return;
				}
				// Never capture an already overridden delay as the original value.
				if (!_rapidFireDelays.ContainsKey(controller))
				{
					_rapidFireDelays.Add(controller, controller._fireDelay);
				}
				if (Mathf.Abs(controller._fireDelay - FireDelay) > 0.0005f)
				{
					controller._fireDelay = FireDelay;
				}
				if (!RapidFireRules.IsCadenceReady(controller._nextFireDelay, FireDelay))
				{
					ReportRapidFireGate(name, inputHeld, "Cadence");
				}
				else
				{
					controller.FireWeapon();
					ReportRapidFireSuccess(name);
				}
			}
		}, Noise.Warn);
	}

	private static bool IsFirearmController(RangedWeaponController controller)
	{
		if ((controller is BowWeaponController || controller is CrossbowWeaponController || controller is FlareWeaponController || controller is GrenadeWeaponController || controller is MolotovWeaponController || controller is RopeGunController || controller is SlingshotWeaponController || controller is SmallRockWeaponController || controller is TimeBombWeaponController) ? true : false)
		{
			return false;
		}
		if ((!(controller is CompactPistolWeaponController) && !(controller is RevolverWeaponController) && !(controller is RifleAnimatorController) && !(controller is ShotgunWeaponController) && !(controller is StunGunWeaponController)) || 1 == 0)
		{
			return !string.IsNullOrWhiteSpace(controller._gunShotAudioEvent);
		}
		return true;
	}

	private static void ReportRapidFireGate(string controllerName, bool inputHeld, string gate)
	{
		if (inputHeld)
		{
			Diag.InfoOnce("rapid-fire.gate." + controllerName + "." + gate, $"Rapid Fire trace: controller={controllerName}; inputHeld=true; firstGate={gate}; successes={RapidFireSuccessCount(controllerName)}");
		}
	}

	private static void ReportRapidFireSuccess(string controllerName)
	{
		int count = RapidFireSuccessCount(controllerName) + 1;
		_rapidFireSuccessCounts[controllerName] = count;
		if ((count == 1 || count == 10 || count == 50) ? true : false)
		{
			Diag.Info($"Rapid Fire trace: controller={controllerName}; inputHeld=true; firstGate=Ready; successes={count}");
		}
	}

	private static int RapidFireSuccessCount(string controllerName)
	{
		if (!_rapidFireSuccessCounts.TryGetValue(controllerName, out var count))
		{
			return 0;
		}
		return count;
	}

	private static void ExtendRope()
	{
		Rescan();
		if (!((Object)(object)_ropeGun == (Object)null))
		{
			Safe.Run("weapons.rope-length", _ropeGun, (RopeGunController gun) =>
			{
				gun._maxRopeLength = RopeLength;
				gun._maxFiringRange = RopeLength;
				gun._currentRopeLength = RopeLength;
			});
		}
	}

	private static void EnforceGravity()
	{
		ApplyGravityState(cancelMomentum: false);
	}

	private static bool ApplyGravityState(bool cancelMomentum)
	{
		if (!_baselineTaken)
		{
			return false;
		}
		GravityWork work = GravityRules.Resolve(Fly, GravityEnabled, _gravityOverrideActive, _gravityRestorePending);
		if (work == GravityWork.None)
		{
			return true;
		}
		FirstPersonCharacter fp = Game.Fp;
		Rigidbody body = Game.Body;
		if ((Object)(object)fp == (Object)null || (Object)(object)body == (Object)null)
		{
			return false;
		}
		if (work == GravityWork.RestoreBaseline)
		{
			bool flag = WriteAndVerifyGravity(fp, body, _baseDisableGravity, _baseUseGravity);
			_gravityRestorePending = !flag;
			if (flag)
			{
				_gravityOverrideActive = false;
			}
			return flag;
		}
		bool enteringOverride = !_gravityOverrideActive;
		if (cancelMomentum | enteringOverride)
		{
			Safe.Run("movement.gravity-transition", fp, (FirstPersonCharacter character) =>
			{
				character.ClearRigidbodyVelocity();
				character.ClearFallDamageFlag();
			});
			Safe.Run("movement.gravity-transition-angular", body, (Rigidbody rigidbody) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				rigidbody.angularVelocity = Vector3.zero;
			});
		}
		_gravityOverrideActive = true;
		_gravityRestorePending = false;
		return WriteAndVerifyGravity(fp, body, targetDisabled: true, targetUseGravity: false);
	}

	private static bool WriteAndVerifyGravity(FirstPersonCharacter fp, Rigidbody body, bool targetDisabled, bool targetUseGravity)
	{
		bool num = Safe.Get("movement.gravity-read-controller", fp, (FirstPersonCharacter character) => character._disableGravity, !targetDisabled) == targetDisabled || Safe.Run("movement.gravity-write-controller", (fp, targetDisabled), ((FirstPersonCharacter Character, bool Disabled) state) =>
		{
			state.Character.SetDisabledGravity(state.Disabled);
		});
		bool bodyWritten = Safe.Get("movement.gravity-read-body", body, (Rigidbody rigidbody) => rigidbody.useGravity, !targetUseGravity) == targetUseGravity || Safe.Run("movement.gravity-write-body", (body, targetUseGravity), ((Rigidbody Body, bool UseGravity) state) =>
		{
			state.Body.useGravity = state.UseGravity;
		});
		bool controllerMatches = Safe.Get("movement.gravity-verify-controller", (fp, targetDisabled), ((FirstPersonCharacter Character, bool Disabled) state) => state.Character._disableGravity == state.Disabled, fallback: false);
		bool bodyMatches = Safe.Get("movement.gravity-verify-body", (body, targetUseGravity), ((Rigidbody Body, bool UseGravity) state) => state.Body.useGravity == state.UseGravity, fallback: false);
		return num & bodyWritten & controllerMatches & bodyMatches;
	}

	private static void EnforceFly()
	{
		if (Fly)
		{
			DoFly();
		}
	}

	private static void DoFly()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Transform player = Game.PlayerTransform;
		Camera camera = Game.Cam;
		if ((Object)(object)player == (Object)null || (Object)(object)camera == (Object)null)
		{
			return;
		}
		Rigidbody body = Game.Body;
		if ((Object)(object)body != (Object)null)
		{
			Safe.Run("movement.fly-zero-velocity", body, (Rigidbody rigidbody) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			});
		}
		if (MenuOpen)
		{
			return;
		}
		Vector3 direction = ReadFlyInput(((Component)camera).transform);
		if (!(direction == Vector3.zero))
		{
			float speed = FlySpeed * (Input.GetKey((KeyCode)304) ? 3f : 1f);
			Safe.Run("movement.fly-move", (player, direction, speed), ((Transform player, Vector3 direction, float speed) t) =>
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				var (val, _, _) = t;
				val.position += t.direction.normalized * t.speed * Time.deltaTime;
			});
		}
	}

	private static Vector3 ReadFlyInput(Transform camera)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		Vector3 direction = Vector3.zero;
		if (Input.GetKey((KeyCode)119))
		{
			direction += camera.forward;
		}
		if (Input.GetKey((KeyCode)115))
		{
			direction -= camera.forward;
		}
		if (Input.GetKey((KeyCode)100))
		{
			direction += camera.right;
		}
		if (Input.GetKey((KeyCode)97))
		{
			direction -= camera.right;
		}
		if (Input.GetKey((KeyCode)32))
		{
			direction += Vector3.up;
		}
		if (Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)99))
		{
			direction -= Vector3.up;
		}
		return direction;
	}

	internal static void SpawnItem(int id, int amount)
	{
		int requested = Mathf.Clamp(amount, 1, 100);
		int spawned = 0;
		for (int i = 0; i < requested; i++)
		{
			if (Game.Console($"spawnitem {id}"))
			{
				spawned++;
			}
		}
		LastResult = ((spawned > 0) ? $"Spawned {spawned}× item {id}" : $"Could not spawn item {id}");
	}

	internal static void SpawnCharacter(string key, int amount)
	{
		int requested = Mathf.Max(1, amount);
		LastResult = (Game.Console($"addcharacter {key} {requested}") ? $"Spawned {requested}× {key}" : "Spawn needs a loaded world");
	}
}
