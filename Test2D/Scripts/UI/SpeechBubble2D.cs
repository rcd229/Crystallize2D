using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/SpeechBubble2D")]
public class SpeechBubble2D : MonoBehaviour {

    public GameObject draggablePhrasePrefab;
    public RectTransform phraseParent;
    public Transform target;

    public void Initialize(Transform target, PhraseSequence phrase) {
        this.target = target;
        var go = Instantiate<GameObject>(draggablePhrasePrefab);
        go.GetComponent<IInitializable<PhraseSequence>>().Initialize(phrase);
        go.transform.SetParent(phraseParent, false);
        MainCanvas.main.Add(transform);
    }

    void Awake() {
        gameObject.GetOrAddComponent<CanvasGroup>().alpha = 0;
    }

    void Start() {
        if (target) {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    void Update() {
        if (target) {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}
