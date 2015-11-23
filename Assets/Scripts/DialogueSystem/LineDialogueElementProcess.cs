using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LineDialogueElementProcess : EnumeratorProcess<PlayDialogueContext, object> {
    public override IEnumerator<SubProcess> Run(PlayDialogueContext args) {
        Debug.Log("Displaying: " + (args.CurrentElement as LineDialogueElement).Line.Phrase.GetText());
        TextDisplayUI.Instance.Display((args.CurrentElement as LineDialogueElement).Line.Phrase);
        UIUtil.WaitForClick(Continue);
        yield return Wait();
        TextDisplayUI.Instance.Close();
    }
}