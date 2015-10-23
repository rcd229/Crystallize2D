using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FreeExploreProcess_SpeakTutorial : BaseFreeExploreProcess {

    class GoHomeSubProcess : EnumeratorProcess<object, object> {
        ITemporaryUI speechBubbleBoxUI;

        public override IEnumerator<SubProcess> Run(object args) {
            UILibrary.ContextActionStatus.Get(new ContextActionArgs("Your inventory is full\nGo home to clear out some space", true, false));
            var goHomeButton = GameObject.FindGameObjectWithTag(TagLibrary.GoHomeButton);
            var helpUIArgs = new UITargetedMessageArgs(goHomeButton.GetComponent<RectTransform>(), "Click here");
            speechBubbleBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);
            PlayerDataConnector.SetTutorialViewed(FreeExploreProcessSelector.SpeakExplore);
            yield return Wait();
        }

        public override void ForceExit() {
            speechBubbleBoxUI.CloseIfNotNull();
        }
    }

    ITemporaryUI contextStatus;
    ITemporaryUI boxUI;
    int targetCount = 0;
    HashSet<Guid> talkedToNPCs = new HashSet<Guid>();
    SceneDoor[] exits;
    bool exited = false;
    bool questNPCTalkedTo = false;
    bool questTutorial = false;

    protected override void AfterInitialize() {
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Home, false);
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Confidence, true);
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Collect, true);

        QuestUtil.RaiseFlag(NPCQuestFlag.TutorialConversations);
        QuestUtil.SetQuestState(OverhearTutorialQuest.QuestID, OverhearTutorialQuest.States[2]);

        SceneData.SchoolHallwayFromClassroom.SetEnabled(false);
        //exits = GameObject.FindObjectsOfType<SceneDoor>();
        //foreach (var exit in exits) {
        //    //exit.gameObject.SetActive(false);
        //}

        CoroutineManager.Instance.WaitAndDo(GetTargets);

        base.AfterInitialize();
    }

    void GetTargets() {
        var sceneNPCs = GameObject.FindObjectsOfType<QuestNPC>();
        targetCount = sceneNPCs.Length;

        contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Talk to " + targetCount + " people to continue", true, false));
    }

    protected override void ConfidenceDepleted() {
        Debug.Log("Restoring confidence");
        PlayerDataConnector.AddConfidence(5);
    }

    protected override void ExploreCallback(object sender, ExploreResultArgs args) {
        //Debug.Log("Interacted with: " + args.Target + "; " +args.Target.transform.GetTransformPath());
        if (args.Target.GetComponent<QuestNPC>() && args.Target.GetComponent<QuestNPC>().NPC != null) {
            talkedToNPCs.Add(args.Target.GetComponent<QuestNPC>().NPC.ID.guid);

            if (args.Target.GetComponent<QuestNPC>().NPC.ID == new FindSakuraQuest().SeekerID) {
                questNPCTalkedTo = true;
            }
        }

        if (args.Target.GetComponent<SceneDoor>()) {
            exited = true;
        }

        contextStatus.CloseIfNotNull();

        base.ExploreCallback(sender, args);
    }

    protected override void AfterExploreCallback(object sender, object e) {
        if (talkedToNPCs.Count < targetCount) {
            contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Talk to " + (targetCount - talkedToNPCs.Count) + " people to continue", true, false));
        } else if (!exited) {
            //foreach (var exit in exits) {
            //    exit.gameObject.SetActive(true);
            //}
            SceneData.SchoolHallwayFromClassroom.SetEnabled(true);
            exits = new SceneDoor[0];
            contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Go into the hallway to continue", true, false));
        } else if (!questNPCTalkedTo) {
            contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Talk to the person to begin a quest", true, false));
        } else if (!questTutorial) {
            PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Home, true);
            PlayerDataConnector.SetHUDPartEnabled(HUDPartType.QuestStatus, true);
            var target = GameObject.FindObjectOfType<QuestHUD>().GetComponent<RectTransform>();
            boxUI = UILibrary.HighlightBox.Get(new UITargetedMessageArgs(target, "Quest status"));
            ProcessLibrary.MessageBox.Get("You have started your first quest.", AfterQuestTutorial, this);
            questTutorial = true;
            return;
        } else if (PlayerDataConnector.QuestCompleted != null) {
            SceneData.SchoolOutdoorFromHallway.SetEnabled(true);
            PlayerDataConnector.SetTutorialViewed(FreeExploreProcessSelector.SpeakExplore);
            CoroutineManager.Instance.WaitAndDo(SetLeaveContextStatus, new WaitForSeconds(5f));
        }
        base.AfterExploreCallback(sender, e);
    }

    void SetLeaveContextStatus() {
        contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Time to go back home", true, false));
    }

    void AfterQuestTutorial(object sender, object e) {
        ProcessLibrary.MessageBox.Get("View your progress on the right.", AfterQuestMessageBox, this);
    }

    void AfterQuestMessageBox(object sender, object e) {
        boxUI.CloseIfNotNull();
        BeginExplore(sender, e);
    }

}
