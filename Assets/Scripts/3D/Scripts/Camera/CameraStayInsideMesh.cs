using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CameraStayInsideMesh : MonoBehaviour {

    public float extraDistanceInside = 0.5f;

    int cameraLayerMask;
    OmniscientCamera omniCam;

    void Start() {
        cameraLayerMask = LayerMask.GetMask(TagLibrary.Camera);
        CrystallizeEventManager.Environment.AfterCameraMove += Environment_AfterCameraMove;
        omniCam = GetComponent<OmniscientCamera>();
    }

    void Environment_AfterCameraMove(object sender, EventArgs e) {
        var origin = transform.position - transform.forward * extraDistanceInside;
        var dir = transform.forward;
        var hit = new RaycastHit();
        if (Physics.Raycast(origin, dir, out hit, omniCam.distance + extraDistanceInside, cameraLayerMask)) {
            transform.position = origin + (hit.distance + extraDistanceInside) * dir;
        }
    }

}
