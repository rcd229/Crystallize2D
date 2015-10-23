using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[XmlExtraType]
public class SearchPersonGeneratedQuestData : GeneratedQuestData {
    public ContextData Context { get; set; }
    public List<Vector3> Points { get; set; }

    public SearchPersonGeneratedQuestData() {
        Context = new ContextData();
        Points = new List<Vector3>();
    }

    public SearchPersonGeneratedQuestData(ContextData context, List<Vector3> points) {
        this.Context = context;
        this.Points = points;
    }
}

public abstract class SearchPersonQuest<T> : BaseQuestGameData,
    IGeneratedQuest<T>, IQuestStateMachine, IHasQuestNPCDialogues, IHasQuestIntroductionDialogue
where T : GeneratedQuestData {

    public override bool IsRepeatable { get { return true; } }
    public override IQuestStateMachine StateMachine { get { return this; } }

    public override abstract string QuestName { get; }
    public abstract NPCID NPCID { get; }
    public abstract NPCCharacterData CharacterData { get; }
    public abstract PhraseSequence ConfirmPrompt { get; }
    public abstract DialogueSequence Introduction { get; }
    public abstract DialogueSequence IntermediateIntroduction { get; }
    public abstract DialogueSequence QuestConfirmed { get; }
    public abstract DialogueSequence CompleteQuest { get; }
    public abstract DialogueSequence PersonApproached { get; }
    public abstract DialogueSequence CorrectPersonConfirmed { get; }
    public abstract DialogueSequence IncorrectPersonConfirmed { get; }
    public abstract DialogueSequence PersonRejected { get; }

    public string FirstState { get { return "Root"; } }

    public string GetNextState(string state, string transition) { return transition; }
    public abstract void UpdateSceneForState(QuestRef quest);

    public DialogueSequence GetIntroductionForState(NPCID npcID, QuestTypeID questID, string state) {
        if (NPCID == npcID && !state.IsEmptyOrNull()) {
            return IntermediateIntroduction;
        }
        return null;
    }

    public IEnumerable<QuestDialogueState> GetDialoguesForState(NPCID npcID, QuestTypeID questID, string state) {
        if (npcID != NPCID) {
            return new QuestDialogueState[0];
        }

        if (state.IsEmptyOrNull()) {
            var d1 = QuestConfirmed;
            d1.AddEvent(0, ActionDialogueEvent.Get(PlayerDataConnector.AddConfidence, 1));
            d1.AddEvent(0, ActionDialogueEvent.Get(Unlock));
            var d2 = QuestConfirmed;
            d2.AddEvent(0, ActionDialogueEvent.Get(Unlock));
            return new QuestDialogueState[] { 
                new QuestDialogueState("", d1, ConfirmPrompt),
                new QuestDialogueState("", d2, new PhraseSequence("<i>Nod</i>")) 
            };
        }
        return new QuestDialogueState[0];
    }

    void Unlock() {
        QuestUtil.UnlockQuest(ID);
        var target = NPCManager.Instance.GetNPC(NPCID);
        target.GetOrAddComponent<Pin>();
    }
}
