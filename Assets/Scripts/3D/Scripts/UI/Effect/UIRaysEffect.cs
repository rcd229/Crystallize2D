using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRaysEffect : UIMonoBehaviour {

	// Use this for initialization
	void Start () {
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.width);
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.width);
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.Rotate (Vector3.forward, 30f * Time.deltaTime);
	}
}
