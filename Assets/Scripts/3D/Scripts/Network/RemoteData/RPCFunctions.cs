using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;
using System.Linq;

public class RPCFunctions : MonoBehaviour {

    public static bool IsLocalOnly = false;

    static RPCFunctions _instance;
    public static RPCFunctions Instance {
        get {
            return _instance;
        }
    }

    //static string LoadPlayerData(string name) {
    //    var path = Application.dataPath;
    //    var dir = Directory.GetParent(path).Parent;
    //    path = dir.FullName + "/CrystallizeRemote/PlayerData/";
    //    var file = name + ".xml";
    //    string data;
    //    try {
    //        var reader = new StreamReader(path + file);
    //        data = reader.ReadToEnd();
    //    } catch {
    //        data = "";
    //    }
    //    return data;
    //}

    //public event EventHandler<EventArgs<bool>> NameResponse;
    //public event EventHandler<EventArgs<PlayerData>> PlayerDataResponse;
    public event EventHandler<EventArgs<LeaderBoardGameData>> LeaderBoardResponse;
    public event EventHandler<EventArgs<AudioArg>> AudioResponse;

    void Awake() {
        _instance = this;
    }

    //public void SavePlayerDataToServer() {
    //    Debug.Log("Sending playerdata");
    //    var pd = PlayerData.Instance;
    //    byte[] pdBytes = System.Text.Encoding.UTF8.GetBytes(Serializer.SaveToXmlString<PlayerData>(pd));
    //    GetComponent<NetworkView>().RPC("Server_SavePlayerData", RPCMode.Server, PlayerData.Instance.PersonalData.Name, pdBytes);
    //    int wordCount = PlayerDataConnector.GetLearnedWordsCount();
    //    int money = pd.Money;
    //    GetComponent<NetworkView>().RPC("Server_SaveLeaderboard", RPCMode.Server, pd.PersonalData.Name, wordCount, money);
    //}

    //public void SaveLeaderBoardDataToServer() {
    //    int wordCount = PlayerDataConnector.GetLearnedWordsCount();
    //    var pd = PlayerData.Instance;
    //    int money = pd.Money;
    //    GetComponent<NetworkView>().RPC("Server_SaveLeaderboard", RPCMode.Server, pd.PersonalData.Name, wordCount, money);
    //}

    

    //public void RequestNameFromServer(string name) {
    //    GetComponent<NetworkView>().RPC("Server_RequestName", RPCMode.Server, Network.player, name);
    //}

    //public void RequestLeaderboardFromServer() {
    //    GetComponent<NetworkView>().RPC("Server_RequestLeaderboard", RPCMode.Server, Network.player);
    //}

    //public void GetAudioClipFromServer(string text, bool isMale) {
    //    GetComponent<NetworkView>().RPC("Server_RequestAudio", RPCMode.Server, Network.player, text, isMale);
    //}

    //  SERVER RPC
    //[RPC]
    //void Server_RequestAudio(NetworkPlayer player, string text, bool isMale) {
    //    //		AudioClip audio;
    //    //look up in server
    //    if (File.Exists(AudioLoader.GetFullFilepath(text, isMale))) {
    //        SendAudioClip(player, text, isMale);
    //    }
    //        //look up in web
    //    else {
    //        NeospeechAudioLoader.GetAudioClip(text, isMale, s => SendAudioClip(player, text, isMale));
    //    }
    //}

    //void SendAudioClip(NetworkPlayer player, string text, bool isMale) {
    //    string path = AudioLoader.GetFullFilepath(text, isMale);
    //    byte[] data;
    //    if (!File.Exists(path)) {
    //        Debug.LogError("audio doesn't exist : " + path);
    //    }

    //    data = System.IO.File.ReadAllBytes(path);

    //    GetComponent<NetworkView>().RPC("Client_SendAudio", player, data, text, isMale);
    //}

    //[RPC]
    //void Server_RequestLeaderboard(NetworkPlayer player) {
    //    List<LeaderBoardDataItem<int>> top10_money = (from item in playerMoney.ToList()
    //                                                  orderby item.Value descending
    //                                                  select new LeaderBoardDataItem<int>(item.Key, item.Value)
    //                                                  ).Take(5).ToList();
    //    List<LeaderBoardDataItem<int>> top10_word = (from item in playerWords.ToList()
    //                                                 orderby item.Value descending
    //                                                 select new LeaderBoardDataItem<int>(item.Key, item.Value)
    //                                                  ).Take(5).ToList();
    //    LeaderBoardGameData leaderBoard = new LeaderBoardGameData(top10_money, top10_word);
    //    string LBString = Serializer.SaveToXmlString<LeaderBoardGameData>(leaderBoard);
    //    GetComponent<NetworkView>().RPC("Client_LeaderBoard", player, LBString);
    //}
    //[RPC]
    //void Server_SaveLeaderboard(string name, int words, int money) {
    //    playerMoney[name] = money;
    //    playerWords[name] = words;
    //}


    //[RPC]
    //void Server_LogMessage(string name, string entry) {
    //    // TODO: save message to server
    //    Debug.Log(name + "; " + entry);
    //    SaveMessage(name, entry);
    //}

    //[RPC]
    //void Server_SavePlayerData(string name, byte[] playerData) {
    //    Debug.Log("Received player data from player: " + name);
    //    var pdString = System.Text.Encoding.UTF8.GetString(playerData);
    //    SavePlayerData(name, pdString);
    //}

    //[RPC]
    //void Server_RequestPlayerData(NetworkPlayer player, string name) {
    //    string pd = LoadPlayerData(name);
    //    byte[] pdBytes = new byte[0];
    //    if (pd == null || pd == "") {
    //        pdBytes = new byte[1];
    //    } else {
    //        pdBytes = System.Text.Encoding.UTF8.GetBytes(pd);
    //        Debug.Log("byte " + pd);
    //        if (pdBytes == null) {
    //            pdBytes = new byte[1];
    //        }
    //    }
    //    GetComponent<NetworkView>().RPC("Client_LoadPlayerData", player, pdBytes);
    //}

    //[RPC]
    //void Server_RequestName(NetworkPlayer player, string name) {
    //    // TODO: check if name can be used
    //    var result = CheckNameValid(name);
    //    GetComponent<NetworkView>().RPC("Client_NameResponse", player, result);
    //}


    // CLIENT RPC
    //[RPC]
    //void Client_SendAudio(byte[] data, string text, bool isMale) {
    //    //		string audioString = System.Text.Encoding.UTF8.GetString (data);
    //    var directory = Directory.GetParent(Application.dataPath) + "/" + AudioLoader.AudioDirectory;
    //    if (!Directory.Exists(directory)) {
    //        Directory.CreateDirectory(directory);
    //    }
    //    File.WriteAllBytes(AudioLoader.GetFullFilepath(text, isMale), data);
    //    AudioResponse.Raise(this, new EventArgs<AudioArg>(new AudioArg(text, isMale)));
    //}

    //[RPC]
    //void Client_LeaderBoard(string LBString) {
    //    LeaderBoardGameData leaderBoardData = Serializer.LoadFromXmlString<LeaderBoardGameData>(LBString);
    //    LeaderBoardResponse.Raise(this, new EventArgs<LeaderBoardGameData>(leaderBoardData));
    //}

    //[RPC]
    //void Client_SetNetworkID(NetworkViewID viewID) {
    //    GetComponent<NetworkView>().viewID = viewID;
    //}

    //[RPC]
    //void Client_NameResponse(bool valid) {
    //    NameResponse.Raise(this, new EventArgs<bool>(valid));
    //}

    //[RPC]
    //void Client_LoadPlayerData(byte[] pdBytes) {
    //    Debug.Log("receive load call");
    //    Debug.Log(pdBytes.Length);
    //    string playerData;
    //    if (pdBytes.Length <= 1) {
    //        playerData = "";
    //    } else {
    //        playerData = System.Text.Encoding.UTF8.GetString(pdBytes);
    //    }
    //    if (playerData.IsEmptyOrNull()) {
    //        PlayerDataResponse.Raise(this, null);
    //    } else {
    //        var pd = Serializer.LoadFromXmlString<PlayerData>(playerData);
    //        PlayerDataResponse.Raise(this, new EventArgs<PlayerData>(pd));
    //    }
    //}

    //void OnApplicationQuit() {
    //    foreach (var key_val in Instance.playerDataLoggers) {
    //        key_val.Value.HandleQuit();
    //    }
    //}

}