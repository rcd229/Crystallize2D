using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseTemplateInstance {

	public int PhraseTemplateID { get; set; }
	public List<PhraseSequenceElement> EnteredElements { get; set; }

	public PhraseTemplateInstance(){
		PhraseTemplateID = -1;
		EnteredElements = new List<PhraseSequenceElement> ();
	}

	public PhraseTemplateInstance(int phraseTemplateID, List<PhraseSequenceElement> enteredElements){
		PhraseTemplateID = phraseTemplateID;
		EnteredElements = enteredElements;
	}

}
