using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockEnergizeEffect : MonoBehaviour {

	float scale = 2f;

	// Use this for initialization
	void Start () {
		var rt = GetComponent<RectTransform> ();
		rt.anchorMin = new Vector2 (0, 0);
		rt.anchorMax = new Vector2 (1f, 1f);
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;

		GetComponent<CanvasGroup> ().alpha = 0;
		transform.localScale = 2f * Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {
		scale = Mathf.MoveTowards (scale, 1f, 4f * Time.deltaTime);
		transform.localScale = scale * Vector3.one;//Vector3.MoveTowards (transform.localScale, Vector3.one, 3f * Time.deltaTime);
		GetComponent<CanvasGroup> ().alpha = 2f - transform.localScale.x;
		if (GetComponent<CanvasGroup> ().alpha >= 1f) {
			Destroy(gameObject);
		}
	}
}
