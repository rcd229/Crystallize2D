using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class BaseFindPersonQuest : FindPersonQuest, IHasNPCSpawner, INPCSpawner {

    protected DialogueSetBuilder set { get { return new DialogueSetBuilder(GetType().ToString()); } }

    public override QuestTypeID ID { get { return new QuestTypeID("5de2f2c58fa84f7db427612cae5c6644"); } }
    //public override abstract QuestTypeID ID { get; }
    public override abstract NPCID NPCID { get; }
    public override abstract int RewardMoney { get; }
    public override abstract NPCCharacterData CharacterData { get; }
    public abstract string SeekPromptKey { get; }

    public override PhraseSequence ConfirmPrompt { get { return set.GetPhrase("Sure"); } }
    public override DialogueSequence QuestConfirmed { get { return set.Get("Great, thanks!", SeekPromptKey); } }
    public override DialogueSequence Introduction { get { return set.Get("I'm looking for a secific person.", "Can you help me?"); } }
    public override DialogueSequence IntermediateIntroduction { get { return set.Get(SeekPromptKey); } }
    public override DialogueSequence PersonApproached { get { return set.Get("Hi there, what do you need?"); } }
    public override DialogueSequence IncorrectPersonConfirmed { get { return set.Get("You have the wrong person."); } }
    public override DialogueSequence PersonRejected { get { return set.Get("Ok, see you then."); } }
    public INPCSpawner Spawner { get { return this; } }

    public override DialogueSequence CorrectPersonConfirmed {
        get {
            var b = set.GetDialogueBuilder();
            b.AddLine("Thanks for letting me know! I'll go now.");
            b.AddEvent(new ConfidenceSafeEvent());
            return b.Build();
        }
    }

    public override DialogueSequence CompleteQuest {
        get {
            var b = set.GetDialogueBuilder();
            b.AddLine("Thanks for all your help.");
            b.AddEvent(new ConfidenceSafeEvent());
            b.AddLine("Here, take this.");
            b.AddEvent(new ConfidenceSafeEvent());
            b.AddEvent(3);
            return b.Build();
        }
    }

    public abstract void GetKeys(out string personKey, out string attributeKey);

    public override FindPersonGeneratedQuestData GenerateNewQuestData(Vector3 target) {
        string personKey, attributeKey;
        GetKeys(out personKey, out attributeKey);

        var context = new ContextData();
        context.Set("person", PersonAttributes.GetPhrase(personKey));
        context.Set("attribute", PersonAttributes.GetPhrase(attributeKey));
        DataLogger.LogTimestampedData("TargetPerson", personKey);
        DataLogger.LogTimestampedData("TargetPerson", attributeKey);

        var points = new PointScatterRegion(target, 10f, target, 5f).GeneratePositions(5);
        return new FindPersonGeneratedQuestData(context, points, personKey, attributeKey);
    }

}
