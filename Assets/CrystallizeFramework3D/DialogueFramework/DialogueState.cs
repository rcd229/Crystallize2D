using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DialogueState {

    public int CurrentID { get; set; }
    public DialogueSequence Dialogue { get; set; }
	public ContextData Context {get; set;}
    public StringMap ActorMap{ get; set; }
    public GameObject[] GameObjects { get; set; }

    public bool ReducesConfidence {
        get {
            return Dialogue.ReducesConfidence(CurrentID);
        }
    }

    public DialogueState(int id, DialogueSequence dialogue, ContextData context, StringMap actorMap, params GameObject[] gameObjects) {
        CurrentID = id;
        Dialogue = dialogue;
        Context = context;
        ActorMap = actorMap;
        GameObjects = gameObjects;
    }

    public DialogueElement GetElement() {
        return GetElement(Dialogue);
    }

    public T GetElement<T>() where T : DialogueElement {
        return GetElement(Dialogue) as T;
    }

    public DialogueElement GetElement(DialogueSequence dialogue) {
        return dialogue.GetElement(CurrentID);
    }

    public GameObject GetTarget() {
        return GetTarget(Dialogue);
    }

    public GameObject GetTarget(DialogueSequence dialogue) {
        return GameObjects.GetSafely(GetElement(dialogue).ActorIndex);
        //var actorIndex = GetElement(dialogue).ActorIndex;
        //var actor = dialogue.GetActor(actorIndex);
        //var name = GetActorName(actor.Name);

        //return Find(name);
    }

    public DialogueState NextElement() {
        return new DialogueState(GetElement().DefaultNextID, Dialogue, Context, ActorMap, GameObjects);
    }

    //public GameObject Find(string name) {
    //    var go = GameObjects.Where(g => g.name == name).FirstOrDefault();

    //    if (!go) {
    //        Debug.Log("go not found locally");
    //        go = GameObject.Find(name);
    //    }

    //    if (go == null) {
    //        Debug.Log("Unable to find actor: " + name + "\nMap is: " + ActorMap);
    //        if (ActorMap != null) {
    //            Debug.Log(ActorMap.Get(name).Value);
    //        }
    //    }
    //    return go;
    //}

    //public string GetActorName(string tag) {
    //    if (ActorMap != null) {
    //        if (ActorMap.ContainsKey(tag)) {
    //            return ActorMap.Get(tag).Value;
    //        }
    //    }
    //    return tag;
    //}

}