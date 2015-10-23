using UnityEngine;
using System;
using System.Collections;

public class SpeechBubbleRequestedEventArgs : System.EventArgs {

    public Transform Target { get; set; }
    public PhraseSequence Phrase { get; set; }
    public PointerType SpeechBubblePointerType { get; set; }
    public bool ReduceConfidence { get; set; }
    public Action<GameObject> Callback { get; set; }

    public SpeechBubbleRequestedEventArgs(Transform target) {
        Target = target;
        Phrase = null;
        ReduceConfidence = false;
    }

    public SpeechBubbleRequestedEventArgs(Transform target, PhraseSequence phrase, bool reduceConfidence, Action<GameObject> callback) {
        Target = target;
        Phrase = phrase;
        ReduceConfidence = reduceConfidence;
        SpeechBubblePointerType = PointerType.Normal;
        this.Callback = callback;
    }

}
