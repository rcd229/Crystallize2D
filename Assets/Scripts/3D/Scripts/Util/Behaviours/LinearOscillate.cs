using UnityEngine;
using System.Collections;

public class LinearOscillate : MonoBehaviour {

	Vector3 origin;
	public Transform target;
	public float magnitude = 0.25f;
	public float speed = 3f;
	public Vector3 direction = Vector3.up;

	// Use this for initialization
	void Start () {
		if (target) {
			origin = transform.localPosition - target.position;
		} else {
			origin = transform.localPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			transform.localPosition = target.position + origin + magnitude * Mathf.Sin (speed * Time.time) * direction;
		} else {
			transform.localPosition = origin + magnitude * Mathf.Sin (speed * Time.time) * direction;
		}
	} 
}
