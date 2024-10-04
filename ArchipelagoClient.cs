using System;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Packets;
using BepInEx5ArchipelagoPluginTemplate.Utils;
using MagicSystem;

namespace BepInEx5ArchipelagoPluginTemplate.Archipelago;

public class ArchipelagoClient
{
    public const string APVersion = "0.4.4";
    private const string Game = "Mages Of Mystralia";
    private static readonly string[] Tags = { "AP" };

    public static bool Authenticated;
    private bool attemptingConnection;

    public static ArchipelagoData ServerData = new();
    private DeathLinkHandler DeathLinkHandler;
    private ArchipelagoSession session;

    /// <summary>
    /// call to connect to an Archipelago session. Connection info should already be set up on ServerData
    /// </summary>
    /// <returns></returns>
    public void Connect()
    {
        if (Authenticated || attemptingConnection) return;

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(ServerData.Uri);
            SetupSession();
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }

        TryConnect();
    }

    /// <summary>
    /// add handlers for Archipelago events
    /// </summary>
    private void SetupSession()
    {
        session.MessageLog.OnMessageReceived += OnMessageReceived;
        session.Items.ItemReceived += OnItemReceived;
        session.Socket.ErrorReceived += OnSessionErrorReceived;
        session.Socket.SocketClosed += OnSessionSocketClosed;
    }

    /// <summary>
    /// attempt to connect to the server with our connection info
    /// </summary>
    private void TryConnect()
    {
        try
        {
            ThreadPool.QueueUserWorkItem(
                _ => HandleConnectResult(
                    session.TryConnectAndLogin(
                        Game,
                        ServerData.SlotName,
                        ItemsHandlingFlags.AllItems, // TODO make sure to change this line
                        new Version(APVersion),
                        Tags,
                        password: ServerData.Password,
                        requestSlotData: false // ServerData.NeedSlotData
                    ), ServerData));
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
            HandleConnectResult(new LoginFailure(e.ToString()), ServerData);
            attemptingConnection = false;
        }
    }

    /// <summary>
    /// handle the connection result and do things
    /// </summary>
    /// <param name="result"></param>
    private void HandleConnectResult(LoginResult result, ArchipelagoData serverData)
    {
        string outText;
        if (result.Successful)
        {
            var success = (LoginSuccessful)result;

            ServerData.SetupSession(success.SlotData, session.RoomState.Seed);
            Authenticated = true;

            DeathLinkHandler = new(session.CreateDeathLinkService(), ServerData.SlotName);

            session.Locations.CompleteLocationChecks(serverData.CheckedLocations.ToArray());

            outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

            Plugin.BepinLogger.LogMessage(outText);
        }
        else
        {
            var failure = (LoginFailure)result;
            outText = $"Failed to connect to {ServerData.Uri} as {ServerData.SlotName}.";
            outText = failure.Errors.Aggregate(outText, (current, error) => current + $"\n    {error}");

            Plugin.BepinLogger.LogError(outText);

            Authenticated = false;
            Disconnect();
        }

        attemptingConnection = false;
    }

    /// <summary>
    /// something we wrong or we need to properly disconnect from the server. cleanup and re null our session
    /// </summary>
    private void Disconnect()
    {
        Plugin.BepinLogger.LogDebug("disconnecting from server...");

        session?.Socket.Disconnect();
        session = null;
        Authenticated = false;
    }

    /// <summary>
    /// we received and need to handle a message from the server
    /// </summary>
    /// <param name="message">the received server message</param>
    private void OnMessageReceived(LogMessage message)
    {
        Plugin.BepinLogger.LogMessage(message);
        ArchipelagoConsole.LogMessage(message.ToString());
    }

    public void SendMessage(string message)
    {
        session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }

    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        var receivedItem = helper.PeekItem();

		Plugin.BepinLogger.LogMessage(session.Items.GetItemName(receivedItem.Item) + " " + receivedItem.Location);

        ReceiveItem(session.Items.GetItemName(receivedItem.Item));

		helper.DequeueItem();

        if (helper.Index < ServerData.Index) return;

        ServerData.Index++;
    }

    public void SendItem(int id)
    {
		session.Locations.CompleteLocationChecks(id);
       
	}

    public void ReceiveItem(string item_name)
    {
		foreach (var item in ArchipelagoData.items)
		{
			if (item.ap_item_name == item_name)
			{
                //wip, the rest of the item types need to be added
                if (item.item_type == TypeType.RuneType)
                    Inventory.Unlock((MagicSystem.RuneType) Enum.Parse(typeof(MagicSystem.RuneType), item_name, true), true);
			}
		}
	}

    /// <summary>
    /// something went wrong with our socket connection
    /// </summary>
    /// <param name="e">thrown exception from our socket</param>
    /// <param name="message">message received from the server</param>
    private void OnSessionErrorReceived(Exception e, string message)
    {
        Plugin.BepinLogger.LogError(e);
        Plugin.BepinLogger.LogError(message);
    }

    /// <summary>
    /// something went wrong closing our connection. disconnect and clean up
    /// </summary>
    /// <param name="reason"></param>
    private void OnSessionSocketClosed(string reason)
    {
        Plugin.BepinLogger.LogError($"Connection to Archipelago lost: {reason}");
        Disconnect();
    }
}
