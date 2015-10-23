using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour, ICameraController {

	public Transform target;

	float speed = 10f;
	bool isController = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isController){
            transform.position = PlayerManager.Instance.PlayerGameObject.transform.position;

			var cam = OmniscientCamera.main;
			var dist = Vector3.Distance(cam.transform.position, target.position);
			if(dist > 0.1f){
				var moveDist = speed * Time.deltaTime;
				cam.SetPosition(Vector3.MoveTowards(cam.transform.position, target.position, moveDist));
				cam.SetRotation(Quaternion.Lerp(cam.transform.rotation, target.rotation, moveDist / dist));
			} else {
				cam.SetPosition(target.position);
				cam.SetRotation(target.rotation);
			}
		}
	}

	public void SetAsController(bool isController){
		this.isController = isController;
		if (isController) {
			OmniscientCamera.main.Suspend();
		} else {
			OmniscientCamera.main.Resume();
		}
	}

}
