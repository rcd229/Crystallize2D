using UnityEngine;
using System.Collections;

/// <summary>
/// This behavior limites where the camera can go. Mostly useful for indoor scenes.
/// It uses the box collider attached to the game object to determine the bounds.
/// </summary>
public class CameraBox : MonoBehaviour, ICameraMotionFilter {

    Bounds bounds;

	// Use this for initialization
	void Start () {
        OmniscientCamera.main.AddMotionFilter(this);

        bounds = GetComponent<BoxCollider>().bounds;
	}

    void OnEnable() {
        if (OmniscientCamera.main) {
            OmniscientCamera.main.AddMotionFilter(this);
        }

        bounds = GetComponent<BoxCollider>().bounds;
    }

    void OnDisable() {
        if (OmniscientCamera.main) {
            OmniscientCamera.main.RemoveMotionFilter(this);
        }
    }

    public Vector3 UpdatePosition(Transform cameraTransform, Vector3 currentPosition, Vector3 desiredPosition) {
        if (!bounds.Contains(desiredPosition)) {
            return GetComponent<BoxCollider>().ClosestPointOnBounds(desiredPosition);
        }
        return desiredPosition;
    }

}
