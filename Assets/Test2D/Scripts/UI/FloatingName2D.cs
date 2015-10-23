using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/FloatingName2D")]
public class FloatingName2D : MonoBehaviour, IInitializable<Transform> {

    public GameObject draggablePhrase;
    public RectTransform phraseParent;

    [SerializeField]
    Transform target;

    public void Initialize(Transform target) {
        this.target = target;
        //TransformPath.Add(TransformPath.Get("UI", "3D", "Names"));
        MainCanvas.main.Add(transform);
        target.gameObject.GetOrAddComponent<DestroyEvent>().Destroyed += TargetDestroyed;
    }

    public void Initialize(PhraseSequence phrase) {
        var inst = Instantiate<GameObject>(draggablePhrase);
        inst.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        inst.transform.SetParent(phraseParent, false);
    }

    void TargetDestroyed(object sender, GameObjectArgs e) {
        Destroy(gameObject);
    }

    void Update() {
        if (target) {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
            //transform.position = target.position;
        }
    }
}
