using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DialogueSequence : DialogueElement {

    public List<SceneObjectGameData> Actors { get; set; }
    public SerializableDictionary<int, DialogueElement> Elements { get; set; }
    public List<IDialogueEvent> Events { get; set; }
    public ConversationCameraType Camera { get; set; }

    public override ProcessFactoryRef<DialogueState, DialogueState> Factory {
        get { return null; }
    }

    public const int ConfusedExit = -2;

    public DialogueSequence(ConversationCameraType camera = null)
        : base() {
        Actors = new List<SceneObjectGameData>();
        Elements = new SerializableDictionary<int, DialogueElement>();
        Events = new List<IDialogueEvent>();
        Camera = ConversationCameraType.Default;
    }

    public DialogueSequence(PhraseSequence phrase)
        : this() {
        AddNewDialogueElement(new LineDialogueElement(phrase));
    }

    public DialogueSequence(string actor, PhraseSequence phrase)
        : this() {
        Actors.Add(new SceneObjectGameData(actor));
        AddNewDialogueElement(new LineDialogueElement(phrase));
    }

    public IEnumerable<SceneObjectGameData> GetActors() {
        return Actors;
    }

    public SceneObjectGameData GetActor(int index) {
        if (Actors.IndexInRange(index)) {
            return Actors[index];
        }
        return new SceneObjectGameData("[default]");
    }

    public DialogueElement GetElement(int i) {
        return Elements.Get(i);
    }

    public DialogueElement GetNewDialogueElement(Type t) {
        if (!typeof(DialogueElement).IsAssignableFrom(t)) {
            Debug.LogError(t + " does not derive from DialogueElement!");
            return null;
        }

        int count = 0;
        foreach (var ele in Elements.Items) {
            if (ele.ID >= count) {
                count = ele.ID + 1;
            }
        }
        var newEle = (DialogueElement)Activator.CreateInstance(t);
        newEle.ID = count;
        Elements.Add(newEle);
        return newEle;
    }

    public T GetNewDialogueElement<T>() where T : DialogueElement, new() {
        int count = 0;
        foreach (var ele in Elements.Items) {
            if (ele.ID >= count) {
                count = ele.ID + 1;
            }
        }
        var newEle = new T();
        newEle.ID = count;
        Elements.Add(newEle);
        return newEle;
    }

    public DialogueElement AddNewDialogueElement(DialogueElement e) {
        int count = 0;
        foreach (var ele in Elements.Items) {
            if (ele.ID >= count) {
                count = ele.ID + 1;
            }
        }
        e.ID = count;
        Elements.Add(e);
        return e;
    }

    public void AddEvent(int id, IDialogueEvent dEvent) {
        dEvent.SetKey(id);
        Events.Add(dEvent);
    }

    //public void AddEvent(int id, Action action) {
    //    Events.Add(new ActionDialogueEvent(id, action));
    //}

    //public void AddEvent(int id, int confidence) {
    //    Events.Add(new ConfidenceDialogueEvent(id, confidence));
    //}

    public bool ContainsEvent(int id) {
        foreach (var e in Events) {
            if (e.Key == id) {
                return true;
            }
        }
        return false;
    }

    public void RaiseEvents(int id) {
        foreach (var e in Events) {
            if (e.Key == id) {
                e.RaiseEvent();
            }
        }
    }

    public bool ReducesConfidence(int id) {
        foreach (var e in Events) {
            if (e.Key == id && e is ConfidenceSafeEvent) {
                return false;
            }
        }
        return true;
    }

    public IEnumerable<PhraseSequenceElement> AggregateNPCWords() {
        return from p in AggregateNPCPhrases()
               from w in p.PhraseElements
               where w.IsDictionaryWord
               select w;
    }

    public IEnumerable<PhraseSequence> AggregateNPCPhrases() {
        var searched = new HashSet<DialogueElement>();
        searched.Add(this);
        return AggregateNPCPhrasesRecursively(searched);
    }

    IEnumerable<PhraseSequence> AggregateNPCPhrasesRecursively(HashSet<DialogueElement> searchedElements) {
        var list = new List<PhraseSequence>();
        foreach (var e in Elements.Items) {
            if (searchedElements.Contains(e)) {
                continue;
            }

            if (e is LineDialogueElement) {
                list.Add(((LineDialogueElement)e).Line.Phrase);
                searchedElements.Add(e);
            }

            if (e is DialogueSequence) {
                searchedElements.Add(e);
                list.AddRange(((DialogueSequence)e).AggregateNPCPhrasesRecursively(searchedElements));
            }
        }
        return list;
    }

    public string GetSummary() {
        var s = "Elements";
        foreach (var e in Elements.Items) {
            s += "\n\t" + e.ID + "; " + e.DefaultNextID + "; " + e.GetType();
        }
        s += "\nEvents";
        foreach (var e in Events) {
            s += "\n\t" + e.Key + "; " + e.GetType();
        }
        return s;
    }

}
