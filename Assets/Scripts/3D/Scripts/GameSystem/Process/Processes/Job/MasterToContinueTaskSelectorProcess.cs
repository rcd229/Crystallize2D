using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MasterToContinueTaskSelectorProcess : BaseTaskSelectorProcess<MasterToContinueTaskSelectorGameData> {

    protected override JobTaskRef SelectTaskInner(IJobRef job, MasterToContinueTaskSelectorGameData selector) {
        int i = 0;
        foreach (var task in job.GameDataInstance.Tasks) {
            if (!job.PlayerDataInstance.ViewedTask(i)) {
                return new JobTaskRef(job, task);
            }
            
            if (task.TargetPhrases.Count > 0) {
                foreach (var tp in task.TargetPhrases) {
                    if (!PlayerDataConnector.ContainsLearnedItem(tp)) {
                        //if (UnityEngine.Random.value < 0.5f) {
                        //    var rand = UnityEngine.Random.Range(0, i);
                        //    return new JobTaskRef(job, )
                        //} else {
                        return new JobTaskRef(job, task);
                        //}
                    }
                }
            }
            i++;
        }
        return new JobTaskRef(job, job.GameDataInstance.Tasks[job.GameDataInstance.Tasks.Count - 1]);
    }

}
