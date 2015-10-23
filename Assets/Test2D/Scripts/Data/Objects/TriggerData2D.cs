using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TriggerData2D : IHasGuid, IHasPhraseName {
    public Guid Guid { get; set; }
    public PhraseSequence Name { get; set; }
    public int Size { get; set; }
    public int Orientation { get; set; }
    public SerializableVector2 Position { get; set; }

    public TriggerData2D() {
        Guid = Guid.NewGuid();
        Name = new PhraseSequence();
        Size = 1;
        Orientation = 0;
        Position = Vector2.zero;
    }
}
