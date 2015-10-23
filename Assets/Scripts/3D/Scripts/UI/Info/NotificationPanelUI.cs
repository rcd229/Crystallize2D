using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NotificationPanelUI : MonoBehaviour, IPointerClickHandler, INotificationPanel {

	public string text;
	public Text notificationText;
	public AnimationCurve jumpCurve;

	float timer = 0;
	float stayDuration = 1.1f;

	// Use this for initialization
	void Start () {
		if (!GetComponent<CanvasGroup> ()) {
			gameObject.AddComponent<CanvasGroup>();
		}

		Reset (text);
	}
	
	// Update is called once per frame
	void Update () {
		notificationText.text = text;

		var offset = GetComponent<RectTransform> ().rect.position;
		offset.x = -offset.x;
		var target = (Vector3)((new Vector2 (0, Screen.height) + offset));
		timer += Time.deltaTime;
		if (timer < stayDuration) {
			transform.position = target - 50f * Vector3.up * jumpCurve.Evaluate (timer / stayDuration);
			GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.white, timer / stayDuration);
		} else {
			GetComponent<Image>().color = Color.white;
			transform.position = target;
		}
		/*if (timer < 0.5f) {
			GetComponent<CanvasGroup> ().alpha = timer * 2f;
		} else if (timer < stayDuration) {
			GetComponent<CanvasGroup> ().alpha = 1f;
		} else {
			var offset = GetComponent<RectTransform>().rect.position;
			offset.x = -offset.x;
			transform.position = Vector3.MoveTowards(transform.position, 
			                                         (Vector3)((new Vector2(0, Screen.height) + offset)),
			                                         1500f * Time.deltaTime);
		}*/
	}

	public void Close(){
		Debug.Log ("Closing");
		Destroy (gameObject);
		GetComponent<Image> ().color = Color.yellow;
	}



	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		Close ();
	}
	#endregion

	public void Reset(string text){
		this.text = text;
		//GetComponent<RectTransform> ().position = 0.5f * new Vector2 (Screen.width, Screen.height);
		timer = 0;
	}

}
