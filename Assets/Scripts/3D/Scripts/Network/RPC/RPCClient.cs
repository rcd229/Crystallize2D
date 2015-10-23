using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Util.Serialization;

public class RPCClient : ICrystallizeClient {

    public PlayerTransformTable TransformTable {
        get { return RPCMessager.Instance.TransformTable; }
    }

    public int ConnectionID {
        get { return RPCMessager.Instance.PlayerID; }
    }

    public bool Connected {
        get {
            return Network.isClient;
        }
    }

    string ip = "";
    HostData hostData = null;

    public RPCClient(string ip) {
        this.ip = ip;
    }

    public RPCClient(HostData hostData) {
        this.hostData = hostData;
    }

    public void Initialize() {
        Reconnect();
    }

    public void Reconnect() {
        if (hostData != null) {
            Debug.Log("Connecting through hostdata.");
            Network.Connect(hostData);
        } else {
            Debug.Log("Connecting through ip: " + ip + " port: " + CrystallizeNetwork.LegacyPort);
            Network.Connect(ip, CrystallizeNetwork.LegacyPort);
        }
    }

    public void Close() {
        Network.Disconnect();
    }

    public void RequestAudioClipFromServer(bool isMale, string text, Action<AudioKey> callback) {
        var key = new AudioKey(isMale, text);
        Action<byte[]> wrappedCallback = (b) => AudioCallback(key, b, callback);
        RPCMessager.Instance.MakeRequest(ServerRequestType.Audio, key.ToKeyString(), new ByteNetworkSerializer(), wrappedCallback);
    }

    void AudioCallback(AudioKey key, byte[] data, Action<AudioKey> callback) {
        var directory = Directory.GetParent(Application.dataPath) + "/" + AudioLoader.AudioDirectory;
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllBytes(AudioLoader.GetFullFilepath(key.Text, key.IsMale), data);
        callback.Raise(key);
    }

    public void RequestLeaderboardFromServer(Action<LeaderBoardGameData> callback) {
        RPCMessager.Instance.MakeRequest(ServerRequestType.LeaderBoard, "", new XmlNetworkSerializer<LeaderBoardGameData>(), callback);
    }

    public void RequestNameFromServer(string name, Action<bool> callback) {
        RPCMessager.Instance.MakeRequest(ServerRequestType.Name, name, new BoolSerializer(), callback);
    }

    public void RequestNameFromServer(UsernamePasswordPair pair, Action<bool> callback) {
        string key = Serializer.SaveToXmlString<UsernamePasswordPair>(pair);
        RPCMessager.Instance.MakeRequest(ServerRequestType.NameWithPassword, key, new BoolSerializer(), callback);
    }

    public void RequestPlayerDataFromServer(UsernamePasswordPair pair, Action<PlayerData> callback) {
        string key = Serializer.SaveToXmlString<UsernamePasswordPair>(pair);
        RPCMessager.Instance.MakeRequest(ServerRequestType.PlayerDataWithPassword, key, new PlayerDataNetworkSerializer(), callback);
    }

    public void RequestPlayerDataFromServer(string name, Action<PlayerData> callback) {
        RPCMessager.Instance.MakeRequest(ServerRequestType.PlayerData, name, new PlayerDataNetworkSerializer(), callback);
    }

    public void RequestAvatarFromServer(int playerID, Action<PlayerAvatarData> callback) {
        RPCMessager.Instance.MakeRequest(ServerRequestType.PlayerAvatar, playerID.ToString(), new PlayerAvatarSerializer(), callback);
    }

    public void RequestAllAvatarsFromServer() {
        RPCMessager.Instance.DoServerRPC(RPCMessager.Instance.Server_SendAllAvatars, Network.player);
    }

    public void SendChatToServer(string line, int mode) {
        RPCMessager.Instance.DoAllRPC(RPCMessager.Instance.All_UpdateChatLog, ConnectionID, line, mode);
    }

    public void SendLeaderBoardDataToServer() {
        int wordCount = PlayerDataConnector.GetLearnedWordsCount();
        var pd = PlayerData.Instance;
        int money = pd.Money;
        RPCMessager.Instance.DoServerRPC(RPCMessager.Instance.Server_UpdateLeaderBoard, pd.PersonalData.Name, wordCount, money);
    }

    public void SendLogMessageToServer(string message) {
        RPCMessager.Instance.DoServerRPC(RPCMessager.Instance.Server_LogMessage, PlayerData.Instance.PersonalData.Name, message);
    }

    public void SendPlayerDataToServer() {
        Debug.Log("Sending playerdata");
        var pd = PlayerData.Instance;
        byte[] pdBytes = new PlayerDataNetworkSerializer().Serialize(pd);
        Debug.Log(PlayerData.Instance.PersonalData.Name + "; " + pdBytes.Length);
        RPCMessager.Instance.DoServerRPC<string, byte[]>(RPCMessager.Instance.Server_SavePlayerData, PlayerData.Instance.PersonalData.Name, pdBytes);

        SendLeaderBoardDataToServer();
        SendAvatarDataToServer();
    }

    public void SendAvatarDataToServer() {
        var ad = new PlayerAvatarData(PlayerData.Instance.PersonalData.Name, PlayerData.Instance.Appearance);
        RPCMessager.Instance.DoServerRPC(RPCMessager.Instance.Server_UpdateAvatar, Network.player, PlayerAvatarSerializer.Instance.Serialize(ad));
    }

    public void SendPositionDataToServer() {
        if (ConnectionID != -1) {
            var t = PlayerManager.Instance.PlayerGameObject.transform;
            RPCMessager.Instance.DoAllRPC(RPCMessager.Instance.All_UpdateTransformData, ConnectionID, t.position, t.rotation);
        } else {
            Debug.Log("Client ID has not been set. ");
        }
    }

    public void SendSpeechBubbleToAll(PhraseSequence phrase) {
        if (ConnectionID != -1) {
            var xml = Serializer.SaveToXmlString<PhraseSequence>(phrase);
            RPCMessager.Instance.DoAllRPC(RPCMessager.Instance.All_SetSpeechBubble, ConnectionID, xml);
        } else {
            Debug.Log("Client ID has not been set. ");
        }
    }

    public void SendEmoteToAll(int emoteType) {
        if (ConnectionID != -1) {
            RPCMessager.Instance.DoAllRPC(RPCMessager.Instance.All_SetEmote, ConnectionID, emoteType);
        } else {
            Debug.Log("Client ID has not been set. ");
        }
    }

    public void BroadcastTransforms() {
        //NetworkServer.SendByChannelToAll(CrystallizeMessageType.TransformTable, new TransformListDataMessage(PlayerTransformTable.GetTransforms()), unreliableChannelID);
    }

}
