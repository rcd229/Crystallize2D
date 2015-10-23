using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

public class PictureQATaskData : QATaskGameData {
	
	Dictionary<string, Sprite> answerPictureDictionary;

	public PictureQATaskData() : base(){
		answerPictureDictionary = new Dictionary<string, Sprite> ();
	}

	new public void AddQA(PhraseSequence question, PhraseSequence answer){
		base.AddQA (question, answer);
		answerPictureDictionary.Add (answer.GetText(), new Sprite ());
	}

	public void AddQA(PhraseSequence question, PhraseSequence answer, Sprite picture){
		QALine newline = new QALine (question, answer);
		QAlist.Add (newline);
		answerPictureDictionary.Add (answer.GetText(), picture);
	}

	public Sprite getPicture(PhraseSequence key){
		Sprite ret;
		answerPictureDictionary.TryGetValue (key.GetText(), out ret);
		return ret;
	}
}
