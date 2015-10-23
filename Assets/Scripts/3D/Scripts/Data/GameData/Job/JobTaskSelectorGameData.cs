using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobTaskSelectorGameData {

    public ProcessTypeRef SelectionProcess { get; set; }

    public JobTaskSelectorGameData(){
        SelectionProcess = new ProcessTypeRef(typeof(DefaultTaskSelectionProcess));
    }

    public JobTaskSelectorGameData(Type process) {
        SelectionProcess = new ProcessTypeRef(process);
    }

    public TaskSelectorArgs GetArgs(IJobRef job) {
        return new TaskSelectorArgs(job, this);
    }

}
