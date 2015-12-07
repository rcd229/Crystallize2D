using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DraggedWord : MonoBehaviour, IInitializable<PhraseSequence> {
    public void Initialize(PhraseSequence args1) {
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(args1);
    }
}
