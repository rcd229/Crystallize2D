using UnityEngine;
using System.Collections;

public class TutorialRegisteredObject : MonoBehaviour {

    public string rectName = "";

	// Use this for initialization
	void Start () {
        TutorialCanvas.main.RegisterGameObject(rectName, gameObject);
	}
	
}
