using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class Tourist01 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.Tourist1, "Tourist 1", 0, 5);

            //var t = AddTask<StandardDialogueTask, GreetingDialogue01>("StreetSession", "Observer");
            //t.TargetPhrases.Add(GetPhrase("hello"));
            AddOverhearAndPracticeTask<HelloDialogue01>("StreetSession", "Observer", "hello");
            AddOverhearAndPracticeTask<GoodbyeDialogue01>("StreetSession", "Observer", "goodbye");
            AddOverhearAndPracticeTask<GoodMorningDialogue01>("StreetSession", "Observer", "good morning");
            AddOverhearAndPracticeTask<GoodEveningDialogue01>("StreetSession", "Observer", "good evening");

            if (PromotionTaskData.UsePromotion) {
                job.PromotionMistakes = 3;
                job.PromotionReq = 0;
                job.PromotionTask = GetOverhearAndPracticeTask<HelloDialogue01>("StreetSession", "Observer", "hello");
            }

            AddLine("see you tomorrow");
            AddLine("you are a nice person");
            AddLine("the weather is nice");
            AddLine("this is a nice place");

			job.formalLevel = JobFormalLevel.Versatile;
            job.TaskSelector = new MasterToContinueTaskSelectorGameData();
            job.Money = 500;
        }
    }

    public class Tourist02 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.Tourist2, "Tourist 2", 1, 5);

			job.formalLevel = JobFormalLevel.Versatile;

            AddOverhearAndPracticeTask<HowAreYouDialogue01>("StreetSession", "Observer", "How are you?");
            AddOverhearAndPracticeTask<IntroductionDialogue01>("StreetSession", "Observer", "How do you do?");
            AddOverhearAndPracticeTask<HobbyDialogue01>("StreetSession", "Observer", "what is your hobby?");
            AddOverhearAndPracticeTask<IntroductionDialogue02>("StreetSession", "Observer", "What's your name?");
            AddOverhearAndPracticeTask<StudentDialogue01>("StreetSession", "Observer", "are you a student?");

            if (PromotionTaskData.UsePromotion) {
                job.PromotionMistakes = 2;
                job.PromotionTask = GetOverhearAndPracticeTask<HelloDialogue01>("StreetSession", "Observer", "How are you?");
            }

            job.TaskSelector = new MasterToContinueTaskSelectorGameData();
            job.AddRequirement(GetPhrase("good"));
            job.AddRequirement(JobID.Tourist1);
            job.Money = 1000;

            job.Hide = false;
        }
    }

    public class Tourist03 : StaticSerializedJobGameData {
        protected override void PrepareGameData() {
            Initialize(JobID.Tourist3, "Tourist 3", 1, 5);

			job.formalLevel = JobFormalLevel.Versatile;
            AddTask<StandardDialogueTask, LibraryDialogue01>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, HobbyDialogue01>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue10>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue11>("StreetSession", "Observer");
            AddTask<StandardDialogueTask, GreetingDialogue12>("StreetSession", "Observer");
            AddOverhearAndPracticeTask<WhatIsThisDialogue01>("StreetSession", "Observer", "what is this?");
            AddOverhearAndPracticeTask<WhereIsThatDialogue01>("StreetSession", "Observer", "what is this?");
            job.Hide = true;
        }
    }

    public class HelloDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("hello", 0);
            AddAnimation(new GestureDialogueAnimation("Bow"), 1);
            AddLine("hello", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }


    class GoodbyeDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("SingleWave"), 0);
            AddLine("goodbye", 0);
            AddAnimation(new GestureDialogueAnimation("SingleWave"), 1);
            AddLine("goodbye", 1);
            AddAnimation(new MovementDialogueAnimation((t) => t.position - 10f * t.forward), 0);
            AddAnimation(new MovementDialogueAnimation((t) => t.position - 10f * t.forward), 1);
            AddAnimation(new WaitDialogueAnimation(2f));
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class GoodMorningDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("good morning", 0);
            AddAnimation(new GestureDialogueAnimation("Bow"), 1);
            AddLine("good morning", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class GoodEveningDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddLine("good evening", 1);
            AddLine("good evening", 0);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class IntroductionDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddLine("How do you do?", 0);
            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("I'm [name].", 0);
            AddLine("How do you do?", 1);
            AddAnimation(new GestureDialogueAnimation("Bow"), 1);
            AddLine("I'm [name].", 1);
            AddLine("Nice to meet you", 0);
            AddLine("The same to you.", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class IntroductionDialogue02 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddLine("What's your name?", 0);
            AddLine("I'm [name].", 1);
            AddLine("What's your name?", 1);
            AddLine("I am [name].", 0);
            AddLine("Nice to meet you", 0);
            AddLine("The same to you.", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class HowAreYouDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("Wave"), 0);
            AddAnimation(new GestureDialogueAnimation("Wave"), 1);
            AddLine("how are you?", 0);
            AddLine("I'm fine. Thanks!", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class StudentDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("are you a student?", 0);
            AddLine("no, I'm not a student", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class HobbyDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("what is your hobby?", 1);
            AddLine("I read books", 0);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class WhatIsThisDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            AddLine("what is this?", 0);
            AddLine("it is a souvenir", 1);
            AddLine("Thank you very much", 0);
            AddLine("not at all", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class WhereIsThatDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddLine("where is the toilet?", 0);
            AddAnimation(new GestureDialogueAnimation("Point"), 0);
            AddLine("over there", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class LibraryDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("over there is the library", 0);
            AddLine("it is spacious", 1);
            //AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class GreetingDialogue10 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("Do you have books?", 1);
            AddLine("Yes, I have books", 0);
            AddLine("Do you have a pen?", 1);
            AddLine("No, I don't have a pen", 0);
            AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class GreetingDialogue11 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("What time is it now?", 1);
            AddLine("It is three o'clock", 0);
            AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

    class GreetingDialogue12 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Target01");
            AddActor("Target02");

            AddMessage("During your touring, you overhear two people talking.");
            //            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("How much is it?", 1);
            AddLine("It is three hundred yen", 0);
            AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }

}
