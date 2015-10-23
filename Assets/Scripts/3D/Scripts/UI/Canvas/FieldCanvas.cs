using UnityEngine;
using System.Collections;

public class FieldCanvas : MonoBehaviour {

	public static FieldCanvas main { get; set; }

	// Use this for initialization
	void Awake () {
		main = this;
	}

}
