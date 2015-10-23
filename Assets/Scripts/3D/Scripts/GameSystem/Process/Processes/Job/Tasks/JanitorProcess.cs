using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class JanitorProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string NiceJob = "Nice job!";
    public const string OkayJob = "That was passable.";
    public const string WorkHarder = "You need to work harder.";
    public const string BadJob = "What are you doing!? Please do your work well.";
    public const string CleanArea = "Clean the [area]";

    public static PropType[] GetFurnature1() {
        return new PropType[] { PropType.Table, PropType.Chair, PropType.PottedPlant };
    }

    public static PropType[] GetColors() {
        return new PropType[] { PropType.BlueArea, PropType.RedArea, PropType.YellowArea, PropType.WhiteArea,
                                PropType.BlackArea, PropType.GreenArea, PropType.BrownArea, PropType.PurpleArea};
    }

    public static IEnumerable<PropType> GetAllProps() {
        var list = new List<PropType>();
        list.AddRange(GetFurnature1());
        list.AddRange(GetColors());
        return list;
    }

    public static PropType[] GetPropsForDifficulty(int difficulty) {
        if (difficulty == 0) {
            return GetFurnature1();
        } else if (difficulty == 1) {
            return GetColors();
        } else {
            var l = new List<PropType>();
            l.AddRange(GetFurnature1());
            l.AddRange(GetColors());
            return l.ToArray();
        }
    }

    public ProcessExitCallback OnExit { get; set; }

    GameObject actor;
    JobTaskRef task;

    int taskCount = 0;
    //    int maxTries = 3;
    GameObject targetRegion;
    ITemporaryUI messageUI;
    List<GameObject> tutorialInstances;
    List<GameObject> instances = new List<GameObject>();
    List<string> targets = new List<string>();
    ConversationArgs convArgs;
    int totalTaskCount = 0;
    int correctIndex = 0;

    public void Initialize(JobTaskRef param1) {
        task = param1;

        if (task.IsPromotion) {
            InitializePromotion();
        }

        actor = GetActor();
        actor.name = task.Data.Actor.Name;

        var convArgs = new ConversationArgs(actor, task.Data.Dialogue, null, true);

        ProcessLibrary.BeginConversation.Get(convArgs, HandleConversationExit, this);
    }

    void HandleConversationExit(object sender, object obj) {
        //ProcessLibrary.BlackOut.GetNested(
        //    ProcessLibrary.EquipItem, new EquipmentArgs("Broom", PlayerManager.Instance.PlayerGameObject), HandleGetItemComplete, this);
        HandleGetItemComplete(null, null);
    }

    void HandleGetItemComplete(object sender, object obj) {
        GetNewTargets();
        var d = new DialogueSequence(task.Job.GetPhrase(CleanArea));
        var c = GetNewContext();
        actor.GetOrAddComponent<_SceneSayPhrase>().phrase = task.Job.GetPhrase(CleanArea).InsertContext(c);
        var args = ConversationArgs.OpenSegmentArgs(actor, d, c);
        ProcessLibrary.ConversationSegment.Get(args, HandleConversationEnded, this);

        TaskState.Instance.SetInstructions("Clean up all the things.");
        //Debug.Log("conv");
    }

    void HandleConversationEnded(object sender, object obj) {
        //Debug.Log("end");
        ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(actor, task.Data.Dialogue, false), HandleBeginExplore, this);
    }

    void HandleBeginExplore(object sender, object obj) {
        totalTaskCount = GetTotalTaskCount();
        HandleContinueExplore();
    }

    void AdditionalExplore() {
        foreach (var t in targets) {
            GameObject.Destroy(GameObject.Find(t));
        }
        targets = null;

        actor.GetComponentInChildren<DialogueActor>().SetLine(null);
        var blackout = UILibrary.BlackScreen.Get(null);
        Debug.Log("Adding callback to complete");
        blackout.Complete += AdditionalBlackoutComplete;
    }

    void AdditionalBlackoutComplete(object sender, EventArgs<object> e) {
        GetNewTargets();

        var convArgs = new ConversationArgs(actor, task.Data.Dialogue, null, false);
        ProcessLibrary.BeginConversation.Get(convArgs, AdditionalBeginConversationCallback, this);
    }

    void AdditionalBeginConversationCallback(object sender, object e) {
        var d = new DialogueSequence(task.Job.GetPhrase(CleanArea));
        var c = GetNewContext();
        actor.GetOrAddComponent<_SceneSayPhrase>().phrase = task.Job.GetPhrase(CleanArea).InsertContext(c);
        convArgs = ConversationArgs.OpenSegmentArgs(actor, d, c);
        CoroutineManager.Instance.WaitAndDo(
            () => ProcessLibrary.ConversationSegment.Get(convArgs, AdditionalConversationCallback, this),
            new WaitForSeconds(1f));
    }

    void AdditionalConversationCallback(object sender, object args) {
        ProcessLibrary.EndConversation.Get(convArgs, (s, e) => HandleContinueExplore(), this);
    }

    void HandleContinueExplore() {
        // TODO: get tutorial out of here
        if (!PlayerData.Instance.Tutorial.GetTutorialViewed("Place")) {
            tutorialInstances = new List<GameObject>();
            var prefab = Resources.Load<GameObject>("Tutorial/DownArrow");
            foreach (var p in GameObject.FindGameObjectsWithTag(TagLibrary.Place)) {
                if (p.name != TagLibrary.Client) {
                    var instance = GameObject.Instantiate<GameObject>(prefab);
                    instance.transform.position = p.transform.position + 1.5f * Vector3.up;
                    instance.transform.parent = p.transform;
                    instance.AddComponent<IndicatorComponent>().Initialize("", null, new MapIndicator(MapResourceType.Standard, Color.yellow), false);
                    tutorialInstances.Add(instance);
                }
            }
            messageUI = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click on things to clean them up.", true, false)); //UILibrary.Message.Get("-Click on things to clean them up.");
            //messageUI.CloseOnPlayerMove();
        }
        // end tutorial

        ProcessLibrary.Explore.Get(new ExploreInitArgs("Broom", "click to clean this", "cleaning here won't do any good"), HandleExitExplore, this);
    }

    void HandleExitExplore(object sender, ExploreResultArgs obj) {
        // TODO: get tutorial out of here
        if (tutorialInstances != null) {
            PlayerData.Instance.Tutorial.SetTutorialViewed("Place");
            foreach (var i in tutorialInstances) {
                GameObject.Destroy(i);
            }
            tutorialInstances = null;
        }
        messageUI.CloseIfNotNull();
        // end tutorial

        if (obj.Target == null) {
            HandleBeginExplore(null, null);
            Debug.Log("No object returned.");
        } else {
            if (obj.Target == targetRegion.transform.parent.gameObject) {
                DataLogger.LogTimestampedData("CleanedCorrect", obj.Target.name);
                var ui = UILibrary.PositiveFeedback.Get("");
                ui.Complete += PositiveFeedbackComplete;
            } else {
                DataLogger.LogTimestampedData("CleanedWrong", obj.Target.name);
                UILibrary.NegativeFeedback.Get("", NegativeFeedbackComplete, this);
            }
        }
    }

    void PositiveFeedbackComplete(object sender, EventArgs<object> e) {
        taskCount++;
        //PlayerDataConnector.AddRepetitionToJob(task.Job, task);
        UpdateStatus(taskCount);

        if (taskCount >= totalTaskCount) {
            PlayExitDialogue();
        } else {
            AdditionalExplore();
        }
    }

    void NegativeFeedbackComplete(object sender, EventArgs<object> e) {
        HandleContinueExplore();
    }

    void PlayExitDialogue() {
        PhraseSequence p = null;
        var s = PlayerData.Instance.Session.GetScore();
        if (s >= 0.99f) {
            p = task.Job.GetPhrase(NiceJob);
        } else if (s >= 0.75f) {
            p = task.Job.GetPhrase(OkayJob);
        } else if (s >= 0.25f) {
            p = task.Job.GetPhrase(WorkHarder);
        } else {
            p = task.Job.GetPhrase(BadJob);
        }

        var d = new DialogueSequence(p);
        d.AddEvent(0, ConfidenceSafeEvent.Instance);
        ProcessLibrary.Conversation.Get(new ConversationArgs(actor, d), HandleExitConversationExit, this);
    }

    void HandleExitConversationExit(object sender, object obj) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    void GetNewTargets() {
        RegenerateAreas();
        targetRegion = instances.GetSafely(correctIndex);
    }

    ContextData GetNewContext() {
        var c = new ContextData();
        c.Set("area", targetRegion.GetComponentInChildren<EnvironmentPhrase>().phrase.Get());
        return c;
    }

    void RegenerateAreas() {
        //        var area = GameObject.Find(task.Data.AreaName);
        var count = GetTargetCount();
        targets = SceneAreaUtil.ScatterTargets(task.Data.AreaName, count, actor.transform.position);

        instances = new List<GameObject>();
        foreach (var p in GetPrefabs(count)) {
            var instance = GameObjectUtil.GetResourceInstance(p.ResourcePath);
            instance.GetComponent<EnvironmentPhrase>().phrase.Set(task.Job.GetPhrase(p.Label));
            instances.Add(instance);
        }
        instances = GameObjectUtil.RandomAssignInstancesToTargets(targets, instances);
    }

    public virtual int GetTotalTaskCount() {
        if (task.IsPromotion) {
            return GetPromotionTargetCount();
        } else {
            return GetNormalTotalTaskCount();
        }
    }

    int GetNormalTotalTaskCount() {
        switch (task.Job.GameDataInstance.Difficulty) {
            case 0:
                return 5 + ProgressUtil.GetAmountForTiers(task.Job.PlayerDataInstance.Days, 3, 5);
        }
        return 5;
    }

    int GetTargetCount() {
        if (task.IsPromotion) {
            return GetPromotionTargetCount();
        } else {
            return GetNormalTargetCount();
        }
    }

    int GetNormalTargetCount() {
        var reps = task.Job.PlayerDataInstance.Repetitions;
        var count = 3;
        var completeList = GetPropsForDifficulty(task.Job.GameDataInstance.Difficulty);
        correctIndex = 0;
        switch (task.Job.GameDataInstance.Difficulty) {
            case 0:
                count = 1 + ProgressUtil.GetAmountForTiers(reps, 1, 2);
                correctIndex = ProgressUtil.GetRandomIndexForTiers_(reps, 2, 3);
                break;
            case 1:
                count = 1 + ProgressUtil.GetAmountForTiers(reps, 1, 2, 3, 4, 5, 5, 5);
                correctIndex = ProgressUtil.GetRandomIndexForTiers_(reps, 2, 2, 3, 4, 5, 5, 5);
                break;
        }

        var clamped = Mathf.Min(completeList.Length, count);
        return clamped;
    }

    List<PropType> GetPrefabs(int count) {
        var completeList = GetPropsForDifficulty(task.Job.GameDataInstance.Difficulty);
        var subList = new List<PropType>();
        for (int i = 0; i < count; i++) {
            subList.Add(completeList.GetSafely(i));
        }
        return subList;
    }

    AppearancePlayerData GetClientAppearance() {
        var appearance = new AppearancePlayerData();
        appearance.Gender = (int)AppearanceGender.Male;
        appearance.HairType = 0;
        appearance.HairColor = (int)AppearanceHairColor.Brown;
        appearance.TopType = 1;
        appearance.TopMaterial = (int)AppearanceShirt02Material.Gray;
        appearance.BottomType = 1;
        appearance.BottomMaterial = (int)AppearanceLegs02Material.Gray;
        return appearance;
    }

    public void UpdateStatus(int count) {
        PlayerDataConnector.AddRepetitionToJob(task.Job, task);
        TaskState.Instance.SetState("Things", string.Format("{0}/{1}", count, totalTaskCount));
    }

    #region virtual methods
    public virtual GameObject GetActor() {
        return DialogueActorUtil.GetNewActor(GetClientAppearance(), task.GetClientTarget());
    }
    #endregion

    #region Promotion
    List<int> promotionCorrectTargets = new List<int>();

    void InitializePromotion() {
        promotionCorrectTargets = new List<int>();
        for (int i = 0; i < GetPromotionTargetCount(); i++) {
            promotionCorrectTargets.Add(i);
        }
        promotionCorrectTargets.Shuffle();
    }

    int GetPromotionTargetCount() {
        correctIndex = promotionCorrectTargets.GetSafely(taskCount);
        return GetPropsForDifficulty(task.Job.GameDataInstance.Difficulty).Length;
    }
    #endregion

}