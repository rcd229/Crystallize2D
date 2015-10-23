using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class JobSelectionTutorialProcess : IProcess<object, DaySessionArgs> {

    ITemporaryUI<JobPanelArgs, DaySessionArgs> ui;
    ITemporaryUI box;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(object param1) {
        PlayerDataConnector.UpdateShownJobs(new string[]{"Tourist 1"});//, "Greeter"});
        MessageBoxExit2(null, null);
    }

    public void ForceExit() {
        Exit(new DaySessionArgs("", new IDJobRef(JobID.Tourist1)));
    }

    void ShowRequirement() {
        ui = UILibrary.Jobs.Get(new JobPanelArgs(false));
        ProcessLibrary.MessageBox.Get("You'll need to learn some words before you can get a job.", MessageBoxExit, this);
    }

    void MessageBoxExit(object sender, object args) {
        var j = GetJob("Greeter");
        if (j) {
            var wp = j.transform.FindChild("WordParent");
            box = UILibrary.HighlightBox.Get(new UITargetedMessageArgs(wp.GetComponent<RectTransform>(), "You will need to learn this"));
            ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.LeftClick), BoxExit, this);
        } else {
            Debug.Log("Greeter not found.");
            ForceExit();
        }
    }

    void BoxExit(object sender, object args) {
        box.Close();
        ProcessLibrary.MessageBox.Get("Do activities to find new words.", MessageBoxExit2, this);
    }

    void MessageBoxExit2(object sender, object args) {
        ui.CloseIfNotNull();
        ui = UILibrary.Jobs.Get(new JobPanelArgs(true));

        var j = GetJob("Tourist 1");
        if (j) {
            ui.Complete += JobChosen;

            CoroutineManager.Instance.WaitAndDo(
                () => {
                    var j2 = GetJob("Tourist 1");
                    var wp = j2.transform.FindChild("JobButton");
                    box = UILibrary.HighlightBox.Get(new UITargetedMessageArgs(wp.GetComponent<RectTransform>(), "Click to do this!"));
                }
                );
        } else {
            Debug.Log("Tourist not found.");
            ForceExit();
        }
    }

    void BoxExit2(object sender, object args) {
        //box.Close();
        //Exit();
    }

    void JobChosen(object sender, EventArgs<DaySessionArgs> args) {
        Debug.Log("args is" + args);
        if (args == null) {
            Exit(null);
        } else {
            Exit(args.Data);
        }
    }

    void Exit(DaySessionArgs args) {
        ui.CloseIfNotNull();
        box.CloseIfNotNull();
        OnExit.Raise(this, args);
    }

    JobPanelEntryUI GetJob(string jobname) {
        var jobs = GameObject.FindGameObjectsWithTag("JobEntry").Select((g) => g.GetComponent<JobPanelEntryUI>());
        foreach (var j in jobs) {
            if (j.Job.GameDataInstance.Name == jobname) {
                return j;
            }
        }
        return null;
    }

}
