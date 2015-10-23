using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EmoticonDialogueAnimation : DialogueAnimation {

    string type;

    public EmoticonDialogueAnimation(string typeKey) {
        this.type = typeKey;
    }

    public EmoticonDialogueAnimation() {
        type = EmoticonType.Annoyed.TypeKey;
    }

    public EmoticonDialogueAnimation(EmoticonType emoticon) {
        this.type = emoticon.TypeKey;
    }

    public override DialogueAnimation GetInstance() {
        return new EmoticonDialogueAnimation(type);
    }

    public override void Play(GameObject actor) {
        UILibrary.Emoticon.Get(new EmoticonInitArgs(actor.transform, EmoticonType.Get(type)), (s, e) => Exit());
    }

}
