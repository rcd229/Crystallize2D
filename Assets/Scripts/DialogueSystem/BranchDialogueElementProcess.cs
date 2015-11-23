using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BranchDialogueElementProcess : EnumeratorProcess<List<BranchDialogueElementLink>, int> {
    public override IEnumerator<SubProcess> Run(List<BranchDialogueElementLink> args) {
        var subproc = Get(PhraseSelectionPanelUI.GetFactory(), args);
        yield return subproc;
        //ExitArgs = subproc.Data.Data;
        Debug.Log("Exitargs:" + ExitArgs);
    }
}