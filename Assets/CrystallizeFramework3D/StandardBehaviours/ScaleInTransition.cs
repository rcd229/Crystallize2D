using UnityEngine;
using System.Collections;

public class ScaleInTransition : MonoBehaviour {

	public AnimationCurve animationCurve = new AnimationCurve();
	public bool destroy = true;

	float speed = 4f;
	float life = 0;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		life = 0;
	}

	public void Reset(){
		life = 0;
	}
	
	// Update is called once per frame
	void Update () {
		life += Time.deltaTime * speed;
		transform.localScale = animationCurve.Evaluate (life) * Vector3.one;
		if (life >= 1f) {
			transform.localScale = Vector3.one;
			if(destroy){
				Destroy(this);
			}
		}
	}
}
