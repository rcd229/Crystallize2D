using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorTracker {

    const string SpeechBubbleTargetTag = "SpeechBubbleTarget";
    static Vector3 DefaultOffset = new Vector3(0, 2f, 0);
    //static Dictionary<GameObject, string> actorNames = new Dictionary<GameObject, string>();

    public static List<GameObject> Actors { get; private set; }

    static Dictionary<Transform, Transform> actorSpeechBubbleTargets = new Dictionary<Transform, Transform>();

    static ActorTracker() {
        Actors = new List<GameObject>();
    }

    public static void AddActor(GameObject actor) {
        if (!Actors.Contains(actor)) {
            Actors.Add(actor);
        }
    }

    public static void RemoveActor(GameObject actor) {
        if (Actors.Contains(actor)) {
            Actors.Remove(actor);
        }
    }

    // TODO: decide if this needs to be moved somehwere else
    public static ActorType GetActorType(GameObject target) {
        if (target == PlayerManager.Instance.PlayerGameObject) {
            return ActorType.Self;
        } else if (target.transform.IsHumanControlled()) {
            return ActorType.Other; 
        } else {
            return ActorType.NPC;
        }
    }

    public static GameObject GetActorForName(string name) {
        foreach (var a in Actors) {
            if (a.name == name) {
                return a;
            }
        }
        return null;
    }

    public static string GetName(GameObject target) {
        switch (GetActorType(target)) {
            case ActorType.Self:
                return PlayerData.Instance.PersonalData.Name;
            case ActorType.Other:
                return "Partner";
            default:
                if (target.GetComponent<DialogueActor>()) {
                    return target.GetComponent<DialogueActor>().actorName;
                } else {
                    return "???";
                }
        }
    }

    public static Transform GetActorSpeechBubbleTarget(Transform target) {
        if (!actorSpeechBubbleTargets.ContainsKey(target)) {
            foreach (var t in target.GetComponentsInChildren<Transform>()) {
                if (t.CompareTag(SpeechBubbleTargetTag)) {
                    actorSpeechBubbleTargets[target] = t;
                    break;
                }
            }

            if (!actorSpeechBubbleTargets.ContainsKey(target)) {
                var t = new GameObject(SpeechBubbleTargetTag).transform;
                t.SetParent(target);
                t.localPosition = DefaultOffset;
                t.tag = SpeechBubbleTargetTag;
                actorSpeechBubbleTargets[target] = t;
            }
        }
        return actorSpeechBubbleTargets[target];
    }

}
