using System.Collections.Generic;
using Newtonsoft.Json;

namespace BepInEx5ArchipelagoPluginTemplate.Archipelago;

public class ArchipelagoData
{
    public string Uri = "";
    public string SlotName = "";
    public string Password = "";
    public int Index;

    public List<long> CheckedLocations;

    /// <summary>
    /// seed for this archipelago data. Can be used when loading a file to verify the session the player is trying to
    /// load is valid to the room it's connecting to.
    /// </summary>
    private string seed;

    private Dictionary<string, object> slotData;

    public bool NeedSlotData => slotData == null;

    public ArchipelagoData()
    {
        Uri = "localhost";
        SlotName = "Player1";
        CheckedLocations = new();
    }

    public ArchipelagoData(string uri, string slotName, string password)
    {
        Uri = uri;
        SlotName = slotName;
        Password = password;
        CheckedLocations = new();
    }

    /// <summary>
    /// assigns the slot data and seed to our data handler. any necessary setup using this data can be done here.
    /// </summary>
    /// <param name="roomSlotData">slot data of your slot from the room</param>
    /// <param name="roomSeed">seed name of this session</param>
    public void SetupSession(Dictionary<string, object> roomSlotData, string roomSeed)
    {
        slotData = roomSlotData;
        seed = roomSeed;
    }

    /// <summary>
    /// returns the object as a json string to be written to a file which you can then load
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
	}

    //attempting to "translate" names in the apworld to those used in game, as well as have some extra info on certain checks such as green bead count and the scene the item is in
    //a lot of the translating was done with a python script and is probably wrong or there might be duplicates, i havent gotten to using this list yet
    //some checks will also have duplicate conditions for being met, probably need to check player pos or figure out a better way to check for items being collected
    public static List<ItemParams> items = new List<ItemParams>
    {
		new ItemParams(62014, 62001, "Green|5", "Haven", "FiveGreenBeads", "Haven_InfrontOfTower", TypeType.LootType),
        new ItemParams(62010, 62003, "ManaScarab", "Haven", "ManaCharm", "Haven_ManaLilyTwo", TypeType.ItemType),
        new ItemParams(62010, 62002, "ManaScarab", "Haven", "ManaCharm", "Haven_EnchanterManaLily", TypeType.ItemType),
        new ItemParams(62010, 62004, "ManaScarab", "Haven", "ManaCharm", "Haven_ManaLilyThree", TypeType.ItemType),
        new ItemParams(62010, 62005, "ManaScarab", "Haven", "ManaCharm", "Haven_ManaLilyFour", TypeType.ItemType),
        new ItemParams(62010, 62006, "ManaScarab", "Haven", "ManaCharm", "Haven_ManaLilyFive", TypeType.ItemType),
        new ItemParams(62012, 62007, "Unimplemented", "Haven", "HealthUpgrade", "Haven_UpgradeFountainOne", TypeType.HealthUp),
        new ItemParams(62013, 62008, "Unimplemented", "Haven", "ManaUpgrade", "Haven_UpgradeFountainTwo", TypeType.ManaUp),
        new ItemParams(62012, 62009, "Unimplemented", "Haven", "HealthUpgrade", "Haven_UpgradeFountainThree", TypeType.HealthUp),
        new ItemParams(62013, 62010, "Unimplemented", "Haven", "ManaUpgrade", "Haven_UpgradeFountainFour",TypeType.ManaUp),
        new ItemParams(62012, 62011, "Unimplemented", "Haven", "HealthUpgrade", "Haven_UpgradeFountainFive", TypeType.HealthUp),
        new ItemParams(62013, 62012, "Unimplemented", "Haven", "ManaUpgrade", "Haven_UpgradeFountainSix", TypeType.ManaUp),
        new ItemParams(62012, 62013, "Unimplemented", "Haven", "HealthUpgrade", "Haven_UpgradeFountainSeven", TypeType.HealthUp),
        new ItemParams(62013, 62014, "Unimplemented", "Haven", "ManaUpgrade", "Haven_UpgradeFountainEight", TypeType.ManaUp),
        new ItemParams(62012, 62015, "Unimplemented", "Haven", "HealthUpgrade", "Haven_UpgradeFountainNine", TypeType.HealthUp),
        new ItemParams(62013, 62016, "Unimplemented", "Haven", "ManaUpgrade", "Haven_UpgradeFountainTen", TypeType.ManaUp),
        new ItemParams(62011, 62017, "Purple|1", "Haven", "Purple_Soulbead", "Haven_UnderBridge", TypeType.LootType),
        new ItemParams(62017, 62018, "Random", "Haven", "RandomRune", "Haven_RandomRunePuzzleRoom", TypeType.BonusType),
        new ItemParams(62015, 62019, "Teleport", "WindingGlade", "TeleportRune", "WindingGlade_TeleportRunePuzzleRoom", TypeType.RuneType),
        new ItemParams(62011, 62020, "Purple|1", "MystralWoods", "Purple_Soulbead", "MystralWoods_ManaLilyCelestialPuzzle", TypeType.LootType),
        new ItemParams(62018, 62021, "Seed", "MystralWoods", "ManaLilyBulb", "MystralWoods_ManaLily", TypeType.ItemType),
        new ItemParams(62011, 62023, "Purple|1", "MystralWoods", "Purple_Soulbead", "MystralWoods_PostCart_PurpleBeadPuzzleRoom", TypeType.LootType),
        new ItemParams(62020, 62025, "Move", "MystralWoods", "MoveRune", "MystralWoods_Lardee_MoveRune", TypeType.RuneType),
        new ItemParams(62021, 62028, "ForestKey", "MystralWoods", "ForestKey", "MystralWoods_DeepWoods_StrangeOldGoblin", TypeType.KeyType),
        new ItemParams(62022, 62030, "Treant", "MystralWoods", "WoodWretchElixer", "MystralWoods_Twiggs_LifeElixer", TypeType.BossSoulType),
        new ItemParams(62018, 62022, "Seed", "GreyleafHamlet", "ManaLilyBulb", "GreyleafHamlet_ManaLily", TypeType.ItemType),
        new ItemParams(62025, 62032, "Homing", "TheRiseSouth", "HomingRune", "TheRiseSouth_CelestialPuzzle_CloseToMystralWoodsEntrance", TypeType.RuneType),
        new ItemParams(62026, 62033, "FireRod", "TheRiseSouth", "IgniWand", "TheRiseSouth_StrangeMan_IgniWand", TypeType.BaguetteType),
        new ItemParams(62011, 62034, "Purple|1", "TheRiseSouth", "Purple_Soulbead", "TheRiseSouth_TorchPuzzleNearRopeBridges", TypeType.LootType),
        new ItemParams(62027, 62035, "Green|30", "TheRise", "ThirtyGreenBeads", "TheRise_SecretRuinsPath", TypeType.LootType),
        new ItemParams(62028, 62036, "Right", "TheRise", "RightRune", "TheRise_RightRune", TypeType.BonusType),
        new ItemParams(62121, 62037, "HUB_Plaine_CrypteKey", "TheRise", "TheRiseKey", "TheRise_RightRunePuzzle", TypeType.KeyType), //not sure if correct item
        new ItemParams(62011, 62038, "Purple|1", "TheRiseNorth", "Purple_Soulbead", "TheRiseNorth_HiddenUnderStairsPurpleSoulBead", TypeType.LootType),
        new ItemParams(62011, 62039, "Purple|1", "TheRiseNorth", "Purple_Soulbead", "TheRiseNorth_RuinsTorchPuzzle", TypeType.LootType),
        new ItemParams(62122, 62040, "SkyTempleKey1", "SkyTempleFrontDoorTorchPuzzle", "SkyTempleFrontDoorKey", "SkyTempleFrontDoorTorchPuzzle", TypeType.KeyType), //not sure if correct
        new ItemParams(62029, 62041, "Green|25", "SkyTempleHubDetonateRock", "TwentyFiveGreenBeads", "SkyTempleHubDetonateRock", TypeType.LootType),
        new ItemParams(62123, 62042, "SkyTempleKey2", "SkyTempleHubEastBigKey", "SkyTempleEasternDoorKey", "SkyTempleHubEastBigKey", TypeType.KeyType),
        new ItemParams(62016, 62043, "Detonation", "SkyTempleHubNorthDetonateRune", "DetonateRune", "SkyTempleHubNorthDetonateRune", TypeType.RuneType),
        new ItemParams(62011, 62044, "Purple|1", "SkyTempleHubEastPurpleSoulbeadBehindDetonate", "Purple_Soulbead", "SkyTempleHubEastPurpleSoulbeadBehindDetonate", TypeType.LootType),
        new ItemParams(62011, 62045, "Purple|1", "SkyTempleHubEastPuzzleRoomBehindDetonateRock", "Purple_Soulbead", "SkyTempleHubEastPuzzleRoomBehindDetonateRock", TypeType.LootType),
        new ItemParams(62030, 62046, "Mastery", "SkyTempleHubPuzzleMasteryRune", "MasteryRune", "SkyTempleHubPuzzleMasteryRune", TypeType.RuneType),
        new ItemParams(62031, 62047, "Green|10", "SkyTempleHubChestBehindDetonateRock", "TenGreenBeads", "SkyTempleHubChestBehindDetonateRock", TypeType.LootType),
        new ItemParams(62124, 62048, "SkyTempleKey3", "SkyTempleUpperBigKey", "SkyTempleBossKey", "SkyTempleUpperBigKey", TypeType.KeyType),
        new ItemParams(62011, 62049, "Purple|1", "SkyTempleUpperCombatPuzzleRoom", "Purple_Soulbead", "SkyTempleUpperCombatPuzzleRoom", TypeType.LootType),
        new ItemParams(62032, 62050, "SkyShard", "SkyTempleUpperSleetsRemains", "SkyShard", "SkyTempleUpperSleetsRemains", TypeType.ItemType),
        new ItemParams(62033, 62051, "Lizard", "SkyTempleUpperIceLizardElixerChest", "IceLizardElixer", "SkyTempleUpperIceLizardElixerChest", TypeType.BossSoulType),
        new ItemParams(62011, 62052, "Purple|1", "SkyTempleUpperCelestialPuzzleNearRemains", "Purple_Soulbead", "SkyTempleUpperCelestialPuzzleNearRemains", TypeType.LootType),
        new ItemParams(62034, 62053, "OnHit", "TheRistNorthUpperLedge", "ImpactRune", "TheRistNorthUpperLedge_ImpactRune", TypeType.RuneType),
        new ItemParams(62018, 62054, "Seed", "TheRise", "ManaLilyBulb", "TheRise_ManaLily", TypeType.ItemType),
        new ItemParams(62011, 62055, "Purple|1", "MystralWoods", "Purple_Soulbead", "MystralWoods_Backwoods_DetonateTorch", TypeType.LootType),
        new ItemParams(62035, 62056, "Charge", "MystralWoods", "OverchargeRune", "MystralWoods_TorchPuzzleOverchargeRune", TypeType.RuneType),
        new ItemParams(62036, 62057, "AirRod", "MystralWoods", "AuraWand", "MystralWoods_SuperLardee_AuraWand", TypeType.BaguetteType),
        new ItemParams(62011, 62058, "Purple|1", "OldMinesLardee", "Purple_Soulbead", "OldMinesLardee_PurpleBeadDetonateRock", TypeType.LootType),
        new ItemParams(62011, 62059, "Purple|1", "HavenWest", "Purple_Soulbead", "HavenWest_RemoteDetonateRock", TypeType.LootType),
        new ItemParams(62037, 62060, "PortalStone", "Haven", "PortalStone", "Haven_PortalStone", TypeType.ItemType),
        new ItemParams(62011, 62061, "Purple|1", "Haven", "Purple_Soulbead", "Haven_HallOfTrialsWaveFour", TypeType.LootType),
        new ItemParams(62040, 62062, "OnTrespass", "Haven", "ProximityRune", "Haven_HallOfTrialsWaveEight", TypeType.NestedRuneType),
        new ItemParams(62038, 62063, "SkinElite", "Haven", "ArchmagesRobe", "Haven_HallOfTrialsWaveTwelve", TypeType.ItemType),
        new ItemParams(62039, 62065, "TrialWand", "Haven", "TrialWand", "Haven_HallOfTrialsTenMinutes", TypeType.BaguetteType),
        new ItemParams(62041, 62066, "Green+15", "Haven", "FifteenGreenBeads", "Haven_ChestNearManaFountain", TypeType.LootType),
        new ItemParams(62011, 62067, "Purple|1", "MystralWoodsMiningArea", "Purple_Soulbead", "MystralWoodsMiningArea_PuzzleInTheOldMines", TypeType.LootType),
        new ItemParams(62011, 62068, "Purple|1", "MystralWoodsMiningArea", "Purple_Soulbead", "MystralWoodsMiningArea_TorchesInTheRiver", TypeType.LootType),
        new ItemParams(62042, 62069, "Speed", "MystralWoodsMiningArea", "TimeRune", "MystralWoodsMiningArea_WoodedCombatPuzzle", TypeType.BonusType),
        new ItemParams(62044, 62071, "Green|75", "Highlands", "SeventyFiveGreenBeads", "Highlands_Farmer", TypeType.LootType),
        new ItemParams(62051, 62072, "WaterRod", "Highlands", "AquaWand", "Highlands_Fisherman", TypeType.BaguetteType),
        new ItemParams(62048, 62073, "BrokenStone", "Highlands", "BrokenPortalStone", "Highlands_Beggar", TypeType.ItemType),
        new ItemParams(62046, 62074, "EarthRod", "Highlands", "GaeaWand", "Highlands_GuardTowerStealthPuzzle", TypeType.BaguetteType),
        new ItemParams(62050, 62077, "BloodWand", "TheRiseNorth", "LifeStaff", "TheRiseNorth_TravelingMerchantAnna", TypeType.BaguetteType), //not sure if correct
        new ItemParams(62024, 62079, "Green|50", "Highlands", "FiftyGreenBeads", "Highlands_ChestOnSmallLandAcrossWater", TypeType.LootType),
        new ItemParams(62011, 62080, "Purple|1", "Highlands", "Purple_Soulbead", "Highlands_SewerCelestialPuzzle", TypeType.LootType),
        new ItemParams(62052, 62082, "Bounce", "Highlands", "BounceRune", "Highlands_WaterWalkingTorchPuzzle", TypeType.RuneType),
        new ItemParams(62125, 62083, "CrypteKeyEntrance", "TombOfTheMageKing", "TombEntranceKey", "TombOfTheMageKing_BigKeyOne", TypeType.KeyType),
        new ItemParams(62054, 62084, "Duplicate", "TombOfTheMageKing", "DuplicateRune", "TombOfTheMageKing_DuplicateRuneChest", TypeType.RuneType),
        new ItemParams(62011, 62085, "Purple|1", "TombOfTheMageKing", "Purple_Soulbead", "TombOfTheMageKing_SwampyPurpleBeadChest", TypeType.LootType),
        new ItemParams(62011, 62086, "Purple|1", "TombOfTheMageKing", "Purple_Soulbead", "TombOfTheMageKing_DuplicatePuzzleRoom", TypeType.LootType),
        new ItemParams(62126, 62087, "CrypteCenterKey", "TombOfTheMageKing", "TombMidKey", "TombOfTheMageKing_BigKeyTwo", TypeType.KeyType), //unsure
        new ItemParams(62011, 62088, "Purple|1", "TombOfTheMageKing", "Purple_Soulbead", "TombOfTheMageKing_CelestialPuzzle", TypeType.LootType),
        new ItemParams(62127, 62089, "CrypteKey1", "TombOfTheMageKing", "TombBossKey", "TombOfTheMageKing_BigKeyThree", TypeType.KeyType), //unsure
        new ItemParams(62011, 62090, "Purple|1", "TombOfTheMageKing", "Purple_Soulbead", "TombOfTheMageKing_BeamPuzzle", TypeType.LootType),
        new ItemParams(62127, 62091, "CrypteKey2", "TombOfTheMageKing", "TombBossKey", "TombOfTheMageKing_BigKeyFour", TypeType.KeyType), //unsure
        new ItemParams(62055, 62092, "Inverse", "TombOfTheMageKing", "InverseRune", "TombOfTheMageKing_PuzzleRoomInverseRune", TypeType.BonusType),
        new ItemParams(62056, 62093, "Queen", "TombOfTheMageKing", "GhostQueenElixer", "TombOfTheMageKing_GhostQueenLifeElixer", TypeType.BossSoulType),
        new ItemParams(62128, 62095, "HUB_MontagneKey", "Graveyard", "HighlandsGraveyardKey", "Graveyard_StatueBigKey", TypeType.KeyType), //unsure
        new ItemParams(62011, 62096, "Purple|1", "HighlandsUpper", "Purple_Soulbead", "HighlandsUpper_BounceTorchPuzzle", TypeType.LootType),
        new ItemParams(62058, 62097, "Ether", "HighlandsUpper", "EtherRune", "HighlandsUpper_CelestialPuzzle", TypeType.RuneType),
        new ItemParams(62059, 62098, "Size", "HighlandsUpper", "SizeRune", "HighlandsUpper_LowerTorchPuzzle", TypeType.BonusType),
        new ItemParams(62060, 62099, "Rain", "TheRise", "RainRune", "TheRise_BouncePuzzle", TypeType.RuneType),
        new ItemParams(62011, 62100, "Purple|1", "GreyleafHamlet", "Purple_Soulbead", "GreyleafHamlet_FireRainPuzzle", TypeType.LootType),
        new ItemParams(62061, 62101, "Repeating", "GreyleafHamlet", "SwiftRune", "GreyleafHamlet_BurnedTowerPuzzle", TypeType.RuneType),
        new ItemParams(62062, 62102, "Left", "GreyleafHamlet", "LeftRune", "GreyleafHamlet_BarTorchPuzzleRoom", TypeType.BonusType),
        new ItemParams(62129, 62103, "VillageKey", "GreyleafHamlet", "GreyleafHamletKey", "GreyleafHamlet_SavedTheCitizens", TypeType.KeyType), //unsure
        new ItemParams(62063, 62104, "Push", "GreyleafHamlet", "PushRune", "GreyleafHamlet_LedgeCombatPuzzleRoom", TypeType.RuneType),
        new ItemParams(62011, 62105, "Purple|1", "GreyleafHamletCave", "Purple_Soulbead", "GreyleafHamletCave_CelestialPuzzle", TypeType.LootType),
        new ItemParams(62064, 62106, "Air", "GreyleafHamletCave", "Aura_Essence", "GreyleafHamletCave_AuraEssence", TypeType.Essences),
        new ItemParams(62065, 62107, "SoulScepter", "GreyleafHamlet", "Soul_Scepter", "GreyleafHamlet_JeffsThankYouGift", TypeType.BaguetteType),
        new ItemParams(62066, 62108, "PenetratingWand", "GreyleafHamlet", "NegationScepter", "GreyleafHamlet_Xavier", TypeType.BaguetteType),
        new ItemParams(62069, 62112, "OnCast", "SunkenQuarry", "AtOnceRune", "SunkenQuarry_HiddenPuzzleRoomNorthWest", TypeType.NestedRuneType),
        new ItemParams(62011, 62113, "Purple|1", "SunkenQuarry", "Purple_Soulbead", "SunkenQuarry_LedgePuzzleRoomSouthEast", TypeType.LootType),
        new ItemParams(62011, 62114, "Purple|1", "SunkenQuarry", "Purple_Soulbead", "SunkenQuarry_SouthEastCactusChest", TypeType.LootType),
        new ItemParams(62011, 62115, "Purple|1", "SunkenQuarry", "Purple_Soulbead", "SunkenQuarry_CelestialPuzzleNorthEast", TypeType.LootType),
        new ItemParams(62024, 62116, "Green|50", "SunkenQuarry", "FiftyGreenBeads", "SunkenQuarry_NorthWaterChest", TypeType.LootType),
        new ItemParams(62011, 62117, "Purple|1", "SunkenQuarry", "Purple_Soulbead", "SunkenQuarry_NorthCombatArenaChest", TypeType.LootType),
        new ItemParams(62070, 62118, "Ice", "SunkenQuarry", "Aqua_Essence", "SunkenQuarry_AquaEssence", TypeType.Essences),
        new ItemParams(62130, 62120, "MinesKey", "OldMines", "OldMinesKey", "OldMines_BigKey", TypeType.KeyType),
        new ItemParams(62071, 62121, "OnEnd", "OldMines", "ExpireRune", "OldMines_CrystalCombatPuzzleRoom", TypeType.NestedRuneType),
        new ItemParams(62011, 62122, "Purple|1", "OldMines", "Purple_Soulbead", "OldMines_CrystalCartPuzzle", TypeType.LootType),
        new ItemParams(62011, 62123, "Purple|1", "OldMines", "Purple_Soulbead", "OldMines_CelestialPuzzle", TypeType.LootType),
        new ItemParams(62072, 62124, "Earth", "OldMines", "Gaea_Essence", "OldMines_GaeaEssence", TypeType.Essences),
        new ItemParams(62074, 62126, "LifeStaff", "TheRiseSouth", "RodOfTheBerserker", "TheRiseSouth_GoblinAmbush_CloseToMystralWoodsEntrance", TypeType.BaguetteType),
        new ItemParams(62029, 62127, "Green|25", "TheRise", "TwentyFiveGreenBeads", "TheRise_ChestNearLavaGrotto", TypeType.LootType),
        new ItemParams(62075, 62128, "Green|20", "TheRise", "TwentyGreenBeads", "TheRise_ChestAcrossLava", TypeType.LootType),
        new ItemParams(62011, 62129, "Purple|1", "LavaGrotto", "Purple_Soulbead", "LavaGrotto_CelestialPuzzle", TypeType.LootType),
        new ItemParams(62076, 62130, "OnTick", "LavaGrotto", "PeriodicRune", "LavaGrotto_TorchPuzzleBehindLavaWaterfall", TypeType.NestedRuneType),
        new ItemParams(62011, 62131, "Purple|1", "LavaGrotto", "Purple_Soulbead", "LavaGrotto_RockPillarPurpleBead", TypeType.LootType),
        new ItemParams(62011, 62132, "Purple|1", "LavaGrotto", "Purple_Soulbead", "LavaGrotto_DrainedLavaPoolPurpleBead", TypeType.LootType),
        new ItemParams(62011, 62133, "Purple|1", "LavaGrotto", "Purple_Soulbead", "LavaGrotto_RockPillarToCombatRoomPurpleBead", TypeType.LootType),
        new ItemParams(62102, 62134, "Fire", "LavaGrotto", "Igni_Essence", "LavaGrotto_IgniEssence", TypeType.Essences),
        new ItemParams(62100, 62135, "Mystralian", "TheRise", "MystralianElixer", "TheRise_SerpentLockedDoor", TypeType.BossSoulType),
        new ItemParams(62011, 62136, "Purple|1", "TheRise", "Purple_Soulbead", "TheRise_WaterfallPurpleBead", TypeType.LootType),
        new ItemParams(62011, 62137, "Purple|1", "GreyleafHamlet", "Purple_Soulbead", "GreyleafHamlet_WaterboundPurpleBead", TypeType.LootType),
        new ItemParams(62011, 62138, "Purple|1", "Highlands", "Purple_Soulbead", "Highlands_CliffsidePurpleBead", TypeType.LootType),
        new ItemParams(62018, 62139, "Seed", "HighlandsUpper", "ManaLilyBulb", "HighlandsUpper_ManaLily", TypeType.ItemType),
        new ItemParams(62101, 62140, "AfterLife", "Highlands", "AfterLifeElixer", "Highlands_PortNecromancerGhostBusters", TypeType.BossSoulType),
        new ItemParams(62011, 62141, "Purple|1", "Highlands", "Purple_Soulbead", "Highlands_SouthernTorchPuzzle", TypeType.LootType),
        new ItemParams(62011, 62142, "Purple|1", "TheRise", "Purple_Soulbead", "TheRise_ChestNearDarkTower", TypeType.LootType),
        new ItemParams(62011, 62143, "Purple|1", "MystralWoods", "Purple_Soulbead", "MystralWoods_DeepWoods_StrangeOldGoblinRippingYouOff", TypeType.LootType),
        new ItemParams(62024, 62144, "Green|50", "GreyleafHamlet", "FiftyGreenBeads", "GreyleafHamlet_MariesBagOfWaresResult", TypeType.LootType),
        new ItemParams(62043, 62125, "Fork", "GreyleafHamlet", "RepairedPitchfork", "GreyleafHamlet_Zako", TypeType.ItemType), //fork
        new ItemParams(62073, 62119, "Material", "OldMines", "ChunkOfMetal", "OldMines_MetalIngot", TypeType.ItemType),
        new ItemParams(62067, 62109, "BrokenFork", "Highlands", "BrokenPitchfork", "Highlands_CousinsFarmer", TypeType.ItemType),
        new ItemParams(62068, 62110, "Breads", "HighlandsUpper", "LoavesOfBread", "HighlandsUpper_GoblinBreadThieves", TypeType.ItemType),
        new ItemParams(62047, 62111, "Bread", "Highlands", "HotBread", "Highlands_BakerTwo", TypeType.ItemType), //bread, not sure if this is correct
        new ItemParams(62057, 62094, "Ghost0", "Highlands", "BottleForSpirits", "Highlands_PortNecromancer", TypeType.ItemType),
        new ItemParams(62053, 62081, "Badge", "Highlands", "Badge", "Highlands_PortBadge", TypeType.ItemType),
        new ItemParams(62045, 62078, "Meat", "Highlands", "BoarMeat", "Highlands_Boar", TypeType.ItemType),
        new ItemParams(62047, 62075, "Bread", "Highlands", "HotBread", "Highlands_Baker", TypeType.ItemType), //given that there are two bread locations but only one bread item
        new ItemParams(62043, 62070, "Fork", "Highlands", "Pitchfork", "Highlands_Pitchfork", TypeType.ItemType), //same here, 2nd instance of fork
        new ItemParams(62023, 62031, "IOU", "GreyleafHamlet", "Token", "GreyleafHamlet_JeffsGiftForwife", TypeType.ItemType),
        new ItemParams(62019, 62029, "Wares", "MystralWoods", "BagOfWares", "MystralWoods_DeepWoods_BagOfWaresTwo", TypeType.ItemType),
        new ItemParams(62019, 62026, "Wares", "OldMinesLardee", "BagOfWares", "OldMinesLardee_BagOfWares", TypeType.ItemType),
        new ItemParams(62019, 62027, "Wares", "MystralWoods", "BagOfWares", "MystralWoods_DeepWoods_BagOfWaresOne", TypeType.ItemType),
        new ItemParams(62019, 62024, "Wares", "MystralWoods", "BagOfWares", "MystralWoods_GoblinCamp_BagOfWares", TypeType.ItemType),
        new ItemParams(62049, 62076, "Flowers", "Highlands", "Flowers", "Highlands_TownsPersonOnPlatform", TypeType.ItemType)
	};
}

public class ItemParams
{
    public int ap_item_id;
    public int ap_loc_id;
	public string vanilla_item_name;
	public string scene_name;
	public string ap_item_name;
	public string ap_location_name;
	public TypeType item_type;

    public ItemParams(int iid, int lid, string i, string s, string api, string apl, TypeType t) {
        ap_item_id = iid;
        ap_loc_id = lid;
        vanilla_item_name = i;
        scene_name = s;
        ap_item_name = api;
        ap_location_name = apl;
        item_type = t;
    }

}

public enum TypeType
{
    None,
    SpellType,
    KeyType,
    ItemType,
    BaguetteType,
    Essences,
    RuneType,
    NestedRuneType,
    BonusType,
    LootType,
    BossSoulType,
    HealthUp,
    ManaUp
}
