using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DragWordsTutorialProcess : IProcess<DialogueState, DialogueState>, ISettableParent {

    const int MinDragTimes = 4;

    static int draggedTimes = 0;
    static bool conversationDragCompleted = false;
    static bool phraseDragCompleted = false;

    DialogueState state;

    public ProcessExitCallback OnExit { get; set; }
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    GameObject actorTarget;
    GameObject speechBubble;
    ITemporaryUI helpInstance;
    ITemporaryUI clickToContinueUI;
    ITemporaryUI highlightBox;
    List<PhraseSequence> targetPhrases = new List<PhraseSequence>();

    public void Initialize(DialogueState data) {
        this.state = data;
        var target = state.GetTarget();
        if (target.transform.parent != null) {
            actorTarget = target.transform.parent.gameObject;
            Debug.Log("Doing drag tutorial. Target is: " + actorTarget.name);
        } else {
            Debug.Log("Doing drag tutorial. No target.");
        }

        CrystallizeEventManager.UI.OnSpeechBubbleOpen += UI_OnSpeechBubbleOpen;
        if (data.GetElement() != null) {
            var actor = target.GetComponent<DialogueActor>();
            var context = actor.GetOrCreateRandomContext().Context.OverrideWith(data.Context);
            target.GetComponentInChildren<DialogueActor>().SetLine(((LineDialogueElement)data.GetElement()).Line, context);
        } else {
            Exit();
        }
    }

    void UI_OnSpeechBubbleOpen(object sender, SpeechBubbleRequestedEventArgs e) {
        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= UI_OnSpeechBubbleOpen;
        speechBubble = (GameObject)sender;

        var hasNewWord = false;
        foreach (var w in e.Phrase.PhraseElements) {
            if (PlayerDataConnector.CanLearn(new PhraseSequence(w), false)) {
                targetPhrases.Add(new PhraseSequence(w));
                hasNewWord = true;
            }
        }

        if (actorTarget) {
            if (actorTarget.name.Contains("Generated")
                && hasNewWord
                && !conversationDragCompleted) {
                conversationDragCompleted = true;
                ProcessLibrary.MessageBox.Get("You can learn words from any speech bubble.", ConversationMessageBoxCallback, this);
                return;
            } else {
                //Debug.Log("Can't do conv. tutorial. Name: " + actorTarget.name + "; hasNewWord: " + hasNewWord + "; convDragCompleted: " + conversationDragCompleted);
            }
        }

        //Debug.Log("got: " + e.Phrase.GetText());
        if (!PlayerDataConnector.CanLearn(e.Phrase, false) || draggedTimes > MinDragTimes) {
            //Debug.Log("no tutorial");
            MessageBoxCallback(null, null);
            return;
        }

        if (!e.Phrase.IsWord) {
            if (phraseDragCompleted) {
                MessageBoxCallback(null, null);
                return;
            } else {
                var targ = speechBubble.GetComponentInChildren<LearnPhraseButtonUI>().GetComponent<RectTransform>();
                var args = new UITargetedMessageArgs(targ, "click to learn phrase");
                highlightBox = UILibrary.HighlightBox.Get(args);
                ProcessLibrary.MessageBox.Get("You can learn phrases by clicking the star in the speech bubble.", PhraseMessageBox, this);
                phraseDragCompleted = true;
                return;
            }
        }

        UI_OnPhraseDropped(sender, new PhraseEventArgs(e.Phrase));
    }

    void ConversationMessageBoxCallback(object sender, object e) {
        UI_OnPhraseDropped(sender, null);
    }

    void PhraseMessageBox(object obj, object e) {
        ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.LeftClick), HighlightClickCallback, this);
    }

    void HighlightClickCallback(object obj, object e) {
        highlightBox.CloseIfNotNull();
        CoroutineManager.Instance.WaitAndDo(() => MessageBoxCallback(null, null));
    }

    public void SetParent(IProcess parent) { }

    public void ForceExit() {
        Exit();
    }

    void OnEnvironmentClick(object sender, InputListenerArgs e) {
        Exit();
    }

    void UI_OnPhraseDropped(object sender, PhraseEventArgs e) {
        CrystallizeEventManager.UI.OnPhraseDropped -= UI_OnPhraseDropped;

        //Debug.Log("Dropped");
        SetUI(null);

        bool learnedWord = false;
        foreach (var w in targetPhrases) {
            if (PlayerDataConnector.ContainsLearnedItem(w)) {
                learnedWord = true;
            }
        }

        if (learnedWord) {
            //ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.EnvironmentClick), OnEnvironmentClick, this);
            draggedTimes++;
            if (draggedTimes == 1) {
                ProcessLibrary.MessageBox.Get("Click anywhere in the world to continue the dialogue.", MessageBoxCallback, this);
            } else {
                MessageBoxCallback(null, null);
            }
        } else {
            var args = new UITargetedMessageArgs(speechBubble.GetComponent<RectTransform>(), "Drag words from here");
            var ui = UILibrary.HighlightBox.Get(args);
            SetUI(ui);

            CrystallizeEventManager.UI.OnWordDragged += UI_OnPhraseDragged;
            CrystallizeEventManager.UI.OnPhraseDragged += UI_OnPhraseDragged;
        }
    }

    void UI_OnPhraseDragged(object sender, PhraseClickedEventArgs e) {
        var target = GameObject.FindGameObjectWithTag("CollectUI");
        var args = new UITargetedMessageArgs(target.GetComponent<RectTransform>(), "...to here");
        var ui = UILibrary.HighlightBox.Get(args);
        SetUI(ui);

        CrystallizeEventManager.UI.OnWordDragged -= UI_OnPhraseDragged;
        CrystallizeEventManager.UI.OnPhraseDragged -= UI_OnPhraseDragged;

        CrystallizeEventManager.UI.OnPhraseDropped += UI_OnPhraseDropped;
    }

    void MessageBoxCallback(object sender, object args) {
        clickToContinueUI = UILibrary.ClickToContinue.Get(null);
        ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.EnvironmentClick), ClickCallback, this);
    }

    void ClickCallback(object sender, object args) {
        Exit();
    }

    DialogueState GetNextState() {
        return new DialogueState(state.GetElement().DefaultNextID, state.Dialogue, state.Context, state.ActorMap, state.GameObjects);
    }

    void Exit() {
        if (phraseDragCompleted && conversationDragCompleted && draggedTimes >= MinDragTimes) {
            Debug.Log("Drag tutorial finished");
            PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.DragWords);
        } else {
            Debug.Log("not finished with drag tutorial. phraseDrag: " + phraseDragCompleted + "; convDrag: " + conversationDragCompleted + "; draggedTimes: " + draggedTimes);
        }

        clickToContinueUI.CloseIfNotNull();
        OnExit.Raise(this, GetNextState());
    }

    void SetUI(ITemporaryUI newUI) {
        if (helpInstance != null) {
            helpInstance.Close();
        }

        helpInstance = newUI;
    }

}
