using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DraggablePhrase : DraggableObject {

    public GameObject wordPrefab;
    public RectTransform notLearnableParent;
    public RectTransform isLearnableParent;

    List<GameObject> instances = new List<GameObject>();

    protected void Refresh() {
        var parent = notLearnableParent;
        if (PlayerDataConnector.CanLearn(Phrase)) {
            isLearnableParent.gameObject.SetActive(true);
            parent = isLearnableParent;
        } else {
            isLearnableParent.gameObject.SetActive(false);
        }
        UIUtil.GenerateChildren(Phrase.PhraseElements, instances, parent, CreateChild);
    }

    GameObject CreateChild(PhraseSequenceElement word) {
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetInterface<IInitializable<PhraseSequenceElement>>().Initialize(word);
        return instance;
    }
}
