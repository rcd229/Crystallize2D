using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ValueModifier {

    public string Label { get; set; }
    public float Value { get; set; }

    public ValueModifier(string label, float value) {
        Label = label;
        Value = value;
    }

}
