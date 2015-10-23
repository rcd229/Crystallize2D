using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DialogueBuilder;

[SerializedNPCGroups]
public class StandardNPCGroup : HasDialogueBase {

    public static IEnumerable<NPCGroup> GetValues(){
        return typeof(StandardNPCGroup).GetFieldAndPropertyValues<NPCGroup>(null);
    }

    public static NPCGroup HelloGroup = new NPCGroup(
        "d2529148c0084d8e8959afb38172e06b", "Hello",
        Animation(0, TagLibrary.Bow),
        Line(0, "hello"),
        Animation(1, TagLibrary.Bow),
        Line(1, "hello")
        );

    public static NPCGroup GoodMorningGroup = new NPCGroup(
        "8cde54140cfb42d8bc4e6c6fd4ddb9d1", "Good morning",
        Animation(0, TagLibrary.Bow),
        Line(0, "good morning"),
        Animation(1, TagLibrary.Bow),
        Line(1, "good morning")
        );

    public static NPCGroup GoodbyeGroup = new NPCGroup(
        "b45620bece454d2daf105ba9c2196189", "Goodbye",
        Animation(0, TagLibrary.Wave),
        Line(0, "goodbye"),
        Animation(1, TagLibrary.Wave),
        Line(1, "goodbye"),
        Animation(0, new MovementDialogueAnimation((t) => t.position - 10f * t.forward)),
        Animation(1, new MovementDialogueAnimation((t) => t.position - 10f * t.forward)),
        Animation(0, new WaitDialogueAnimation(1f)),
        Animation(0, new ResetDialogueAnimation())
        );

    public static NPCGroup IntroductionGroup1 = new NPCGroup(
        "eb5e9836830d43ea80ed4bdda77a8931", "Introduction1",
        Line(0, "How do you do?"),
        Animation(0, new GestureDialogueAnimation("Bow")),
        Line(0, "I'm [name]."),
        Line(1, "How do you do?"),
        Animation(1, new GestureDialogueAnimation("Bow")),
        Line(1, "I'm [name].")
        );

    public static NPCGroup IntroductionGroup2 = new NPCGroup(
        "1fa1e6d4a05c4ec3bd51cfea37d0cbb8", "Introduction2",
        Line(0, "Your name?"),
        Line(1, "I'm [name]."),
        Animation(1, new GestureDialogueAnimation("Bow")),
        Line(1, "Nice to meet you.")
        );

    public static NPCGroup AreYouAStudentGroup = new NPCGroup(
        "eff73cb3e43b47e99e1a6f6915d72582", "Are you a student?",
        Line(0, "Are you a student?"),
        Line(1, "Yes, I'm a student.")
        );

}
