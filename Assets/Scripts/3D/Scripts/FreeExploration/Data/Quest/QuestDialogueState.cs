using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;


//for each npc, the quest stores a list of dialogueElements
//each element will have a prerequisite state
public class QuestDialogueState {
    public string State { get; set; }
    public PhraseSequence Prompt { get; set; }
    public DialogueSequence Dialogue { get; set; }

    [XmlIgnore]
    public Func<ContextData> ContextGetter { get; set; }

    public QuestDialogueState(string state, DialogueSequence dialogue, PhraseSequence prompt, Func<ContextData> contextGetter = null) {
        this.State = state;
        this.Dialogue = dialogue;
        this.Prompt = prompt;
        this.ContextGetter = contextGetter;
    }

}