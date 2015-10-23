using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class QuestNPCItemData : ISerializableDictionaryItem<NPCID>, ISetableKey<NPCID> {

    public NPCID ID { get; set; }
    public string Name {
        get {
            if (CharacterData != null) {
                return CharacterData.Name;
            }
            return TargetName;
        }
    }

    public string OverheadName {
        get {
            if (OverrideOverheadName.IsEmptyOrNull()) {
                return Name;
            } else {
                return OverrideOverheadName;
            }
        }
    }

    public string OverrideOverheadName { get; set; }
    public string TargetName { get; set; }
    public List<Guid> FlagPrerequisites { get; set; }
    public DialogueSequence EntryDialogue { get; set; }
    public DialogueSequence ExitDialogue { get; set; }

	public QuestTypeID QuestID { 
		get{return QuestIDs.FirstOrDefault();} 
		set{if(QuestIDs.Count == 0){ QuestIDs.Insert(0, value);}} }
	public List<QuestTypeID> QuestIDs {get;set;}
    public bool IsStatic { get; set; }
    public NPCCharacterData CharacterData { get; set; }
    //public bool AlwaysSpawned 

    public NPCID Key {
        get {
            return ID;
        }
    }

    //npc is unlocked iff all quest requirements are finished and TODO add more criteria
    public bool Unlocked {
        get {
            return !FlagPrerequisites.Except(PlayerData.Instance.QuestData.Flags).Any();
        }
    }

    public QuestNPCItemData() {
        TargetName = "";
        FlagPrerequisites = new List<Guid>();
        EntryDialogue = new DialogueSequence();
		QuestIDs = new List<QuestTypeID>();
        IsStatic = true;
    }


    public void SetKey(NPCID key) {
        ID = key;
    }
}
