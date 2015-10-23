using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEnergyRect : MonoBehaviour {

	public float energy = 1f;
	public Color emptyColor = Color.white;
	public Color fullColor = Color.white;
	public Gradient gradient;
	public RectTransform rectTransform { get; set; }

	public float currentEnergy = 0;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentEnergy = Mathf.MoveTowards (currentEnergy, energy, Time.deltaTime);
		rectTransform.localScale = new Vector3 (currentEnergy, 1f, 1f);
		GetComponent<Image> ().color = gradient.Evaluate (currentEnergy); //Color.Lerp (emptyColor, fullColor, energy);
	}
}
