using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DialogueBuilder;
using System.Linq;

[SerializedQuest]
public class GetToKnowNPCQuest : BaseGetToKnowNPCQuest {
    static DialogueSetBuilder builder = new DialogueSetBuilder("GetToKnowNPCQuest");

    public override DialogueSequence FirstTimeDialogue {
        get {
            return BuildDialogue(
                Line("hello"),
                Branch(
                    Prompted("hello", Line("nice to meet you")),
                    Prompted("nice to meet you", Line("How do you do?"), Line("please remember me")),
                    Prompted("goodbye", Line("I see..."), Line("goodbye"))
                ),
                Line("what's your name?"),
                Branch(
                    Prompted("My name is [name]", Animation(EmoticonType.Excited), Confidence(2)),
                    Prompted("I am [name]", Animation(EmoticonType.Happy), Confidence(1)),
                    Prompted("I'm [name]", Animation(EmoticonType.Happy))
                ),
                Branch(
                    Prompted("Would you mind telling me your name?", Animation(EmoticonType.Excited), Confidence(4)),
                    Prompted("What's your name?", Animation(EmoticonType.Happy), Confidence(2)),
                    Prompted("Your name?", Animation(EmoticonType.Happy))
                ),
                Line("My name is [name]."),
                Animation(EmoticonType.Happy),
                Event(ActionDialogueEvent.Get(LearnContext, "name")),
                Event(ActionDialogueEvent.Get(AddFriendship, 1)),
                Message(GetMessageString()),
                Line("goodbye"),
                Event(new ConfidenceSafeEvent()),
                Event(ActionDialogueEvent.Get(ClearSceneNPC))
            );
        }
    }

    public Element[] PostIntroductionGoodbye {
        get {
            return new Element[]{
                Line("I'm a bit busy now."),
                Event(new ConfidenceSafeEvent()),
            };
        }
    }

    public Element[] QuizIntroduction {
        get {
            return new Element[] {
                Line("Do you remember me?"),
                Branch(
                    Prompted("Of course", Animation(EmoticonType.Excited), Confidence(3), Line("I'm happy to hear it.")),
                    EnglishPrompted("<i>shake head</i>",Confidence(-2), Line("Aren't we friends?")),
                    Prompted("Yes", Animation(EmoticonType.Happy), Confidence(1), Line("Great!")),
                    EnglishPrompted("<i>nod</i>", Line("That's good.")),
                    Prompted("No", Animation(EmoticonType.Sad), Confidence(-1), Line("Aren't we friends?"))
                )
            };
        }
    }

    public Element[] NewQuestionIntroduction {
        get {
            return new Element[]{
                Line("It's [playername]!"),
                Line("Long time, no see"),
                Branch(
                    Prompted("Long time, no see", Animation(EmoticonType.Happy), Confidence(2), Line("Isn't it?")),
                    Prompted("What?", Animation(EmoticonType.Sad), Confidence(-2), Line("You forgot me?")),
                    Prompted("hello", Animation(EmoticonType.Annoyed), Line("How are you?"))
                ),
                Line("Do you have a question for me?")
            };
        }
    }

    public LineElement NameQuestion { get { return Line("What's your name?"); } }
    public LineElement HobbyQuestion { get { return Line("What's your hobby?"); } }
    public LineElement HomeQuestion { get { return Line("Where are you from?"); } }
    public LineElement AgeQuestion { get { return Line("How old are you?"); } }
    public LineElement JobQuestion { get { return Line("What's your occupation?"); } }

    public Element NameQuiz { get { return Line("Did you forget my name?"); } }
    public Element HobbyQuiz { get { return Line("Did you forget my hobby?"); } }
    public Element HomeQuiz { get { return Line("Do you remember where I'm from?"); } }
    public Element AgeQuiz { get { return Line("Did you forget my age?"); } }
    public Element JobQuiz { get { return Line("Did you forget my occupation?"); } }

    public Element NameReminder { get { return Line("My name is [name]."); } }
    public Element HobbyReminder { get { return Line("I like [doinghobby]."); } }
    public Element HomeReminder { get { return Line("I'm from [hometown]."); } }
    public Element AgeReminder { get { return Line("I'm [age] years old."); } }
    public Element JobReminder { get { return Line("I'm a [occupation]."); } }

    public Element[] RememberedLines {
        get {
            return new Element[]{
                Animation(EmoticonType.Happy),
                Confidence(3),
                Line("You really remembered!"),
                Event(ActionDialogueEvent.Get(AddFriendship, 1)),
                Event(ActionDialogueEvent.Get(ClearSceneNPC))
            };
        }
    }

    public Element ForgotLine { get { return Line("I'm a little sad..."); } }
    public Element ForgotQuestionLine { get { return Line("You forgot..."); } }
    public Element[] GoodbyeLine {
        get {
            return new Element[]{
                Line("See you later!"),
                Line("Goodbye"),
                Line("Let's talk later!")
            };
        }
    }

    public NPCGroup IntroductionSupportGroup1 { get { return StandardNPCGroup.IntroductionGroup2; } }
    public NPCGroup HobbySupportGroup { get { return new NPCGroup("109f7b0e27994710bfca10165dcdd762", "Hobby support", HobbyQuestion, HobbyReminder); } }
    public NPCGroup HomeSupportGroup { get { return new NPCGroup("180da092ab1f456da0f594420ffa363f", "Hometown support", HomeQuestion, HomeReminder); } }
    public NPCGroup AgeSupportGroup { get { return new NPCGroup("94427e94d3954a879593cdf56716a5a7", "Age support", AgeQuestion, AgeReminder); } }
    public NPCGroup JobSupportGroup { get { return new NPCGroup("6a997528ad684b508b3a2ebcae5b5c3d", "Job support", JobQuestion, JobReminder); } }

    public override DialogueSequence GetQuizDialogueForContext(string context) {
        var quiz = new Element[1];
        if (quizzes.ContainsKey(context)) {
            quiz[0] = quizzes[context];
        } else {
            Debug.LogError(context + " is not a valid context item");
        }
        return BuildDialogue(QuizIntroduction.Concat(quiz).Concat(GetChoices(context)).ToArray());
    }

    IEnumerable<Element> GetChoices(string context) {
        var reminder = new Element[1];
        if (reminders.ContainsKey(context)) {
            reminder[0] = reminders[context];
        } else {
            Debug.LogError(context + " is not a valid context item");
        }

        var elements = new List<PromptResponsePair>();
        var correct = GetGeneratedData().CurrentNPC.Context.Get(context).Data.GetText();
        elements.Add(Prompted(correct, RememberedLines));

        var additionalOptions = new List<string>();
        if (getAdditionalOptions.ContainsKey(context)) {
            additionalOptions = getAdditionalOptions[context](correct);
        } else {
            Debug.LogError("No additional option found for " + context);
        }

        foreach (var option in additionalOptions) {
            elements.Add(Prompted(option, reminders[context], Animation(EmoticonType.Sad), Confidence(-3), ForgotLine, Event(ActionDialogueEvent.Get(ClearSceneNPC))));
        }
        elements.Shuffle();
        return new Element[] { EnglishBranch(elements.ToArray()) };
    }

    List<string> GetNameOptions(string correctOption) {
        var isMale = GetGeneratedData().CurrentNPC.CharacterData.Appearance.Gender == 0;
        var nameList = new List<string>();
        while (nameList.Count < 4) {
            var newName = RandomNameGenerator.GetRandomName(isMale);
            if (correctOption != newName && !nameList.Contains(newName)) {
                nameList.Add(newName);
            }
        }
        return nameList;
    }

    List<string> GetHobbyOptions(string correctOption) { return GetOptions(correctOption, HobbyPhraseResources.GetHobbyKeys()); }
    List<string> GetHomeOptions(string correctOption) { return GetOptions(correctOption, PlacePhraseResources.GetCityKeys()); }
    List<string> GetAgeOptions(string correctOption) { return GetOptions(correctOption, AgePhraseResources.GetAgeRangeKeys(15, 35)); }
    List<string> GetJobOptions(string correctOption) { return GetOptions(correctOption, OccupationPhraseResources.GetOccupationKeys()); }

    List<string> GetOptions(string correct, IEnumerable<string> available) {
        var hash = new List<string>(available);
        var list = new List<string>();
        while (list.Count < 4 && hash.Count > 0) {
            var item = hash.GetRandom();
            hash.Remove(item);
            if (item != correct) {
                list.Add(item);
            }
        }
        return list;
    }

    string GetMessageString() {
        if (Application.isPlaying && GetGeneratedData().CurrentNPC != null) {
            var isMale = GetGeneratedData().CurrentNPC.CharacterData.Appearance.Gender == 0;
            return "You became friends with "
                + GetGeneratedData().CurrentNPC.Context.Get("name").Data.GetText()
                + ". Try to remember "
                + StringExtensions.GetPossessivePronoun(isMale)
                + " name in case you run in to "
                + StringExtensions.GetDOPronoun(isMale)
                + " again.";
        } else {
            return "";
        }
    }

    Dictionary<string, Element> quizzes = new Dictionary<string, Element>();
    Dictionary<string, Element> reminders = new Dictionary<string, Element>();
    Dictionary<string, LineElement> questions = new Dictionary<string, LineElement>();
    Dictionary<string, NPCGroup> supports = new Dictionary<string, NPCGroup>();
    Dictionary<string, Func<string, List<string>>> getAdditionalOptions = new Dictionary<string, Func<string, List<string>>>();
    string name = "name";
    string hobby = "hobby";
    string hometown = "hometown";
    string age = "age";
    string job = "occupation";

    public GetToKnowNPCQuest() {
        quizzes[name] = NameQuiz;
        quizzes[hobby] = HobbyQuiz;
        quizzes[hometown] = HomeQuiz;
        quizzes[age] = AgeQuiz;
        quizzes[job] = JobQuiz;
        reminders[name] = NameReminder;
        reminders[hobby] = HobbyReminder;
        reminders[hometown] = HomeReminder;
        reminders[age] = AgeReminder;
        reminders[job] = JobReminder;
        questions[name] = NameQuestion;
        questions[hobby] = HobbyQuestion;
        questions[hometown] = HomeQuestion;
        questions[age] = AgeQuestion;
        questions[job] = JobQuestion;
        getAdditionalOptions[name] = GetNameOptions;
        getAdditionalOptions[hobby] = GetHobbyOptions;
        getAdditionalOptions[hometown] = GetHomeOptions;
        getAdditionalOptions[age] = GetAgeOptions;
        getAdditionalOptions[job] = GetJobOptions;
        supports[name] = IntroductionSupportGroup1;
        supports[hobby] = HobbySupportGroup;
        supports[hometown] = HomeSupportGroup;
        supports[age] = AgeSupportGroup;
        supports[job] = JobSupportGroup;
    }


    public override DialogueSequence GetNewQuestionDialogue(string targetContext) {
        var branches = new List<PromptResponsePair>();
        branches.Add(GetQuestionBranch(targetContext));
        foreach (var c in GetGeneratedData().CurrentNPC.KnownContext) {
            branches.Add(GetQuestionBranch(c));
        }

        var all = new HashSet<string>(ContextPhraseResources.GetRepeatableContext());
        all.ExceptWith(GetGeneratedData().CurrentNPC.KnownContext);
        all.Remove(targetContext);
        foreach (var c in all) {
            branches.Add(GetQuestionBranch(c));
        }

        var branchElement = new Element[] { Branch(branches.Take(3).Randomize().ToArray()) };

        return BuildDialogue(
            NewQuestionIntroduction
            .Concat(branchElement)
            .ToArray()
            );
    }

    public PromptResponsePair GetQuestionBranch(string context) {
        if (GetGeneratedData().CurrentNPC.KnownContext.Contains(context)) {
            return Prompted(questions[context].phraseKey, Animation(EmoticonType.Sad), Confidence(-2), ForgotQuestionLine, reminders[context],
                Event(ActionDialogueEvent.Get(ClearSceneNPC))
                );
        } else {
            return Prompted(questions[context].phraseKey, Animation(EmoticonType.Happy), Confidence(2), reminders[context],
                Event(ActionDialogueEvent.Get(AddFriendship, 1)), 
                Event(ActionDialogueEvent.Get(ClearSceneNPC)),
                GoodbyeLine.GetRandom(),
                Event(ActionDialogueEvent.Get(ClearSceneNPC))
                );
        }
    }

    public override NPCGroup GetSupportGroupForContext(string targetContext) {
        return supports[targetContext];
    }
}
