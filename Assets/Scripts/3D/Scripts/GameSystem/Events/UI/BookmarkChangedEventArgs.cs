using UnityEngine;
using System.Collections;

public class BookmarkChangedEventArgs : System.EventArgs {

    public PhraseSequence Phrase { get; set; }

    public BookmarkChangedEventArgs(PhraseSequence bookmarkedPhrase) {
        Phrase = bookmarkedPhrase;
    }

}
