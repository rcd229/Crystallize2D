using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ExperienceTextUI : UIBounceInAndFade, IInitializable<int> {

    public Color positiveColor = Color.white;
    public Color negativeColor = Color.white;

    public void Initialize(int amount) {
        var t = GetComponent<Text>();
        if (amount >= 0) {
            t.color = positiveColor;
            t.text = "+" + amount;
        } else {
            t.color = negativeColor;
            t.text = amount.ToString();
            direction = -0.5f;
        }
    }
}
