using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChatSpeechBubbleUIInitArgs {
    public PhraseSequence Phrase { get; private set; }
    public DialogueActor Target { get; private set; }
    public ChatSpeechBubbleUIInitArgs(PhraseSequence phrase, DialogueActor actor) {
        this.Phrase = phrase;
        this.Target = actor;
    }
}

public class ChatSpeechBubbleUI : ITemporaryUI<ChatSpeechBubbleUIInitArgs, object> {

    public static ChatSpeechBubbleUI GetInstance() { return new ChatSpeechBubbleUI(); }

    public event EventHandler<EventArgs<object>> Complete;

    GameObject speechBubble;

    public void Initialize(ChatSpeechBubbleUIInitArgs args1) {
        args1.Target.SetPhrase(args1.Phrase, false, SpeechBubbleOpened);
    }

    void SpeechBubbleOpened(GameObject speechBubble) {
        //Debug.Log("speech bubble opened");
        CoroutineManager.Instance.WaitAndDo(() => DestroySpeechBubble(speechBubble), new WaitForSeconds(10f));
    }

    void DestroySpeechBubble(GameObject speechBubble) {
        //Debug.Log("speech bubble being destroyed: " + speechBubble);
        if (speechBubble) {
            GameObject.Destroy(speechBubble);
        }
    }

    public void Close() {
        Complete.Raise(this, null);
    }
}
