using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

//Quest Manager: manages progress of quest
//provide dialogues for npcs based on quest progress
//help resource manager to manage resources in the game
public static class QuestUtil {

    static IEnumerable<QuestRef> quests {
        get {
            return GameData.Instance.Quests.Items.Select(s => new QuestRef(s.ID));
        }
    }

    //Initialize quest status based on player and game data for this particular player
    public static void Initialize() {
        Debug.Log("Initializing QuestManager");
        foreach (var quest in quests) {
            //Debug.Log("Initialize: " + quest.QuestName);
            if (GameData.Instance.Quests.DefaultUnlock.Contains(quest.ID)) {
                quest.Unlock();
            }

            if (quest.GameDataInstance.StateMachine is IHasGeneratedQuestData) {
                quest.PlayerDataInstance.State = null;
                //quest.PlayerDataInstance.GeneratedQuestData = null;
            }

            quest.GameDataInstance.StateMachine.UpdateSceneForState(new QuestRef(quest.ID));
        }
    }

    public static IEnumerable<string> ViewedQuests {
        get {
            return quests.Where(s => s.isViewed).Select(s => s.QuestName);
        }
    }

    public static IEnumerable<QuestRef> UnlockedQuests {
        get {
            return quests.Where(s => s.isUnlocked);//.Select(s => s.QuestName);
        }
    }

    //    Dictionary<Guid, IQuestInstance> activeQuestInstances = new Dictionary<Guid, IQuestInstance>();

    //unlock quest if it has not been unlocked
    public static void UnlockQuest(QuestTypeID id) {
        var quest = new QuestRef(id);

        if (quest != null && !quest.isUnlocked) {
            DataLogger.LogTimestampedData("UnlockQuest", id.guid.ToString());
            quest.Unlock();
        } else {
            Debug.Log("Unable to unlock quest; " + quest.isUnlocked);
        }
    }

    public static void UnlockQuest<T>() where T : IQuestGameData, new() {
        UnlockQuest(new T().ID);
    }

    //TODO is this suitable to all kinds of quests?
    //set state of quest to certain state.
    public static void SetQuestState(QuestTypeID id, string state) {
        var questRef = getQuest(id);
        if (questRef == null) {
            return;
        }
        questRef.SetState(state);
    }

    public static bool GetQuestAvailable(QuestTypeID id) {
        if (id == null) { return false; }
        var q = getQuest(id);
        if (q == null) { return false; }
        if (q.PlayerDataInstance.Finished && !q.GameDataInstance.IsRepeatable) { return false; }
        return true;
    }

	public static bool GetQuestAvailable(QuestNPCItemData npc) {
		foreach (var id in npc.QuestIDs){
			if(GetQuestAvailable(id)){
				return true;
			}
		}
		return false;
	}

    //end the quest. If quest is repeatable, reset the quest
    public static void EndQuest(QuestTypeID id) {
        Debug.Log("Ending quest: " + id.guid);
        DataLogger.LogTimestampedData("EndQuest", id.guid.ToString());

        var quest = new QuestRef(id);

        PlayerDataConnector.QuestCompleted = "-Quest complete!";
        if (quest.GameDataInstance.StateMachine is IHasQuestReward) {
            PlayerDataConnector.QuestReward = (quest.GameDataInstance.StateMachine as IHasQuestReward).GetReward(quest);
        }

        if (quest.IsRepeatable) {
            quest.Reset();
        } else {
            quest.SetFinish();
        }

        quest.PlayerDataInstance.GeneratedQuestData = null;
    }

    public static void RaiseFlag(Guid flag) {
        PlayerDataConnector.SetFlags(flag);
    }

    public static void RaiseFlags(params Guid[] flags) {
        PlayerDataConnector.SetFlags(flags);
    }

    public static void SetViewed(QuestTypeID id) {
        var questRef = getQuest(id);
        if (questRef != null) {
            questRef.SetViewed();
        }
    }

    static QuestRef getQuest(QuestTypeID id) {
        return quests.Where(s => s.ID == id).FirstOrDefault();
    }

    //public static List<QuestDialogueState> GetNPCQuestDialogueStates(NPCID npc) {
    //    var states = from quest in quests
    //                 let state = quest.GetQuestDialogueStates(npc)
    //                 where state != null
    //                 select state;
    //    return states.SelectMany(s => s).ToList();
    //}


    public static List<QuestDialogueState> GetNPCQuestDialogueStates(QuestNPCItemData npc) {
        var states = from quest in quests
                     let state = quest.GetQuestDialogueStates(npc.ID)
                     where state != null
                     select state;
        return states.SelectMany(s => s).ToList();
    }

    public static DialogueSequence GetIntroductionDialogue(QuestNPCItemData npc) {
        var dialogue = npc.EntryDialogue;
        foreach (var q in quests) {
            if (q.GameDataInstance.StateMachine is IHasQuestIntroductionDialogue) {
                var hasIntro = q.GameDataInstance.StateMachine as IHasQuestIntroductionDialogue;
                var newDialogue = hasIntro.GetIntroductionForState(npc.ID, q.ID, q.PlayerDataInstance.State);
                if (newDialogue != null) {
                    dialogue = newDialogue;
                }
            }
        }
        return dialogue;
    }

    public static List<PhraseSequence> GetChoices(QuestNPCItemData npc) {
        return GetNPCQuestDialogueStates(npc).Select(s => s.Prompt).ToList();
    }

    public static DialogueSequence GetResponse(QuestNPCItemData npc, PhraseSequence choice) {
        return GetNPCQuestDialogueStates(npc).Where(s => PhraseSequence.PhrasesEquivalent(s.Prompt, choice)).Select(s => s.Dialogue).FirstOrDefault();
    }

    public static ContextData GetResponseContext(QuestNPCItemData npc, PhraseSequence choice) {
        var getter = GetNPCQuestDialogueStates(npc).Where(s => s.Prompt == choice).Select(s => s.ContextGetter).FirstOrDefault();
        return getter == null ? null : getter();
    }

}
