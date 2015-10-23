using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class OrderedSelectorProcess : BaseTaskSelectorProcess<OrderedSelectorGameData> {

    /// <summary>
    /// Choose tasks in order until all tasks have been selected, then choose a random task
    /// </summary>
    /// <param name="job"></param>
    /// <param name="selector"></param>
    protected override JobTaskRef SelectTaskInner(IJobRef job, OrderedSelectorGameData selector) {
        var gd = job.GameDataInstance;
        var pd = job.PlayerDataInstance;
        int var = 0;
        if (pd.Repetitions < selector.Tasks.Count) {
            var = selector.Tasks[pd.Repetitions];
            var = Mathf.Clamp(var, 0, selector.Tasks.Count);
        } else {
            var = UnityEngine.Random.Range(0, selector.Tasks.Count);
        }

        return new JobTaskRef(job, gd.Tasks[var], 0, JobTaskType.Normal);
    }

    public OrderedSelectorProcess() { }

}
