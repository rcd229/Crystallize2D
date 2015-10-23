using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergizeEffect : MonoBehaviour {

	//public Color color;

	float time = 0;
	CanvasGroup canvasGroup;

	// Use this for initialization
	void Awake () {
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	void OnEnable(){
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;

		transform.localScale = Vector3.one;
		canvasGroup.alpha = 0;
		if (time < 1f) {
			transform.localScale = (1f + (1f - time)) * Vector3.one;
			canvasGroup.alpha = time;
		} else if (time < 2f) {
			canvasGroup.alpha = 1f;
			transform.localScale = Vector3.one;
			var c = GetComponentInChildren<Text> ().color;
			c.a = time - 1f;
			GetComponentInChildren<Text> ().color = c;
		} else if (time < 3f) {
			canvasGroup.alpha = 3f - time;
		}
	}
}
