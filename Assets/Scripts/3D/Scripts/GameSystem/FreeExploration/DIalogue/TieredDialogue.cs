using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//set of dialogue that will be retrieved based on a pre-defined tier
public class TieredDialogue {

	//give dialogue according to tiers
	//eg. level 5 for 1, 2, 2, 3 will return 2 (third tier)
	public static DialogueSequence GetDialogue(List<DialogueSequence> sequence, int level, params int[] tiers){
		int index = 0;
		foreach (var i in tiers){
			if(level <= i){
				return sequence[index];
			}
			level -= i;
			index ++;
		}
		return sequence[index - 1];
	}

	public List<DialogueSequence> Dialogues{get;set;}
	public List<int> Tiers{get;set;}
	public int Level{get;set;}

	public TieredDialogue(){
		Dialogues = new List<DialogueSequence>();
		Tiers = new List<int>();
		Level = 0;
	}

	public TieredDialogue(IEnumerable<DialogueSequence> dialogues, IEnumerable<int> tiers, int level){
		Dialogues = dialogues.ToList();
		Tiers = tiers.ToList();
		Level = level;
	}

	public void AddLevel(int addition){
		Level += addition;
	}

	public void MinusLevel(int subtraction){
		Level -= subtraction;
	}

	public DialogueSequence GetDialogue(){
		return GetDialogue(Dialogues, Level, Tiers.ToArray());
	}
}
