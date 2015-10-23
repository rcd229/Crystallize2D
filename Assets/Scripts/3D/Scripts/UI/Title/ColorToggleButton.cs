using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorToggleButton : MonoBehaviour {

    public bool isOn = false;

    public void SwitchState() {
        isOn = !isOn;
    }

    void Update() {
        if (isOn) {
            GetComponent<Image>().color = Color.white;
        } else {
            GetComponent<Image>().color = Color.gray;
        }
    }

}
