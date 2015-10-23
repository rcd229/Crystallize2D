using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseTemplate : ISerializableDictionaryItem<int> {

	public int Key { 
		get {
			return ID;
		}
	}

	public int ID { get; set; }
	public PhraseSequence Phrase { get; set; }

	public PhraseTemplate(){
		ID = -1;
		Phrase = new PhraseSequence ();
	}

	public PhraseTemplate(int id) : this(){
		ID = id;
	}

}
