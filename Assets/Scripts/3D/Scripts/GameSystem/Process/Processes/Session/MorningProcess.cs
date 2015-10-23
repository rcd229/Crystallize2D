using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MorningProcess : TimeSessionProcess<MorningSessionArgs, DaySessionArgs>, IProcess<MorningSessionArgs, DaySessionArgs>, IDebugMethods {

    public static readonly ProcessFactoryRef<object, DaySessionArgs> RequestPlanSelection = new ProcessFactoryRef<object, DaySessionArgs>();

    protected override string TimeName {
        get { return TagLibrary.Morning; }
    }

    MorningSessionArgs args;

    protected override void BeforeInitialize(MorningSessionArgs input) {
        MusicManager.FadeToMusic(MusicType.Morning1);
        args = input;
    }

    protected override void AfterLoad() {
        //UILibrary.GetStatusUI();

        if (BedSelector.Instance) {
            BedSelector.Instance.SetBed(args.Home.ID);
        }
        if (SunInstance.Instance) {
            SunInstance.Instance.SetMorning();
        }

        string s = "";
        //var quality = args.Home.GameDataInstance.Quality;
        //var session = new SessionPlayerData();
        //session.RestQuality = quality;
        //PlayerData.Instance.Session = new SessionPlayerData();
        //PlayerData.Instance.Session.Confidence = PlayerData.Instance.Proficiency.Confidence;
        var comfort = PlayerDataConnector.GetComfort();
        if (comfort < 3) {
            s = "You didn't sleep so well... You'll probably have a hard time working today.";
        } else if (comfort < 6) {
            s = "You slept alright. You feel ready for work.";
        } else {
            s = "You slept great! You should be able to work hard today.";
        }

        if (PlayerData.Instance.PersonalData.TotalPlayTime > new TimeSpan(0, 15, 0) && PlayerData.Instance.PersonalData.SurveysRequested < 2) {
            UILibrary.SurveyPrompt.Get(null);
        } else if (PlayerData.Instance.PersonalData.TotalPlayTime > new TimeSpan(1, 0, 0) && PlayerData.Instance.PersonalData.SurveysRequested < 3) {
            UILibrary.SurveyPrompt.Get(null);
        } else if (PlayerData.Instance.PersonalData.TotalPlayTime > new TimeSpan(2, 0, 0) && PlayerData.Instance.PersonalData.SurveysRequested < 4) {
            UILibrary.SurveyPrompt.Get(null);
        }

        //ProcessLibrary.MessageBox.Get(s, MessageBoxCallback, this);
        DoExplore();
    }

    void MessageBoxCallback(object sender, object args) {
        //if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.Doors)) {
        //    ProcessLibrary.MessageBox.Get("You can click on the doors to interact.", TutorialMessageBoxCallback, this);
        //    PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.Doors);
        //} else {
        DoExplore();
        //}
    }

    void TutorialMessageBoxCallback(object sender, object args) {
        DoExplore();
    }

    void DoExplore() {
        new FreeExploreProcessSelector().Run(null, ExploreExit, this);
        //ProcessLibrary.Explore.Get(new ExploreInitArgs(), ExploreCallback, this);
    }

    //void ExploreCallback(object sender, ExploreResultArgs e) {
    //    var interactable = e.Target.GetInterface<IInteractableSceneObject>();
    //    if (interactable is HomeDoor) {
    //        var jr = new IDJobRef(JobID.FreeExplore);
    //        PlanSelectionCallback(null, new DaySessionArgs("", jr));
    //        return;
    //    } 

    //    if (interactable != null) {
    //        interactable.BeginInteraction(TutorialMessageBoxCallback, this);
    //        return;
    //    }

    //    DoExplore();
    //}

    //void InteractionComplete(object sender, EventArgs<object> e) {
    //    DoExplore();
    //}

    void ExploreExit(object sender, object args) {
        var jr = new IDJobRef(JobID.FreeExplore);
        PlanSelectionCallback(null, new DaySessionArgs("", jr));
    }

    void PlanSelectionCallback(object sender, DaySessionArgs args) {
        if (args == null) {
            DoExplore();
        } else {
            DataLogger.LogTimestampedData("JobSelected", args.Job.GameDataInstance.Name);
            Exit(args);
        }
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(GoToGabeExplore, GoToShiyuExplore);
    }

    public string GoToGabeExplore(string s) {
        var jr = new IDJobRef(JobID.TestExplorer);
        PlanSelectionCallback(null, new DaySessionArgs("", jr));
        return "changing levels";
    }

    public string GoToShiyuExplore(string s) {
        var jr = new IDJobRef(JobID.Explorer);
        PlanSelectionCallback(null, new DaySessionArgs("", jr));
        return "changing levels";
    }
    #endregion
}
