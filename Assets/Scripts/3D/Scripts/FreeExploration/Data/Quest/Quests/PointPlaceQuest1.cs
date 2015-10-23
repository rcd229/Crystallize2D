using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[XmlExtraType]
public class PointPlaceGeneratedQuestData : GeneratedQuestData {
    public string SceneTargetAskerName { get; set; }
    public Vector3 SceneTargetPos { get; set; }
    public PhraseSequence correctPhrase { get; set; }

    public PointPlaceGeneratedQuestData() {
        SceneTargetAskerName = "";
        SceneTargetPos = Vector3.zero;
        correctPhrase = null;
    }

    public PointPlaceGeneratedQuestData(string name, Vector3 position, PhraseSequence context) {
        SceneTargetAskerName = name;
        SceneTargetPos = position;
        correctPhrase = context;
    }
}

public abstract class PointPlaceQuest1 : BaseQuestGameData,
    IGeneratedQuest<PointPlaceGeneratedQuestData>, IQuestStateMachine, IHasQuestIntroductionDialogue, IHasNPCSpawner {

    public override string QuestName { get { return "Point places to lost traveller"; } }
    public override bool IsRepeatable { get { return true; } }
    public override IQuestStateMachine StateMachine { get { return this; } }
    public abstract DialogueSequence AskPlace { get; }
    public abstract INPCSpawner Spawner { get; }
    public override int RewardMoney { get { return 500; } }

    public string FirstState { get { return "Get"; } }
    public string GetNextState(string state, string transition) { return transition; }

    public void UpdateSceneForState(QuestRef quest) {
        if (!quest.PlayerDataInstance.State.IsEmptyOrNull()) {
            SceneResourceManager.Instance.SetResources(ID.guid, GeneratePlaceIndicators());
        } else {
            SceneResourceManager.Instance.SetResources(ID.guid, null);
        }
    }

    protected abstract IEnumerable<GameObject> GeneratePlaceIndicators();

    public DialogueSequence GetIntroductionForState(NPCID npcID, QuestTypeID questID, string state) {
        if (isLostPerson(npcID) && isPointQuest(questID)) {
            if (state.IsEmptyOrNull()) {
                var d1 = AskPlace;
                d1.AddEvent(0, ActionDialogueEvent.Get(QuestUtil.UnlockQuest, questID));
                d1.AddEvent(0, ActionDialogueEvent.Get(BeginFollow));
                return d1;
            } else {
                return AskPlace;
            }
        }
        return null;
    }

    void BeginFollow() {
        var npc = NPCManager.Instance.GetNPC(NPCID.LostPerson);
        if (npc) {
            npc.GetComponent<FollowPlayer>().enabled = true;
        }
    }

    protected bool isLostPerson(NPCID npcID) {
        return npcID == NPCID.LostPerson;
    }

    protected bool isPointQuest(QuestTypeID questID) {
        return ID == questID;
    }
}
