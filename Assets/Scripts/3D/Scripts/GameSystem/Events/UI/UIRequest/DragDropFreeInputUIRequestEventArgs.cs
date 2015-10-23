using UnityEngine;
using System.Collections;

public class DragDropFreeInputUIRequestEventArgs : UIRequestEventArgs {

    public PhraseSequence Phrase { get; set; }

    public DragDropFreeInputUIRequestEventArgs(GameObject menuParent, PhraseSequence phrase)
        : base(menuParent) {
            Phrase = phrase;
    }

}
