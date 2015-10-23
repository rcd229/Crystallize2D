using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class GameData {

    const string FileName = "CrystallizeGameData";
    const string FileExtension = ".txt";
    const string EditorFilePath = "/crystallize/Resources/";
    const string PlayerFilePath = "/PlayerGameData/";

    static GameData _instance;

    public static GameData Instance {
        get {
            if (_instance == null) {
                LoadInstance();
            }
            return _instance;
        }
    }

    public static bool CanSave { get; set; }

    static GameData() {
        CanSave = true;
    }

    public static void LoadInstance() {
        if (Application.isEditor) {
            //Debug.Log("Loading GameData. Is editor.");
            _instance = Serializer.LoadFromXml<GameData>(GetEditorDataPath(), false);
            if (_instance == null) {
                _instance = new GameData();
            }
        } else {
            Debug.Log("Loading GameData. Is player.");
            if (File.Exists(GetPlayerDataPath())) {
                Debug.LogWarning("(not implemented)");
            } else {
                var text = Resources.Load<TextAsset>(FileName);
                if (text != null) {
                    _instance = Serializer.LoadFromXmlString<GameData>(text.text);
                } else {
                    _instance = new GameData();
                }
            }
        }
    }

    public static void SaveInstance() {
        PhraseSetCollectionGameData.SaveAll();

        if (!CanSave) {
            Debug.LogError("GameData contains temporary data. Unable to save.");
            return;
        }

        if (_instance != null) {
            if (Application.isEditor) {
                Serializer.SaveToXml<GameData>(GetEditorDataPath() + ".tmp", _instance);

                if (File.Exists(GetEditorDataPath() + ".bak2")) {
                    File.Delete(GetEditorDataPath() + ".bak2");
                }

                if (File.Exists(GetEditorDataPath() + ".bak1")) {
                    File.Move(GetEditorDataPath() + ".bak1", GetEditorDataPath() + ".bak2");
                }

                if (File.Exists(GetEditorDataPath())) {
                    File.Move(GetEditorDataPath(), GetEditorDataPath() + ".bak1");
                }

                File.Move(GetEditorDataPath() + ".tmp", GetEditorDataPath());
            } else {
                Debug.LogWarning("Is player. (not implemented)");
            }
        }
    }

    static string GetEditorDataPath() {
        return Application.dataPath + EditorFilePath + FileName + FileExtension;
    }

    static string GetPlayerDataPath() {
        return Application.dataPath + PlayerFilePath + FileName + FileExtension;
    }


    public JobCollectionGameData Jobs { get; set; }
    public HomeCollectionGameData Homes { get; set; }
    public ChallengeProgressionGameData ProgressionData { get; set; }
    public NavigationGameData NavigationData { get; set; }
    public TradeGameData TradeData { get; set; }
    public EquipmentGameData Equipment { get; set; }
    public NPCGameData NPCs { get; set; }
	public QuestCollectionGameData Quests{get;set;}
    public SpawnableGameData Spawn { get; set; }

    public GameData() {
        //QuestData = new QuestGameData ();
        Jobs = new JobCollectionGameData();
        Homes = new HomeCollectionGameData();
        ProgressionData = new ChallengeProgressionGameData();
        NavigationData = new NavigationGameData();
        //PhraseClassData = new PhraseClassGameData();
        //WorldData = new WorldGameData();
        //DialogueData = new DialogueGameData();
        TradeData = new TradeGameData();
        Equipment = new EquipmentGameData();
        NPCs = new NPCGameData();
		Quests = new QuestCollectionGameData();
        Spawn = new SpawnableGameData();
    }

}
