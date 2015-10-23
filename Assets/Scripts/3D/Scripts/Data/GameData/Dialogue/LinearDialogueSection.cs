using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearDialogueSection : ISerializableDictionaryItem<int> {

    public int Key {
        get {
            return ID;
        }
        set {
            ID = value;
        }
    }

    public int ID { get; set; }
    // move this up a level when branched and linear dialogues are joined
    public bool AvailableByDefault { get; set; }
    public bool GiveObjectives { get; set; }
    public List<DialogueActorLine> Lines { get; set; }

    public LinearDialogueSection() {
        ID = -1;
        AvailableByDefault = true;
        GiveObjectives = true;
        Lines = new List<DialogueActorLine>();
    }

    public LinearDialogueSection(int id) : this() {
        ID = id;
    }

    public void AddNewNPCLine() {
        Lines.Add(new NPCActorLine());
    }

    public void AddNewPlayerLine() {
        Lines.Add(new PlayerActorLine());
    }

    public List<PhraseSequenceElement> GetRemainingNeededWords() {
        var words = GetNeededWords();
        var rem = new List<PhraseSequenceElement>();
        foreach (var w in words) {
            if (!PlayerDataConnector.ContainsLearnedItem(w)){// PlayerData.Instance.WordStorage.ContainsFoundWord(w.WordID)) {
                rem.Add(w);
            }
        }
        return rem;
    }

    public List<PhraseSequenceElement> GetNeededWords() {
        var words = new List<PhraseSequenceElement>();
        foreach (var l in Lines) {
            if (l is PlayerActorLine) {
                var pl = (PlayerActorLine)l;
                foreach (var mw in pl.GetMissingWords()) {
                    if (mw >= 1000000) {
                        words.Add(new PhraseSequenceElement(mw, 0));
                    }
                }
                /*if (pl.OverrideGivenWords) {
                    for (int i = 0; i < pl.Phrase.PhraseElements.Count; i++) {
                        if (!pl.GetWordGiven(i)) {
                            words.Add(pl.Phrase.PhraseElements[i]);
                        }
                    }
                } else {
                    for (int i = 0; i < pl.Phrase.PhraseElements.Count; i++) {
                        if (pl.Phrase.PhraseElements[i].WordID >= 1000000) {
                            words.Add(pl.Phrase.PhraseElements[i]);
                        }
                    }
                    //Debug.LogError("Not implemented.");
                    //foreach(var )
                }*/
            }
        }
        return words;
    }

}
