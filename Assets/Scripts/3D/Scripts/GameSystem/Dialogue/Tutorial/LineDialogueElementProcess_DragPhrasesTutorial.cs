using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LineDialogueElementProcess_DragPhrasesTutorial : LineDialogueElementProcess_BaseDragTutorial {
    protected override string InitialText {
        get { return "You can learn phrases by dragging the <b>star</b> button."; }
    }

    protected override PhraseSequence GetTargetItem(DialogueState args) {
        var line = args.GetElement<LineDialogueElement>();
        if (PlayerDataConnector.CanLearn(line.Line.Phrase, false)) {
            return line.Line.Phrase;
        } else {
            return null;
        }
    }

    protected override RectTransform GetDragStartTarget(SpeechBubbleUI speechBubble, PhraseSequence targetItem) {
        return GameObject.FindGameObjectWithTag("LearnPhraseButton").GetComponent<RectTransform>();
    }

    protected override void HookDragEvent() {
        CrystallizeEventManager.UI.OnPhraseDragged += Continue;
    }

    protected override void UnhookDragEvent() {
        CrystallizeEventManager.UI.OnPhraseDragged -= Continue;
    }
}
