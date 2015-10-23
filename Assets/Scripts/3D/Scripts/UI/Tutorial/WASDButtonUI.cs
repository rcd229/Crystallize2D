using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WASDButtonUI : MonoBehaviour {

	GameObject arrow;

	// Use this for initialization
	void Start () {
		arrow = GetComponentInChildren<WASDArrowBehavior> ().gameObject;
	}

	void Update(){
		if (arrow.gameObject.activeInHierarchy) {
			arrow.GetComponent<WASDArrowBehavior>().Update();
		}
	}
	
	public void SetState(bool isOn){
		if (isOn) {
			GetComponent<Image>().color = Color.yellow;
			arrow.SetActive(true);
		} else {
			GetComponent<Image>().color = Color.white;
			arrow.SetActive(false);
		}
	}

}
