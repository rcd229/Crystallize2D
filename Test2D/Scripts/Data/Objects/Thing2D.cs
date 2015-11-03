using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ThingTemplate2D : IHasGuid, IHasPhraseName {
    public Guid Guid { get; set; }
    public PhraseSequence Name { get; set; }
    public PhraseSequence Description { get; set; }

    public ThingTemplate2D(){
        Guid = Guid.NewGuid();
        Name = new PhraseSequence();
        Description = new PhraseSequence();
    }
}

public class ThingInstance2D : IHasGuid, IHasPhraseName {
    public Guid Guid { get; set; }
    public PhraseSequence Name { get; set; }
    public PhraseSequence Description { get; set; }
    public SerializableVector2 Position { get; set; }
    
    public ThingInstance2D() {
        Guid = Guid.NewGuid();
        Name = new PhraseSequence();
        Description = new PhraseSequence();
        Position = Vector2.zero;
    }

    public ThingInstance2D(ThingTemplate2D template) {
        Guid = Guid.NewGuid();
        Name = template.Name;
        Description = template.Description;
        Position = Vector2.zero;
    }
}
