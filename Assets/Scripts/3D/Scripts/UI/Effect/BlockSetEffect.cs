using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockSetEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var rt = GetComponent<RectTransform> ();
		rt.anchorMin = new Vector2 (0, 0);
		rt.anchorMax = new Vector2 (1f, 1f);
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;

		GetComponent<CanvasGroup> ().alpha = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale *= (1f + Time.deltaTime * 0.2f);
		GetComponent<CanvasGroup> ().alpha -= Time.deltaTime * 3f;
		if (GetComponent<CanvasGroup> ().alpha <= 0) {
			//Debug.Log ("Set effect destroyed." + Time.time);
			Destroy(gameObject);
		}
	}
}
