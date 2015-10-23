using UnityEngine;
using System.Collections;

public class MovementAnimationController : MonoBehaviour {

    Animator animator;
    float time = 0;
    float thresholdSpeed = 0.5f;
    Vector3 lastPosition;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
            if (!animator)
            {
                return;
            }
        }

        if (!animator.gameObject.activeInHierarchy) {
            return;
        }

        var thisPosition = transform.position;
            
        if (Vector3.Distance(thisPosition, lastPosition) > thresholdSpeed * Time.fixedDeltaTime) {
            var f = thisPosition - lastPosition;
            f.y = 0;
            if (f.sqrMagnitude > 0.001f) {
                //Debug.Log(f.normalized);

                var rot = Quaternion.LookRotation(f.normalized, Vector3.up);
                var rb = transform.GetComponent<Rigidbody>();
                rb.rotation = Quaternion.RotateTowards(rb.rotation, rot, 720f * Time.fixedDeltaTime);
                animator.SetBool("Running", true);
            }
            time = 0;
        } else {
            time += Time.deltaTime;
        }

        if (time > 0.1f) {
            animator.SetBool("Running", false);
        }

        lastPosition = thisPosition;
	}
}
