using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EnergizeInEffect : MonoBehaviour, IPointerClickHandler, INotificationPanel {

	//public Color color;

	public float Lifetime { get; set; }

	float time = 0;
	CanvasGroup canvasGroup;

	// Use this for initialization
	void Awake () {
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	void OnEnable(){
		time = 0;
	}

	public void Reset(string text){
		GetComponentInChildren<Text> ().text = text;
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

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
		} else {
			canvasGroup.alpha = 1f;
		}

		if (Lifetime > 0) {
			if(time > Lifetime){
				Destroy(gameObject);
			} else if(time > Lifetime - 1f){
				canvasGroup.alpha = Lifetime - time;
			}
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		Destroy (gameObject);
	}

}
