using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IHasQuestIntroductionDialogue {
    DialogueSequence GetIntroductionForState(NPCID npcID, QuestTypeID questID, string state);
}
