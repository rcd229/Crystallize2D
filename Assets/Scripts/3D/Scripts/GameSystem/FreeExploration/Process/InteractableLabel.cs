using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class InteractableLabel : MonoBehaviour {

    public GameObject target;

    GameObject[] children;
    IInteractableSceneObject interactableTarget;

    void Start() {
        interactableTarget = target.GetInterface<IInteractableSceneObject>();
        if (interactableTarget == null) {
            Debug.Log("No interactable target attached");
            Destroy(this);
        }

        children = new GameObject[transform.childCount];
        for (int i = 0; i < children.Length; i++) {
            children[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update() {
        if (interactableTarget != null && interactableTarget.enabled) {
            foreach (var c in children) {
                c.SetActive(true);
            }
        } else {
            foreach (var c in children) {
                c.SetActive(false);
            }
        }
    }

    //public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
    //    Debug.Log("clicked");
    //    if (interactableTarget != null && interactableTarget.enabled) {
    //        interactableTarget.BeginInteraction(callback, parent);
    //    } else {
    //        callback(this, null);
    //    }
    //}
}
