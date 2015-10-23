using UnityEngine;
using System.Collections;

public class BranchDialogueElementLink {

    public int NextID { get; set; }
    public PhraseSequence Prompt { get; set; }

    public BranchDialogueElementLink() {
        Prompt = new PhraseSequence();
    }

    public BranchDialogueElementLink(int nextID, PhraseSequence prompt) {
        NextID = nextID;
        Prompt = prompt;
    }

    public BranchDialogueElementLink(DialogueElement ele, PhraseSequence prompt) :this(ele.ID, prompt) {    }

}