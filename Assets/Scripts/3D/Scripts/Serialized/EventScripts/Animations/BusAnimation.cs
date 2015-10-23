using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class BusAnimation : MonoBehaviour, ITemporaryUI<string, object> {

    public GameObject prefab;
    public Transform target;
    public Transform cameraTarget;
    float acceleration = 5f;
    float speed = 0;

    IEnumerator Run() {
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;

        OmniscientCamera.main.Suspend();
        OmniscientCamera.main.SetPosition(cameraTarget.position);
        OmniscientCamera.main.SetRotation(cameraTarget.rotation);
        speed = 10f;

        yield return new WaitForSeconds(1f);

        while (speed > 0) {
            speed -= acceleration * Time.deltaTime;

            yield return null;
        }
        speed = 0;

        yield return new WaitForSeconds(3f);
        var instance = Instantiate<GameObject>(prefab);
        instance.transform.position = target.position;
        instance.transform.rotation = target.rotation;

        while (speed < 10f) {
            speed += acceleration * Time.deltaTime;

            yield return null;
        }
        speed = 10f;

        yield return new WaitForSeconds(1f);

        OmniscientCamera.main.Resume();

        Close();

        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        OmniscientCamera.main.Resume();
        Close();
        Destroy(gameObject);
    }

    void Update() {
        transform.position += speed * Vector3.forward * Time.deltaTime;
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
