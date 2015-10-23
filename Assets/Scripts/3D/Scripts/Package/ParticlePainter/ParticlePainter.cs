using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePainter : MonoBehaviour {

	const int PaintLayerMask = 1 << 25;

	//public LayerMask paintLayerMask = new LayerMask();
	public ParticleBrush particleBrush;
	public Transform brushObject;
	public float baseSize = 0.6f;
	public float speedSensitivity = 0.015f;
	public float speedMultiply = 0.01f;
	public float minSize = 0.3f;
	public float randomPercentSize = 0;

	Vector3 lastPoint;
	Vector3 lastBrushPoint;

	protected bool isPainting = false;
	protected RaycastHit lastHit;

	protected virtual void Start(){
		particleBrush.size = minSize;
	}

	protected virtual void Update () {
		if (GetInputActive()) {
			//Debug.Log("Input active.");
			ContinuePainting();
		}
		
		if (GetInputUp() && isPainting) {
			EnterStroke();
			particleBrush.CompleteStroke();
			isPainting = false;
		}
		
		/*if (GetClearInput()) {
			particleBrush.Clear();
			OnClear();
		}*/
	}

	protected void ContinuePainting(){
		var c = GetComponent<Camera>();
		if(!c){
			c = Camera.main;
		}
		var ray = c.ScreenPointToRay(Input.mousePosition);
		var hit = new RaycastHit ();
		if (Physics.Raycast(ray, out hit, 999f, GetLayerMask())){//, 1 << gameObject.layer
			lastHit = hit;
			
			if (Input.GetMouseButtonDown (0)) {
				//Debug.Log("Mouse down");
				particleBrush.BeginStroke(hit.point);
				isPainting = true;
				BeginStroke(hit.point);
			}
			
			if(isPainting){
				var d = Vector3.Distance(lastPoint, hit.point);
				
				if(brushObject){
					brushObject.position = hit.point;
					if(Vector3.Distance(hit.point, lastBrushPoint) > 0.2f){
						brushObject.forward = hit.point - lastPoint;
						lastBrushPoint = hit.point;
					}
				}
				
				particleBrush.size = Mathf.Max(minSize, (1f - speedMultiply*d)* (baseSize - speedSensitivity * d / Time.deltaTime));
				particleBrush.size *= 1f + Random.Range(-randomPercentSize, randomPercentSize);
				lastPoint = hit.point;
				if(particleBrush.AddSegment(hit.point)){
					//Debug.Log(
					ContinueStroke(hit.point);
				}
			}
		}
	}

	protected virtual bool GetInputActive(){
		return Input.GetMouseButton (0);
	}

	protected virtual bool GetInputUp(){
		return Input.GetMouseButtonUp (0);
	}

	protected virtual bool GetClearInput(){
		return Input.GetKeyDown (KeyCode.C);
	}

	protected virtual int GetLayerMask(){
		return PaintLayerMask;
	}

	protected virtual void BeginStroke(Vector3 point){}
	protected virtual void ContinueStroke(Vector3 point){}
	protected virtual void EnterStroke(){}
	protected virtual void OnClear(){}

}
