using UnityEngine;
using System.Collections;


//namespace CrystallizeData {
//<<<<<<< HEAD
//    public class SakuraFirstQuest : StaticSerializedLinearQuestGameData {

//        //states
//        public const string Get = "Get Quest";
//        public const string GetAnnaPreference = "Get Anna Preference";
//        public const string MeetAnna = "Meet Anna";
//        //in between has anna quest
//        public const string BefriendAnna = "Befriend Anna";
//        public const string KnowPresent = "KnowPresent";
//        public const string Done = "Done";

//        protected override void Prepare() {
//            //Find my friend Anna. Help me ask what she wants for birthday present.
//            //There is little hope in asking directly. Make sure you can speak fluently
//            //and befriend her before you ask

//            isTest = true;
//            Initialize("Help Sakura to Speak to Anna", false, QuestTypeID.SakuraFirstQuest);

//            SetStates(Get, GetAnnaPreference, MeetAnna, BefriendAnna, KnowPresent, Done);

//            SetCurrentNPC(NPCID.Sakura);
//            var whatToDo = GetPhrase("What should I do?");
//            AddDialogueStateToCurrent(Get, whatToDo, new SakuraGiveAnnaQuestDialogue().GetDialogue());

//            var getIt = GetPhrase("Sure, let me do it");
//            var tips = GetPhrase("What does she like?");
//            AddDialogueStateToCurrent(GetAnnaPreference, getIt, helperCreateDialogue("Good luck then", MeetAnna));
//            AddDialogueStateToCurrent(GetAnnaPreference, tips, helperCreateDialogue("She likes watching movies", MeetAnna));

//            SetCurrentNPC(NPCID.Anna);
//            var greet = GetPhrase("How do you do");
//            AddDialogueStateToCurrent(MeetAnna, greet, helperCreateDialogue("How do you do", UnlockQuestEvent(QuestTypeID.BecomeFriendWithAnna)));

//            var beFriends = GetPhrase("Are we friends now?");
//            AddDialogueStateToCurrent(BefriendAnna, beFriends, helperCreateDialogue("I guess so", KnowPresent));

//            var askPresent = GetPhrase("What birthday Present Do you want?");
//            AddDialogueStateToCurrent(KnowPresent, askPresent, new AnnaPresentAnswerDialogue().GetDialogue());

//            SetCurrentNPC(NPCID.Sakura);
//            var sheWantsCake = GetPhrase("Anna wants you to buy her a cake");
//            AddDialogueStateToCurrent(Done, sheWantsCake, new SakuraKnowPresentDialogue().GetDialogue());
//        }
//    }

//    public class BefriendOfAnnaQuest : StaticSerializedLinearQuestGameData {

//        public const string Introduction = "Introduction";
//        public const string PlayHobby = "play hobby";
//        public const string wentMovie = "went movie";

//        protected override void Prepare() {
//            isTest = true;
//            Initialize("Become a friend of anna", false, QuestTypeID.BecomeFriendWithAnna);
//            SetStates(Introduction, PlayHobby, wentMovie);

//            SetCurrentNPC(NPCID.Anna);
//            var intro = GetPhrase("I heard you from Sakura");
//            AddDialogueStateToCurrent(Introduction, intro, helperCreateDialogue("I'm Anna. Nice to meet you", PlayHobby, SetViewedFunc()));

//            var suggestPlay = GetPhrase("Do you mind me joining you>");
//            AddDialogueStateToCurrent(PlayHobby, suggestPlay, helperCreateDialogue("Please come. What do you want to do?", wentMovie));

//            var suggestMovie = GetPhrase("Let's go to movies");
//            var suggestFood = GetPhrase("Let's go buy some snacks");
//            var suggestShopping = GetPhrase("Let's go shopping");
//            AddDialogueStateToCurrent(wentMovie, suggestShopping, helperCreateDialogue("I'm not interested in shopping", wentMovie, ConfidenceEvent(-1)));// ReduceConfiFunc()));
//            AddDialogueStateToCurrent(wentMovie, suggestFood, helperCreateDialogue("Sorry. I'm not hungry", wentMovie, ConfidenceEvent(-1)));  //ReduceConfiFunc()));
//            AddDialogueStateToCurrent(wentMovie, suggestMovie, helperCreateDialogue("Let's go!", EndFunc(), UpdateStateEvent(SakuraFirstQuest.BefriendAnna, QuestTypeID.SakuraFirstQuest)));
//        }
//    }

//    public class SakuraGiveAnnaQuestDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("My friend Anna is having her birthday");
//            AddLine("I want to know what present she wants");
//            AddLine("Please help me to get this information");
//            AddEvents(
//                ActionDialogueEvent.Get(QuestUtil.SetQuestState, QuestTypeID.SakuraFirstQuest, SakuraFirstQuest.GetAnnaPreference),
//                ActionDialogueEvent.Get(QuestUtil.SetViewed, QuestTypeID.SakuraFirstQuest),
//                ActionDialogueEvent.Get(QuestUtil.RaiseFlag, NPCQuestFlag.AnnaIntroduced)
//                );
//        }
//    }

//    public class AnnaPresentAnswerDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("Thank you");
//            AddLine("Anything is good");
//            AddLine("But I do hope my friend Anna can buy me a cake");
//            AddEvent(ActionDialogueEvent.Get(QuestUtil.SetQuestState, QuestTypeID.SakuraFirstQuest, SakuraFirstQuest.Done));
//        }
//    }

//    public class SakuraKnowPresentDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("Thank you");
//            AddLine("You saved me");
//            AddLine("How ever you need a job for my letter of recommendation");
//            AddLine("Find a job and come back to me please");
//            AddEvent(ActionDialogueEvent.Get(QuestUtil.EndQuest, QuestTypeID.SakuraFirstQuest));
//        }
//    }
//=======
//    public class SakuraFirstQuest : StaticSerializedLinearQuestGameData {
//
//        //states
//        public const string Get = "Get Quest";
//        public const string GetAnnaPreference = "Get Anna Preference";
//        public const string MeetAnna = "Meet Anna";
//        //in between has anna quest
//        public const string BefriendAnna = "Befriend Anna";
//        public const string KnowPresent = "KnowPresent";
//        public const string Done = "Done";
//
//        protected override void Prepare() {
//            //Find my friend Anna. Help me ask what she wants for birthday present.
//            //There is little hope in asking directly. Make sure you can speak fluently
//            //and befriend her before you ask
//
//            isTest = true;
//            Initialize("Help Sakura to Speak to Anna", false, QuestTypeID.SakuraFirstQuest);
//
//            SetStates(Get, GetAnnaPreference, MeetAnna, BefriendAnna, KnowPresent, Done);
//
//            SetCurrentNPC(NPCID.Sakura);
//            var whatToDo = GetPhrase("What should I do?");
//            AddDialogueStateToCurrent(Get, whatToDo, new SakuraGiveAnnaQuestDialogue().GetDialogue());
//
//            var getIt = GetPhrase("Sure, let me do it");
//            var tips = GetPhrase("What does she like?");
//            AddDialogueStateToCurrent(GetAnnaPreference, getIt, helperCreateDialogue("Good luck then", MeetAnna));
//            AddDialogueStateToCurrent(GetAnnaPreference, tips, helperCreateDialogue("She likes watching movies", MeetAnna));
//
//            SetCurrentNPC(NPCID.Anna);
//            var greet = GetPhrase("How do you do");
//            AddDialogueStateToCurrent(MeetAnna, greet, helperCreateDialogue("How do you do", UnlockQuestEvent(QuestTypeID.BecomeFriendWithAnna)));
//
//            var beFriends = GetPhrase("Are we friends now?");
//            AddDialogueStateToCurrent(BefriendAnna, beFriends, helperCreateDialogue("I guess so", KnowPresent));
//
//            var askPresent = GetPhrase("What birthday Present Do you want?");
//            AddDialogueStateToCurrent(KnowPresent, askPresent, new AnnaPresentAnswerDialogue().GetDialogue());
//
//            SetCurrentNPC(NPCID.Sakura);
//            var sheWantsCake = GetPhrase("Anna wants you to buy her a cake");
//            AddDialogueStateToCurrent(Done, sheWantsCake, new SakuraKnowPresentDialogue().GetDialogue());
//        }
//    }
//
//    public class BefriendOfAnnaQuest : StaticSerializedLinearQuestGameData {
//
//        public const string Introduction = "Introduction";
//        public const string PlayHobby = "play hobby";
//        public const string wentMovie = "went movie";
//
//        protected override void Prepare() {
//            isTest = true;
//            Initialize("Become a friend of anna", false, QuestTypeID.BecomeFriendWithAnna);
//            SetStates(Introduction, PlayHobby, wentMovie);
//
//            SetCurrentNPC(NPCID.Anna);
//            var intro = GetPhrase("I heard you from Sakura");
//            AddDialogueStateToCurrent(Introduction, intro, helperCreateDialogue("I'm Anna. Nice to meet you", PlayHobby, SetViewedFunc()));
//
//            var suggestPlay = GetPhrase("Do you mind me joining you>");
//            AddDialogueStateToCurrent(PlayHobby, suggestPlay, helperCreateDialogue("Please come. What do you want to do?", wentMovie));
//
//            var suggestMovie = GetPhrase("Let's go to movies");
//            var suggestFood = GetPhrase("Let's go buy some snacks");
//            var suggestShopping = GetPhrase("Let's go shopping");
//            AddDialogueStateToCurrent(wentMovie, suggestShopping, helperCreateDialogue("I'm not interested in shopping", wentMovie, ConfidenceEvent(-1)));// ReduceConfiFunc()));
//            AddDialogueStateToCurrent(wentMovie, suggestFood, helperCreateDialogue("Sorry. I'm not hungry", wentMovie, ConfidenceEvent(-1)));  //ReduceConfiFunc()));
//            AddDialogueStateToCurrent(wentMovie, suggestMovie, helperCreateDialogue("Let's go!", EndFunc(), UpdateStateEvent(SakuraFirstQuest.BefriendAnna, QuestTypeID.SakuraFirstQuest)));
//        }
//    }
//
//    public class SakuraGiveAnnaQuestDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("My friend Anna is having her birthday");
//            AddLine("I want to know what present she wants");
//            AddLine("Please help me to get this information");
//            AddEvents(
//                ActionDialogueEvent.Get(FreeExploreQuestManager.SetQuestState, QuestTypeID.SakuraFirstQuest, SakuraFirstQuest.GetAnnaPreference),
//                ActionDialogueEvent.Get(FreeExploreQuestManager.SetViewed, QuestTypeID.SakuraFirstQuest),
//                ActionDialogueEvent.Get(FreeExploreQuestManager.RaiseFlag, NPCQuestFlag.AnnaIntroduced)
//                );
//        }
//    }
//
//    public class AnnaPresentAnswerDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("Thank you");
//            AddLine("Anything is good");
//            AddLine("But I do hope my friend Anna can buy me a cake");
//            AddEvent(ActionDialogueEvent.Get(FreeExploreQuestManager.SetQuestState, QuestTypeID.SakuraFirstQuest, SakuraFirstQuest.Done));
//        }
//    }
//
//    public class SakuraKnowPresentDialogue : StaticSerializedDialogueGameData {
//        protected override void PrepareGameData() {
//            isTest = true;
//            AddLine("Thank you");
//            AddLine("You saved me");
//            AddLine("How ever you need a job for my letter of recommendation");
//            AddLine("Find a job and come back to me please");
//            AddEvent(ActionDialogueEvent.Get(FreeExploreQuestManager.EndQuest, QuestTypeID.SakuraFirstQuest));
//        }
//    }
//>>>>>>> origin/master
//}
