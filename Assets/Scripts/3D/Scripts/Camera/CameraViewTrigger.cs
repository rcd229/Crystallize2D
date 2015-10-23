using UnityEngine;
using System.Collections;

/// <summary>
/// Change the camera angle when the attached collider is entered. This is useful if you want the player to look at something specific when they get close to something.
/// </summary>
public class CameraViewTrigger : MonoBehaviour {

    public Vector3 eulerAngles = Vector3.zero;

    Quaternion previousAngle;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.IsPlayer()) {
            previousAngle = Camera.main.transform.rotation;

            OmniscientCamera.main.SetTargetAngles(eulerAngles);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.IsPlayer()) {
            OmniscientCamera.main.SetTargetAngles(previousAngle.eulerAngles);
        }
    }

}
