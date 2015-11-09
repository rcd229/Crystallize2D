using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CollectableWordUI : MonoBehaviour, IInitializable<PhraseSequence>, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler {

    public void Initialize(PhraseSequence args1) {
        GetComponentInChildren<Text>().text = args1.GetText((JapaneseTools.JapaneseScriptType)GameSettings.Instance.TextMode);
    }

    public void OnBeginDrag(PointerEventData eventData) {

    }

    public void OnDrag(PointerEventData eventData) {

    }

    public void OnEndDrag(PointerEventData eventData) {

    }

    public void OnPointerClick(PointerEventData eventData) {

    }
}
