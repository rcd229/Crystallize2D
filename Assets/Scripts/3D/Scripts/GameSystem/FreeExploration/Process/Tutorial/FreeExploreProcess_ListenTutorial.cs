using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FreeExploreProcess_ListenTutorial : BaseFreeExploreProcess {

    class GoHomeSubProcess : EnumeratorProcess<object, object> {
        ITemporaryUI speechBubbleBoxUI;

        public override IEnumerator<SubProcess> Run(object args) {
            PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Home, true);
            UILibrary.ContextActionStatus.Get(new ContextActionArgs("Your inventory is full\nGo home to clear out some space", true, false));
            var goHomeButton = GameObject.FindGameObjectWithTag(TagLibrary.GoHomeButton);
            var helpUIArgs = new UITargetedMessageArgs(goHomeButton.GetComponent<RectTransform>(), "Click here");
            speechBubbleBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);
            PlayerDataConnector.SetTutorialViewed(FreeExploreProcessSelector.ListenExplore);
            yield return Wait();
        }

        public override void ForceExit() {
            speechBubbleBoxUI.CloseIfNotNull();
        }
    }

    ITemporaryUI contextStatus;

    protected override void AfterInitialize() {
        QuestUtil.UnlockQuest<OverhearTutorialQuest>();
        SceneData.SchoolOutdoorFromHallway.SetEnabled(false);

        foreach (HUDPartType part in Enum.GetValues(typeof(HUDPartType))) {
            PlayerDataConnector.SetHUDPartEnabled(part, false);
        }
        base.AfterInitialize();
    }

    protected override void ConfidenceDepleted() {
        Debug.Log("Restoring confidence");
        PlayerDataConnector.AddConfidence(5);
    }

    protected override void ExploreCallback(object sender, ExploreResultArgs arg) {
        contextStatus.CloseIfNotNull();
        if (arg.Target.GetComponent<SceneNPCGroup>()) {
            PlayerDataConnector.SetTutorialViewed(TagLibrary.Listen);
        }
        base.ExploreCallback(sender, arg);
    }

    protected override void AfterExploreCallback(object sender, object e) {
        //Debug.Log("Beginning tutorial explore");
        if (PlayerDataConnector.GetTutorialViewed(TagLibrary.Explore) && !PlayerDataConnector.GetTutorialViewed(TagLibrary.Listen)) {
            contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click the people to listen", true, false));
            PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Collect, true);
        } else if (PlayerDataConnector.GetTutorialViewed(TagLibrary.Listen) && !PlayerDataConnector.GetTutorialViewed(TagLibrary.ListenMore)) {
            var q = new QuestRef(new OverhearTutorialQuest().ID);
            if (q.PlayerDataInstance.State == OverhearTutorialQuest.States[0]) {
                BlackScreenUI.Instance.FadeOut(0.5f, HandleListenFinished);
            } else {
                HandleListenFinished(sender, e);
            }
            return;
        }
        BeginExplore(sender, e);
    }

    void HandleListenFinished(object sender, object args) {
        var q = new QuestRef(new OverhearTutorialQuest().ID);
        if (q.PlayerDataInstance.State == OverhearTutorialQuest.States[0]) {
            q.SetState(OverhearTutorialQuest.States[1]);
            BlackScreenUI.Instance.FadeIn(1f, null);
        }

        var remaining = 3 - PlayerData.Instance.Session.TodaysCollectedWords.Count;
        if (remaining <= 0) {
            new GoHomeSubProcess().Run(null, (s, e) => Exit(null), this);
        } else {
            contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Collect " + remaining + " more words to continue", true, false));
            BeginExplore(sender, args);
        }
    }

}
