using UnityEngine;
using System.Collections;
using System;

public class PromotionTaskProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string MistakeStatus = "Chances";
    ProcessFactoryRef<JobTaskRef, JobTaskExitArgs> ChildTaskFactory = new ProcessFactoryRef<JobTaskRef, JobTaskExitArgs>();
    IJobRef jobref;
    JobGameData job;
    PromotionTaskData taskData;
    JobTaskGameData childTask;
    IProcess innerProcess;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(JobTaskRef param1) {
        //BlackScreenUI.GetInstance().SetAlpha(1f);
        jobref = param1.Job;
        job = jobref.GameDataInstance;
        taskData = (PromotionTaskData)(param1.Data);
        childTask = taskData.child;
        TaskState.Instance.SetState(MistakeStatus, string.Format("{0}/{1}", job.PromotionMistakes, job.PromotionMistakes));
        TaskState.Instance.OtherStateChanged += HandleStateChanged;
        var ui = UILibrary.ActivityText.Get("Promotion Time!");

        var targ = param1.GetHomePosition();
        if (targ != default(Vector3)) {
            PlayerManager.Instance.PlayerGameObject.transform.position = targ;
        }

        ui.Complete += HandleActivityTextComplete;
    }

    void HandleStateChanged(object sender, EventArgs<object> e) {
        //Debug.Log("changes - 1");
        var mistakes = Math.Max(0, job.PromotionMistakes - PlayerData.Instance.Session.Mistakes);
        TaskState.Instance.SetStateWithoutRaise(MistakeStatus, string.Format("{0}/{1}", mistakes, job.PromotionMistakes));

        if (mistakes == 0) {
            innerProcess.TryForceExit();
            ChildrenFinished(null, null);
        }
    }

    void HandleActivityTextComplete(object sender, EventArgs<object> e) {
        UILibrary.MessageBox.Get("You can only make " + job.PromotionMistakes + " mistakes", DoInnerTask, this);
    }

    void DoInnerTask(object e, object o) {
        BlackScreenUI.Instance.FadeIn(1f, null);
        PlayerData.Instance.Session.isPromotion = true;
        ChildTaskFactory.Set(childTask.ProcessType.ProcessType);
        innerProcess = ChildTaskFactory.Get(new JobTaskRef(jobref, childTask, PromotionTaskData.PromotionVariation, JobTaskType.Promotion), ChildrenFinished, this);
    }

    void ChildrenFinished(object o, object e) {
        if (PlayerData.Instance.Session.Mistakes < job.PromotionMistakes) {
            var ui = UILibrary.ActivityText.Get("You have been promoted!");
            PlayerDataConnector.PromoteJob(jobref);
            DataLogger.LogTimestampedData("Promotion", job.ID.ToString(), job.Name);
            ui.Complete += PromotionDone;
        } else {
            DataLogger.LogTimestampedData("PromotionFailed", job.ID.ToString(), job.Name);
            var ui = UILibrary.PromotionFailedText.Get("Promotion Failed...");
            ui.Complete += PromotionDone;
        }
    }

    void PromotionDone(object o, object e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        TaskState.Instance.OtherStateChanged -= HandleStateChanged;
        OnExit.Raise(this, null);
    }


}
