using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/**
 * Intermediate representation of dialogue sequence to work with xml
 * A serializer instead of this is needed for JSON format
 */ 
public class SequentialConveration {

	public List<List<PhraseSequence>> Dialogue{get;set;}
	public List<string> Translations{get;set;}
	public SequentialConveration(){
		Dialogue = new List<List<PhraseSequence>>();
		Translations = new List<string>();
	}

	public DialogueSequence Deserialize(){
		DialogueSequence ds = new DialogueSequence();
		for (int i = 0; i < Dialogue.Count; i++){
			PhraseSequence line = new PhraseSequence();
			line.PhraseElements = Dialogue[i].ConvertAll(s => s.Word);
			line.Translation = Translations[i];
			ds.AddNewDialogueElement(new LineDialogueElement(line));
		}
		return ds;
	}
}
