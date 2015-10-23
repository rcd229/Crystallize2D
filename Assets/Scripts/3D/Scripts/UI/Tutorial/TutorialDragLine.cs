using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialDragLine {

	const int DotCount = 6;
	const float Speed = 400f;
	const float Spacing = 60f;

	public RectTransform Origin { get; set; }
	public RectTransform Destination { get; set; }

	List<RectTransform> dotList = new List<RectTransform>();

	public TutorialDragLine (RectTransform t1, RectTransform t2, RectTransform parent, Sprite image){
		Origin = t1;
		Destination = t2;
		CreateDots (parent, image);
	}

	void CreateDots(RectTransform parent, Sprite image){
		for (int i = 0; i < DotCount; i++) {
			var go = new GameObject("DragLineDot");
			go.transform.SetParent(parent);
			go.AddComponent<Image>().sprite = image;
			go.AddComponent<CanvasGroup>().alpha = 1f - (float)i / DotCount;
			var rt = go.GetComponent<RectTransform>();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30f);
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
			dotList.Add(rt);
		}
	}

	public void Update(){
		var p1 =(Vector2) Origin.position + Origin.rect.center;
		var p2 = (Vector2)Destination.position + Destination.rect.center;
		var d = Vector2.Distance (p1, p2);
		var f = Mathf.Repeat (Time.time * Speed, d);
		int intpos = Mathf.RoundToInt (f / Spacing);
		f = intpos * Spacing;
		var back = (p1 - p2).normalized;
		var pos = p1 - back * f;
		int offset = 0;
		for (int i = 0; i < dotList.Count; i++) {
			dotList[i].position = pos + back * (i - offset) * Spacing;

			intpos -= 1;
			if(intpos == -1){
				offset = i;
				pos = p2;
			}
		}
	}

	public void Clear(){
		foreach (var obj in dotList) {
			GameObject.Destroy(obj.gameObject);
		}
		dotList.Clear ();
	}

}
