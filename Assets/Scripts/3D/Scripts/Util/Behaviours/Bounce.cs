using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Bounce : MonoBehaviour {

    public AnimationCurve curve;
    public float dist = 0.5f;
    public float speed = 1f;

    Vector3 basePosition;

    void Start() {
        basePosition = transform.localPosition;
    }

    void Update() {
        //Debug.Log(dist * curve.Evaluate(Time.time * speed));
        transform.localPosition = basePosition + dist * curve.Evaluate(Mathf.Repeat(Time.time * speed, 1f)) * Vector3.up;
    }

}
