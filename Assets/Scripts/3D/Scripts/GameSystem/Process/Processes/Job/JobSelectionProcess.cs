using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobSelectionProcess : IProcess<object, DaySessionArgs> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<JobPanelArgs, DaySessionArgs> panel;

    public void Initialize(object data) {
        PlayerDataConnector.UpdateShownJobs();
        panel = UILibrary.Jobs.Get(new JobPanelArgs(true));
        panel.Complete += HandleItemSelected;
    }

    public void ForceExit() {
        Exit(null);
    }

    void HandleItemSelected(object sender, EventArgs<DaySessionArgs> e) {
        if (e != null) {
            Exit(e.Data);
        } else {
            Exit(null);
        }
    }

    void Exit(DaySessionArgs args) {
        panel.Close();
        OnExit.Raise(this, args);
    }

}
