using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceTutorialProcess : IProcess<object, object> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<ContextActionArgs, object> contextMessage;
    List<GameObject> instances = new List<GameObject>();

    public void Initialize(object param1) {
        var prefab = Resources.Load<GameObject>("Tutorial/DownArrow");
        foreach (var p in GameObject.FindGameObjectsWithTag("Place")) {
            if (p.transform.childCount > 0) {
                var instance = GameObject.Instantiate<GameObject>(prefab);
                instance.transform.position = p.transform.position + 2f * Vector3.up;
                instances.Add(instance);
            }
        }
        contextMessage = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Find the correct area", true));
        CoroutineManager.Instance.StartCoroutine(RunSequence());
        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    void Input_OnEnvironmentClick(object sender, System.EventArgs e) {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.attachedRigidbody) {
                if (hit.collider.attachedRigidbody.CompareTag("Place")) {
                    if (PlayerManager.Instance.InInteractionRange(hit.collider.attachedRigidbody.transform)) {
                        contextMessage.CloseIfNotNull();
                    } else {
                        contextMessage.Initialize(new ContextActionArgs("You need to move closer", true, true));
                        CoroutineManager.Instance.StartCoroutine(WaitAndSet());
                    }
                    return;
                }
            }
        }

        contextMessage.Initialize(new ContextActionArgs("That's not the right place!", true, true));
        CoroutineManager.Instance.StartCoroutine(WaitAndSet());
    }

    public void ForceExit() {
        Exit();
    }

    IEnumerator WaitAndSet() {
        yield return new WaitForSeconds(5f);
        SetIfOpen();
    }

    void SetIfOpen() {
        //Debug.Log("Setting :" + contextMessage.IsOpen());
        if (contextMessage.IsOpen()) {
            contextMessage.Initialize(new ContextActionArgs("Click on the area your boss mentioned to clean it.", true));
        }
    }

    IEnumerator RunSequence() {
        while (true) {
            bool inRange = false;
            foreach (var p in GameObject.FindGameObjectsWithTag("Place")) {
                if (p.transform.childCount > 0) {
                    if (PlayerManager.Instance.InInteractionRange(p.transform)) {
                        inRange = true;
                        break;
                    }
                }
            }

            if (inRange) {
                break;
            }

            yield return null;
        }

        SetIfOpen();
    }

    void Exit() {
        foreach (var i in instances) {
            GameObject.Destroy(i);
        }
        contextMessage.CloseIfNotNull();
        OnExit.Raise(this, null);
    }

}