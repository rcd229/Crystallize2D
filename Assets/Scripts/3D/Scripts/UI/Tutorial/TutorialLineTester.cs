using UnityEngine;
using System.Collections;

public class TutorialLineTester : MonoBehaviour {

	public RectTransform t1;
	public RectTransform t2;

	// Use this for initialization
	void Start () {
		TutorialCanvas.main.CreateUILine (t1, t2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
