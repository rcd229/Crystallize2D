using UnityEngine;
using System.Collections;

public class OverheadUI : UIMonoBehaviour {

    const string SpeechBubbleTargetTag = "SpeechBubbleTarget";
    static Vector3 DefaultOffset = new Vector3(0, 2f, 0);

    public Transform target;

    Transform _target;

    float innerDistance = 5f;
    float outerDistance = 10f;

	// Use this for initialization
	void Start () {
        this._target = null;
        foreach (var t in target.GetComponentsInChildren<Transform>()) {
            if (t.CompareTag(SpeechBubbleTargetTag)) {
                this._target = t;
                break;
            }
        }
        if (this._target == null) {
            this._target = new GameObject(SpeechBubbleTargetTag).transform;
            this._target.SetParent(target);
            this._target.localPosition = DefaultOffset;
            this._target.tag = SpeechBubbleTargetTag;
        }

        CrystallizeEventManager.Environment.AfterCameraMove += HandleAfterCameraMove;
	}

    void HandleAfterCameraMove(object sender, System.EventArgs e) {
        var sqDist = (PlayerManager.Instance.PlayerGameObject.transform.position - target.transform.position).sqrMagnitude;
        if (sqDist < innerDistance * innerDistance) {
            canvasGroup.alpha = 1f;
        } else if (sqDist < outerDistance * outerDistance) {
            canvasGroup.alpha = 1f - (Mathf.Sqrt(sqDist) - innerDistance) / (outerDistance - innerDistance);
        } else {
            canvasGroup.alpha = 0;
        }
        transform.position = Camera.main.WorldToScreenPoint(_target.position + 0.5f * Vector3.up);//target.position + height * Vector3.up);
    }
	
}
