using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FreeExploreProcess_ReviewTutorial : BaseFreeExploreProcess {

    class GoHomeSubProcess : EnumeratorProcess<object, object> {
        ITemporaryUI speechBubbleBoxUI;

        public override IEnumerator<SubProcess> Run(object args) {
            UILibrary.ContextActionStatus.Get(new ContextActionArgs("Your inventory is full\nGo home to clear out some space", true, false));
            var goHomeButton = GameObject.FindGameObjectWithTag(TagLibrary.GoHomeButton);
            var helpUIArgs = new UITargetedMessageArgs(goHomeButton.GetComponent<RectTransform>(), "Click here");
            speechBubbleBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);
            yield return Wait();
        }

        public override void ForceExit() {
            speechBubbleBoxUI.CloseIfNotNull();
        }
    }

    ITemporaryUI contextStatus;
    IEnumerable<HomeDoor> doors;

    protected override void AfterInitialize() {
        foreach (HUDPartType part in Enum.GetValues(typeof(HUDPartType))) {
            PlayerDataConnector.SetHUDPartActive(part, false);
        }

        var dressers = GameObject.FindObjectsOfType<SceneDresser>();
        foreach (var d in dressers) {
            d.enabled = false;
        }

        doors = GameObject.FindObjectsOfType<HomeDoor>();
        foreach (var d in doors) {
            d.enabled = false;
        }

        UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click the desk to review your words", true, false));

        base.AfterInitialize();
    }

    protected override void ConfidenceDepleted() {
        Debug.Log("Restoring confidence");
        PlayerDataConnector.AddConfidence(5);
    }

    protected override void ExploreCallback(object sender, ExploreResultArgs arg) {
        contextStatus.CloseIfNotNull();
        base.ExploreCallback(sender, arg);
    }

    protected override void AfterExploreCallback(object sender, object args) {
        if (PlayerDataConnector.CollectedWordCount > 0) {
            UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click the desk to review your words", true, false));
        } else {
            foreach (var d in doors) {
                d.enabled = true;
            }
            PlayerDataConnector.SetTutorialViewed(FreeExploreProcessSelector.ReviewExplore);
            UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click the exit to continue", true, false));
        }
        base.AfterExploreCallback(sender, args);
    }

}
