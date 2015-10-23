using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class WalkInAnimation : MonoBehaviour, ITemporaryUI<string, object> {

    public Transform actor;
    public Transform target;
    public Transform cameraTarget;
    float speed = 0;

    IEnumerator Run() {
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;

        OmniscientCamera.main.Suspend();
        OmniscientCamera.main.SetPosition(cameraTarget.position);
        OmniscientCamera.main.SetRotation(cameraTarget.rotation);
        speed = 2f;

        yield return new WaitForSeconds(3f);

        OmniscientCamera.main.Resume();

        Close();
        Destroy(gameObject);

        yield return new WaitForSeconds(1f);
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
        Destroy(gameObject);
    }

    //void Start() {
    //    StartCoroutine(Run());
    //}

    void Update() {
        actor.position = Vector3.MoveTowards(actor.position, target.position, speed * Time.deltaTime);
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    public void Close() {
        Complete.Raise(this, null);
        Complete = null;
    }

    public void Initialize(string param1) {
        StartCoroutine(Run());
    }

    public event EventHandler<EventArgs<object>> Complete;

}
