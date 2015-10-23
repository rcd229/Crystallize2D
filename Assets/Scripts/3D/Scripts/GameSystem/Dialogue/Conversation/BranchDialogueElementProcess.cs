using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BranchDialogueElementProcess : IProcess<DialogueState, DialogueState>, IDebugMethods {

    DialogueState state;
    DialogueState nextState;
    BranchDialogueElement branchElement;
    PhraseSelectorInitArgs selectorArgs;

    public ProcessExitCallback OnExit { get; set; }
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public void Initialize(DialogueState data) {
        this.state = data;

        branchElement = (BranchDialogueElement)data.GetElement();
        var phrases = branchElement.Branches.Select((b) => b.Prompt).ToList();
        if (branchElement.IncludeDefaultBranch) {
            phrases.Add(new PhraseSequence("?"));
        }

        Debug.Log("beginning branch: " + branchElement.DisplayTranslation + "; " + branchElement.CheckAvailable);
        for (int i = 0; i < phrases.Count; i++) {
            if (phrases[i] == null) {
                phrases[i] = new PhraseSequence("NULL");
            }
        }

        selectorArgs = new PhraseSelectorInitArgs(phrases);
        selectorArgs.CheckLearned = branchElement.CheckAvailable;
        selectorArgs.UseTranslation = branchElement.DisplayTranslation;
        ProcessLibrary.PhraseSelectionProcess.Get(selectorArgs, HandlePhrasePanelExit, this);
        //UILibrary.PhraseSelector.Get(selectorArgs, ui_Complete, this);
    }

    //void ui_Complete(object sender, EventArgs<PhraseSequence> e) {
    //    //Debug.Log("Handling " + e.Data.GetText());
    //    if (e.Data.GetText().Trim() == "?") {
    //        HandlePhrasePanelExit(sender, e.Data);
    //    } else if (branchElement.DisplayTranslation || e.Data.ComparableElementCount == 0) {
    //        HandlePhrasePanelExit(sender, e.Data);
    //    } else if (!PlayerDataConnector.CanSelectPhrase(e.Data)) {
    //        var needed = PlayerDataConnector.GetNeededWords(e.Data);
    //        foreach (var nw in needed) {
    //            Debug.Log("n");
    //        }
    //        UILibrary.NeededWords.Get(needed, neg_Complete, this);
    //    } else if (PlayerDataConnector.NeedToConstructPhrase(e.Data)) {
    //        UILibrary.PhraseConstructor.Get(new PhraseConstructorArgs(e.Data, PlayerDataConnector.GetConstructorWordsForPhrase(e.Data)), ConstructorExit, this);
    //    } else {
    //        HandlePhrasePanelExit(sender, e.Data);
    //    }
    //}

    //void ConstructorExit(object sender, EventArgs<List<PhraseSequence>> args) {
    //    HandlePhrasePanelExit(sender, args.Data.Flatten());
    //}

    void HandlePhrasePanelExit(object sender, PhraseSequence args) {
        //Debug.Log("[" + args.GetText() + "] is default " + (args.GetText().Trim() == "?"));
        if (args.GetText().Trim() == "?") {
            PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(new PhraseSequence("..."));
            nextState = new DialogueState(branchElement.DefaultNextID, state.Dialogue, state.Context, state.ActorMap, state.GameObjects);
            Exit(nextState);//new DialogueState(DialogueSequence.ConfusedExit, state.Dialogue, state.Context, state.ActorMap));
            return;
        }

        var promptElement = (BranchDialogueElement)state.GetElement();
        foreach (var link in promptElement.Branches) {
            bool textMatch = link.Prompt.ComparableElementCount == 0 && link.Prompt.GetText() == args.GetText();
            bool phraseMatch = link.Prompt.ComparableElementCount > 0 && args.FulfillsTemplate(link.Prompt);
            Debug.Log("args: " + args.GetText() + " prompt: " + link.Prompt.GetText() + "; phrasematch " + phraseMatch);
            if (textMatch || phraseMatch) {
                PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args);
                nextState = new DialogueState(link.NextID, state.Dialogue, state.Context, state.ActorMap, state.GameObjects);
                if (link.Prompt.ComparableElementCount > 0) {
                    var p = link.Prompt.InsertContext(PlayerData.Instance.PersonalData.Context);
                    PlayerDataConnector.CollectItem(p, false);
                    var rev = PlayerData.Instance.Reviews.GetOrCreateReview(p);
                    if (rev.NeedsReview()) {
                        rev.AddEntry(1);
                    }

                    if (PlayerDataConnector.IsConstructed(link.Prompt)) {
                        UILibrary.PositiveFeedback.Get("", pos_Complete, this);
                    } else {
                        pos_Complete(null, null);
                    }
                } else {
                    Exit(nextState);
                }
                return;
            }
        }

        PlayerDataConnector.AddConfidence(-1);
        UILibrary.NegativeFeedback.Get(".", neg_Complete, this);
    }

    void HandlePositiveFeedback(DialogueState nextState) {
        Exit(nextState);
    }

    void pos_Complete(object sender, EventArgs<object> e) {
        Exit(nextState);
    }

    void neg_Complete(object sender, EventArgs<object> e) {
        //UILibrary.PhraseSelector.Get(selectorArgs, ui_Complete, this);
        ProcessLibrary.PhraseSelectionProcess.Get(selectorArgs, HandlePhrasePanelExit, this);
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(DialogueState args) {
        Debug.Log("Exiting to " + args.CurrentID);
        OnExit.Raise(this, args);
    }

    #region Debug
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(UnlockAllNeededWords);
    }

    string UnlockAllNeededWords(string input) {
        var phrases = branchElement.Branches.Select((b) => b.Prompt).ToList();
        var words = phrases.AggregateWords();
        foreach (var w in words) {
            PlayerDataConnector.CollectItem(w);
        }

        return "Words unlocked";
    }
    #endregion
}