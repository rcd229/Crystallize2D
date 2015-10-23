using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DraggablePhraseUI : MonoBehaviour, IInitializable<PhraseSequence> {
    public RectTransform wordParent;

    public void Initialize(PhraseSequence phrase) {
        UIUtil.GenerateChildren(phrase.PhraseElements, wordParent, GenerateWord);
    }

    GameObject GenerateWord(PhraseSequenceElement word) {
        var instance = GameObjectUtil.GetResourceInstanceFromAttribute<SpeechBubbleWordUI>();
        instance.Initialize(word);
        return instance.gameObject;
    }
}
