using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IHasQuestNPCDialogues {
    IEnumerable<QuestDialogueState> GetDialoguesForState(NPCID npcID, QuestTypeID questID, string state);
}
