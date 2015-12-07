using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class DraggableWord: DraggableObject {
    public override void Initialize(PhraseSequence phrase) {
        base.Initialize(phrase);
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        if (PlayerDataConnector.CanLearn(phrase)) {
            GetComponent<Image>().enabled = true;
        } else {
            GetComponent<Image>().enabled = false;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        PhraseDragDropPipeline.BeginDrag(Phrase, eventData.position);
    }
}