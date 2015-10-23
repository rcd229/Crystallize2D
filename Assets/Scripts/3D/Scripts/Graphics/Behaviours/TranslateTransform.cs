using UnityEngine;
using System.Collections;

public class TranslateTransform : MonoBehaviour {

	public Vector3 direction = Vector3.up;
	public float speed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction.normalized * speed * Time.deltaTime);
	}
}
