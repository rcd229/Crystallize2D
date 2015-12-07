using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WordInventorySlot : MonoBehaviour, IInitializable<PhraseSequence> {
    public void Initialize(PhraseSequence args1) {
        if(args1 == null) {
            GetComponentInChildren<Text>().text = "";
        } else {
            GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(args1);
        }
    }
}
