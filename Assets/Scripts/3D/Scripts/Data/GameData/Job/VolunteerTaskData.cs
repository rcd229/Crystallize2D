using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;


public class VolunteerTaskData : QATaskGameData {

	public DialogueSequence AnswerDialogue {get; set;}
	public SceneObjectGameData PlayerIdentifier{get;set;}
	public VolunteerTaskData() : base(){
		AnswerDialogue = new DialogueSequence();
		PlayerIdentifier = new SceneObjectGameData();
	}

}
