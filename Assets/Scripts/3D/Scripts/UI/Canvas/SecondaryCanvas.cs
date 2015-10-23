using UnityEngine;
using System.Collections;

public class SecondaryCanvas : MonoBehaviour {

	public static SecondaryCanvas main { get; set; }

	// Use this for initialization
	void Awake () {
		main = this;
	}

}
