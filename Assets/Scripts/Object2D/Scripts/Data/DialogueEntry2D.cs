using UnityEngine;
using System;
using System.Collections;

public class DialogueEntry2D {
    public Guid Guid { get; set; }
    public string Description { get; set; }

    public DialogueEntry2D() {
        Guid = Guid.NewGuid();
        Description = "";
    }
}
