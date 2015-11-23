using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DraggedPhrase: MonoBehaviour {

    PhraseSequence phrase;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(PhraseSequence phrase, Vector3 position) {
        MainCanvas.main.Add(transform);
        this.phrase = phrase;
        transform.position = position;
    }

    void Update() {
        transform.position = Input.mousePosition;

        if (Input.GetMouseButtonUp(0)) {
            var raycastResults = new List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);

            foreach(var result in raycastResults) {
                var dropAreas = result.gameObject.GetInterfacesInChildren<IDropArea>();
                if(dropAreas.Length > 0) {
                    foreach (var a in dropAreas) {
                        a.AcceptDrop(phrase, gameObject);
                        Exit(true);
                    }
                    return;
                }
            }

            Exit(false);
        }
    }

    void Exit(bool accepted) {
        Destroy(gameObject);
        //OnExit.Raise(null, accepted);
    }

    public void ForceExit() {
        Destroy(gameObject);
    }
}