using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public abstract class BaseFreeExploreProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public static bool isPhraseEnglish = true;
    public const string consumerTag = TagLibrary.Actor;
    const string mapTargetName = "OmniscientCameraTarget";

    ProcessFactoryRef<JobArgs, JobTaskExitArgs> JobWrapper = new ProcessFactoryRef<JobArgs, JobTaskExitArgs>();
    ProcessFactoryRef<TaskSelectorArgs, JobTaskRef> TaskSelector = new ProcessFactoryRef<TaskSelectorArgs, JobTaskRef>();

    ITemporaryUI ui;

    public void Initialize(JobTaskRef param1) {
        JobWrapper.Set<JobWrapperProcess>();
        SceneChangeEvent.Instance.SceneChanged += HandleSceneChanged;
        LoadSceneObjects();
        AfterInitialize();
        //BeginExplore(null, null);
    }

    void LoadSceneObjects() {
        // TODO: move this to after scene load
        //Debug.Log("Loading explore objectes");
        HookEvents();

        if (Map.Instance == null && PlayerDataConnector.MapOpenStatus) {
            UILibrary.MiniMap.Get(mapTargetName);
        }

        if (PlayerDataConnector.QuestStatusPanelOpenStatus) {
            UILibrary.QuestHUD.Get(this);
        }

        MapManager.Instance.AddPlayerMapElement("Player");
        NPCManager.Instance.SpawnNPCs();
        SceneAreaManager.Instance.ActivateAll();
        QuestUtil.Initialize();
        //CoroutineManager.Instance.WaitAndDo(NPCManager.Instance.AssignIndicators);
    }

    void HandleSceneChanged(object sender, EventArgs args) {
        CoroutineManager.Instance.WaitAndDo(LoadSceneObjects, new WaitForSeconds(0.1f));
    }

    void HandleNoEnergy(object sender, EventArgs<int> e) {
        if (PlayerData.Instance.Session.Confidence == 0) {
            ConfidenceDepleted();
        }
    }

    protected virtual void ConfidenceDepleted() {
        Debug.Log("No Energy");
        UILibrary.PromotionFailedText.Get("Confidence depleted...", HandleFailureTextComplete, this);
    }

    public void HandleFailureTextComplete(object obj, EventArgs<object> args) {
        var exitArgs = new JobTaskExitArgs(true);
        Exit(exitArgs);
    }

    public void ForceExit() {
        UnHookEvents();
        ui.CloseIfNotNull();
    }

    public ProcessExitCallback OnExit { get; set; }

    protected void Exit(JobTaskExitArgs exitArgs) {
        //Debug.Log(exitArgs);
        SceneChangeEvent.Instance.SceneChanged -= HandleSceneChanged;
        UnHookEvents();
        ui.CloseIfNotNull();
        OnExit.Raise(this, exitArgs);

        AfterExit();
    }

    //Procedure methods
    protected virtual void BeginExplore(object sender, object e) {
        //Debug.Log("Beginning day.");
        ProcessLibrary.Explore.Get(new ExploreInitArgs(null, null, "have a talk", "invalid action"), ExploreCallback, this);
    }

    // TODO: retreive the child process from the found component (IExploreSubProcessContainer?)
    protected virtual void ExploreCallback(object sender, ExploreResultArgs args) {
        var interactable = args.Target.GetInterface<IInteractableSceneObject>();
        if (interactable is IInteractableProcessTerminator && interactable.enabled) {
            var exitArgs = new JobTaskExitArgs(true);
            Exit(exitArgs);
            return;
        }

        if (interactable != null && interactable.enabled) {
            interactable.BeginInteraction(AfterExploreCallback, this);
            return;
        }

        //TODO how are the components placed in the gameobject?
        var consumer = args.Target.GetInterfaceInSelfOrChild<IEnergyConsumer>();

        if (consumer is JobNPC) {
            //TODO wrapper for greeting and destroying gameobject
            var jobref = ((JobNPC)consumer).gameObject.GetComponentInSelfOrChild<JobRefContainer>().Data;
            TaskSelector.Set(jobref.GameDataInstance.TaskSelector.SelectionProcess.ProcessType);
            TaskSelector.Get(jobref.GameDataInstance.TaskSelector.GetArgs(jobref), (s, e) => HandleJobSelected(s, new JobArgs(e, (JobNPC)consumer)), this);
        } else {
            AfterExploreCallback(null, null);
        }
    }

    void HandleJobSelected(object sender, JobArgs data) {
        JobWrapper.Get(data, AfterExploreCallback, this);
    }

    protected virtual void AfterExploreCallback(object sender, object args) {
        // TODO: this is the worst code in the entire project
        if (PlayerDataConnector.QuestCompleted != null) {
            Debug.Log("Making quest completed");
            var s = PlayerDataConnector.QuestCompleted;
            if (PlayerDataConnector.QuestReward != null) {
                s += "\n" + PlayerDataConnector.QuestReward.RewardDescription;
                PlayerDataConnector.QuestReward.GrantReward();
            }
            ui = UILibrary.ActivityText.Get(s);
            SoundEffectManager.Play(SoundEffectType.PositiveFeedback);
        }
        PlayerDataConnector.QuestCompleted = null;
        PlayerDataConnector.QuestReward = null;

        BeginExplore(null, null);
    }

    void HookEvents() {
        CrystallizeEventManager.PlayerState.ConfidenceChanged += HandleNoEnergy;
        CrystallizeEventManager.PlayerState.QuestFlagChanged += NPCManager.Instance.SpawnNPCDelegate;
    }

    void UnHookEvents() {
        CrystallizeEventManager.PlayerState.ConfidenceChanged -= HandleNoEnergy;
        CrystallizeEventManager.PlayerState.QuestFlagChanged -= NPCManager.Instance.SpawnNPCDelegate;
    }

    protected virtual void AfterInitialize() { BeginExplore(null, null); }
    protected virtual void AfterExit() { }

}
