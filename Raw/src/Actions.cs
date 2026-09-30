namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("curated command tables")]
internal static class Actions
{
	public static readonly ToggleCommand[] PlayerSwitches;

	public static readonly ActionCommand[] PlayerActions;

	public static readonly ValueCommand[] PlayerValues;

	public static readonly ToggleCommand[] CombatSwitches;

	public static readonly ActionCommand[] CombatActions;

	public static readonly ValueCommand[] RadiusValues;

	public static readonly ValueCommand[] AiValues;

	public static readonly ToggleCommand[] WorldSwitches;

	public static readonly ActionCommand[] WorldActions;

	public static readonly ToggleCommand[] MovementSwitches;

	public static readonly ActionCommand[] MovementActions;

	public static readonly ToggleCommand[] DisplaySwitches;

	public static readonly ActionCommand[] DisplayActions;

	public static readonly ValueCommand[] DisplayValues;

	public static readonly ActionCommand[] MultiplayerActions;

	public static readonly ToggleCommand[] MultiplayerSwitches;

	public static readonly ActionCommand[] ItemActions;

	public static readonly ValueCommand[] ItemValues;

	static Actions()
	{
		PlayerSwitches = new ToggleCommand[10]
		{
			new ToggleCommand("Invisible", "invisible"),
			new ToggleCommand("Regenhealth", "regenhealth"),
			new ToggleCommand("Fake Drown", "fakeDrown", danger: true),
			new ToggleCommand("Capsule Mode", "capsulemode"),
			new ToggleCommand("Block Player Final Death", "blockPlayerFinalDeath"),
			new ToggleCommand("Player Visibility", "playerVisibility"),
			new ToggleCommand("Crouch Toggle", "crouchToggle"),
			new ToggleCommand("Invert Look", "invertLook"),
			new ToggleCommand("First Look Force", "firstLookForce"),
			new ToggleCommand("Player Interrupt Keys", "playerInterruptKeys")
		};
		PlayerActions = new ActionCommand[8]
		{
			new ActionCommand("Heal Local Player", "heallocalplayer"),
			new ActionCommand("Revive Local Player", "revivelocalplayer"),
			new ActionCommand("Kill Local Player", "killlocalplayer", danger: true),
			new ActionCommand("Knockdown Local Player", "knockdownLocalPlayer", danger: true),
			new ActionCommand("Hit Local Player", "hitlocalplayer", danger: true),
			new ActionCommand("Instant Respawn Here", "instantRespawnHere"),
			new ActionCommand("Reset Held Anim", "resetHeldAnim"),
			new ActionCommand("Clear Mid-Action Flag", "clearmidactionflag")
		};
		PlayerValues = new ValueCommand[3]
		{
			new ValueCommand("Set Inventory %", "setinventorypercent", "100"),
			new ValueCommand("Set Player Race", "setPlayerRace", "0"),
			new ValueCommand("Death Count", "deathCount", "0")
		};
		CombatSwitches = new ToggleCommand[14]
		{
			new ToggleCommand("AI Disable", "aiDisable"),
			new ToggleCommand("AI God Mode", "aiGodMode", danger: true),
			new ToggleCommand("AI Knockdown Disable", "aiKnockdownDisable"),
			new ToggleCommand("AI Force Strafe", "aiForceStrafe"),
			new ToggleCommand("AI Radar", "aiRadar"),
			new ToggleCommand("AI Show Health", "aiShowHealth"),
			new ToggleCommand("AI Show Paths", "aiShowPaths"),
			new ToggleCommand("AI Show Debug", "aiShowDebug"),
			new ToggleCommand("AI Show Stats", "aiShowStats"),
			new ToggleCommand("AI Show Thoughts", "aiShowThoughts"),
			new ToggleCommand("AI Vision Debug", "aiVisionDebug"),
			new ToggleCommand("AI Show Anims", "aiShowAnims"),
			new ToggleCommand("Creepy Attack Party", "creepyAttackParty", danger: true),
			new ToggleCommand("Animals Enabled", "animalsEnabled")
		};
		CombatActions = new ActionCommand[6]
		{
			new ActionCommand("Remove Dead", "removeDead"),
			new ActionCommand("Remove Living", "removeLiving", danger: true),
			new ActionCommand("Destroy Ragdoll", "destroyRagdoll"),
			new ActionCommand("Creepy Village", "creepyVillage"),
			new ActionCommand("AI Run World Event", "aiRunWorldEvent"),
			new ActionCommand("AI Pool Stats", "aiPoolStats")
		};
		RadiusValues = new ValueCommand[8]
		{
			new ValueCommand("Kill Radius", "killRadius", "30"),
			new ValueCommand("Damage Radius", "damageRadius", "30"),
			new ValueCommand("Ignite Radius", "igniteRadius", "20"),
			new ValueCommand("Shock Radius", "shockRadius", "20"),
			new ValueCommand("Dismember Radius", "dismemberRadius", "20"),
			new ValueCommand("Gib Radius", "gibRadius", "20"),
			new ValueCommand("Burn Body Radius", "burnBodyRadius", "20"),
			new ValueCommand("Clear Bush Radius", "clearBushRadius", "30")
		};
		AiValues = new ValueCommand[5]
		{
			new ValueCommand("AI Anger Level", "aiAngerLevel", "1"),
			new ValueCommand("AI Armor Level", "aiArmorLevel", "0"),
			new ValueCommand("AI Armor Tier", "aiArmorTier", "0"),
			new ValueCommand("AI Anim Speed", "aiAnimSpeed", "1"),
			new ValueCommand("Animal Limit Mult", "animalLimitMult", "1")
		};
		WorldSwitches = new ToggleCommand[9]
		{
			new ToggleCommand("Builder Mode", "buildermode"),
			new ToggleCommand("Trees Cut All", "treesCutAll"),
			new ToggleCommand("Lock Time Of Day", "lockTimeOfDay"),
			new ToggleCommand("Enable Structure Ghosts", "enableStructureGhosts"),
			new ToggleCommand("Greebles", "greebles"),
			new ToggleCommand("Cloud Enable", "cloudEnable"),
			new ToggleCommand("Cloud Shadows Enable", "cloudShadowsEnable"),
			new ToggleCommand("Demo Mode", "demoMode"),
			new ToggleCommand("Auto Drive Raft", "autoDriveRaft")
		};
		WorldActions = new ActionCommand[12]
		{
			new ActionCommand("Regrow All Trees", "regrowAllTrees"),
			new ActionCommand("Force Remove Trees", "forceRemoveTrees", danger: true),
			new ActionCommand("Refill Containers", "refillContainers"),
			new ActionCommand("Restore All World Locators", "restoreAllWorldLocators"),
			new ActionCommand("Digging Clear", "diggingClear"),
			new ActionCommand("Door Open", "doorOpen"),
			new ActionCommand("Door Close", "doorClose"),
			new ActionCommand("Save", "save"),
			new ActionCommand("Save Player", "saveplayer"),
			new ActionCommand("Load Player", "loadplayer"),
			new ActionCommand("GC Collect", "gccollect"),
			new ActionCommand("Refresh Entities", "refreshEntities")
		};
		MovementSwitches = new ToggleCommand[2]
		{
			new ToggleCommand("Super Jump", "superJump"),
			new ToggleCommand("Free Camera", "freecamera")
		};
		MovementActions = new ActionCommand[2]
		{
			new ActionCommand("Follow Stop", "followStop"),
			new ActionCommand("Get Game Mode", "getGameMode")
		};
		DisplaySwitches = new ToggleCommand[30]
		{
			new ToggleCommand("Show HUD", "showHud"),
			new ToggleCommand("Show FPS", "showFps"),
			new ToggleCommand("Show In World UI", "showInWorldUi"),
			new ToggleCommand("Show Stimuli", "showStimuli"),
			new ToggleCommand("Show Projectile Trails", "showProjectileTrails"),
			new ToggleCommand("Show Active Lights", "showActiveLights"),
			new ToggleCommand("Show Collision Object Names", "showCollisionObjectNames"),
			new ToggleCommand("Show Mesh Object Names", "showMeshObjectNames"),
			new ToggleCommand("Show Mesh Triangle Counts", "showMeshTriangleCounts"),
			new ToggleCommand("Show Mesh Material Names", "showMeshMaterialNames"),
			new ToggleCommand("Show Trigger Collision", "showTriggerCollision"),
			new ToggleCommand("Show Object Location", "showObjectLocation"),
			new ToggleCommand("Render Spheres", "renderSpheres"),
			new ToggleCommand("Aniso Enabled", "anisoEnabled"),
			new ToggleCommand("Billboard Enabled", "billboardEnabled"),
			new ToggleCommand("New Fog Rendering", "newFogRendering"),
			new ToggleCommand("Logging", "logging"),
			new ToggleCommand("Log Hack", "loghack"),
			new ToggleCommand("Log Show Errors", "logShowErrors"),
			new ToggleCommand("Log Show Warnings", "logShowWarnings"),
			new ToggleCommand("Log Show Info", "logShowInfo"),
			new ToggleCommand("Diag Renderers", "diagRenderers"),
			new ToggleCommand("LOD Debug Ranges", "lodDebugRanges"),
			new ToggleCommand("LOD Debug Materials", "lodDebugMaterials"),
			new ToggleCommand("LOD Debug Billboards", "lodDebugBillboards"),
			new ToggleCommand("Damage Debug", "damageDebug"),
			new ToggleCommand("Audio Debug", "audioDebug"),
			new ToggleCommand("Footstep Debug", "footstepDebug"),
			new ToggleCommand("Radio Debug", "radioDebug"),
			new ToggleCommand("DAG Debug", "dagDebug")
		};
		DisplayActions = new ActionCommand[9]
		{
			new ActionCommand("Clear", "clear"),
			new ActionCommand("Help", "help"),
			new ActionCommand("Player Kill Stats", "playerKillStats"),
			new ActionCommand("List Objects", "listObjects"),
			new ActionCommand("List Active Entities", "listActiveEntities"),
			new ActionCommand("Profiler Snapshot", "profilersnapshot"),
			new ActionCommand("Log Show None", "logShowNone"),
			new ActionCommand("Material Snapshot", "materialSnapshot"),
			new ActionCommand("Show FPS", "showFps")
		};
		DisplayValues = new ValueCommand[5]
		{
			new ValueCommand("Quality Texture", "qualityTexture", "2"),
			new ValueCommand("Physics Update Time", "physicsUpdateTime", "0.02"),
			new ValueCommand("Set Setting", "setSetting", ""),
			new ValueCommand("Count", "count", ""),
			new ValueCommand("Count Tag", "counttag", "")
		};
		MultiplayerActions = new ActionCommand[4]
		{
			new ActionCommand("Kick Players", "kickPlayers", danger: true),
			new ActionCommand("Disconnect Players", "disconnectPlayers", danger: true),
			new ActionCommand("Dump Lobby Info", "dumplobbyinfo"),
			new ActionCommand("Net Spawn Player", "netSpawnPlayer")
		};
		MultiplayerSwitches = new ToggleCommand[6]
		{
			new ToggleCommand("Local Voice Debugging", "localVoiceDebugging"),
			new ToggleCommand("Net Animator", "netAnimator"),
			new ToggleCommand("Player Net Animator", "playernetanimator"),
			new ToggleCommand("Net Skinned Bones", "netSkinnedBones"),
			new ToggleCommand("Golf Cart Network Debug", "golfCartNetworkDebug"),
			new ToggleCommand("Players Trigger Traps", "playersTriggerTraps")
		};
		ItemActions = new ActionCommand[2]
		{
			new ActionCommand("Add All Items", "addAllItems"),
			new ActionCommand("Remove All Items", "removeAllItems", danger: true)
		};
		ItemValues = new ValueCommand[5]
		{
			new ValueCommand("Add Item", "additem", ""),
			new ValueCommand("Remove Item", "removeitem", ""),
			new ValueCommand("Equip Item", "equipitem", ""),
			new ValueCommand("Add Itemswithtag", "additemswithtag", ""),
			new ValueCommand("Set Stat", "setstat", "")
		};
		Cyberfox1337xFunction();
	}

	private static string Cyberfox1337xFunction()
	{
		return "curated-command-tables";
	}
}
