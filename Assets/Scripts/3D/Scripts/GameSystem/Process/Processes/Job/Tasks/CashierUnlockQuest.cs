using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuest]
public class CashierUnlockQuest : JobUnlockQuest {
	public override QuestTypeID ID { get { return new QuestTypeID("cc5e48d8591d4e519b18ed2783026d58");}}
	public override string QuestName { get { return "Become a cashier"; } }
	public override JobID JobID {get{return JobID.OpenCashier;}}

	public override string RewardString {
		get { return "You can now do the cashier job"; }
	}
	
	public override DialogueSequence QuestDialogue {
		get {
			return BuildDialogue(
				Line("You want to become a cashier?"),
				Line("How good are you?"),
				Confidence(-3),
				Branch(
				Prompted("I am very good", Line("I see"), Line("You have a strong spirit")),
				Prompted("Yes", Animation(EmoticonType.Annoyed), Confidence(-2), Line("I'm not convinced."), Line("Your spirit is weak")),
				EnglishPrompted("<i>Shake in fear</i>", Animation(EmoticonType.Angry), Confidence(-5), Line("This is a waste of time"))
				),
				Line("What's your name?"),
				Branch(
				Prompted("I'm [name]", Line("[playername] is it?"), Line("I'm [name]"))
				),
				Line("You are hired"),
				Line("Work hard, ok?"),
				Event(ConfidenceSafeEvent.Instance),
				Event(ActionDialogueEvent.Get(QuestUtil.EndQuest, ID)),
				Event(ActionDialogueEvent.Get(UnlockJob))
				);
		}
	}
}
