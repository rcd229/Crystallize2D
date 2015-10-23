using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EarnMoneyModifierUI : MonoBehaviour {

    public Text labelText;
    public Text valueText;

    public void Initialize(ValueModifier modifier) {
        labelText.text = modifier.Label;
        valueText.text = string.Format("{0}%", Mathf.RoundToInt(modifier.Value*100f));
    }

}
