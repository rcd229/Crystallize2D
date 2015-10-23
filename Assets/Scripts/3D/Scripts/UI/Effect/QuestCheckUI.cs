using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestCheckUI : MonoBehaviour {
	
	public Transform target;

	CanvasGroup canvasGroup;
	float time;
	Color originalColor;
	//Color currentColor;

	public void Initialize(Transform target){
		this.target = target;
	}

	void Start(){
		transform.SetParent(WorldCanvas.Instance.transform, false);
		canvasGroup = GetComponent<CanvasGroup> ();
		originalColor = GetComponent<Image> ().color;
		time = 0;
	}

	// Update is called once per frame
	void Update () {
		if (!transform) {
			Destroy(gameObject);
		}

		transform.LookAt (Camera.main.transform);
		transform.forward = -transform.forward;
		transform.position = target.position + Vector3.up * 2.5f;

		time += Time.deltaTime;
		if (time < 1f) {
			canvasGroup.alpha = time;
			transform.localScale = (2f - time) * Vector3.one;
			GetComponent<Image> ().color = originalColor;
		} else if (time < 1.05f) {
			canvasGroup.alpha = 1f;
			transform.localScale = Vector3.one;
			GetComponent<Image> ().color = Color.yellow;
		} else {
			canvasGroup.alpha = 1f;
			transform.localScale = Vector3.one;
			GetComponent<Image> ().color = Vector4.MoveTowards(GetComponent<Image>().color, originalColor, Time.deltaTime);
		}
	}
}
