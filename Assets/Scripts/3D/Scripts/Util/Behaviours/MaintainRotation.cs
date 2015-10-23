using UnityEngine;
using System.Collections;

public class MaintainRotation : MonoBehaviour {

	public Vector3 euler = Vector3.zero;

	Quaternion rotation;

	// Use this for initialization
	void Start () {
		rotation = Quaternion.Euler (euler);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = rotation;
	}
}
