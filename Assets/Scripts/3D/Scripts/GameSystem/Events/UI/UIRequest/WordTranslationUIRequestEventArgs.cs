using UnityEngine;
using System;
using System.Collections;

public class WordTranslationUIRequestEventArgs : UIRequestEventArgs {

    public PhraseSequenceElement Word { get; set; }
    public RectTransform Target { get; set; }
    public Action SuccessCallback { get; set; }
    public Action FailureCallback { get; set; }

	public WordTranslationUIRequestEventArgs(GameObject menuParent, PhraseSequenceElement word, RectTransform target, Action successCallback, Action failureCallback) : base(menuParent){
        Word = word;
        Target = target;
        SuccessCallback = successCallback;
        FailureCallback = failureCallback;
    }

}
