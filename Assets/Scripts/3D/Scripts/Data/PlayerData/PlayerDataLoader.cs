using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Serialization;
using Newtonsoft.Json;
//using System.Runtime.Serialization.

public class PlayerDataLoader {

    const string DefaultFileName = "TempPlayerData.xml";

    static string DefaultFilePath {
        get {
            return Application.dataPath + "/../" + DefaultFileName;
        }
    }

    public static void Save() {
        SaveLocal();
        PlayerData.Instance.PersonalData.TotalPlayTime += DateTime.Now - PlayerData.Instance.PersonalData.StartPlayTime;
        PlayerData.Instance.PersonalData.StartPlayTime = DateTime.Now;

        //var s = JsonConvert.SerializeObject(PlayerData.Instance, Formatting.Indented);
        //using (var writer = new StreamWriter(DefaultFilePath + ".json", false)) {
        //    writer.Write(s);
        //}

        //PlayerData.Instance
        if (CrystallizeNetwork.Connected) {
            CrystallizeNetwork.Client.SendPlayerDataToServer();
        }
    }

    public static void Load(string name, Action<PlayerData> loadComplete) {
        if (CrystallizeNetwork.Connected) {
            CrystallizeNetwork.Client.RequestPlayerDataFromServer(name, loadComplete);
        } else {
            LoadLocal();
        }
    }

    public static void SaveLocal() {
        SavePlayerData(PlayerData.Instance, DefaultFilePath);
    }

    public static void LoadLocal() {
        if (File.Exists(DefaultFilePath)) {
            LoadPlayerData(DefaultFilePath);
        }
        Debug.Log("Loading player data.");
    }

    public static void LoadPlayerData(string filePath) {
        PlayerData.Initialize(Serializer.LoadFromXml<PlayerData>(filePath));

        if (ReviewManager.main) {
            ReviewManager.main.LoadReviewLog(PlayerData.Instance.ReviewLog);
        }

        CrystallizeEventManager.main.RaiseLoad(null, EventArgs.Empty);

        if (Application.isEditor) {
            LoadPhraseLog();
        }
    }

    public static void SavePlayerData(PlayerData playerData, string filePath) {
        //Debug.Log(AreaManager.GetCurrentAreaID());

        if (ReviewManager.main) {
            playerData.ReviewLog = ReviewManager.main.reviewLog;
        }
        Serializer.SaveToXml<PlayerData>(filePath, playerData);

        CrystallizeEventManager.main.RaiseSave(null, EventArgs.Empty);

        if (Application.isEditor) {
            SavePhraseLog();
        }
    }

    public static void ClearData() {
        if (File.Exists(DefaultFilePath)) {
            File.Delete(DefaultFilePath);
        }

        PlayerData.Initialize(new PlayerData());

        CrystallizeEventManager.main.RaiseLoad(null, EventArgs.Empty);
    }

    /****
     * TODO: EWWWWWWW. Fix this later.
     * */
    const string SentenceFile = "sentences.txt";
    const string WordFile = "words.txt";

    static HashSet<string> sentences = new HashSet<string>();
    static HashSet<string> words = new HashSet<string>();

    public static void LogSentence(string speaker, string sentence) {
        string line = speaker + ": " + sentence;
        Debug.Log("Logging " + line);
        if (!sentences.Contains(line)) {
            sentences.Add(line);
        }
    }

    public static void LogWord(string word) {
        if (!words.Contains(word)) {
            words.Add(word);
        }
    }

    public static void SavePhraseLog() {
        SaveSet(WordFile, words);
        SaveSet(SentenceFile, sentences);
        Debug.Log("Saved phrase log data.");
        //_instance = null;
    }

    static void SaveSet(string filename, HashSet<string> strings) {
        if (File.Exists(filename)) {
            File.Delete(filename);
        }

        using (var writer = new StreamWriter(filename)) {
            var list = strings.OrderBy((s) => s);
            foreach (var item in list) {
                writer.WriteLine(item);
            }
        }
    }

    public static void LoadPhraseLog() {
        LoadSet(WordFile, words);
        LoadSet(SentenceFile, sentences);
    }

    static void LoadSet(string filename, HashSet<string> strings) {
        if (File.Exists(filename)) {
            using (var reader = new StreamReader(filename)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    //Debug.Log(line);
                    strings.Add(line);
                }
            }
        }
    }

}