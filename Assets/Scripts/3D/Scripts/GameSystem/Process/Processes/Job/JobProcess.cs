using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobProcess : IProcess<DaySessionArgs, object> {

    static readonly ProcessFactoryRef<TaskSelectorArgs, JobTaskRef> RequestJobTask = new ProcessFactoryRef<TaskSelectorArgs, JobTaskRef>();

    public ProcessExitCallback OnExit { get; set; }

    DaySessionArgs args;
    JobTaskRef task;

    public void Initialize(DaySessionArgs param1) {
        args = param1;

        PlayerData.Instance.Session.BaseMoney = args.Job.GameDataInstance.Money;
        PlayerData.Instance.Session.Mistakes = 0;
        PlayerData.Instance.Session.MaxMistakes = args.Job.GameDataInstance.Tries;
        //PlayerData.Instance.Session.ReducedMoney = args.Job.GameDataInstance.Money;

        if (args.ForceSelection) {
            RequestJobTask.Set(typeof(UITaskSelectorProcess));
            RequestJobTask.Get(new TaskSelectorArgs(args.Job, null), SelectTaskCallback, this);
        } else {
            RequestJobTask.Set(args.Job.GameDataInstance.TaskSelector.SelectionProcess.ProcessType);
            RequestJobTask.Get(args.Job.GameDataInstance.TaskSelector.GetArgs(args.Job), SelectTaskCallback, this);
        }
    }

    public void ForceExit() {
       
    }

    void SelectTaskCallback(object sender, JobTaskRef task) {
        this.task = task;

        if (task.IsPromotion) {
            MusicManager.FadeToMusic(MusicType.Promotion1);
        } else {
            MusicManager.FadeToMusic(MusicType.Day1);
        }

        TaskState.Initialize(task.Job.GameDataInstance.Name);
        TaskState.Instance.SetState("Salary", "¥ " + task.Job.GameDataInstance.Money);

        var wrappedTask = task;
        if (task.IsPromotion) {
            wrappedTask = new JobTaskRef(task.Job, PromotionTaskData.Get(task.Data), PromotionTaskData.PromotionIndex, JobTaskType.Promotion);
        }
        DayProcess.RequestJob.Get(wrappedTask, JobCompleteCallback, this);
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
        MusicManager.FadeToMusic(MusicType.Day1);

        PlayerDataConnector.AddDayToJob(this.args.Job);
        var earned = PlayerData.Instance.Session.GetEarnedMoney(task.Job);
        PlayerDataConnector.AddMoney(earned.GetValue());
        Debug.Log("Calling earn money");
        var ui = UILibrary.EarnMoney.Get(earned);
        ui.Complete += MoneyResultCompleteCallback;
    }

    void MoneyResultCompleteCallback(object sender, EventArgs<object> args) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
