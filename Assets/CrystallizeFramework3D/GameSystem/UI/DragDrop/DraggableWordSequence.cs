using UnityEngine;
using System.Collections;
using System;

public class DraggableWordSequence : MonoBehaviour, IInitializable<PhraseSequence> {

    public GameObject wordPrefab;

    public void Initialize(PhraseSequence args1) {
        foreach(var w in args1.PhraseElements) {
            var wordInstance = Instantiate(wordPrefab);
            wordInstance.GetInterface<IInitializable<PhraseSequence>>().Initialize(new PhraseSequence(w));
            wordInstance.transform.SetParent(transform, false);
        }
    }
	
}
