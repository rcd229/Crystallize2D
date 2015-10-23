using UnityEngine;
using System.Collections;

public class PathObjectsController : MonoBehaviour {

	public PathFollower[] pathFollowers;

	// Use this for initialization
	void Start () {
		foreach (var pathFollower in pathFollowers) {
			pathFollower.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var pathFollower in pathFollowers) {
			pathFollower.MoveStep();
		}
	}
}
