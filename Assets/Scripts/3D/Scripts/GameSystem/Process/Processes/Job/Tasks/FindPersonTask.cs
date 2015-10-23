using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class FindPersonProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string PleaseHelpMe = "Please help me.";
    public const string OkayLetsGo = "Okay, let's go.";

    public const string LookingForPerson = "I'm looking for a [person]";
    public const string LookingForAttributePerson = "I'm looking for a [attribute] [person]";
    public const string LookingForItemPerson = "I'm looking for a [person] with a [item]";
    public const string LookingForAttributeItemPerson = "I'm looking for a [attribute] [person] with a [item]";

    public const string Person = "person";
    public const string Boy = "Boy";
    public const string Girl = "Girl";
    public const string RedHaired = "red hair";
    public const string BlackHaired = "black hair";
    public const string BrownHaired = "brown hair";
    public const string BlondHaired = "blond hair";
    public const string BlueHaired = "blue hair";
    public const string GreenHaired = "green hair";
    public const string GrayHaired = "gray hair";
    public const string Shorts = "Shorts";
    public const string Pants = "Pants";
    public const string ShortSleevedShirt = "short sleeved shirt";
    public const string LongSleevedShirt = "long sleeved shirt";

    public static string[] SeekStrings() {
        return new string[] {
            LookingForPerson, LookingForAttributePerson, LookingForItemPerson, LookingForAttributeItemPerson
        };
    }

    public static string[] PersonStrings() {
        return new string[] {
            Person, Boy, Girl
        };
    }

    public static string[] HairStrings() {
        return new string[] {
            RedHaired, BlackHaired, BrownHaired, BlondHaired, BlueHaired, GreenHaired, GrayHaired
        };
    }

    public static string[] ClothsStrings() {
        return new string[]{
            Shorts, Pants, ShortSleevedShirt, LongSleevedShirt
        };
    }

    public static List<string> AllStrings() {
        var list = new List<string>();
        list.AddRange(SeekStrings());
        list.AddRange(PersonStrings());
        list.AddRange(HairStrings());
        list.AddRange(ClothsStrings());
        return list;
    }

    public ProcessExitCallback OnExit { get; set; }

    JobTaskRef task;
    TalkToActorsInitArgs talkArgs;
    ConversationArgs convArgs;

    int currentReps = 0;
    int totalReps = 5;
    string phraseKey;
    string personKey;
    string attributeKey;
    string itemKey;
    ContextData context;
    GameObject client;
    List<string> sceneTargets;

    public void Initialize(JobTaskRef param1) {
        task = param1;

        if (task.IsPromotion) {
            StartPromotion();
        }

        //var area = GameObject.Find(task.Data.AreaName);
        var clientTarget = SceneAreaManager.Instance.Get(task.Data.AreaName).transform.Find(TagLibrary.Client);
        client = DialogueActorUtil.GetNewActor(GetClientAppearance(), clientTarget);
        client.name = task.Data.Actor.Name;
        client.tag = TagLibrary.Untagged;
        convArgs = new ConversationArgs(client, task.Data.Dialogue, null, false);
        
        ProcessLibrary.BeginConversation.Get(convArgs, ConversationSegment0Exit, this);

        TaskState.Instance.SetInstructions("Find the specified people.");
        TaskState.Instance.SetState("People", string.Format("{0}/{1}", 0, totalReps));
    }

    void BeginAdditionalConversation() {
        ClearTargets();

        var area = GameObject.Find(task.Data.AreaName);
        var clientTarget = area.transform.Find(TagLibrary.Client);
        client.transform.position = clientTarget.transform.position;
        client.transform.rotation = clientTarget.transform.rotation;

        CoroutineManager.Instance.WaitAndDo(
        () => ProcessLibrary.BeginConversation.Get(convArgs, ConversationSegment1Exit, this));
    }

    void ConversationSegment0Exit(object sender, object args) {
        ProcessLibrary.ConversationSegment.Get(convArgs, ConversationSegment1Exit, this);
    }

    void ConversationSegment1Exit(object sender, object obj) {
        GenerateNewContext();
        convArgs.Dialogue = new DialogueSequence(task.Job.GetPhrase(phraseKey));
        convArgs.Context = context;
        ProcessLibrary.ConversationSegment.Get(convArgs, ConversationSegment2Exit, this);
    }

    void ConversationSegment2Exit(object sender, object obj) {
        convArgs.ClearOnClose = false;
        ProcessLibrary.EndConversation.Get(convArgs, ConversationExit, this);
    }

    void ConversationExit(object sender, object obj) {
        var blackout = UILibrary.BlackScreen.Get(null);
        blackout.Complete += blackout_Complete;
    }

    void blackout_Complete(object sender, EventArgs<object> e) {
        RegenerateTargets();

        var area = GameObject.Find(task.Data.AreaName);
        var home = area.transform.Find(TagLibrary.Home);
        if (home != null) {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = home.position;
        } else {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = area.transform.position - Vector3.up;
        }
        
        ProcessLibrary.TalkToActors.Get(talkArgs, TaskExit, this);
    }

    void TaskExit(object sender, object obj) {
        currentReps++;
        TaskState.Instance.SetState("People", string.Format("{0}/{1}", currentReps, totalReps));
        PlayerDataConnector.AddRepetitionToJob(task.Job, task);
        if (currentReps >= totalReps) {
            Exit();
        } else {
            BeginAdditionalConversation();
        }
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    PlayerConversationInitArgs GetConversationArgs(GameObject target) {
        //Debug.Log("Actor: " + target + "; " + target.name.Contains("Correct"));
        return PlayerConversationInitArgs.YesNoArgs(target, new DialogueSequence(
            target.name, 
            new PhraseSequence(task.Job.GetPhrase(OkayLetsGo))), 
            target.name.Contains("Correct"));
    }

    AppearancePlayerData GetClientAppearance() {
        var appearance = new AppearancePlayerData();
        appearance.Gender = (int)AppearanceGender.Male;
        appearance.HairType = 1;
        appearance.HairColor = (int)AppearanceHairColor.Black;
        appearance.TopType = 1;
        appearance.TopMaterial = (int)AppearanceShirt02Material.Black;
        appearance.BottomType = 1;
        appearance.BottomMaterial = (int)AppearanceLegs02Material.Jeans;
        return appearance;
    }

    void RegenerateTargets() {
        var area = GameObject.Find(task.Data.AreaName);
        int count = 0;
        if (task.IsPromotion) {
            count = PersonStrings().Length;
        } else {
            count = 1 + ProgressUtil.GetAmountForTiers(task.Job.PlayerDataInstance.Repetitions, 1, 3, 4, 6, 10);
        }
        var clientTarget = area.transform.Find(TagLibrary.Client);
        sceneTargets = SceneAreaUtil.ScatterTargets(task.Data.AreaName, count, clientTarget.position);
        talkArgs = TalkToActorsInitArgs.GetYesNo(GetConversationArgs);

        var personTag = "";
        switch (personKey) {
            case Boy:
                personTag = "Male";
                break;
            case Girl:
                personTag = "Female";
                break;
        }
        //Debug.Log("Person tag: " + personTag + "; key: " + personKey);

        var actors = new List<GameObject>();
        if (IsLegs(itemKey)) {
           actors = DialogueActorUtil.GetActorsForTargetsWithTag(sceneTargets, personTag, attributeKey, "", "", "", itemKey);
        } else {
            actors = DialogueActorUtil.GetActorsForTargetsWithTag(sceneTargets, personTag, attributeKey, "", "", itemKey);
        }

        if (personKey == Person) {
            foreach (var t in actors) {
                t.name += "Correct";
            }
        }
    }

    void ClearTargets() {
        if (sceneTargets != null) {
            foreach (var t in sceneTargets) {
                GameObject.Destroy(GameObject.Find(t));
            }
        }
        sceneTargets = null;
    }

    void GenerateNewContext() {
        phraseKey = SeekStrings().GetSafely(task.Job.GameDataInstance.Difficulty);

        var index = ProgressUtil.GetRandomIndexForTiers(
            task.Job.PlayerDataInstance.Repetitions, 
            task.Job.GameDataInstance.Difficulty, 0, 
            1, 3
            );
        personKey = PersonStrings().GetSafely(index);

        index = ProgressUtil.GetRandomIndexForTiers(
            task.Job.PlayerDataInstance.Repetitions,
            task.Job.GameDataInstance.Difficulty, 1,
            1, 2, 3, 3, 3, 3
            );
        attributeKey = HairStrings().GetSafely(index);

        index = ProgressUtil.GetRandomIndexForTiers(
            task.Job.PlayerDataInstance.Repetitions,
            task.Job.GameDataInstance.Difficulty, 1,
            1, 2, 3
            );
        itemKey = ClothsStrings().GetSafely(index);


        switch(task.Job.GameDataInstance.Difficulty){
            case 0:
                attributeKey = "";
                itemKey = "";
                if (personKey == Person && task.Job.PlayerDataInstance.Repetitions > 0) {
                    personKey = Girl;
                }
                break;

            case 1:
                itemKey = "";
                break;

            case 2:
                attributeKey = "";
                break;
        }

        context = new ContextData();
        context.Set("person", task.Job.GetPhrase(personKey));
        if (attributeKey != "") {
            context.Set("attribute", task.Job.GetPhrase(attributeKey));
        }
        if (itemKey != "") {
            context.Set("item", task.Job.GetPhrase(itemKey));
        }
        DataLogger.LogTimestampedData("TargetPerson", personKey, itemKey, attributeKey);

        if (task.IsPromotion) {
            UpdateWithPromotionContext();
        }
    }

    bool IsLegs(string item) {
        if (item == Shorts || item == Pants) {
            return true;
        }
        return false;
    }

    #region Promotion
    List<string> promotionPersonStrings = new List<string>();

    void StartPromotion() {
        promotionPersonStrings = new List<string>(PersonStrings());
        promotionPersonStrings.Shuffle();
        totalReps = promotionPersonStrings.Count;
    }

    void UpdateWithPromotionContext() {
        if (task.Job.GameDataInstance.Difficulty == 0) {
            personKey = promotionPersonStrings.GetSafely(currentReps);
            context.Set("person", task.Job.GetPhrase(personKey));
        }
        DataLogger.LogTimestampedData("TargetPerson", personKey, itemKey, attributeKey);
    }
    #endregion

}
