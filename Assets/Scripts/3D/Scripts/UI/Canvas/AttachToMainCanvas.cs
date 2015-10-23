using UnityEngine;
using System.Collections;

public class AttachToMainCanvas : MonoBehaviour {

    // Use this for initialization
    void Start() {
        MainCanvas.main.Add(transform);
    }

}
