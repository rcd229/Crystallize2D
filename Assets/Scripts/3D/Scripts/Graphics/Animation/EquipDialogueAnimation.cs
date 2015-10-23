using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipDialogueAnimation : DialogueAnimation {

    string item;

    public EquipDialogueAnimation(string item) {
        this.item = item;
    }

    public override DialogueAnimation GetInstance() {
        return new EquipDialogueAnimation(item);
    }

    public override void Play(GameObject actor) {
        var i = new EquipmentItemRef(item);
        //Debug.Log(actor);
        i.SetTo(actor);

        CoroutineManager.Instance.WaitAndDo(Exit);
    }
}
