using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class GameSettings {

    public const string InterdependenceModule = "InterdependenceModule";

    const string FileName = "CrystallizeSettings";
    const string FileExtension = ".txt";
    const string EditorFilePath = "/Settings/";
    const string PlayerFilePath = "/Settings/";

    static GameSettings _instance;

    static HashSet<string> locks = new HashSet<string>();

    public static GameSettings Instance {
        get {
            if (_instance == null) {
                LoadInstance();
            }
            return _instance;
        }
    }

    static GameSettings() {
        SetFlag(UIFlags.LockCompass, true);
    }

    public static void SetFlag(string lck, bool val) {
        if (val) {
            if (!locks.Contains(lck)) {
                locks.Add(lck);
            }
        } else {
            if (locks.Contains(lck)) {
                locks.Add(lck);
            }
        }
    }

    public static bool GetFlag(string lck) {
        return locks.Contains(lck);
    }

    public static void LoadInstance() {
        _instance = Serializer.LoadFromXml<GameSettings>(GetFilePath());
        if (_instance == null) {
            _instance = new GameSettings();
        }
    }

    public static void SaveInstance() {
        if (_instance != null) {
            Serializer.SaveToXml<GameSettings>(GetFilePath(), _instance);
        }
    }

    static string GetFilePath() {
        var dir = Directory.GetParent(Application.dataPath);
        return dir.FullName + EditorFilePath + FileName + FileExtension;
    }

    static void HandleQuit(object sender, System.EventArgs args) {
        SaveInstance();
    }

    public int TextMode { get; set; }
    public bool IsServer { get; set; }
    public bool Local { get; set; }
    public string ServerIP { get; set; }
    public string MasterServerIP { get; set; }
    public string ExperimentModule { get; set; }
    public int ExperimentCondition { get; set; }
	public bool IsDebug { get; set; }
    public bool UseTutorials { get; set; }

    public GameSettings() {
        TextMode = (int)JapaneseTools.JapaneseScriptType.Romaji;
        IsServer = false;
        Local = false;
        ServerIP = "";
        MasterServerIP = "";
        ExperimentModule = "";
        ExperimentCondition = 0;
		IsDebug = false;
        UseTutorials = true;
    }

}
