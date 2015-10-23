using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DialogueBuilder;

[XmlExtraType]
public class RewardGeneratedQuestData : GeneratedQuestData {
    public int Reward { get; set; }
    public RewardGeneratedQuestData() {
        Reward = 0;
    }

    public RewardGeneratedQuestData(int reward) {
        this.Reward = reward;
    }
}

[SerializedQuests]
public class WhatsThisQuest : SayPhraseQuest, IHasQuestNPCDialogues, IGeneratedQuest<RewardGeneratedQuestData> {

    static DialogueSetBuilder builderSet = new DialogueSetBuilder("SayPhraseQuest");

    public static readonly WhatsThisQuest Bench = new WhatsThisQuest(
        new Guid("067bb92030f34ce4a10c39a24f21d552"), new Guid("497975340c7849b98e424041038e07ca"), new Guid("843130c812bc4417a32ec97b0bbc0104"),
        "bench");

    public static readonly WhatsThisQuest Bicycle = new WhatsThisQuest(
        new Guid("0cdae50f4bdb4e6287cae7ad9ed88712"), new Guid("c70708cb0a2143c6a7c519988ed584e1"), new Guid("35f1d35d3bf44bb1aa23b5498c57d68e"),
        "bicycle");

    public static readonly WhatsThisQuest Tree = new WhatsThisQuest(
        new Guid("014858d2b25341559348325a5ed2a2ac"), new Guid("4227fcabaa844a51b700a19a832e9ea3"), new Guid("e9a310d335b24749b879ca698496fbab"),
        "tree");

    QuestTypeID id;
    QuestNPCItemData npc;
    string itemKey;
    IEnumerable<QuestDialogueState> states;

    public override QuestTypeID ID { get { return id; } }
    public override string QuestName { get { return "What's this?"; } }
    public override QuestNPCItemData NPC { get { return npc; } }
    public override string ItemKey { get { return itemKey; } }

    public override string TaskDescription { get { return "Tell " + NPC.CharacterData.Name + " how to say '" + ItemKey + "'"; } }

    public override int RewardMoney { get { return this.GetGeneratedDataInstance().Reward; } }

    public NPCGroup ExampleGroup { get; private set; }

    WhatsThisQuest(Guid questGuid, Guid npcGuid, Guid npcGroupID, string itemKey) {
        this.id = new QuestTypeID(questGuid);
        this.itemKey = itemKey;
        this.npc = GeneratedNPC.GetNew(npcGuid, true, isStatic: true, questID: ID);
        npc.TargetName = GetTargetPath(itemKey + " client");

        var builder = builderSet.GetDialogueBuilder();
        builder.AddLine("What is this?");
        builder.AddEvent(ActionDialogueEvent.Get(QuestUtil.UnlockQuest, ID));
        npc.EntryDialogue = builder.Build();

        var promptKeys = new string[]{
            itemKey,
            "It's " + itemKey.GetArticle() + " " + itemKey,
            "This is " + itemKey.GetArticle() + " " + itemKey
        };

        var exitDialogue1 = builderSet.GetDialogueBuilder();
        exitDialogue1.AddLine("Um...thanks");
        exitDialogue1.AddEvent(-2);
        exitDialogue1.AddEvent(ActionDialogueEvent.Get(() => this.SetGeneratedQuestDatInstance(new RewardGeneratedQuestData(100))));
        exitDialogue1.AddEvent(ActionDialogueEvent.Get(QuestUtil.EndQuest, ID));

        var exitDialogue2 = builderSet.GetDialogueBuilder();
        exitDialogue2.AddAnimation(EmoticonType.Excited);
        exitDialogue2.AddLine("thanks");
        exitDialogue2.AddEvent(ActionDialogueEvent.Get(() => this.SetGeneratedQuestDatInstance(new RewardGeneratedQuestData(200))));
        exitDialogue2.AddEvent(ActionDialogueEvent.Get(QuestUtil.EndQuest, ID));

        var exitDialogue3 = builderSet.GetDialogueBuilder();
        exitDialogue3.AddAnimation(EmoticonType.Excited);
        exitDialogue3.AddLine("thank you so much!");
        exitDialogue3.AddEvent(ActionDialogueEvent.Get(() => this.SetGeneratedQuestDatInstance(new RewardGeneratedQuestData(500))));
        exitDialogue3.AddEvent(ActionDialogueEvent.Get(QuestUtil.EndQuest, ID));

        var dstate1 = new QuestDialogueState(States[0], exitDialogue1.Build(), builderSet.GetPhrase(promptKeys[0]));
        var dstate2 = new QuestDialogueState(States[0], exitDialogue2.Build(), builderSet.GetPhrase(promptKeys[1]));
        var dstate3 = new QuestDialogueState(States[0], exitDialogue3.Build(), builderSet.GetPhrase(promptKeys[2]));
        states = new QuestDialogueState[] { dstate1, dstate2, dstate3 };

        //Debug.Log(builderSet.GetPhrase(promptKeys[2]).Translation);

        ExampleGroup = new NPCGroup(npcGroupID.ToString(), GetTargetPath(itemKey + " group"), NPCGroupFormation.DialoguePair, true,
            new AnimationElement(0, new GestureDialogueAnimation(TagLibrary.Point)),
            new LineElement(0, "What is this?"),
            new LineElement(1, promptKeys.GetRandom())
            );
    }

    public IEnumerable<QuestDialogueState> GetDialoguesForState(NPCID npcID, QuestTypeID questID, string state) {
        //Debug.Log("Getting dialogues: " + npcID.guid);
        if (npcID == NPC.ID) {
            //Debug.Log("got");
            return states;
        } else {
            //Debug.Log("failed");
            return new QuestDialogueState[0];
        }
    }
}
