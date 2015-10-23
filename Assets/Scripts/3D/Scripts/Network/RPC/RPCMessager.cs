using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class RPCMessager : MonoBehaviour {

    static RPCMessager _instance;
    public static RPCMessager Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
        TransformTable = new PlayerTransformTable();
        DontDestroyOnLoad(gameObject);
    }

    public int PlayerID { get { return client_playerID; } }
    public PlayerTransformTable TransformTable { get; private set; }

    int client_playerID = -1;
    int client_currentRequestID = 0;
    HashSet<RPCRequest> client_openRequests = new HashSet<RPCRequest>();

    INetworkSerializer<string, byte[]> stringSerializer = new StringNetworkSerializer();

    int server_currentPlayerID = 0;
    Dictionary<string, int> server_playerIDs = new Dictionary<string, int>();

    Dictionary<int, byte[]> avatars = new Dictionary<int, byte[]>();

    #region Client
    public void DoServerRPC<T1>(Action<T1> rpc, T1 data) {
        var methodName = rpc.Method.Name;
        Debug.Log(methodName);
        GetComponent<NetworkView>().RPC(methodName, RPCMode.Server, data);
    }

    public void DoServerRPC<T1, T2>(Action<T1, T2> rpc, T1 t1, T2 t2) {
        var methodName = rpc.Method.Name;
        Debug.Log(methodName);
        GetComponent<NetworkView>().RPC(methodName, RPCMode.Server, t1, t2);
    }

    public void DoServerRPC<T1, T2, T3>(Action<T1, T2, T3> rpc, T1 t1, T2 t2, T3 t3) {
        var methodName = rpc.Method.Name;
        Debug.Log(methodName);
        GetComponent<NetworkView>().RPC(methodName, RPCMode.Server, t1, t2, t3);
    }

    public void MakeRequest<O>(ServerRequestType requestType, string key, INetworkSerializer<O, byte[]> serializer, Action<O> callback) {
        var req = new RPCRequest<O>(client_currentRequestID, serializer, callback);
        client_openRequests.Add(req);

        Action<NetworkPlayer, int, int, string> action = Server_RequestFromServer;
        var method = action.Method.Name;
        Debug.Log(method);
        GetComponent<NetworkView>().RPC(method, RPCMode.Server, Network.player, client_currentRequestID, (int)requestType, key);
        client_currentRequestID++;
    }

    [RPC]
    public void Client_ReceiveResponse(int requestID, byte[] data) {
        foreach (var request in client_openRequests) {
            if (request.RequestID == requestID) {
                request.DoResponse(data);
                return;
            }
        }
    }

    [RPC]
    public void Client_SetPlayerID(int playerID) {
        this.client_playerID = playerID;
    }
    #endregion

    #region Server
    public void SendResponseToPlayer(NetworkPlayer player, int requestID, byte[] data) {
        Action<int, byte[]> del = Client_ReceiveResponse;
        var method = del.Method.Name;
        Debug.Log(method);
        GetComponent<NetworkView>().RPC(method, player, requestID, data);
    }

    void SendToAll() { }

    byte[] GetPlayerData(string key) {
        return stringSerializer.Serialize(ServerData.LoadPlayerData(key));
    }

    byte[] GetPlayerDataWithPassword(string key) {
        var pair = Serializer.LoadFromXmlString<UsernamePasswordPair>(key);
        if (ServerData.IsUsernamePasswordValid(pair)) {
            return stringSerializer.Serialize(ServerData.LoadPlayerData(pair.Username));
        } else {
            return stringSerializer.Serialize(".invalid username or password");
        }
    }

    byte[] GetAudio(string key) {
        var akey = new AudioKeySerializer().Deserialize(key);
        string path = AudioLoader.GetFullFilepath(akey.Text, akey.IsMale);
        if (!File.Exists(path)) {
            Debug.LogError("audio doesn't exist : " + path);
            return null;
        }
        return System.IO.File.ReadAllBytes(path);
    }

    byte[] GetLeaderBoard(string key) {
        var leaderBoard = CrystallizeNetwork.Server.LeaderBoard.GetTopN(5);
        return new XmlNetworkSerializer<LeaderBoardGameData>().Serialize(leaderBoard);
    }

    byte[] GetName(string key) {
        var result = ServerData.CheckNameValid(key);
        return new BoolSerializer().Serialize(result);
    }

    byte[] GetNameWithPassword(string key) {
        var pair = Serializer.LoadFromXmlString<UsernamePasswordPair>(key);
        var result = ServerData.CheckNameValid(pair.Username);
        if (result) {
            ServerData.AddUsernamePasswordEntry(pair);
        }
        return new BoolSerializer().Serialize(result);
    }

    byte[] GetAvatar(string key) {
        int id = int.Parse(key);
        if (avatars.ContainsKey(id)) {
            return avatars[id];
        } else {
            throw new Exception("The avatar was not found in the dictionary.");
        }
    }

    [RPC]
    public void Server_RequestFromServer(NetworkPlayer player, int requestID, int requestType, string key) {
        byte[] data = null;
        switch ((ServerRequestType)requestType) {
            case ServerRequestType.PlayerData:
                data = GetPlayerData(key);
                break;
            case ServerRequestType.PlayerDataWithPassword:
                data = GetPlayerDataWithPassword(key);
                break;
            case ServerRequestType.Audio:
                data = GetAudio(key);
                break;
            case ServerRequestType.LeaderBoard:
                data = GetLeaderBoard(key);
                break;
            case ServerRequestType.Name:
                data = GetName(key);
                break;
            case ServerRequestType.NameWithPassword:
                data = GetNameWithPassword(key);
                break;
            case ServerRequestType.PlayerAvatar:
                data = GetAvatar(key);
                break;
        }
        SendResponseToPlayer(player, requestID, data);
    }

    [RPC]
    public void Server_LogMessage(string name, string message) {
        Debug.Log("Server logging message");
        CrystallizeNetwork.Server.Logger.SaveMessage(name, message);
    }

    [RPC]
    public void Server_SavePlayerData(string name, byte[] data) {
        Debug.Log("Received player data from player: " + name);
        var pdString = stringSerializer.Deserialize(data);
        ServerData.SavePlayerData(name, pdString);
    }

    [RPC]
    public void Server_UpdateLeaderBoard(string name, int words, int money) {
        CrystallizeNetwork.Server.LeaderBoard.Update(name, words, money);
    }

    [RPC]
    public void Server_UpdateAvatar(NetworkPlayer player, byte[] avatarData) {
        var id = server_playerIDs[player.guid];
        avatars[id] = avatarData;
    }

    [RPC]
    public void Server_SendAllAvatars(NetworkPlayer player) {
        foreach (var t in TransformTable.Transforms) {
            DoRPC(All_UpdateTransformData, player, t.ConnectionID, t.Position, t.Rotation);
        }
    }
    #endregion

    #region All
    public void DoAllRPC<T1>(Action<T1> rpc, T1 t1) {
        DoRPC(rpc, RPCMode.All, t1);
    }

    public void DoAllRPC<T1, T2>(Action<T1, T2> rpc, T1 t1, T2 t2) {
        DoRPC(rpc, RPCMode.All, t1, t2);
    }

    public void DoAllRPC<T1, T2, T3>(Action<T1, T2, T3> rpc, T1 t1, T2 t2, T3 t3) {
        DoRPC(rpc, RPCMode.All, t1, t2, t3);
    }

    [RPC]
    public void All_UpdateChatLog(int playerID, string line, int mode) {
        ChatLog.Instance.AddLine(playerID, line, mode);
    }

    [RPC]
    public void All_UpdateTransformData(int playerID, Vector3 pos, Quaternion rot) {
        var tdata = new TransformData(playerID, pos, rot);
        TransformTable.Update(tdata);
    }

    [RPC]
    public void All_RemoveTransformData(int playerID) {
        TransformTable.Remove(playerID);
    }

    [RPC]
    public void All_SetSpeechBubble(int playerID, string phraseXml) {
        CrystallizeEventManager.Network.RaiseNetworkSpeechBubbleRequested(this, new NetworkSpeechBubbleRequestedEventArgs(playerID, phraseXml));
    }

    [RPC]
    public void All_SetEmote(int playerID, int emoteType) {
        CrystallizeEventManager.Network.RaiseNetworkEmoteRequested(this, new NetworkEmoteArgs(playerID, emoteType));
    }
    #endregion

    public void DoRPC<T1>(Action<T1> rpc, RPCMode mode, T1 t1) {
        var methodName = rpc.Method.Name;
        Debug.Log(methodName);
        GetComponent<NetworkView>().RPC(methodName, mode, t1);
    }

    public void DoRPC<T1, T2>(Action<T1, T2> rpc, RPCMode mode, T1 t1, T2 t2) {
        var methodName = rpc.Method.Name;
        GetComponent<NetworkView>().RPC(methodName, mode, t1, t2);
    }

    public void DoRPC<T1, T2, T3>(Action<T1, T2, T3> rpc, RPCMode mode, T1 t1, T2 t2, T3 t3) {
        var methodName = rpc.Method.Name;
        GetComponent<NetworkView>().RPC(methodName, mode, t1, t2, t3);
    }

    public void DoRPC<T1, T2, T3>(Action<T1, T2, T3> rpc, NetworkPlayer player, T1 t1, T2 t2, T3 t3) {
        var methodName = rpc.Method.Name;
        Debug.Log(methodName);
        GetComponent<NetworkView>().RPC(methodName, player, t1, t2, t3);
    }

    public void OnPlayerConnected(NetworkPlayer player) {
        var id = Mathf.Max(server_currentPlayerID, 1);
        server_currentPlayerID = id + 1;
        server_playerIDs[player.guid] = id;

        Action<int> method = Client_SetPlayerID;
        GetComponent<NetworkView>().RPC(method.Method.Name, player, server_playerIDs[player.guid]);
    }

    public void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        var id = server_playerIDs[player.guid];
        server_playerIDs.Remove(player.guid);
        DoAllRPC(All_RemoveTransformData, id);
    }

}
