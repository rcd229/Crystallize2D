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
                IsTest,
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

    public static DialogueSequence StoreFrontGirl
    {
        get
        {
            return BuildDialogue(
                IsTest,
                Line("Hi!"),
                Line("This is the store."),
                Line("You can buy items here such as clothes and food.")
                );
        }
    }

    public static DialogueSequence PathGuy
    {
        get
        {
            return BuildDialogue(
                IsTest,
                Line("Wait!"),
                Line("Do you have a map?"),
                Branch(
                    //if player has gotten map from the map guy, show map and move on
                    Prompted("Yes!", Line("Great! Now you can travel. Good luck!")),
                    //else if player does not have map, cannot move on to next area
                    Prompted("No", Line("You need a map in order to travel."))
                    )
                );
        }

    }

    public static DialogueSequence MapGuy
    {
        get
        {
            return BuildDialogue(
                IsTest,
                Line("Hey! Do you want me to show you the town?"),
                Branch(
                    Prompted("No", Line("Ok then. Goodbye.")),
                    Prompted("Sure", Line("Cool. Follow me!"))
                    ),
                //make map guy move around town - to school, to store, to houses
                Line("This is the school. You can come here to review the words you've learned and talk to other students."),
                Line("This is the store. You can buy useful things here."),
                Line("And this is my house."),
                //give map to player at end of tour if player doesn't have map yet
                Line("That's everything! Thanks for going on my tour. You seem adventurous. Here's a map to help you out!")
                //go into house
                );
        }
    }
}