using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuest]
public class JanitorUnlockQuest : JobUnlockQuest {
    public override QuestTypeID ID { get { return new QuestTypeID(new Guid("708389cc56f043beaf2956bab0f5f84c")); } }
    public override string QuestName { get { return "Become a janitor"; } }

    public override string RewardString {
        get { return "You can now do the janitor job"; }
    }

    public override DialogueSequence QuestDialogue {
        get {
            return BuildDialogue(
                Line("You want to become a janitor?"),
                Line("I don't believe you can do it."),
                Confidence(-5),
                Branch(
                    Prompted("I can definitely do it", Line("I see"), Line("You have a strong spirit")),
                    Prompted("Yes", Animation(EmoticonType.Annoyed), Confidence(-3), Line("I'm not convinced."), Line("Your spirit is weak")),
                    EnglishPrompted("<i>Shake in fear</i>", Animation(EmoticonType.Angry), Confidence(-5), Line("This is a waste of time"))
                ),
                Line("What's your name?"),
                Branch(
                    Prompted("I'm [name]", Line("[playername] is it?"), Line("I'm [name]"))
                ),
                Line("I guess you can join"),
                Line("Work hard, ok?"),
                Event(ConfidenceSafeEvent.Instance),
                Event(ActionDialogueEvent.Get(QuestUtil.EndQuest, ID)),
                Event(ActionDialogueEvent.Get(UnlockJob))
            );
        }
    }
}
