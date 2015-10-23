using UnityEngine;
using System.Collections;
using System;

namespace CrystallizeData {
//    public class FindCatQuest : StaticSerializedLinearQuestGameData {
//        public const string getQuest = "Get";
//        public const string knowLost = "Know lost";
//        public const string knowColor = "Know Color";
//        public const string Done = "Done";
//
//        protected override void Prepare() {
//            isTest = true;
//            PhraseSequence offerHelp = GetPhrase("Can I help you");
//            PhraseSequence askColor = GetPhrase("what's its color?");
//            PhraseSequence giveCat = GetPhrase("Here it is");
//
//            Initialize("Find Cat", true, QuestTypeID.FindCatQuest);
//
//            SetStates(getQuest, knowLost, knowColor, Done);
//            //focus on a npc
//            SetCurrentNPC(NPCID.FindCatNPC);
//            AddDialogueStateToCurrent(getQuest, offerHelp, helperCreateDialogue("I lost my cat", knowLost, SetViewedFunc()));
//            AddDialogueStateToCurrent(knowLost, askColor, helperCreateDialogue("It is black", knowColor, RaiseQuestFlagFunc(NPCQuestFlag.CatColorKnown)));
//
//            var endDialogue = helperCreateDialogue("Thanks", EndFunc(QuestTypeID.FindCatQuest), UnlockQuestEvent(QuestTypeID.PersonHungryQuest));
//            AddDialogueStateToCurrent(Done, giveCat, endDialogue);
//            //focus on another npc
//            SetCurrentNPC(NPCID.FeelHungryNPC);
//
//            AddDialogueStateToCurrent(knowColor, new PhraseSequence("Have you seen a black cat?"),
//                                      helperCreateDialogue("Yes", Done, RaiseQuestFlagFunc(NPCQuestFlag.CatFound)));
//        }
//    }

//    public class PersonHungryQuest : StaticSerializedLinearQuestGameData {
//        public const string getQuest = "Get";
//        public const string knowFood = "knowFood";
//        public const string knowNoMoney = "knowNoMoney";
//        public const string Done = "Done";
//
//        protected override void Prepare() {
//            isTest = true;
//            Initialize("Find Hungry Person Food", false, QuestTypeID.PersonHungryQuest);
//            SetStates(getQuest, knowFood, knowNoMoney, Done);
//
//            SetCurrentNPC(NPCID.FeelHungryNPC);
//            AddDialogueStateToCurrent(getQuest, new PhraseSequence("How are you"), helperCreateDialogue("I'm hungry", knowFood, SetViewedFunc()));
//            AddDialogueStateToCurrent(knowFood, new PhraseSequence("What do you want to eat"), helperCreateDialogue("I want sushi", knowNoMoney));
//            AddDialogueStateToCurrent(knowNoMoney, new PhraseSequence("Over there is a restaurant"), helperCreateDialogue("I have no money", Done));
//            var endDialogue = helperCreateDialogue("Thanks", EndFunc(QuestTypeID.PersonHungryQuest), UnlockQuestEvent(QuestTypeID.KnowSakuraQuest));
//                //() => FreeExploreQuestManager.Instance.EndQuest(QuestID.PersonHungryQuest), UnlockQuestEvent(QuestID.KnowSakuraQuest));
//            AddDialogueStateToCurrent(Done, new PhraseSequence("Let me buy you"), endDialogue);
//
//        }
//    }
}
