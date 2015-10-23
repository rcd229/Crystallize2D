using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class KeyDialoguePrequisite2D : DialoguePrerequisite2D {
    public Guid Key { get; set; }

    public override bool IsFulfilled() {
        return PlayerData.Instance.Flags.GetFlag(Key.ToString());
    }
}
