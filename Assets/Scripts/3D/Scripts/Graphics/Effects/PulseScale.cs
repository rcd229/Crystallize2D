using UnityEngine;
using System.Collections;

public class PulseScale : MonoBehaviour {

    public float minScale = 1f;
    public float maxScale = 1.5f;

    float t = 0;

    Color color;

    // Use this for initialization
    void Start() {
        color = GetComponent<MeshRenderer>().material.GetColor("_TintColor");
    }

    // Update is called once per frame
    void Update() {
        t = Mathf.Repeat(t + Time.deltaTime * 2f, 1f);
        transform.localScale = Mathf.Lerp(minScale, maxScale, t) * Vector3.one;
        color.a = 1f - t;
        GetComponent<MeshRenderer>().material.SetColor("_TintColor", color);
    }

}
