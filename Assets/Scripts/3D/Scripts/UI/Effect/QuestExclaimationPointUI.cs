using UnityEngine;
using System.Collections;

public class QuestExclaimationPointUI : MonoBehaviour {

	const float JumpTime = 0.8f;
	const float CompressTime = 0.2f;
	const float JumpHeight = 0.5f;
	const float CompressAmount = 0.25f;

	public Transform target;
	public AnimationCurve curve;

	float offset;

	enum JumpPhase {
		Rising,
		Falling,
		Compressing,
	}

	public void Initialize(Transform target){
		this.target = target;
	}

	void Start(){
		transform.SetParent(WorldCanvas.Instance.transform);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.forward = -Camera.main.transform.forward;
		transform.LookAt (Camera.main.transform);
		transform.forward = -transform.forward;
		transform.position = target.position + Vector3.up * (2.5f + offset);

		var f = Mathf.Repeat (Time.time, JumpTime + CompressTime);
		if (f < JumpTime) {
			transform.localScale = Vector3.one;
			offset = JumpHeight * curve.Evaluate (f / JumpTime);
		} else {
			var c = curve.Evaluate ((f - JumpTime)/ CompressTime);
			transform.localScale = new Vector3(1f + CompressAmount * c, 1f - CompressAmount * c, 1f);
			offset = 0;
		}
	}
}
