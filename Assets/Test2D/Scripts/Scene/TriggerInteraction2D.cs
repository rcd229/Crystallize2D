using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class TriggerInteraction2D : MonoBehaviour {

    void Start() {
        GetComponent<OneWayTrigger2D>().OnEntered += TriggerInteraction2D_OnEntered;
    }

    void TriggerInteraction2D_OnEntered(object sender, EventArgs e) {
        var interactions = InteractionLoader2D.Load(GetComponent<SceneTrigger2D>().Trigger).Where(i => i.IsAvailable());
        var interaction = interactions.FirstOrDefault();
        Debug.Log("interaction: " + interaction);
        if (interaction != null) {
            var f = new UIFactoryRef<List<PhraseSequence>, object>();
            f.Set<NarrationTextUI>();
            f.Get(
                interaction.Dialogue.Elements.Items
                .Where(ele => ele is LineDialogueElement)
                .Cast<LineDialogueElement>()
                .Select(l => l.Line.Phrase)
                .ToList()
                );
        }
    }
}
