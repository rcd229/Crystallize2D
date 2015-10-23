using UnityEngine;
using System.Collections;

public interface ICameraMotionFilter {

    Vector3 UpdatePosition(Transform cameraTransform, Vector3 currentPosition, Vector3 desiredPosition);

}
