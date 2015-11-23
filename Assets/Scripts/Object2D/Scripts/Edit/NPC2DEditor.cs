using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Object2DEditor(typeof(NPC2D))]
public class NPC2DEditor : Object2DEditorBase {
    public override void ConstructEditor() {
        var npc = (NPC2D)Object;
        Controls.AddLabel("Dialogue");
        //Controls.AddInputArea(npc.Dialogue.Description, (s) => SetText(npc, s));

        var vals = DialoguePipeline.GetDialoguePaths().ToList();
        var display = vals.Select(v => "..." + v.Substring(Mathf.Max(0, v.Length - 24)));
        var index = vals.IndexOf(npc.DialogueKey);
        Controls.AddDropDown(display, index, (i) => SetDialogue(npc, vals, i), true);
    }

    //void SetText(NPC2D npc, string newText) {
    //    npc.Dialogue.Description = newText;
    //    Save();
    //}

    void SetDialogue(NPC2D npc, List<string> keys, int selected) {
        npc.DialogueKey = keys.GetSafely(selected);
        Save();
    }
}
