using UnityEngine;
using System.Collections;

public class GridMaterialScalar : MonoBehaviour {

	public float scalar = 1f;

	// Use this for initialization
	void Start () {
	
	}

	[ExecuteInEditMode]
	void Update () {
		//transform.position = Vector3.up * Mathf.Repeat(Time.time, 1f) * 0.1f;

		GetComponent<Renderer>().material.mainTextureScale = transform.localScale / scalar;
		//var c = renderer.material.GetColor ("_TintColor");
		//c.a = 1f - Mathf.Repeat(Time.time, 1f);
		//renderer.material.SetColor ("_TintColor", c);
	}
}
