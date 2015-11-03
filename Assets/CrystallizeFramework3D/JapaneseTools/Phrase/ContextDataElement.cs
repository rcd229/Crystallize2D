using UnityEngine;
using System.Collections;

public class ContextDataElement {

	public string Name { get; set; }
	public PhraseSequence Data { get; set; }

	public ContextDataElement(){
		Name = "";
		Data = new PhraseSequence();
	}

	public ContextDataElement(string id) : this(){
		Name = id;
	}

	public ContextDataElement(string id, PhraseSequence data) : this(id){
		Data = data;
        foreach (var e in data.PhraseElements) {
            if (!e.Tags.Contains(id)) {
                e.Tags.Add(id);
            }
        }
	}

}
