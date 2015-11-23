using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EndDialogueProcess : EnumeratorProcess<PlayDialogueContext, object> {
    public override IEnumerator<SubProcess> Run(PlayDialogueContext args) {
        TextDisplayUI.Instance.Close();
        yield break;
    }
}
