using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerData {

    static PlayerData _instance;

    public static bool Alive {
        get {
            return _instance != null;
        }
    }

    public static PlayerData Instance {
        get {
            if (_instance == null) {
                PlayerDataLoader.LoadLocal();
                if (_instance == null) {
                    _instance = new PlayerData();
                }
            }
            return _instance;
        }
    }

    public static void Initialize(PlayerData playerData) {
        _instance = playerData;
    }

    //public ReviewLog ReviewLog { get; set; }
    public PhraseReviewPlayerData Reviews { get; set; }
    public KanaReviewPlayerData KanaReviews { get; set; }

    public PersonalPlayerData PersonalData { get; set; }
    public WordCollectionPlayerData WordCollection { get; set; }
    public PhraseCollectionPlayerData PhraseStorage { get; set; }
    //public ItemInventoryPlayerData ItemInventory { get; set; }
    public TutorialPlayerData Tutorial { get; set; }
    //public LocationPlayerData Location { get; set; }
    //public TimePlayerData Time { get; set; }
    //public FlagPlayerData Flags { get; set; }
    //public JobCollectionPlayerData Jobs { get; set; }
    //public HomeCollectionPlayerData Homes { get; set; }
    public ProficiencyPlayerData Proficiency { get; set; }
    //public AppearancePlayerData Appearance { get; set; }
    public SessionPlayerData Session { get; set; }
    //public NPCPlayerData NPCData { get; set; }
    public int Money { get; set; }
	//public QuestPlayerData QuestData{get;set;}
	//public UIPlayerData UIData{get;set;}

    public JapaneseTools.JapaneseScriptType ScriptType { get; set; }

    public PlayerData() {
        Reviews = new PhraseReviewPlayerData();
        KanaReviews = new KanaReviewPlayerData();
        PersonalData = new PersonalPlayerData();
        //WordStorage = new WordCollectionPlayerData();
        //PhraseStorage = new PhraseCollectionPlayerData();
        //ItemInventory = new ItemInventoryPlayerData();
        //Tutorial = new TutorialPlayerData();
        //Location = new LocationPlayerData();
        //Time = new TimePlayerData();
        //Flags = new FlagPlayerData();
        //Jobs = new JobCollectionPlayerData();
        //Homes = new HomeCollectionPlayerData();
        Proficiency = new ProficiencyPlayerData();
        Session = new SessionPlayerData();
        //Appearance = new AppearancePlayerData();
        ScriptType = (JapaneseTools.JapaneseScriptType)GameSettings.Instance.TextMode;
  //      NPCData = new NPCPlayerData();
		//QuestData = new QuestPlayerData();
		//UIData = new UIPlayerData();
    }

}