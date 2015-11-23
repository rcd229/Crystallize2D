using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchDialogueElement : DialogueElement {

    public bool DisplayTranslation { get; set; }
    public bool CheckAvailable { get; set; }
    public bool IncludeDefaultBranch { get; set; }

    public List<BranchDialogueElementLink> Branches { get; set; }

    public override ProcessFactoryRef<DialogueState, DialogueState> Factory {
        get {
            var f = new ProcessFactoryRef<DialogueState, DialogueState>();
            //f.Set<BranchDialogueElementProcess>();
            return f;
        }
    }

    public BranchDialogueElement()
        : base() {
            CheckAvailable = true;
            DisplayTranslation = true;
            IncludeDefaultBranch = true;
            Branches = new List<BranchDialogueElementLink>();
    }

    public override string ToString() {
        return "BranchDialogueElement[" + Branches.Count + "]";
    }

    public void SetBranches(params BranchDialogueElementLink[] elements) {
        Branches = new List<BranchDialogueElementLink>(elements);
    }

}