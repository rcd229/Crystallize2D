using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Util.Serialization;

public class CrystallizeClient : ICrystallizeClient {

    public NetworkClient Client { get; private set; }
    public PlayerTransformTable TransformTable { get; private set; }
    public bool Initialized { get; set; }

    public bool Connected {
        get {
            if (Client == null) {
                return false;
            }

            return Client.isConnected;
        }
    }

    public int ConnectionID { get; private set; }

    HashSet<NetworkRequestBase> requests = new HashSet<NetworkRequestBase>();
    string ip = "";
    byte reliableChannelID;
    byte unreliableChannelID;

    public CrystallizeClient(string ip) {
        Initialized = false;
        this.ip = ip;
        TransformTable = new PlayerTransformTable();
        Client = new NetworkClient();
    }

    public void Initialize() {
        Debug.Log("Trying to connect: " + ip + ": " + CrystallizeNetwork.UNetPort);
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(CrystallizeMessageType.AssignID, AssignID);
        Client.RegisterHandler(CrystallizeMessageType.GenericRequest, HandleGenericRequestResponse);
        Client.RegisterHandler(CrystallizeMessageType.TransformTable, HandleUpdateTransformTable);

        Client.Configure(CrystallizeNetwork.GetConfig(out reliableChannelID , out unreliableChannelID), 1);
        Client.Connect(ip, CrystallizeNetwork.UNetPort);
    }

    public void Reconnect() {
        Client.Connect(ip, CrystallizeNetwork.UNetPort);
    }

    public void Close() {
        foreach (var h in Client.handlers) {
            Client.UnregisterHandler(h.Key);
        }
        Client.Shutdown();
    }

    public string GetState() {
        var s = "Client";
        s += "\nConnected: " + Client.isConnected;
        s += "\nServer IP: " + Client.serverIp;
        s += "\nServer port: " + Client.serverPort;
        return s;
    }

    #region Send (one-directional)
    public void SendLogMessageToServer(string message) {
        Debug.Log("Sending message");
        Client.Send(CrystallizeMessageType.LogMessage, new LogMessage(PlayerData.Instance.PersonalData.Name, message));
    }

    public void SendPlayerDataToServer() {
        Debug.Log("Sending playerdata");
        var pd = PlayerData.Instance;
        byte[] pdBytes = System.Text.Encoding.UTF8.GetBytes(Serializer.SaveToXmlString<PlayerData>(pd));

        Client.SendByChannel(CrystallizeMessageType.PlayerData, new PlayerDataMessage(PlayerData.Instance.PersonalData.Name, pdBytes), reliableChannelID);

        SendLeaderBoardDataToServer();
    }

    public void SendLeaderBoardDataToServer() {
        int wordCount = PlayerDataConnector.GetLearnedWordsCount();
        var pd = PlayerData.Instance;
        int money = pd.Money;
        Client.Send(CrystallizeMessageType.LeaderBoard, new LeaderBoardMessage(pd.PersonalData.Name, wordCount, money));
    }

    public void SendPositionDataToServer() {
        var t = PlayerManager.Instance.PlayerGameObject.transform;
        Client.SendByChannel(CrystallizeMessageType.TransformData, new TransformDataMessage(ConnectionID, t.position, t.rotation), unreliableChannelID);
    }
    #endregion

    #region Requests
    public void RequestNameFromServer(string name, Action<bool> callback) {
        MakeRequest(ServerRequestType.Name, name, callback, new BoolSerializer());
    }

    public void RequestPlayerDataFromServer(string name, Action<PlayerData> callback) {
        MakeRequest(ServerRequestType.PlayerData, name, callback, new PlayerDataNetworkSerializer());
    }

    public void RequestPlayerDataFromServer(UsernamePasswordPair pair, Action<PlayerData> callback) {
        throw new System.NotImplementedException();
        //MakeRequest(ServerRequestType.p, name, callback, new PlayerDataNetworkSerializer());
    }

    public void RequestLeaderboardFromServer(Action<LeaderBoardGameData> callback) {
        MakeRequest(ServerRequestType.LeaderBoard, "", callback, new XmlNetworkSerializer<LeaderBoardGameData>());
    }

    public void RequestAudioClipFromServer(bool isMale, string text, Action<AudioKey> callback) {
        var key = new AudioKey(isMale, text);
        var stringKey = new AudioKeySerializer().Serialize(key);
        Action<byte[]> wrappedCallback = (b) => AudioCallback(key, b, callback);
        MakeRequest(ServerRequestType.Audio, stringKey, wrappedCallback);
    }

    public void RequestAvatarFromServer(int playerID, Action<PlayerAvatarData> callback) {
        throw new System.NotImplementedException();
    }

    void AudioCallback(AudioKey key, byte[] data, Action<AudioKey> callback) {
        var directory = Directory.GetParent(Application.dataPath) + "/" + AudioLoader.AudioDirectory;
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllBytes(AudioLoader.GetFullFilepath(key.Text, key.IsMale), data);
        callback.Raise(key);
    }

    void MakeRequest(ServerRequestType requestType, string key, Action<string> callback) {
        AddRequest(new NetworkRequest(Client, reliableChannelID, requestType, key, callback));
    }

    void MakeRequest(ServerRequestType requestType, string key, Action<byte[]> callback) {
        AddRequest(new NetworkRequest<byte[]>(Client, reliableChannelID, requestType, key, callback, new ByteNetworkSerializer()));
    }

    void MakeRequest<O>(ServerRequestType requestType, string key, Action<O> callback, INetworkSerializer<O, string> serializer) {
        AddRequest(new NetworkRequest<O>(Client, reliableChannelID, requestType, key, callback, serializer));
    }

    void MakeRequest<O>(ServerRequestType requestType, string key, Action<O> callback, INetworkSerializer<O, byte[]> serializer) {
        AddRequest(new NetworkRequest<O>(Client, reliableChannelID, requestType, key, callback, serializer));
    }

    void AddRequest(NetworkRequestBase req) {
        requests.Add(req);
        req.Run();
    }
    #endregion

    #region Server message handlers
    void OnConnected(NetworkMessage message) {
        Debug.Log("Connected");
    }

    void AssignID(NetworkMessage message) {
        ConnectionID = message.ReadMessage<IntMessage>().val;
        Debug.Log("Connected as ID: " + ConnectionID);
        Initialized = true;
    }

    void HandleGenericRequestResponse(NetworkMessage message) {
        var msg = message.ReadMessage<RequestMessage>();
        foreach (var req in requests) {
            if (req.RequestID == msg.requestID) {
                req.HandleResponse(msg);
                requests.Remove(req);
                return;
            }
        }
        Debug.Log("No request was found for: " + (ServerRequestType)msg.requestType + "; ID = " + msg.requestID);
    }

    void HandleUpdateTransformTable(NetworkMessage message) {
        var table = message.ReadMessage<TransformListDataMessage>();
        TransformTable.Update(table.transforms);
    }
    #endregion



    public void RequestAllAvatarsFromServer() {
        throw new NotImplementedException();
    }


    public void SendChatToServer(string line, int mode) {
        throw new NotImplementedException();
    }


    public void SendSpeechBubbleToAll(PhraseSequence speechBubble) {
        throw new NotImplementedException();
    }


    public void SendEmoteToAll(int emoteType) {
        throw new NotImplementedException();
    }


    public void RequestNameFromServer(UsernamePasswordPair pair, Action<bool> callback) {
        throw new NotImplementedException();
    }
}
