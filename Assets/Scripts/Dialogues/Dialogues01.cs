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
                    Prompted("Let's chat", 
                    //add event here (allow person to move on)
                    Line("Ok")),
                    Prompted("Nothing", Line("See you then"))
                    )
                );
        }
    }

    public static DialogueSequence NewStuff
    {
        get
        {
            return BuildDialogue(
                false,
                Line("What's up"),
                Line("What do you need?"),
                Branch(
                    Prompted("Let's chat",
                    //add event here (allow person to move on)
                    Line("Ok")),
                    Prompted("Nothing", Line("See you then"))
                    )
                );
        }

    }
}