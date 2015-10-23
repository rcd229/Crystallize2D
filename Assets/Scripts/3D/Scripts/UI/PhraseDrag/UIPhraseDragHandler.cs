using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIPhraseDragHandler : MonoBehaviour, IPhraseDragHandler {

	//public GameObject canvas;
	public GameObject phraseUIPrefab;

	public event EventHandler<PhraseEventArgs> OnBeginDrag;

	public bool IsDragging { get; set; }

	// Use this for initialization
	void Start () {
		OnBeginDrag += CrystallizeEventManager.UI.RaiseBeginDragWord;
	}

	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			IsDragging = false;
		}
	}
	
	public GameObject BeginDrag (PhraseSequenceElement phraseElement, Vector2 mousePosition)
	{
		IsDragging = true;

		var go = Instantiate (phraseUIPrefab) as GameObject;
        //if (MainCanvas.main) {
        //    go.transform.SetParent (MainCanvas.main.transform);
        //} else {
        //    go.transform.SetParent (FieldCanvas.main.transform);
        //}
        //go.transform.position = mousePosition;
        //go.GetComponent<InventoryEntryUI>().word = phraseElement;
        //go.GetComponent<InventoryEntryUI> ().phraseData = PhraseSegmentData.GetWordInstance(phraseElement);
        //go.GetComponent<InventoryEntryUI>().word = phraseElement;
        //go.GetComponent<InventoryEntryUI> ().BeginDrag (mousePosition);

        ////Debug.Log("Dragging " + phraseElement.Tags.Count);

        //if (OnBeginDrag != null) {
        //    OnBeginDrag(go, new PhraseEventArgs(phraseElement));
        //}

		return go;
	}
	
}
