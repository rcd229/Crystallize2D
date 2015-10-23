using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestConversationProcess : IProcess<QuestArgs, object> {

    //TODO put this in serialized data
    public static PhraseSequence Quit = new PhraseSequence("<i>Leave</i>");

    QuestNPCItemData myNPC;
    GameObject target;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(QuestArgs arg) {
        myNPC = arg.NPC;
        target = arg.NPCTarget;

        var dialogue = QuestUtil.GetIntroductionDialogue(myNPC);
        ProcessLibrary.BeginConversation.Get(new ConversationArgs(target, dialogue, null, true), HandleGreetingExit, this);
    }

    void HandleGreetingExit(object sender, object arg) {
        var choices = QuestUtil.GetChoices(myNPC);
        Debug.Log("choices : " + choices.Count + " " + myNPC.Name); 
        if (choices.Count == 0) {
            //		if(false){
            PlayExitDialogue();
        } else {
            choices.Add(Quit);
            var selectorArgs = new PhraseSelectorInitArgs(choices);
            selectorArgs.CheckLearned = true;
            selectorArgs.UseTranslation = false;
            ProcessLibrary.PhraseSelectionProcess.Get(selectorArgs, ChoiceSelected, this);
        }
    }

    void ChoiceSelected(object sender, PhraseSequence selection) {
        //since it is prototype, compare string directly
        //		if(PhraseSequence.PhrasesEquivalent(selection.Data, Quit)){
        if (FreeExploreProcess.isPhraseEnglish && selection.GetText() == Quit.GetText()) {
            PlayExitDialogue();
        } else if (!FreeExploreProcess.isPhraseEnglish && PhraseSequence.PhrasesEquivalent(selection, Quit)) {
            PlayExitDialogue();
        } else {
            //continue dialogue does the job of handling branching in the underlying structure
            var dialogue = QuestUtil.GetResponse(myNPC, selection);
            if (dialogue == null) {
                PlayerDataConnector.AddConfidence(-1);
                UILibrary.NegativeFeedback.Get(".", HandleNegativeFeedback, this);
            } else {
                var context = QuestUtil.GetResponseContext(myNPC, selection);
                if (selection.ComparableElementCount > 0) {
                    var p = selection.InsertContext(PlayerData.Instance.PersonalData.Context);
                    PlayerDataConnector.CollectItem(p, false);
                    var rev = PlayerData.Instance.Reviews.GetOrCreateReview(p);
                    if (rev.NeedsReview()) {
                        rev.AddEntry(1);
                    }

                    if (PlayerDataConnector.IsConstructed(selection)) {
                        UILibrary.PositiveFeedback.Get("", (s, e) => HandlePositiveFeedback(dialogue, context), this);
                    } else {
                        HandlePositiveFeedback(dialogue, context);
                    }
                } else {
                    ProcessLibrary.ConversationSegment.Get(new ConversationArgs(target, dialogue, context), HandleGreetingExit, this);
                }
            }
        }
    }

    void HandlePositiveFeedback(DialogueSequence dialogue, ContextData context) {
        ProcessLibrary.ConversationSegment.Get(new ConversationArgs(target, dialogue, context), HandleGreetingExit, this);
    }

    void HandleNegativeFeedback(object sender, object args) {
        HandleGreetingExit(null, null);
    }

    void PlayExitDialogue() {
        Debug.Log("playing exit dialogue");
        if (myNPC.ExitDialogue != null) {
            ProcessLibrary.ConversationSegment.Get(new ConversationArgs(target, myNPC.ExitDialogue), EndDialogue, this);
        } else {
            ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(target, myNPC.EntryDialogue), EndConversation, this);
        }
    }

    void EndDialogue(object sender, object arg) {
        ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(target, myNPC.ExitDialogue), EndConversation, this);
    }

    void EndConversation(object sender, object arg) {
        Debug.Log("Conversation ended");
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
