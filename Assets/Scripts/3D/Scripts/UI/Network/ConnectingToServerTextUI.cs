using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectingToServerTextUI : MonoBehaviour {

    string[] messages = new string[4];

	// Use this for initialization
	void Start () {
        messages[0] = "Connecting to server<color=black>...</color>";
        messages[1] = "Connecting to server.<color=black>..</color>";
        messages[2] = "Connecting to server..<color=black>.</color>";
        messages[3] = "Connecting to server...";
	}
	
	// Update is called once per frame
	void Update () {
        var index = ((int)Time.time) % 4;
        GetComponent<Text>().text = messages[index];
	}
}
