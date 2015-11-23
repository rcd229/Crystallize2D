using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueBuilder;

[StaticDialogues]
public class Dialogues01 : ContainsDialogueBase {
    const bool IsTest = true;

    public static DialogueSequence GreetingDialogue {
        get {
            return BuildDialogue(
                IsTest,
                Line("Hi there"),
                Line("What do you need?"),
                Branch(
                    Prompted("Let's chat", Line("Ok")),
                    Prompted("Nothing", Line("See you then"))
                    )
                );
        }
    }
}