using UnityEngine;
using System.Collections;

//namespace CrystallizeData {
//<<<<<<< HEAD
//    public class KnowSakuraQuest : StaticSerializedLinearQuestGameData<ResetableLinearQuestStateMachine> {
//        public const string askLetter = "ask letter";
//        public const string suggestSakura = "Suggest Sakura";
//        public const string learnWords = "Learn Words";
//        public const string talkToSakura = "talk to Sakura";
//        public const string askSakura = "Ask Sakura for letter";
////        public const string Done = "Done";

//        protected override void Prepare() {
//            isTest = true;
//            Initialize("Get to know Sakura", false, QuestTypeID.KnowSakuraQuest);
//            SetStates(askLetter, suggestSakura, learnWords, talkToSakura, askSakura);

//            SetCurrentNPC(NPCID.FeelHungryNPC);
//            var askForLetter = GetPhrase("Can you give me a recommendation letter?");
//            AddDialogueStateToCurrent(askLetter, askForLetter, new AskHungryLetterDialogue().GetDialogue());

//            SetCurrentNPC(NPCID.Sakura);
//            var greeting = GetPhrase("Nice to meet you");
//            AddDialogueStateToCurrent(learnWords, greeting, helperCreateDialogue("Nice to meet you", talkToSakura));
//            AddDialogueStateToCurrent(talkToSakura, askForLetter, helperCreateDialogue("I need your help before I can decide", askSakura));
//            var yes = GetPhrase("I would like to");
//            var no = GetPhrase("Sorry");
//            AddDialogueStateToCurrent(askSakura, yes, helperCreateDialogue("very well", EndFunc(), UnlockQuestEvent(QuestTypeID.SakuraFirstQuest)));
//            AddDialogueStateToCurrent(askSakura, no, helperCreateDialogue("I can't just give you the letter then", talkToSakura));

//            SetCurrentNPC(NPCID.Tourist1);
//            var askTeach = GetPhrase("Let's talk");
//            AddDialogueStateToCurrent(suggestSakura, askTeach, new TalkToTouristAndLearnWordsDialogue().GetDialogue());
//        }
//    }

//    public class TalkToTouristAndLearnWordsDialogue : StaticSerializedDialogueGameData {
//        BranchRef b1 = new BranchRef();
//        BranchRef b2 = new BranchRef();
//        BranchRef b3 = new BranchRef();
//        BranchRef b4 = new BranchRef();

//        protected override void PrepareGameData() {
//            isTest = true;

//            b1.Prompt = GetPhrase("Hello");
//            b2.Prompt = GetPhrase("What is your name");
//            b3.Prompt = GetPhrase("Nice to meet you");
//            b4.Prompt = GetPhrase("Thank you for teaching me");

//            AddBranch(false, true, b1, b2, b3, b4);

//            b1.Index = AddLine("Can I help you");
//            Break();
//            b2.Index = AddLine("I am Nazu");
//            Break();
//            b3.Index = AddLine("Nice to meet you");
//            Break();

//            b4.Index = AddLine("No problem");
//            AddEvent(() => QuestUtil.SetQuestState(QuestTypeID.KnowSakuraQuest, KnowSakuraQuest.learnWords));
//        }
//    }

//    public class AskHungryLetterDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;

//            AddLine("I have given my letter");
//            AddLine("You can look for Sakura");

//            AddEvent(() => {
//                QuestUtil.SetViewed(QuestTypeID.KnowSakuraQuest);
//                QuestUtil.SetQuestState(QuestTypeID.KnowSakuraQuest, KnowSakuraQuest.suggestSakura);
//                QuestUtil.RaiseFlags(NPCQuestFlag.SakuraIntroduced);
//            });


//            AddLine("Learn some words before you talk to her");
//        }
//    }
//=======
//    public class KnowSakuraQuest : StaticSerializedLinearQuestGameData<ResetableLinearQuestStateMachine> {
//        public const string askLetter = "ask letter";
//        public const string suggestSakura = "Suggest Sakura";
//        public const string learnWords = "Learn Words";
//        public const string talkToSakura = "talk to Sakura";
//        public const string askSakura = "Ask Sakura for letter";
////        public const string Done = "Done";
//
//        protected override void Prepare() {
//            isTest = true;
//            Initialize("Get to know Sakura", false, QuestTypeID.KnowSakuraQuest);
//            SetStates(askLetter, suggestSakura, learnWords, talkToSakura, askSakura);
//
//            SetCurrentNPC(NPCID.FeelHungryNPC);
//            var askForLetter = GetPhrase("Can you give me a recommendation letter?");
//            AddDialogueStateToCurrent(askLetter, askForLetter, new AskHungryLetterDialogue().GetDialogue());
//
//            SetCurrentNPC(NPCID.Sakura);
//            var greeting = GetPhrase("Nice to meet you");
//            AddDialogueStateToCurrent(learnWords, greeting, helperCreateDialogue("Nice to meet you", talkToSakura));
//            AddDialogueStateToCurrent(talkToSakura, askForLetter, helperCreateDialogue("I need your help before I can decide", askSakura));
//            var yes = GetPhrase("I would like to");
//            var no = GetPhrase("Sorry");
//            AddDialogueStateToCurrent(askSakura, yes, helperCreateDialogue("very well", EndFunc(), UnlockQuestEvent(QuestTypeID.SakuraFirstQuest)));
//            AddDialogueStateToCurrent(askSakura, no, helperCreateDialogue("I can't just give you the letter then", talkToSakura));
//
//            SetCurrentNPC(NPCID.Tourist1);
//            var askTeach = GetPhrase("Let's talk");
//            AddDialogueStateToCurrent(suggestSakura, askTeach, new TalkToTouristAndLearnWordsDialogue().GetDialogue());
//        }
//    }
//
//    public class TalkToTouristAndLearnWordsDialogue : StaticSerializedDialogueGameData {
//        BranchRef b1 = new BranchRef();
//        BranchRef b2 = new BranchRef();
//        BranchRef b3 = new BranchRef();
//        BranchRef b4 = new BranchRef();
//
//        protected override void PrepareGameData() {
//            isTest = true;
//
//            b1.Prompt = GetPhrase("Hello");
//            b2.Prompt = GetPhrase("What is your name");
//            b3.Prompt = GetPhrase("Nice to meet you");
//            b4.Prompt = GetPhrase("Thank you for teaching me");
//
//            AddBranch(false, true, b1, b2, b3, b4);
//
//            b1.Index = AddLine("Can I help you");
//            Break();
//            b2.Index = AddLine("I am Nazu");
//            Break();
//            b3.Index = AddLine("Nice to meet you");
//            Break();
//
//            b4.Index = AddLine("No problem");
//            AddEvent(() => FreeExploreQuestManager.SetQuestState(QuestTypeID.KnowSakuraQuest, KnowSakuraQuest.learnWords));
//        }
//    }
//
//    public class AskHungryLetterDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//
//            AddLine("I have given my letter");
//            AddLine("You can look for Sakura");
//
//            AddEvent(() => {
//                FreeExploreQuestManager.SetViewed(QuestTypeID.KnowSakuraQuest);
//                FreeExploreQuestManager.SetQuestState(QuestTypeID.KnowSakuraQuest, KnowSakuraQuest.suggestSakura);
//                FreeExploreQuestManager.RaiseFlags(NPCQuestFlag.SakuraIntroduced);
//            });
//
//
//            AddLine("Learn some words before you talk to her");
//        }
//    }
//>>>>>>> origin/master
//}
