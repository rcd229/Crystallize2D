using UnityEngine;
using System.Collections;

public class PersonAnimationController : MonoBehaviour {

    const string RunFlag = "Running";
    const float SpeedThreshold = 0.1f;
    const float SpeedRateOfChange = 0.5f;

    Animator animator;

    float speed = 0;
    Vector3 lastPosition;

    // Use this for initialization
    void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        var thisPosition = transform.position;

        //Debug.Log (gameObject + "; " + thisPosition + "; " + lastPosition + "; " + Vector3.Distance(thisPosition, lastPosition));
        var d = Vector3.Distance(thisPosition, lastPosition);
        speed = Mathf.Lerp(speed, d / Time.deltaTime, SpeedRateOfChange);
        //Debug.Log(speed);
        if (speed > SpeedThreshold) {
            animator.SetBool(RunFlag, true);
        } else {
            animator.SetBool(RunFlag, false);
        }
        lastPosition = thisPosition;
    }
}
