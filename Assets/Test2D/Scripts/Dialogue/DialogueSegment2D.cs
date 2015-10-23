using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DialogueSegment2D {
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<DialoguePrerequisite2D> Prerequisites { get; set; }
    public DialogueSequence Dialogue { get; set; }

    public DialogueSegment2D() {
        Guid = Guid.NewGuid();
        Name = "New interaction";
        Description = "Add description";
        Prerequisites = new List<DialoguePrerequisite2D>();
        Dialogue = new DialogueSequence();
    }

    public bool IsAvailable() {
        foreach (var prereq in Prerequisites) {
            if (!prereq.IsFulfilled()) return false;
        }
        return true;
    }
}
