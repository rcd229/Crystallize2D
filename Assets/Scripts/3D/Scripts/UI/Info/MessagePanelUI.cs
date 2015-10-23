using UnityEngine;
using System.Collections;

public class MessagePanelUI : MonoBehaviour {

    public static MessagePanelUI main { get; private set; }

	// Use this for initialization
	void Awake () {
        main = this;
	}
	
}
