using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMoneyEffect : MonoBehaviour {

	const float Height = 64f;

	public Text text;
	public float duration = 1f;
	public string currency;
	public string amount = "1";
	
	// Use this for initialization
	void Start () {
		text = gameObject.AddComponent<Text> ();
		text.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		text.color = Color.white;
		text.fontStyle = FontStyle.BoldAndItalic;
		text.fontSize = 20;
		text.alignment = TextAnchor.MiddleCenter;
		text.text = "+" + currency + amount;

		gameObject.AddComponent<Outline> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += Vector3.up * Time.deltaTime * duration * Height;
		transform.localScale *= 1f + Time.deltaTime;
		var c = text.color;
		c.a = duration * 2f;
		text.color = c;
		
		duration -= Time.deltaTime;
		if (duration < 0) {
			Destroy(gameObject);
		}
	}
}
