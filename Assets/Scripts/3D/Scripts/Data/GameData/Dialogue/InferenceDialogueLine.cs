using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//[XmlInclude(typeof(NPCActorLine))]
//[XmlInclude(typeof(PlayerActorLine))]
public class InferenceDialogueLine : DialogueActorLine {

	private List<string> category;
	private string tag;

	public  string Tag { 
		get{
			return tag;
		}
		
	}

	public  List<string> Category { 
		get{
			return category;
		}

	}

    public InferenceDialogueLine(List<string> category, string tag) : base(){
		this.category = category;
		this.tag = tag;
    }

}
