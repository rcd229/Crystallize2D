using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DayProcess : TimeSessionProcess<DaySessionArgs, object>, IProcess<DaySessionArgs, object> {

    public static readonly ProcessFactoryRef<TaskSelectorArgs, JobTaskRef> RequestJobTask = new ProcessFactoryRef<TaskSelectorArgs, JobTaskRef>();
    public static readonly ProcessFactoryRef<JobTaskRef, JobTaskExitArgs> RequestJob = new ProcessFactoryRef<JobTaskRef, JobTaskExitArgs>();

    protected override string TimeName {
        get { return TagLibrary.Day; }
    }

    JobTaskRef task;
    DaySessionArgs args;
    ICloseable statusUI;
    CollectUI collectUI;

    bool loadPosition = false;
    bool loadToOrigin = false;

    public override void Initialize(DaySessionArgs input) {
        this.args = input;
        PlayerData.Instance.Session.BaseMoney = args.Job.GameDataInstance.Money;
        PlayerData.Instance.Session.Mistakes = 0;
        PlayerData.Instance.Session.MaxMistakes = args.Job.GameDataInstance.Tries;
        //PlayerData.Instance.Session.ReducedMoney = args.Job.GameDataInstance.Money;

        if (input.ForceSelection) {
            RequestJobTask.Set(typeof(UITaskSelectorProcess));
            RequestJobTask.Get(new TaskSelectorArgs(input.Job, null), SelectTaskCallback, this);
        } else {
            RequestJobTask.Set(input.Job.GameDataInstance.TaskSelector.SelectionProcess.ProcessType);
            RequestJobTask.Get(input.Job.GameDataInstance.TaskSelector.GetArgs(input.Job), SelectTaskCallback, this);
        }
    }

    protected override string SelectNextLevel(DaySessionArgs args) {
        if (task == null) {
            return args.LevelName;
        } else if (args.Job.GameDataInstance.ID == JobID.FreeExplore) {
            if (!PlayerDataConnector.GetTutorialViewed(FreeExploreProcessSelector.SpeakExplore)) {
                return SceneData.SchoolClassroomFromHallway.SceneID;
            } else if (PlayerDataConnector.GetTutorialViewed(FreeExploreProcessSelector.OpenExplore)
                && !PlayerData.Instance.Session.Area.IsEmptyOrNull()) {
                loadPosition = true;
                if (PlayerData.Instance.Session.Area == "HomeScene") {
                    loadPosition = false;
                    PlayerData.Instance.Session.Area = SceneData.SchoolOutdoorFromHallway.SceneID;
                }
                return PlayerData.Instance.Session.Area;
            } else {
                return SceneData.SchoolOutdoorFromHallway.SceneID;
            }
        } else {
            return task.Data.SceneName;
        }
    }

    protected override void AfterLoad() {
        if (task.IsPromotion) {
            MusicManager.FadeToMusic(MusicType.Promotion1);
        } else {
            MusicManager.FadeToMusic(MusicType.Day1);
        }

        if (loadPosition) {
            PlayerManager.Instance.PlayerGameObject.transform.position = PlayerData.Instance.Session.Position.ToVector3();
        }

        //var ui = UILibrary.SessionTimeText.Get(PlayerData.Instance.Time.GetFormattedString());

        var trans = GameObject.FindGameObjectWithTag("SceneTransitionAnimation");
        if (trans) {
            var t = trans.GetInterface<ITemporaryUI<string, object>>();
            if (t != null) {
                t.Initialize(PlayerData.Instance.Time.GetFormattedString());
                t.Complete += AfterTransitionComplete;
                return;
            }
        }

        //ui.Complete += AfterTransitionComplete;
        AfterTransitionComplete(null, null);
    }

    void AfterTransitionComplete(object sender, EventArgs<object> e) {
        //var skip = UILibrary.SkipSessionButton.Get(null);
        //skip.Complete += Skip_Complete;

        TaskState.Initialize(task.Job.GameDataInstance.Name);
        TaskState.Instance.SetState("Salary", "¥ " + task.Job.GameDataInstance.Money);

        var wrappedTask = task;
        if (task.IsPromotion) {
            wrappedTask = new JobTaskRef(task.Job, PromotionTaskData.Get(task.Data), PromotionTaskData.PromotionIndex, JobTaskType.Promotion);
        }
        RequestJob.Get(wrappedTask, JobCompleteCallback, this);

        //statusUI = UILibrary.GetStatusUI();// StatusUI.GetInstance();
        if (args.Job.PlayerDataInstance.ViewedTask(task.Index)) {
            //collectUI = CollectUI.GetInstance();
        } else {
            //collectUI = CollectUI.GetInstance(new CollectInitArgs(task.Data.TargetPhrases, false));//task.Data.TargetPhrases);
        }
    }

    void SelectTaskCallback(object sender, JobTaskRef task) {
        this.task = task;
        Run(args);
    }

    void Skip_Complete(object sender, EventArgs<object> e) {
        Exit();
    }

    void JobCompleteCallback(object sender, JobTaskExitArgs args) {
        if (args != null) {
            Debug.Log(args.IsFailed);
        }
        if ((task.Variation == PromotionTaskData.PromotionVariation && task.IsPromotionFailed)
            || (args != null && args.IsFailed)) {
            MoneyResultCompleteCallback(null, null);
            return;
        }
        var ui = UILibrary.ActivityText.Get("Task complete!");
        ui.Complete += TaskCompleteText;
    }

    void TaskCompleteText(object obj, EventArgs<object> e) {
        PlayerDataConnector.AddDayToJob(this.args.Job);
        var earned = PlayerData.Instance.Session.GetEarnedMoney(task.Job);
        PlayerDataConnector.AddMoney(earned.GetValue());
        Debug.Log("Calling earn money");
        var ui = UILibrary.EarnMoney.Get(earned);
        ui.Complete += MoneyResultCompleteCallback;
    }

    void MoneyResultCompleteCallback(object sender, EventArgs<object> args) {
        statusUI.CloseIfNotNull();
        collectUI.CloseIfNotNull();

        Exit();
    }

    public override void ForceExit() {
        statusUI.CloseIfNotNull();
        collectUI.CloseIfNotNull();
    }

}
