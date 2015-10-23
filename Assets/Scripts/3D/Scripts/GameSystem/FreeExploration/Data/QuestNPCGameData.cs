using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class QuestNPCGameData : GuidSerializableDictionary<NPCID, QuestNPCItemData> {
	public IEnumerable<QuestNPCItemData> UnlockedItems{
		get{
			return Items.Where(s => s.Unlocked);
		}
	}
}
