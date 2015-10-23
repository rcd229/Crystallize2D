using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevelUpEffect : MonoBehaviour {

	const float Height = 64f;

	public Text text;
	public float duration = 1f;
	public string levelUpString = "Level up!";
	public int fontSize = 20;
	public Color textColor = Color.white;
	public bool rise = true;

	// Use this for initialization
	void Start () {
		text = gameObject.AddComponent<Text> ();
		gameObject.AddComponent<Outline> ();
		text.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		text.color = textColor;
		text.fontStyle = FontStyle.BoldAndItalic;
		text.fontSize = fontSize;
		text.alignment = TextAnchor.MiddleCenter;
		text.text = levelUpString;
		text.resizeTextForBestFit = true;

		gameObject.AddComponent<Outline> ();
	}
	
	// Update is called once per frame
	void Update () {
		var dir = Vector3.up;
		if (!rise) {
			dir = Vector3.down;
		}
		transform.localPosition += dir * Time.deltaTime * duration * Height;

		if (rise) {
			transform.localScale *= 1f + Time.deltaTime;
		}

		var c = text.color;
		c.a = duration * 2f;
		text.color = c;
		
		duration -= Time.deltaTime;
		if (duration < 0) {
			Destroy(gameObject);
		}
	}
}
