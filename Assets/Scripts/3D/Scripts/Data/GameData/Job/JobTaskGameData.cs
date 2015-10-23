using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

//[XmlInclude(typeof(InferenceTaskGameData))]
public class JobTaskGameData {

    public string Name { get; set;}
    public string SceneName { get; set;}
	public string PhraseSetName {get; set;}
    public string AreaName { get; set;}
    public ProcessTypeRef ProcessType {get; set;}
    public List<SceneObjectGameData> Actors {get; set;}
	public List<DialogueSequence> Dialogues {get; set;}
    public List<PhraseSequence> RequiredPhrases {get; set;}
    public List<PhraseSequence> TargetPhrases {get; set;}
	public List<int> Props{get;set;}

    public string StartingPosition { get; set; }

	public DialogueSequence Dialogue { 
		get{
			if(Dialogues.Count == 0){
				Dialogues.Add(new DialogueSequence());
			}
			return Dialogues[0];
		}
	}

    public SceneObjectGameData Actor {
        get {
            if (Actors.Count == 0) {
                Actors.Add(new SceneObjectGameData());
            }
            return Actors[0];
        }
        set {
            if (Actors.Count == 0) {
                Actors.Add(new SceneObjectGameData());
            }
            Actors[0] = value;
        }
    }

    public JobTaskGameData() {
        Name = "";
		PhraseSetName = "";
        SceneName = "";
        AreaName = "Area01";
        Actors = new List<SceneObjectGameData>();
		Dialogues = new List<DialogueSequence>();
        ProcessType = new ProcessTypeRef();
        RequiredPhrases = new List<PhraseSequence>();
        TargetPhrases = new List<PhraseSequence>();
		Props = new List<int>();
        StartingPosition = "";
    }

}
