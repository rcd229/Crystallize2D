using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CrystallizeData {

    public class TestExplorer : StaticSerializedJobGameData {
        public class TestExplorerTask : StaticSerializedTaskGameData<JobTaskGameData> {
            protected override void PrepareGameData() {
                Initialize("Explore freely", "SchoolClassroomSession"); //"NPCTest");
                SetProcess<FreeExploreProcess_ListenTutorial>();
            }
        }

        protected override void PrepareGameData() {
            Initialize(JobID.TestExplorer, "TestExplorer", 0, 1000);
            AddTask<TestExplorerTask>();

            job.TaskSelector = new JobTaskSelectorGameData();
            job.Money = 0; ;

            job.Hide = true;
        }
    }

    public class FreeExplorer : StaticSerializedJobGameData {
        class FreeExploreTask : StaticSerializedTaskGameData<JobTaskGameData> {
            protected override void PrepareGameData() {
                Initialize("Explore freely", "SchoolClassroomSession"); //"OutdoorSchoolSession");
                SetProcess<FreeExploreProcessSelector>();
            }
        }

        protected override void PrepareGameData() {
            Initialize(JobID.FreeExplore, "FreeExplore", 0, 1000);
            AddTask<FreeExploreTask>();

            job.TaskSelector = new JobTaskSelectorGameData();
            job.Money = 0; ;

            job.Hide = true;
        }
    }

    public class StandardNPC1 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.TestNPC1, "StandardNPC1");
            SetEntryDialogue(new StandardNPC1Dialogue().GetDialogue());
        }
    }

    public class TutorialNPC1 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.TutorialNPC1, "Person 1");
            NPC.FlagPrerequisites.Add(NPCQuestFlag.TutorialConversations);

            var b = new DialogueSequenceBuilder(Name);
            b.AddLine("hello");
            var b1 = new BranchRef();
            var b2 = new BranchRef();
            b.AddJapaneseBranch(b1, b2);

            b1.Prompt = b.GetPhrase("hello");
            b1.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
            b.AddAnimation(new WaitDialogueAnimation(1f));
            b.Break();

            b2.Prompt = b.GetPhrase("good morning");
            b2.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
            b.AddLine("good morning");
            b.Break();

            SetEntryDialogue(b.Build());
        }
    }

    public class TutorialNPC2 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.TutorialNPC2, "Person 2");
            NPC.FlagPrerequisites.Add(NPCQuestFlag.TutorialConversations);

            var b = new DialogueSequenceBuilder(Name);
            b.AddLine("Nice to meet you.");
            var b1 = new BranchRef();
            var b2 = new BranchRef();
            b.AddJapaneseBranch(b1, b2);

            b1.Prompt = b.GetPhrase("Nice to meet you.");
            b1.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
            b.AddAnimation(new WaitDialogueAnimation(1f));
            b.Break();

            b2.Prompt = b.GetPhrase("hello");
            b2.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
            b.AddAnimation(new WaitDialogueAnimation(1f));
            b.Break();

            SetEntryDialogue(b.Build());
        }
    }

    //public class TutorialNPC3 : StaticSerializedQuestNPCGameData {
    //    protected override void PrepareGameData() {
    //        Initialize(NPCID.TutorialNPC3, "Ken");
    //        NPC.QuestID = new FindSakuraQuest().ID;
    //        NPC.FlagPrerequisites.Add(NPCQuestFlag.TutorialConversations);

    //        var b = new DialogueSequenceBuilder(Name);
    //        b.AddEnglishLine("Can you greet the new student?");
    //        b.AddEvent(ActionDialogueEvent.Get(QuestUtil.UnlockQuest, FindSakuraQuest.QuestID));
    //        b.AddEnglishLine("My Japanese isn't so great...");

    //        SetEntryDialogue(b.Build());
    //    }
    //}

    //public class NewGirl1 : StaticSerializedQuestNPCGameData {
    //    protected override void PrepareGameData() {
    //        Initialize(NPCID.NewStudent, "New student");
    //        var charData = NPCCharacterData.GetRandom();
    //        charData.Appearance.Gender = (int)AppearanceGender.Female;
    //        NPC.CharacterData = charData;
    //        //NPC.FlagPrerequisites.Add(FindSakuraQuest.States);

    //        var b = new DialogueSequenceBuilder(Name);
    //        b.AddLine("hello");
    //        var b0 = new BranchRef();
    //        var b1 = new BranchRef();
    //        var b2 = new BranchRef();
    //        b.AddJapaneseBranch(b0, b1, b2);

    //        b0.Prompt = b.GetPhrase("What's your name?");
    //        b0.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
    //        b.AddLine("I am Sakura.");
    //        b.OpenBranch();

    //        b1.Prompt = b.GetPhrase("Nice to meet you.");
    //        b1.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
    //        b.AddLine("Nice to meet you.");
    //        b.AddLine("I'm Sakura.");
    //        b.OpenBranch();

    //        b2.Prompt = b.GetPhrase("hello");
    //        b2.Index = b.AddAnimation(new GestureDialogueAnimation(TagLibrary.Bow));
    //        b.AddAnimation(new WaitDialogueAnimation(1f));
    //        b.Break();

    //        var b3 = new BranchRef();
    //        var b4 = new BranchRef();
    //        b.CloseBranches(b.AddJapaneseBranch(b3, b4));

    //        b3.Prompt = b.GetPhrase("I'm [name]");
    //        b3.Index = b.AddAnimation(EmoticonType.Happy);
    //        b.AddLine("Please remember me.");
    //        b.AddEvent(ActionDialogueEvent.Get(QuestUtil.SetQuestState, FindSakuraQuest.QuestID, FindSakuraQuest.States[1]));
    //        b.Break();

    //        b4.Prompt = b.GetPhrase("Goodbye");
    //        b4.Index = b.AddAnimation(EmoticonType.Sad);
    //        b.AddLine("Goodbye");
    //        b.Break();

    //        SetEntryDialogue(b.Build());
    //    }
    //}

    public class HelpNPC1 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.HelpNPC1, "Guide");

            var b = new DialogueSequenceBuilder(Name);

            var b1 = new BranchRef();
            var b2 = new BranchRef();
            //var b3 = new BranchRef();
            var b4 = new BranchRef();
            b.breakOnBranches = false;
            b.IsTest = true;

            //b.AddAnimation(new EmoticonDialogueAnimation(EmoticonType.Happy));
            b.AddLine("So, you're here trying to become a Japanese citizen too?");
            //b.AddAnimation(new EmoticonDialogueAnimation(EmoticonType.Annoyed));
            b.AddLine("It looks like you just got here, so let me fill you in.");
            b.AddLine("In order to become a citizen, you will need to <b>master the Japanese language</b> and <b>find Japanese friends</b> to support you.");
            b.AddLine("It's going to be hard!");
            b.AddLine("Let me know if you have questions.");

            var questionStart = b.AddBranch(false, false, false, b1, b2, b4);
            b.Break();

            var restartElement = b.AddLine("Do you have other questions?");
            b.thisElement.DefaultNextID = questionStart;
            b.Break();

            b1.Prompt = b.GetPhrase("Ask about <b>learning words</b>");
            b1.Index = b.AddLine("You can learn words by talking to people.");
            b.AddLine("If you walk around, you can definitely find some people who will talk to you.");
            b.thisElement.DefaultNextID = restartElement;
            b.Break();

            b2.Prompt = b.GetPhrase("Ask about <b>confidence</b>");
            b2.Index = b.AddLine("When you hear words you don't know, your confidence go down.");
            b.AddLine("Sometimes people will say things that cause your confidence to drop as well.");
            b.thisElement.DefaultNextID = restartElement;
            b.Break();

            //b3.Prompt = b.GetPhrase("Ask about <b>making friends</b>");
            //b3.Index = b.AddLine("You can make friends by helping them out with certain tasks.");
            //b3.Index = b.AddLine("You should be able to figure it out as you go.");
            //b.thisElement.DefaultNextID = restartElement;
            //b.Break();

            b4.Prompt = b.GetPhrase("Leave");
            b4.Index = b.AddLine("Good luck then!");
            b.Break();

            SetEntryDialogue(b.Build());
        }
    }

    public class StandardNPC1Dialogue : StaticSerializedDialogueGameData {
        BranchRef b1 = new BranchRef();
        BranchRef b2 = new BranchRef();
        BranchRef b3 = new BranchRef();

        protected override void PrepareGameData() {
            //isTest = true;

            b1.Prompt = GetPhrase("hello");
            b2.Prompt = GetPhrase("goodbye");
            //b3.Prompt = GetPhrase("welcome");

            AddJapaneseBranch(b1, b2);

            b1.Index = AddLine("hello");
            AddEvent(1);
            Break();

            b2.Index = AddLine("ok, see you then...");
            AddEvent(-2);
            Break();

            //Debug.Log("Total conf: " + dialogue.GetConfidenceCost());
        }

    }

    public class StandardNPC2 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.TestNPC2, "StandardNPC2");
            SetEntryDialogue(new StandardNPC2Dialogue().GetDialogue());
        }
    }

    public class StandardNPC2Dialogue : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddLine("hello");
        }
    }

    public class StandardNPC3 : StaticSerializedQuestNPCGameData {
        protected override void PrepareGameData() {
            Initialize(NPCID.TestNPC3, "StandardNPC3");
            SetEntryDialogue(new StandardNPC3Dialogue().GetDialogue());
        }
    }

    public class HelpNPC01Dialogue : StaticSerializedDialogueGameData {


        protected override void PrepareGameData() {

        }
    }

    public class StandardNPC3Dialogue : StaticSerializedDialogueGameData {
        BranchRef b1 = new BranchRef();
        BranchRef b2 = new BranchRef();
        BranchRef b3 = new BranchRef();

        protected override void PrepareGameData() {
            breakOnBranches = false;

            AddLine("Nice to meet you.");
            AddLine("I'm Sakura. What's your name?");

            AddJapaneseBranch(b1, b2, b3);
            AddLine("Really...");
            Break();

            b1.Prompt = GetPhrase("hello");
            b1.Index = AddLine("hello");
            AddEvent(-3);
            Break();

            b2.Prompt = GetPhrase("I don't know");
            b2.Index = AddLine("ok, see you then...");
            AddEvent(-5);
            Break();

            b3.Prompt = GetPhrase("I am a foreigner.");
            b3.Index = AddLine("Haha, you're cute.");
            Break();
        }
    }


}
