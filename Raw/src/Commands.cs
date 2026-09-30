namespace Cyberfox1337x.SonsOfTheForest;

[Cyberfox1337x("console command table")]
internal static class Commands
{
	public static readonly string[] All = new string[358]
	{
		"addAllItems", "additem", "additemswithtag", "addmemory", "aiAngerLevel", "aiAnimSpeed", "aiArmorLevel", "aiArmorTier", "aiAttentionDebug", "aiDisable",
		"aiDummy", "aiForceStrafe", "aiGodMode", "aiJumpDebug", "aiKnockdownDisable", "aiLogNavCuts", "aiLogSpawnTimes", "aiMemoryAdjust", "aiPoolStats", "aiRadar",
		"aiRunWorldEvent", "aiShowAnimTags", "aiShowAnims", "aiShowAudio", "aiShowDebug", "aiShowDebugCamera", "aiShowEventMemory", "aiShowHealth", "aiShowNavGraph", "aiShowPaths",
		"aiShowPlayerInfluences", "aiShowStats", "aiShowSurvivalStats", "aiShowThoughts", "aiStatAdjust", "aiStructureLog", "aiTestSleep", "aiThought", "aiThoughtNoCooldown", "aiVailStats",
		"aiVerboseLog", "aiVillageClosest", "aiVisionDebug", "aiWorldEventStats", "aiWorldStats", "aiZoneStats", "allowAsync", "animalLimitMult", "animalsEnabled", "anisoEnabled",
		"anisoMinMax", "areaShadow", "astar", "audio2dTest", "audioDebug", "audioDescription", "audioParameter", "audioPlayEvent", "autoDriveRaft", "billboardEnabled",
		"billboardIgnoreChanges", "billboardLogAliveChanges", "blockPlayerFinalDeath", "breakObjects", "buildermode", "burnBodyRadius", "cameraDlss", "cameraFov", "capsulemode", "caveLight",
		"characterLods", "cheats", "checkAttachedEntities", "checkExitMenu", "checkFrozenEntities", "clear", "clearAudioParameters", "clearBushRadius", "clearallsettings", "clearmidactionflag",
		"cloudEnable", "cloudFactor", "cloudShadowsEnable", "colorGrade", "count", "countGoWithlayer", "counttag", "createLight", "creepyAttackParty", "creepyVillage",
		"crouchToggle", "dagDebug", "dagTestMode", "dagTestNext", "dagTestPrev", "damageDebug", "damageRadius", "deathCount", "debugPlayerHitLog", "debugPlayerMelee",
		"demoMode", "destroy", "destroyRagdoll", "destroyWildcard", "diagRenderers", "diggingClear", "disableGameObjectTester", "disableGoWildcard", "disableScene", "disablecomponent",
		"disablego", "disconnectPlayer", "disconnectPlayers", "dismemberRadius", "doorClose", "doorOpen", "dumpMeshInfo", "dumpTransformInfo", "dumplobbyinfo", "duplicateObject",
		"dynamicResCameraDebug", "dynamicResolutionCycleTest", "dynamicResolutionOverride", "dynamicResolutionTarget", "enableCollisionBasedKillBox", "enableScene", "enableStructureGhosts", "enablecomponent", "enablego", "energyhack",
		"equipitem", "exposureSetSpeed", "fakeDrown", "fakeGpu", "filteraudio", "findObjectsWithShader", "firstLookForce", "fogFakeNotSupported", "follow", "followStop",
		"footstepDebug", "forceCloud", "forceCloudProfile", "forcePlayerExpression", "forceRemoveTrees", "forcerain", "freecamera", "gameOverDelayTime", "gamePadDeadzone", "gamePadXSensitivity",
		"gamePadYSensitivity", "gccollect", "getGameMode", "getlayerculldistance", "gibRadius", "godmode", "golfCartNetworkDebug", "goto", "gotoCoords", "gotoforce",
		"gototag", "gravity", "greebleLayer", "greebleZone", "greebledRocksCollision", "greebles", "heallocalplayer", "help", "hideworldposfor", "hitlocalplayer",
		"igniteRadius", "inspectgo", "instantRespawnHere", "inventorySpeedSensitivity", "invertLook", "invisible", "isWaterDisplacementEnabled", "itemGroupInteractionAudio", "joinSteamLobby", "jumpTimeOfDay",
		"kickPlayers", "killRadius", "killlocalplayer", "knockdownLocalPlayer", "listActiveEntities", "listDeathMarkers", "listGoWithlayer", "listObjects", "load", "loadDebugConsoleMod",
		"loadMacros", "loadScene", "loadSceneSingle", "loadplayer", "localVoiceDebugging", "lockTimeOfDay", "lodDebugBillboards", "lodDebugMaterials", "lodDebugRanges", "lodForce2DDistance",
		"lodForce3DDistance", "logShowErrors", "logShowInfo", "logShowNone", "logShowWarnings", "logTextures", "logWorldObjectLocatorManagerData", "logging", "loghack", "materialSnapshot",
		"meshColliderMeshLog", "mipmapStreaming", "mipmapStreamingBudget", "mipmapStreamingDebug", "mipmapStreamingDiscard", "mouseXSensitivity", "mouseYSensitivity", "navGraphForceUpdate", "netAnimator", "netSkinnedBones",
		"netSpawnPlayer", "networkTransformLodUpdate", "newFogRendering", "oceanColliderDebug", "openmacrosfolder", "physicsUpdateTime", "playDeathCutscene", "playDeathMarker", "playDeathMarkerIndex", "playGameOver",
		"playerAnimParams", "playerDebugCamera", "playerInterruptKeys", "playerKillStats", "playerVisibility", "playernetanimator", "playersTriggerTraps", "postProcessingComponent", "profilersample", "profilersnapshot",
		"qualityTexture", "radioDebug", "refillContainers", "refreshEntities", "regenhealth", "regrowAllTrees", "removeAllItems", "removeDead", "removeLiving", "removeShader",
		"removeitem", "renderSpheres", "replaceShader", "reporterrorsnow", "reportlogsnow", "reportwarningsnow", "resetHeldAnim", "resetSettings", "restoreAllWorldLocators", "revivelocalplayer",
		"robbyCarry", "robbyInCutscenes", "robbyStateInfo", "rumbleTest", "save", "saveplayer", "season", "sendMessageTo", "setCurrentDay", "setDifficultyMode",
		"setExitedEndGame", "setFirstTimeSeenItem", "setGameMode", "setGameSetupSetting", "setGameTimeSpeed", "setLookRotation", "setOpeningCrash", "setPlayerDataFlag", "setPlayerRace", "setProperty",
		"setSetting", "setSpeakerMode", "setTerrainAniso", "setTimeOfDay", "setWindIntensity", "setWorldObjectStateRange", "setinventorypercent", "setlayerculldistance", "setstat", "shockRadius",
		"showActiveLights", "showButterflyInfo", "showCollisionObjectNames", "showFps", "showHud", "showInWorldUi", "showMeshMaterialNames", "showMeshObjectNames", "showMeshTriangleCounts", "showObjectLocation",
		"showProjectileTrails", "showStimuli", "showTriggerCollision", "showVitals", "showWorldObjects", "showui", "showworldposfor", "skinnedMeshesInfo", "skunkSmell", "slapchop",
		"sleepCooldown", "spawnFallingTree", "spawnRenderSpheres", "spawnedObjectStats", "spawnitem", "speedyrun", "sprintToggle", "stonehack", "superJump", "survival",
		"targetFrameRate", "terrainParallax", "terrainPixelError", "terrainRender", "terrainRenderSimple", "terrainTess", "terrainTessDist", "testAutoBuildEffigy", "testeventmask", "timeOfDayConnectionDebug",
		"timeOfDayDebug", "timescale", "timmyPathDebug", "toggleFPSDisplay", "toggleOcclusionCulling", "toggleOverlay", "togglePlayerStats", "toggleVSync", "toggleWorkScheduler", "togglego",
		"treeCutSimulateBolt", "treeFallContactInfo", "treeMapDistributionFilter", "treeOcclusionBonus", "treeRadius", "treesCutAll", "uiForceLastUpdate", "uiForceLateUpdate", "unloadScene", "unloadUnusedAssets",
		"unlockSeason", "useRigidBodyRotation", "virginiaInCutscenes", "virtualPickupDebug", "virtualPlayers", "workscheduler", "worldGroupId", "wsscaling"
	};

	public static readonly string[] Lower = BuildLower();

	private static string[] BuildLower()
	{
		string[] lower = new string[All.Length];
		for (int i = 0; i < All.Length; i++)
		{
			lower[i] = All[i].ToLowerInvariant();
		}
		return lower;
	}
}
