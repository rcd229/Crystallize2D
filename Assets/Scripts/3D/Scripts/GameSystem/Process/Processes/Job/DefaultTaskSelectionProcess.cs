using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class BaseTaskSelectorProcess<T> : IProcess<TaskSelectorArgs, JobTaskRef> where T : JobTaskSelectorGameData {

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(TaskSelectorArgs param1) {
        Exit(SelectTask(param1.Job, (T)param1.Selector));
    }

    protected abstract JobTaskRef SelectTaskInner(IJobRef job, T selector);

    public void ForceExit() {
        Exit(null);
    }

    protected void Exit(JobTaskRef task) {
        OnExit.Raise(this, task);
    }

	bool CanPromote(IJobRef job){
		return job.PlayerDataInstance.ViewedAllTasks(job) && 
				job.PlayerDataInstance.Days >= job.GameDataInstance.PromotionReq && 
				!job.PlayerDataInstance.Promoted;
	}

    public JobTaskRef SelectTask(IJobRef job, T Selector) {
        var original = SelectTaskInner(job, Selector);
        if (CanPromote(job) && PromotionTaskData.UsePromotion) {
            return new JobTaskRef(job, job.GameDataInstance.PromotionTask, PromotionTaskData.PromotionVariation, JobTaskType.Promotion);
        } else {
            return original;
        }
    }
}

public class DefaultTaskSelectionProcess : BaseTaskSelectorProcess<JobTaskSelectorGameData> {

    protected override JobTaskRef SelectTaskInner(IJobRef job, JobTaskSelectorGameData selector) {
        return new JobTaskRef(job, job.GameDataInstance.Tasks[0], 0, JobTaskType.Normal);
    }

}
