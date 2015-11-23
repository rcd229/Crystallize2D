using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueBuilder;

[StaticDialogues]
public class Dialogues01  {
    static DialogueSetBuilder setBuilder = new DialogueSetBuilder("Default");

    static DialogueSequenceBuilder GetBuilder() {
        setBuilder.IsTest = true;
        return setBuilder.GetDialogueBuilder();
    }

    public static DialogueSequence GreetingDialogue {
        get {
            var b = GetBuilder();
            b.AddLine("Hi there.");
            b.AddLine("What do you need?");
            return b.Build();
        }
    }
}