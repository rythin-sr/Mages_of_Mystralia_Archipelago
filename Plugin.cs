using BepInEx;
using BepInEx.Logging;
using BepInEx5ArchipelagoPluginTemplate.Archipelago;
using BepInEx5ArchipelagoPluginTemplate.Utils;
using HarmonyLib;
using MagicSystem;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
namespace BepInEx5ArchipelagoPluginTemplate;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGUID = "rythin.MoM_AP";
    public const string PluginName = "MoM_AP";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
    public static ManualLogSource BepinLogger;
    public static ArchipelagoClient ArchipelagoClient;

	private readonly Harmony harmony = new Harmony(PluginGUID);
	static ButtonGameSlot selected_slot;

	private void Awake()
    {
        // Plugin startup logic
        BepinLogger = Logger;
        ArchipelagoClient = new ArchipelagoClient();
        ArchipelagoConsole.Awake();

        BepinLogger.LogInfo($"{ModDisplayInfo} loaded!");

		harmony.PatchAll();
    }

    private void OnGUI()
    {
        // show the mod is currently loaded in the corner
        GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo + " " + SceneManager.GetActiveScene().name);
        ArchipelagoConsole.OnGUI();

        string statusMessage;
        // show the Archipelago Version and whether we're connected or not
        if (!ArchipelagoClient.Authenticated)
        {
			if (SceneManager.GetActiveScene().name == "MainMenu_Main")
			{
				// if your game doesn't usually show the cursor this line may be necessary
				// Cursor.visible = true;

				statusMessage = " Status: Disconnected";
				GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
				GUI.Label(new Rect(16, 70, 150, 20), "Host: ");
				GUI.Label(new Rect(16, 90, 150, 20), "Player Name: ");
				GUI.Label(new Rect(16, 110, 150, 20), "Password: ");

				ArchipelagoClient.ServerData.Uri = GUI.TextField(new Rect(150, 70, 150, 20), ArchipelagoClient.ServerData.Uri);
				ArchipelagoClient.ServerData.SlotName = GUI.TextField(new Rect(150, 90, 150, 20), ArchipelagoClient.ServerData.SlotName);
				ArchipelagoClient.ServerData.Password = GUI.TextField(new Rect(150, 110, 150, 20), ArchipelagoClient.ServerData.Password);

				// requires that the player at least puts *something* in the slot name
				if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") &&
					!ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
				{
					ArchipelagoClient.Connect();
				}
			}
        }
        else
        {
			// if your game doesn't usually show the cursor this line may be necessary
			// Cursor.visible = false;

			statusMessage = " Status: Connected";
			GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
		}
		// this is a good place to create and add a bunch of debug buttons
		if (GUI.Button(new Rect(200, 130, 100, 30), "GIVE") && ArchipelagoClient.Authenticated)
		{
			//ArchipelagoClient.SendItem(36);
			Plugin.BepinLogger.LogMessage("Right rune name: " + BonusType.Right);
		}
	}

	[HarmonyPatch]
	public class AP_Patches
	{

		//menu stuff, make sure the player can't enter the game while not connected

		[HarmonyPostfix]
		[HarmonyPatch(typeof(UIMainMenu), "OnSlotSelected", typeof(ButtonGameSlot))]
		static void update_selected_slot(ButtonGameSlot slot)
		{
			selected_slot = slot;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(ButtonGameSlot), "OnPointerClick", typeof(PointerEventData))]
		static bool disable_vanilla_start_controller()
		{
			return (ArchipelagoClient.Authenticated);
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(ButtonGameSlot), "OnSubmit", typeof(BaseEventData))]
		static bool disable_vanilla_start_kbm()
		{
			return (ArchipelagoClient.Authenticated);
		}

		//log all checks to file for debugging

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(SpellType), typeof(bool) })]
		static void write_spell_location(SpellType spell, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Spell: " + spell + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(KeyType), typeof(bool) })]
		static void write_key_location(KeyType key, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Key: " + key + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(ItemType), typeof(bool) })]
		static void write_item_location(ItemType item, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Item: " + item + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(BaguetteType), typeof(bool) })]
		static void write_baguette_location(BaguetteType baguette, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Wand: " + baguette + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(Essences), typeof(bool) })]
		static void write_essence_location(Essences essence, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Essence: " + essence + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(RuneType), typeof(bool) })]
		static void write_rune_location(RuneType rune, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Rune: " + rune + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(NestedRuneType), typeof(bool) })]
		static void write_spell_location(NestedRuneType nestedRune, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Nested Rune: " + nestedRune + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(BonusType), typeof(bool) })]
		static void write_spell_location(BonusType bonus, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Bonus Rune: " + bonus + " in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "Unlock", new Type[] { typeof(LootType), typeof(int), typeof(bool) })]
		static void write_spell_location(LootType lootType, int count, bool isUserDriven)
		{
			if (isUserDriven)
				File.AppendAllText("LocLog.txt", "Collected Loot: " + lootType + " (" + count + ") in " + SceneManager.GetActiveScene().name + Environment.NewLine);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Inventory), "ChangeProgression")]
		static void write_progression_change(GlobalProgression p)
		{
			File.AppendAllText("LocLog.txt", "Progression state changed: " + p + Environment.NewLine);
		}


		// make checks
		[HarmonyPrefix]
		[HarmonyPatch(typeof(T_UnlockBaguette), "Trigger")]
		public static void check_wand()
		{
			foreach (var item in ArchipelagoData.items) {
				if (item.vanilla_item_name == baguette.ToString())
				{
					ArchipelagoClient.SendItem(item.ap_loc_id);
				}
			}
		}
	}
}