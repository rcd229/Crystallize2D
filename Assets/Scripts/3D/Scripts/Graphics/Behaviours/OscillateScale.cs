using UnityEngine;
using System.Collections;

public class OscillateScale : MonoBehaviour {

	public float minScale = 1f;
	public float maxScale = 1.5f;
	public float speed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var avg = (maxScale + minScale) * 0.5f;
		var mag = (maxScale - minScale) * 0.5f;
		transform.localScale = (avg + mag * Mathf.Sin (Time.time * speed)) * Vector3.one;
	}
}
