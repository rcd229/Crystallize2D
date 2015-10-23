using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ShoulderCameraSolver : MonoBehaviour {

    const float MaxRotateSpeed = 100f;

    float lastTime = 0;
    Quaternion targetRotation = Quaternion.identity;
    Quaternion lastRotation = Quaternion.identity;
    Quaternion flipped = Quaternion.AngleAxis(180f, Vector3.up);

    void OnEnable() {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate() {
        while (true) {
            targetRotation = transform.parent.rotation;
            var alt = transform.parent.rotation * flipped;
            if (Quaternion.Angle(lastRotation, targetRotation) > Quaternion.Angle(lastRotation, alt)) {
                targetRotation = alt;
            }

            transform.rotation = Quaternion.RotateTowards(lastRotation, targetRotation, MaxRotateSpeed * (Time.time - lastTime));

            lastTime = Time.time;
            lastRotation = transform.rotation;

            yield return new WaitForFixedUpdate();
        }
    }

}
