using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NetworkTest : MonoBehaviour {

    IEnumerator Start() {
        CrystallizeNetwork.InitializeServer();
        CrystallizeNetwork.InitializeClient("localhost");

        while (!CrystallizeNetwork.Connected) {
            yield return null;
        }

        CrystallizeNetwork.Client.SendLogMessageToServer("abc");//.SendPlayerDataToServer();
        CrystallizeNetwork.Client.SendLeaderBoardDataToServer();
        CrystallizeNetwork.Client.SendPlayerDataToServer();

        CrystallizeNetwork.Client.RequestNameFromServer(PlayerData.Instance.PersonalData.Name, HandleName);
        CrystallizeNetwork.Client.RequestLeaderboardFromServer(HandleLeaderBoard);
        CrystallizeNetwork.Client.RequestPlayerDataFromServer(PlayerData.Instance.PersonalData.Name, HandlePlayerData);
    }

    void HandleName(bool nameResp) {
        Debug.Log("Can use name = " + nameResp);
    }

    void HandleLeaderBoard(LeaderBoardGameData ldrBrd) {
        Debug.Log(ldrBrd.ToString());
    }

    void HandlePlayerData(PlayerData playerData) {
        Debug.Log("Got player data with name: " + playerData.PersonalData.Name);
    }

}
