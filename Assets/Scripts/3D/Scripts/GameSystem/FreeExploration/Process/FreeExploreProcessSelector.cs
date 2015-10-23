using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FreeExploreProcessSelector : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string ListenExplore = "ListenExplore";
    public const string ReviewExplore = "ReviewExplore";
    public const string SpeakExplore = "SpeakExplore";
    public const string ReviewExplore2 = "ReviewExplore2";
    public const string OpenExplore = "OpenExplore";

    public static readonly string[] Tutorials = new string[] { ListenExplore, ReviewExplore, SpeakExplore, ReviewExplore2, OpenExplore };

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(JobTaskRef param1) {
        if (!PlayerDataConnector.GetTutorialViewed(ListenExplore)) {
            Run<FreeExploreProcess_ListenTutorial>(param1, "");
        } else if (!PlayerDataConnector.GetTutorialViewed(ReviewExplore)) {
            Run<FreeExploreProcess_ReviewTutorial>(param1, "");
        } else if (!PlayerDataConnector.GetTutorialViewed(SpeakExplore)) {
            Run<FreeExploreProcess_SpeakTutorial>(param1, "");
        } else if (!PlayerDataConnector.GetTutorialViewed(ReviewExplore2)) {
            Run<FreeExploreProcess>(param1, ReviewExplore2);
        } else if (!PlayerDataConnector.GetTutorialViewed(OpenExplore)) {
            Run<FreeExploreProcess_OpenTutorial>(param1, OpenExplore);
        } else {
            SceneData.SchoolOutdoorFromHallway.SetEnabled(true);
            Run<FreeExploreProcess>(param1);
        }
    }

    public void ForceExit() { }

    void Run<T>(JobTaskRef job, string tag = "") where T : IProcess<JobTaskRef, JobTaskExitArgs>, new() {
        if (!tag.IsEmptyOrNull()) {
            PlayerDataConnector.SetTutorialViewed(tag);
        }
        new T().Run(job, Exit, this);
    }

    void Exit(object sender, JobTaskExitArgs args) {
        OnExit.Raise(this, args);
    }

}
