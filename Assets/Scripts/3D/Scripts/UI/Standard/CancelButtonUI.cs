using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CancelButtonUI : MonoBehaviour, IPointerClickHandler {

    public GameObject window;

    public void OnPointerClick(PointerEventData eventData) {
        var w = window.GetInterface<IWindowUI>();
        if (w != null) {
            w.Close();
        }
    }

}
