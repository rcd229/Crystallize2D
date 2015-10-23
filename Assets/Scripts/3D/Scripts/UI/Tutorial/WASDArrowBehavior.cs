using UnityEngine;
using System.Collections;

public class WASDArrowBehavior : MonoBehaviour {

	public Vector2 direction;

	Vector2 localPosition;

	// Use this for initialization
	void Start () {
		localPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	public void Update () {
		transform.localPosition = localPosition + direction * 60f * Mathf.Repeat (Time.time, 1f);
	}
}
