using UnityEngine;
using System.Collections;

public class PointArrowEffect : MonoBehaviour {

    public Transform arrow;

    Color color;
    Material material;
    float a;
    float timer = 0;

	// Use this for initialization
	void Awake () {
        material = arrow.GetComponentInChildren<Renderer>().material;
        color = material.GetColor("_TintColor");
        a = color.a;
        color.a = 0;
        material.SetColor("_TintColor", color);
	}
	
	// Update is called once per frame
	void Update () {
        arrow.localPosition += 0.5f * Time.deltaTime * Vector3.forward;
	    timer += Time.deltaTime;
        color.a = a * Mathf.PingPong(timer * 2f, 1f);

        material.SetColor("_TintColor", color);

        if (timer >= 1f) {
            Destroy(gameObject);
        }
	}
}
