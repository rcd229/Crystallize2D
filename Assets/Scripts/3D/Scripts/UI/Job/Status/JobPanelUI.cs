using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobPanelUI : UIPanel, ITemporaryUI<JobPanelArgs, DaySessionArgs>, IDebugMethods {

    const string ResourcePath = "UI/JobPanel";
    public static JobPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<JobPanelUI>(ResourcePath);
    }

    public GameObject jobEntryPrefab;
    public RectTransform jobEntryParent;

    public event EventHandler<EventArgs<DaySessionArgs>> Complete;

    bool interactive = true;
    bool chooseTask = false;
    List<GameObject> instances = new List<GameObject>();

    public void Initialize(JobPanelArgs param1) {
        if (param1 != null) {
            interactive = param1.Interactive;
        }

        IEnumerable<IJobRef> jobs = (from j in GameData.Instance.Jobs.Items
                                     where new IDJobRef(j.ID).PlayerDataInstance.Shown
                                     orderby j.Ordering, j.Difficulty
                                     select new IDJobRef(j.ID) as IJobRef);
        //Debug.Log(jobs.Count());
        UIUtil.GenerateChildren<IJobRef>(jobs, instances, jobEntryParent, CreateChild);
    }

    GameObject CreateChild(IJobRef job) {
        var instance = Instantiate<GameObject>(jobEntryPrefab);
        instance.GetInterface<IInitializable<IJobRef>>().Initialize(job);
        var b = instance.GetComponentInChildren<UIButton>();
        if (interactive) {
            b.OnClicked += JobPanelUI_OnClicked;
        }
        b.gameObject.AddComponent<DataContainer>().Store(job);
        return instance;
    }

    void JobPanelUI_OnClicked(object sender, EventArgs e) {
        var job = ((Component)sender).gameObject.GetComponent<DataContainer>().Retrieve<IJobRef>();
        if (job.PlayerDataInstance.Unlocked) {
            var args = new DaySessionArgs("Start", job, chooseTask);

            Exit(args);
        } else {
            UILibrary.MessageBox.Get(job.GameDataInstance.HelpText);
        }
    }

    public override void Close() {
        var del = Complete;
        Complete = null;
        del.Raise(this, null);
        base.Close();
    }

    void Exit(DaySessionArgs args) {
        Debug.Log("chose " + args.Job.GameDataInstance.Name);
        var del = Complete;
        Complete = null;
        del.Raise(this, new EventArgs<DaySessionArgs>(args));
    }

    //public void SetChooseTask(bool chooseTask) {
    //    this.chooseTask = chooseTask;
    //}

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(UnlockAllJobs, ChooseTask);
    }

    public string UnlockAllJobs(string s) {
        var jobs = (from j in GameData.Instance.Jobs.Items
                    select new IDJobRef(j.ID));
        foreach (var j in jobs) {
            PlayerDataConnector.RevealJob(j);
            PlayerDataConnector.UnlockJob(j);
        }
        Initialize(null);
        return "Unlocked all jobs.";
    }

    public string ChooseTask(string s) {
        chooseTask = true;
        return "Will choose task";
    }
    #endregion
}