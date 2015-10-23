using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapElement : MonoBehaviour {

	public Transform target;
    public Text text;
    public bool rotate = false;
	Map map;
	RectTransform myRect;
	// Use this for initialization
	void Start () {
		map = Map.Instance;
		transform.SetParent (map.childParent.transform, false);
		myRect = GetComponent<RectTransform> ();
        SetTextActive(false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target == null){
			return;
		}

		Vector2 myPos = map.Position (target.position);
		myRect.localPosition = myPos;
		//myRect.localScale = new Vector2 (map.zoomLevel, map.zoomLevel);
        if (rotate) {
            Quaternion myRotation = map.Rotation(target.rotation);
            myRect.localRotation = myRotation;
        }
	}

    public void SetTextActive(bool isActive) {
        if (text) {
            if (this && isActive && target.GetComponent<QuestNPC>()) {
                text.gameObject.SetActive(true);
                text.text = target.GetComponent<QuestNPC>().NPC.Name;
            } else {
                text.gameObject.SetActive(false);
            }
        }
    }
}
