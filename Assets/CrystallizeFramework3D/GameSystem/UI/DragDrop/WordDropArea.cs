using UnityEngine;
using System.Collections;
using System;

public class WordDropArea : MonoBehaviour, IDropArea {
    public class DroppedWordArgs : EventArgs {
        public PhraseSequence Phrase { get; set; }
        public GameObject DraggedObject { get; set; }
    }

    public event EventHandler<DroppedWordArgs> OnDropped;

    public void AcceptDrop(PhraseSequence phrase, GameObject draggedObject) {
        OnDropped.Raise(this, new DroppedWordArgs() { Phrase = phrase, DraggedObject = draggedObject });
    }
}
