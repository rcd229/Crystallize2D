using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class MouseOverEffect {

    public static void Disable(GameObject target) {
        //Debug.Log("disabling");
        if (target.GetInterface<IMouseOverEffect>() != null) {
            target.GetInterface<IMouseOverEffect>().SetEnabled(false);
        } 
    }

    public static void Enable(GameObject target) {
        //Debug.Log("enabling");
        if (target.GetInterface<IMouseOverEffect>() != null) {
            target.GetInterface<IMouseOverEffect>().SetEnabled(true);
        } else {
            PulseGlow.Get(target).SetEnabled(true);
        }
    }

}
