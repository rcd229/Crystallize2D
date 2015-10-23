using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobProcessSelector : IProcess<JobTaskRef, JobTaskExitArgs> {

    ProcessFactoryRef<JobTaskRef, JobTaskExitArgs> TaskFactory = new ProcessFactoryRef<JobTaskRef, JobTaskExitArgs>();

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(JobTaskRef param1) {
        TaskFactory.Set(param1.Data.ProcessType.ProcessType);
        TaskFactory.Get(param1, ChildCallback, this);
    }

    void ChildCallback(object sender, JobTaskExitArgs args) {
        Exit(args);
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(JobTaskExitArgs args) {
        OnExit.Raise(this, args);
    }

}
