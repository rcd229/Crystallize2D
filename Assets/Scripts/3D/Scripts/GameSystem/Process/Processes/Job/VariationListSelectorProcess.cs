using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class VariationListSelectorProcess : BaseTaskSelectorProcess<VariationListSelectorGameData> {

    /// <summary>
    /// Choose tasks in order until all tasks have been selected, then choose a random task
    /// </summary>
    /// <param name="job"></param>
    /// <param name="selector"></param>
	protected override JobTaskRef SelectTaskInner(IJobRef job, VariationListSelectorGameData selector) {
        var gamedata = job.GameDataInstance;
        var playdata = job.PlayerDataInstance;
        int variation = playdata.Days / selector.RepetitionRequirement;


		// /variation + 1/ represents the length of the task. /variation + 2/ is the range of the words in the list
        return new JobTaskRef(job, gamedata.Tasks[0], variation, JobTaskType.Normal);
    }

	public VariationListSelectorProcess() { }

}
