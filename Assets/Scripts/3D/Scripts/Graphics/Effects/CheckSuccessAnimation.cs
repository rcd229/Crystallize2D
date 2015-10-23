using UnityEngine;
using System.Collections;

public class CheckSuccessAnimation : UIMonoBehaviour {

    const float StayDuration = 2f;
    float time = 0;

	// Use this for initialization
	void Start () {
        transform.SetParent(WorldCanvas.Instance.transform);
        transform.position = PlayerManager.Instance.PlayerGameObject.transform.position + Vector3.up * 3f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = Camera.main.transform.forward;
        //transform.position += Vector3.up * Time.deltaTime;

        time += Time.deltaTime;
        if (time > StayDuration - 1f) {
            canvasGroup.alpha = 1f - (time - (StayDuration - 1f));
        }

        if (time > StayDuration) {
            Destroy(gameObject);
        }
	}
}
