using UnityEngine;
using System.Collections;

public class AlphaFlashUIEffect : UIMonoBehaviour {

    public float speed = 1f;
    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    canvasGroup.alpha = minAlpha + (maxAlpha - minAlpha) * Mathf.PingPong(Time.time * speed, 1f);
	}

}
