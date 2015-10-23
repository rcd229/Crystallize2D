using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;


public class PointPlaceTaskData : QATaskGameData {

	public List<string> StreetTarget{get;set;}
	Dictionary<PhraseSequence, PhraseSequence> afterChats {get; set;}

	public PointPlaceTaskData() : base(){
		StreetTarget = new List<string>();
		afterChats = new Dictionary<PhraseSequence, PhraseSequence>(new PhraseSequenceEquivalentComparator());
	}

	//after chat is linked to the answer phrase
	public void AddAfterChat(PhraseSequence key, PhraseSequence value){
		afterChats[key] = value;
	}

	public void AddQAAndAfterChat(PhraseSequence question, PhraseSequence answer, PhraseSequence afterChat){
		AddQA(question, answer);
		AddAfterChat(answer, afterChat);
	}

	//will return random chat if key not found
	public PhraseSequence GetAfterChat(PhraseSequence key){
		if(afterChats.ContainsKey(key))
			return afterChats[key];
		else
			return new PhraseSequence("...");
	}



}
