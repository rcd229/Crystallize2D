using UnityEngine;
using System;
using System.Collections;

public class MovementDialogueAnimation : DialogueAnimation {

    public float Speed { get; set; }

    Func<Transform, Vector3> getTarget;
    bool waitForFinish = false;
    bool breakProcess = false;

    public MovementDialogueAnimation() {
        Speed = 1.5f;
        getTarget = (t) => Vector3.zero;
    }

    public MovementDialogueAnimation(Func<Transform, Vector3> getTarget)
        : this() {
        this.getTarget = getTarget;
    }

    public override DialogueAnimation GetInstance() {
        return new MovementDialogueAnimation(getTarget);
    }

    public override void Play(GameObject actor) {
        CoroutineManager.Instance.StartCoroutine(MoveToTarget(actor));

        if (!waitForFinish) {
            CoroutineManager.Instance.WaitAndDo(Exit);
        }
    }

    IEnumerator MoveToTarget(GameObject actor) {
        var a = actor;
        var p = actor.transform.position;
        var r = actor.transform.rotation;
        ResetDialogueAnimation.resetActions.Add(
            () => {
                //Debug.Log("Reset actor is: " + a);
                breakProcess = true;
                a.GetComponent<Rigidbody>().position = p;
                a.GetComponent<Rigidbody>().rotation = r;
            }
            );

        //Debug.Log("Actor is: " + actor);
        var t = getTarget(actor.transform);

        while (Vector3.Distance(actor.transform.position, t) > 0.05f) {
            actor.transform.forward = t - actor.transform.position;
            actor.GetComponent<Rigidbody>().position = Vector3.MoveTowards(actor.transform.position, t, Speed * Time.deltaTime);

            yield return null;

            if (!actor || breakProcess) {
                yield break;
            }
        }

        if (waitForFinish) {
            Exit();
        }
    }

}