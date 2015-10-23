using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DraggablePhrase : LearnPhraseButtonUI {

    public GameObject wordPrefab;
    public RectTransform notLearnableParent;
    public RectTransform isLearnableParent;

    List<GameObject> instances = new List<GameObject>();

    protected override void Refresh() {
        var parent = notLearnableParent;
        if (PlayerDataConnector.CanLearn(phrase, false)) {
            isLearnableParent.gameObject.SetActive(true);
            parent = isLearnableParent;
        } else {
            isLearnableParent.gameObject.SetActive(false);
        }
        UIUtil.GenerateChildren(phrase.PhraseElements, instances, parent, CreateChild);
    }

    GameObject CreateChild(PhraseSequenceElement word) {
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetInterface<IInitializable<PhraseSequenceElement>>().Initialize(word);
        return instance;
    }
}
