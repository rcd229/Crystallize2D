using UnityEngine;
using System.Collections;

public class MoneyEffect : MonoBehaviour {

	public float duration = 1f;
	public string currency;
	public string amount = "1";

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "+" + currency + amount;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += Vector3.up * Time.deltaTime * duration;
		transform.localScale *= 1f + Time.deltaTime;
		var textMesh = GetComponent<TextMesh> ();
		var c = textMesh.color;
		c.a = duration;
		textMesh.color = c;

		duration -= Time.deltaTime;
		if (duration < 0) {
			Destroy(gameObject);
		}
	}
}
