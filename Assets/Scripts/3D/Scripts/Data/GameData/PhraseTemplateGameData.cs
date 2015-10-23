using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseTemplateGameData {

	public List<PhraseSequence> PhraseTemplates { get; set; }

	public PhraseTemplateGameData(){
		PhraseTemplates = new List<PhraseSequence> ();
	}

}
