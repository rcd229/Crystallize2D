//using UnityEditor;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class GameDataEditor : UnityEditor.AssetModificationProcessor {

//    static string[] OnWillSaveAssets(string[] assets){
//        SaveGlobalData ();

//        return assets;
//    }

//    static void SaveGlobalData(){
//        //SaveQuestData ();
//        UpdateChallengeData ();

//        //Debug.Log ("Adding test");
//        //GameData.Instance.QuestData.Quests.Set (new QuestInfoGameData (-100, -100));
//        //GameData.Instance.QuestData.Quests.Set (new FetchQuestInfoGameData (-101, -100));

//        GameData.SaveInstance();
//    }

//    static void SaveQuestData(){
//        var levelSettings = GameObject.FindObjectOfType<LevelSettings> ();
//        if (!levelSettings) {
//            Debug.LogWarning("No level settings.");
//            return;
//        }
		
//        if (levelSettings.ignoreGlobalLayout) {
//            Debug.Log("Ignoring global game data.");
//            return;
//        }
		
//        if(levelSettings.areaID == -1){
//            Debug.LogWarning("Level id cannot be -1");
//            return;
//        }
		
//        var currentAreaData = UpdateAreaData (levelSettings);
//        UpdateClientData (currentAreaData);
//        UpdateQuestData ();
//    }

//    static AreaGameData UpdateAreaData(LevelSettings levelSettings){
//        var connections = new List<AreaConnectionGameData> ();
//        foreach (var conn in GameObject.FindObjectsOfType<LevelTransitionTrigger>()) {
//            var connData = new AreaConnectionGameData(conn.targetLevelID, conn.transform.position);
//            connections.Add(connData);
//        }

//        var areaData = GameData.Instance.NavigationData.Areas.Get(levelSettings.areaID);
//        if (areaData == null) {
//            areaData = new AreaGameData (levelSettings.areaID, connections, EditorApplication.currentScene.Replace(".unity", ""), levelSettings.areaName);
//        }
//        areaData.Connections = connections;

//        GameData.Instance.NavigationData.Areas.Set (areaData);
//        return areaData;
//    }

//    static void UpdateClientData(AreaGameData currentArea){
//        var clients = GameObject.FindObjectsOfType<QuestTransform> ();
//        var foundClients = new HashSet<int> ();
//        foreach (var client in clients) {
//            if(client.clientID == -1){
//                Debug.LogWarning(client.name + ": client ID cannot be -1");
//                continue;
//            }

//            if(foundClients.Contains(client.clientID)){
//                Debug.LogWarning(client.name + ": duplicate client ID " + client.clientID);
//                continue;
//            }

//            var currentData = GameData.Instance.QuestData.Clients.Get(client.clientID);
//            if(currentData != null){
//                if(currentData.AreaID != currentData.AreaID){
//                    Debug.LogWarning(client.name + ": unable to override client from area " + currentData.AreaID);
//                    continue;
//                }
//            }

//            foundClients.Add(client.clientID);
//            var clientData = new QuestClientGameData(client.clientID, currentArea.AreaID, client.transform.position, client.name);
//            GameData.Instance.QuestData.Clients.Set(clientData);
//        }

//        //var worldObjs = GameObject.FindObjectsOfType<WorldObjectComponent> ();
//        //foreach (var obj in worldObjs) {
//        //    var worldObj = GameData.Instance.WorldData.WorldObjects;
//        //}

//        var originalClients = new List<QuestClientGameData> (GameData.Instance.QuestData.Clients.Items);
//        foreach (var origClient in originalClients) {
//            if(origClient.AreaID == currentArea.AreaID){
//                if(!foundClients.Contains(origClient.ClientID)){
//                    Debug.Log("Client ID: " + origClient.ClientID + " not found. Removing.");
//                    GameData.Instance.QuestData.Clients.Remove(origClient.ClientID);
//                }
//            }
//        }
//    }

//    static void UpdateQuestData(){
//        //TODO: get rid of this completely
//        var quests = GameObject.FindObjectsOfType<QuestClient> ();
//        var foundQuests = new HashSet<int> ();
//        foreach (var quest in quests) {
//            if(quest.questID == -1){
//                Debug.LogWarning(quest.name + ": quest ID cannot be -1");
//                continue;
//            }
			
//            if(foundQuests.Contains(quest.questID)){
//                Debug.LogWarning(quest.name + ": duplicate quest ID " + quest.questID);
//                continue;
//            }

//            var convClient = quest.GetComponent<QuestTransform>();
//            if(!convClient){
//                Debug.LogWarning(quest.name + ": no client attached.");
//                continue;
//            }
			
//            foundQuests.Add(quest.questID);
//            var questData = new QuestInfoGameData(quest.questID, convClient.clientID);

//            if(quest.GetComponent<FetchQuestClient>()){
//                //Debug.Log("Fetch quest.");
//                //questData.Item = quest.GetComponent<FetchQuestClient>().desiredItem;
//            }

//            GameData.Instance.QuestData.Quests.Set(questData);
//        }
//    }

//    static void UpdateChallengeData(){
//        var clients = GameObject.FindObjectsOfType<ConversationClient> ();

//        foreach (var client in clients) {
//            if(client.clientData){
//                var id = client.clientData.dialog.id;
//                var missingWords = new List<string>();
//                foreach(var w in client.missingWords){
//                    missingWords.Add(w.ID);
//                }
//                var challenge = new ChallengeGameData(id, missingWords);

//                GameData.Instance.ProgressionData.Challenges.Set(challenge);
//            }
//        }
//    }

//}
