using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UITargetedMessageArgs {

    public RectTransform Target { get; set; }
    public string Message { get; set; }
    public bool RequireConfirmation { get; private set; }

    public UITargetedMessageArgs(RectTransform target, string message, bool requireConfirmation = false) {
        Target = target;
        Message = message;
        RequireConfirmation = requireConfirmation;
    }

}
