using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// A Quest NPC contains quest information. They can distribute quests and provide information for you to complete a quest
/// A Quest NPC will have friendliness level towards the player. All dialogues of the NPC will be unlocked based on friendliness
/// Quest NPC can be unlocked as game progresses
/// </summary>
public class QuestNPC : WorldNPC, IInteractableSceneObject, IInitializable<QuestNPCItemData> {

    class QuestNPCMapIndicator : MapIndicator {
        QuestNPC npc;
        public override MapResourceType Type { get { return npc.HasQuest ? MapResourceType.QuestNPC : MapResourceType.Standard; } }
        public override Color Color { get { return npc.HasQuest ? Color.yellow : Color.white; } }
        public QuestNPCMapIndicator(QuestNPC npc) { this.npc = npc; }
    }

    class QuestNPCOverheadIcon : OverheadIcon {
        QuestNPC npc;
        public override IconType Type { get { return npc.HasQuest ? IconType.ExclamationMark : IconType.SpeechBubble; } }
        public override Color Color { get { return npc.HasQuest ? Color.yellow : Color.white; } }
        public QuestNPCOverheadIcon(QuestNPC npc) { this.npc = npc; }
    }

    static HashSet<QuestNPC> allQuestNPCs = new HashSet<QuestNPC>();
    public static QuestNPC Get(Guid id) {
        return (from q in allQuestNPCs
                where q.NPC != null && q.NPC.ID.guid == id
                select q)
               .FirstOrDefault();
    }

    public QuestNPCItemData NPC { get; private set; }
    public bool HasQuest { get { return QuestUtil.GetQuestAvailable(NPC); } }

    public Color color = Color.white;

    public void Initialize(QuestNPCItemData npc) {
        NPC = npc;
        CharacterName = npc.OverheadName;

        foreach (var word in npc.EntryDialogue.AggregateNPCWords()) {
            if (PlayerDataConnector.ContainsLearnedItem(word)) ;
        }
    }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        DataLogger.LogTimestampedData("InteractionStarted", NPC.ID.guid.ToString());
        ProcessLibrary.QuestConversation.Get(new QuestArgs(gameObject, NPC), callback, parent);
    }

    void Start() {
        gameObject.GetOrAddComponent<IndicatorComponent>().Initialize(CharacterName, new QuestNPCOverheadIcon(this), new QuestNPCMapIndicator(this), false);
    }

    void OnEnable() {
        allQuestNPCs.Add(this);
    }

    void OnDisable() {
        allQuestNPCs.Remove(this);
    }

}
