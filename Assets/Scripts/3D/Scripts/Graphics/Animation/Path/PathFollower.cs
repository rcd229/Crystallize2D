using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {

	public MovementPath path;
	public float speed = 5f;

	int targetIndex = 0;
	Transform target;

	// Update is called once per frame
	void Update () {
		MoveStep ();
	}

	public void MoveStep(){
		if (!target) {
			target = path.targets[0];
		}

		transform.LookAt (target);
		transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
		
		if (Vector3.Distance (transform.position, target.position) < 0.1f) {
			targetIndex = (targetIndex + 1) % path.targets.Length;
			target = path.targets[targetIndex];
		}
	}

}
