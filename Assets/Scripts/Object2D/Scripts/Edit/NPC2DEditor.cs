using UnityEngine;
using System.Collections;
using System;

[Object2DEditor(typeof(NPC2D))]
public class NPC2DEditor : Object2DEditorBase {
    public override void ConstructEditor() {
        var npc = (NPC2D)Object;
        Controls.AddLabel("Dialogue text");
        Controls.AddInputArea(npc.Dialogue.Description, (s) => SetText(npc, s));
    }

    void SetText(NPC2D npc, string newText) {
        npc.Dialogue.Description = newText;
        Save();
    }
}
