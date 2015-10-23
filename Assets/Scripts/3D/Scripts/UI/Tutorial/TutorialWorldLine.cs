using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialWorldLine {

	const int DotCount = 3;
	const float Speed = 4f;
	const float Spacing = 0.6f;

	public Transform Origin { get; set; }
	public Transform Destination { get; set; }

	List<Transform> dotList = new List<Transform>();
	float distanceFromOrigin = 0;

	public TutorialWorldLine (Transform t1, Transform t2, Transform parent, Sprite image){
		Origin = t1;
		Destination = t2;
		CreateDots (parent, image);
	}

	void CreateDots(Transform parent, Sprite image){
		for (int i = 0; i < DotCount; i++) {
			var go = new GameObject("WorldLineDot");
			go.transform.SetParent(parent);
			go.AddComponent<Image>().sprite = image;
			go.AddComponent<CanvasGroup>().alpha = 1f - (float)i / DotCount;
			var rt = go.GetComponent<RectTransform>();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.4f);
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.4f);
			dotList.Add(rt);
		}
	}

	public void Update(){
		var p1 =(Vector3) Origin.position;
		var p2 = (Vector3)Destination.position;
		var d = Vector3.Distance (p1, p2);

		if (d < Spacing * DotCount) {
			foreach(var dot in dotList){
				dot.GetComponent<CanvasGroup>().alpha = 0;
			}
			return;
		}

		distanceFromOrigin = Mathf.Repeat (distanceFromOrigin + Time.deltaTime * Speed, d);
		int intpos = Mathf.RoundToInt (distanceFromOrigin / Spacing);

		var f = intpos * Spacing;
		var back = (p1 - p2).normalized;
		var pos = p1 - back * f;
		int offset = 0;

		int count = dotList.Count;
		if (d / Spacing < DotCount) {
			count = (int)(d / Spacing);
		}

		int i = 0;
		for (i = 0; i < count; i++) {
			dotList[i].gameObject.SetActive(true);
			dotList[i].forward = Vector3.up;
			dotList[i].position = (pos + back * (i - offset) * Spacing) + Vector3.up * 0.1f;
			dotList[i].GetComponent<CanvasGroup>().alpha = 1f - (float)i / DotCount;

			intpos -= 1;
			if(intpos == -1){
				offset = i;
				pos = p2;
			}
		}

		for (; i < dotList.Count; i++) {
			dotList[i].gameObject.SetActive(false);
		}
	}

	public void Clear(){
		foreach (var obj in dotList) {
			GameObject.Destroy(obj.gameObject);
		}
		dotList.Clear ();
	}

}
