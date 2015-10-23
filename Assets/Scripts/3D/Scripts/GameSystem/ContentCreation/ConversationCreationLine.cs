using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ResourcePathAttribute("UI/ContentCreation/Line")]
public class ConversationCreationLine : MonoBehaviour {

	public static string ResourcePath{get{return "UI/ContentCreation/Line";}}

	public JapaneseTextEntryUI JText;
	public InputField translation;
	// Use this for initialization
	public LineCreationData CompileAndValidate(){
		List<PhraseSequence> ps = JText.Compile();
		if(translation.text == ""){
			//validation failed
			translation.GetComponent<Image>().color = Color.red;
			return null;
		}
		else{
			return new LineCreationData(ps, translation.text);
		}
	}
}

public class LineCreationData{
	public List<PhraseSequence> words{get;private set;}
	public string translation{get;private set;}
	public LineCreationData(List<PhraseSequence> lst, string trans){
		words = lst;
		translation = trans;
	}
}
