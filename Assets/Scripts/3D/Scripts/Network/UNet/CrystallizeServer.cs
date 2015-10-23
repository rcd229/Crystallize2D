using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.IO;
using Util.Serialization;
using System.Collections.Generic;

public class CrystallizeServer : ICrystallizeServer {

    public ServerLogger Logger { get; private set; }
    public LeaderBoard LeaderBoard { get; private set; }
    public PlayerTransformTable TransformTable { get; private set; }

    public bool IsStarted {
        get {
            return NetworkServer.active;
        }
    }

    //byte reliableChannelID;
    byte unreliableChannelID;
    Dictionary<ServerRequestType, Action<RequestMessage>> genericRequestHandlers = new Dictionary<ServerRequestType, Action<RequestMessage>>();

    public CrystallizeServer() {
        Logger = new ServerLogger();
        LeaderBoard = new LeaderBoard();
        TransformTable = new PlayerTransformTable();
    }

    public void Initialize() {
        byte tmp;
        NetworkServer.Configure(CrystallizeNetwork.GetConfig(out tmp, out unreliableChannelID), 100);

        NetworkServer.Listen(CrystallizeNetwork.UNetPort);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, HandlePlayerDisconnected);
        NetworkServer.RegisterHandler(CrystallizeMessageType.LogMessage, LogMessage);
        NetworkServer.RegisterHandler(CrystallizeMessageType.PlayerData, SavePlayerData);
        NetworkServer.RegisterHandler(CrystallizeMessageType.LeaderBoard, UpdateLeaderBoard);
        NetworkServer.RegisterHandler(CrystallizeMessageType.TransformData, UpdateTransformData);
        NetworkServer.RegisterHandler(CrystallizeMessageType.GenericRequest, HandleGenericRequest);

        genericRequestHandlers[ServerRequestType.Name] = RespondToNameRequest;
        genericRequestHandlers[ServerRequestType.PlayerData] = RespondToPlayerDataRequest;
        genericRequestHandlers[ServerRequestType.LeaderBoard] = RespondToLeaderboardRequest;
        genericRequestHandlers[ServerRequestType.Audio] = RespondToAudioRequest;
    }

    public string GetState() {
        var s = "Server";
        s += "\nServer IP: " + Network.player.ipAddress;
        s += "\nConnections: " + NetworkServer.connections.Count;
        //s += "\n Last message: " + lastMsg;
        return s;
    }

    public void Close() {
        Logger.Close();
        NetworkServer.Shutdown();
    }

    public void BroadcastTransforms() {
        NetworkServer.SendByChannelToAll(CrystallizeMessageType.TransformTable, new TransformListDataMessage(TransformTable.GetTransforms()), unreliableChannelID);
    }

    void OnConnected(NetworkMessage message) {
        Debug.Log("Connected: " + message.conn.connectionId);
        NetworkServer.SendToClient(message.conn.connectionId, CrystallizeMessageType.AssignID, new IntMessage(message.conn.connectionId));
    }

    #region One-directional writes
    void LogMessage(NetworkMessage message) {
        Debug.Log("Server logging message");
        var msg = message.ReadMessage<LogMessage>();
        Logger.SaveMessage(msg.name, msg.message);
    }

    void SavePlayerData(NetworkMessage message) {// string name, byte[] playerData) {
        var data = message.ReadMessage<PlayerDataMessage>();
        Debug.Log("Received player data from player: " + data.name);
        var pdString = System.Text.Encoding.UTF8.GetString(data.data);
        ServerData.SavePlayerData(data.name, pdString);
    }

    void UpdateLeaderBoard(NetworkMessage message) {
        var data = message.ReadMessage<LeaderBoardMessage>();
        LeaderBoard.Update(data.name, data.words, data.money);
    }

    void UpdateTransformData(NetworkMessage message) {
        var data = message.ReadMessage<TransformDataMessage>();
        TransformTable.Update(data.GetTransformData());
    }

    void HandlePlayerDisconnected(NetworkMessage message) {
        Debug.Log("Player disconnected: " + message.conn.connectionId);
    }
    #endregion

    #region requests
    void HandleGenericRequest(NetworkMessage message) {
        var msg = message.ReadMessage<RequestMessage>();
        msg.ConnectionID = message.conn.connectionId;
        var reqType = (ServerRequestType)msg.requestType;
        if (genericRequestHandlers.ContainsKey(reqType)) {
            genericRequestHandlers[reqType](msg);
        } else {
            Debug.LogWarning("No handler found for " + reqType);
        }
    }

    void MakeResponse(RequestMessage message, string data) {
        var resp = new RequestMessage((ServerRequestType)message.requestType, message.requestID, data);
        NetworkServer.SendToClient(message.ConnectionID, CrystallizeMessageType.GenericRequest, resp);
    }

    void MakeResponse(RequestMessage message, byte[] data) {
        var resp = new RequestMessage((ServerRequestType)message.requestType, message.requestID, data);
        NetworkServer.SendToClient(message.ConnectionID, CrystallizeMessageType.GenericRequest, resp);
    }

    void RespondToPlayerDataRequest(RequestMessage message) {
        string pd = ServerData.LoadPlayerData(message.StringValue);
        byte[] pdBytes = new byte[0];
        if (!pd.IsEmptyOrNull()) {
            pdBytes = System.Text.Encoding.UTF8.GetBytes(pd);
        }

        MakeResponse(message, pdBytes);
    }

    void RespondToNameRequest(RequestMessage message) {//NetworkPlayer player, string name) {
        // TODO: check if name can be used
        var result = ServerData.CheckNameValid(message.StringValue);
        var data = result.ToString();
        MakeResponse(message, data);
    }

    void RespondToLeaderboardRequest(RequestMessage message) {
        var leaderBoard = LeaderBoard.GetTopN(5);
        string LBString = Serializer.SaveToXmlString<LeaderBoardGameData>(leaderBoard);
        MakeResponse(message, LBString);
    }

    void RespondToAudioRequest(RequestMessage message) {
        var key = new AudioKeySerializer().Deserialize(message.StringValue);
        //look up in server
        if (File.Exists(AudioLoader.GetFullFilepath(key.Text, key.IsMale))) {
            SendAudioClip(message);
        }
            //look up in web
        else {
            NeospeechAudioLoader.GetAudioClip(key.Text, key.IsMale, s => SendAudioClip(message));
        }
    }

    void SendAudioClip(RequestMessage message) {
        var key = new AudioKeySerializer().Deserialize(message.StringValue);
        string path = AudioLoader.GetFullFilepath(key.Text, key.IsMale);
        byte[] data;
        if (!File.Exists(path)) {
            Debug.LogError("audio doesn't exist : " + path);
        }
        data = System.IO.File.ReadAllBytes(path);
        MakeResponse(message, data);
    }
    #endregion

}
