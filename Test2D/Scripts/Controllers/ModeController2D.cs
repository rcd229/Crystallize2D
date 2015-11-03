using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class ModeController2D : MonoBehaviour, ITemporaryUI<object, object> {

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object args1) {    }

    public void Close() {
        Destroy(gameObject);
    }

    protected virtual void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.transform) {
                OnLeftClick(hit);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.transform) {
                OnRightClick(hit);
            }
        }
    }

    protected abstract void OnLeftClick(RaycastHit2D hit);
    protected abstract void OnRightClick(RaycastHit2D hit);

}
