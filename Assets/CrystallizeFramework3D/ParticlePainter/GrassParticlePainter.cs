using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassParticlePainter : ParticlePainter {

	const int TerrainLayerMask = 1 << 9;

	public float eraseRadius = 0.5f;
	public bool isActive = false;

	public bool IsActive {
		get {
			return isActive;
		}
		set {
			isActive = value;
		}
	}

	protected override void Update ()
	{
		if(Input.GetKeyDown(KeyCode.G)){
			Debug.Log("Active: " + isActive);
			isActive = !isActive;
		}

		if (Input.GetKey (KeyCode.LeftShift) && Input.GetMouseButton (0) && isActive) {
			var hit = new RaycastHit();
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				particleBrush.EraseSphere(hit.point, eraseRadius);	
			}
		}

		base.Update ();
	}

	protected override bool GetInputActive ()
	{
		return Input.GetMouseButton (0);
		//return false;
		//return base.GetInputActive () && isActive && !Input.GetKey(KeyCode.LeftShift);
	}

	protected override int GetLayerMask ()
	{
		return TerrainLayerMask;
	}

}
