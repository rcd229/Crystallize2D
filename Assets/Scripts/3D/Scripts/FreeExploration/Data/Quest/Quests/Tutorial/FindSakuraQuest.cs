using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class FindSakuraQuest : BaseQuestGameData, IQuestStateMachine, 
	IHasQuestNPCDialogues, IHasQuestStateDescription, IHasQuestIntroductionDialogue {
    public static readonly QuestTypeID QuestID = new QuestTypeID("b19adc6b4ba444d692b48805179ca3eb");
    public static readonly string[] States = new string[] { "Root", "Introduced" };

    public override QuestTypeID ID { get { return QuestID; } }
    public override string QuestName { get { return "Introduce yourself to the new student"; } }
    public override bool IsRepeatable { get { return false; } }
    public override IQuestStateMachine StateMachine { get { return this; } }
    public string FirstState { get { return States[0]; } }
    public override int RewardMoney { get { return 2000; } }

    public NPCID SeekerID { get { return new NPCID(new Guid("150cbd39300443b7b85c5c070c5ea36d")); } }
    public QuestNPCItemData SeekerNPC {
        get{
            var npc = new QuestNPCItemData();
            npc.ID = SeekerID;
            npc.TargetName = GetTargetPath("Seeker");
            npc.QuestID = new FindSakuraQuest().ID;
            npc.CharacterData = new NPCCharacterData();
            npc.CharacterData.Name = "Ken";
            npc.CharacterData.Appearance = new AppearancePlayerData(AppearanceGender.Male, AppearanceEyeColor.Green, AppearanceSkinColor.Medium, 
                0, AppearanceHairColor.Orange, 0, 0, 0, 0);
            npc.FlagPrerequisites.Add(NPCQuestFlag.TutorialConversations);
            npc.EntryDialogue = BuildDialogue(
                EnglishLine("Can you greet the new student?"),
                Event(ActionDialogueEvent.Get(QuestUtil.UnlockQuest, FindSakuraQuest.QuestID)),
                EnglishLine("My Japanese isn't so great...")
                );
            return npc;
        }
    }

    public NPCID NewGirlID { get { return new NPCID(new Guid("5ad64018b1d94215ae5e4d977ee93e37")); } }
    public QuestNPCItemData NewGirlNPC {
        get {
            var npc = new QuestNPCItemData();
            npc.ID = NewGirlID;
            npc.TargetName = GetTargetPath("new student");
            npc.OverrideOverheadName = "New student";
            var charData = NPCCharacterData.GetRandom();
            charData.Appearance.Gender = (int)AppearanceGender.Female;
            npc.CharacterData = charData;
            npc.EntryDialogue = BuildDialogue(
                Line("hello"),
                Branch(
                    Prompted("What's your name?", Animation(TagLibrary.Bow), Line("I'm Sakura."), Line("Nice to meet you.")),
                    Prompted("Nice to meet you.", Animation(TagLibrary.Bow), Line("Nice to meet you."), Line("I'm Sakura.")),
                    Prompted("hello", Animation(TagLibrary.Bow), Animation(new WaitDialogueAnimation(1f)), ExitDialogue)
                ),
                Branch(
                    Prompted("I'm [name]", 
                        Animation(EmoticonType.Happy), 
                        Line("Please remember me."),
                        Event(ActionDialogueEvent.Get(QuestUtil.SetQuestState, QuestID, States[1]))
                        ),
                    Prompted("goodbye", Animation(EmoticonType.Sad), Line("goodbye"))
                )
                );
            return npc;
        }
    }

    public string GetNextState(string state, string transition) {
        return transition;
    }

    public void UpdateSceneForState(QuestRef quest) {
        var state = quest.PlayerDataInstance.State;
        if (state.IsEmptyOrNull()) {
            SceneResourceManager.Instance.SetResources(ID.guid, null);
        } else {
            var resources = new List<GameObject>();
            var target = NPCTarget.Get(GenericNPCTarget.Person.guid);
            if (target) {
                if (target.Spawned) {
                    resources.Add(target.NPC);
                } else {
                    resources.Add(NPCManager.Instance.SpawnNPC(NewGirlNPC, target, true));
                }
            }

            var t1 = NPCTarget.Get(StandardNPCGroup.IntroductionGroup1.guid);
            if (t1) {
                resources.Add(NPCManager.Instance.SpawnNPCGroup(t1.transform, StandardNPCGroup.IntroductionGroup1));
            }

            SceneResourceManager.Instance.SetResources(ID.guid, resources);
        }
    }

    public QuestHUDItem GetDescriptionForState(QuestRef quest) {
        var state = quest.PlayerDataInstance.State;
        if (state.IsEmptyOrNull()) {
            return null;
        } else {
            int currentStateIndex = Array.IndexOf(States, state);
            Debug.Log("Current state: " + state + "; " + currentStateIndex);
            var hudItem = new QuestHUDItem(QuestName,
                new QuestHUDSubItem("Introduced yourself", currentStateIndex > 0),
                new QuestHUDSubItem("Return to Ken", currentStateIndex > 1)
                );
            return hudItem;
        }
    }
	
	public DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state)
	{
        if (npcID == SeekerID) {
            if (state == States[0]) {
                return BuildDialogue(
                    EnglishLine("Did you do it yet?")
                );
            } else if (state == States[1]) {
                return BuildDialogue(
                    EnglishLine("Did you do it yet?")
                );
            }
            var instance = PlayerData.Instance.QuestData.GetOrCreateItem(questID);
            if (instance.Finished) {
                return BuildDialogue(
                    EnglishLine("Thanks!")
                    );
            }
        }
		return null;
	}

    public IEnumerable<QuestDialogueState> GetDialoguesForState(NPCID npcID, QuestTypeID questID, string state) {
        if (npcID == SeekerID) {
            if (state == States[1]) {
                var d = new DialogueSequence(new PhraseSequence("Thanks!"));
                d.AddEvent(0, ActionDialogueEvent.Get(QuestUtil.EndQuest, QuestID));
                var qds = new QuestDialogueState(state, d, new PhraseSequence("I talked to her."));
                var l = new List<QuestDialogueState>();
                l.Add(qds);
                return l;
            }
        }
        return new QuestDialogueState[0];
    }

}
