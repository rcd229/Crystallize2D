using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIComponent : MonoBehaviour {
    public Text label;

    public void SetLabel(string label) {
        this.label.text = label;
    }
}
