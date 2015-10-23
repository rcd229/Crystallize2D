using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LineDialogueElementProcess_DragWordsTutorial : LineDialogueElementProcess_BaseDragTutorial {
    protected override string InitialText {
        get { return "You can learn words by dragging them from speech bubbles."; }
    }

    protected override PhraseSequence GetTargetItem(DialogueState args) {
        var line = args.GetElement<LineDialogueElement>();
        return PlayerDataConnector.GetNeededWords(line.Line.Phrase).FirstOrDefault();
    }

    protected override RectTransform GetDragStartTarget(SpeechBubbleUI speechBubble, PhraseSequence targetItem) {
        return speechBubble.GetWord(targetItem.Word);
    }

    protected override void HookDragEvent() {
        CrystallizeEventManager.UI.OnWordDragged += Continue;
    }

    protected override void UnhookDragEvent() {
        CrystallizeEventManager.UI.OnWordDragged -= Continue;
    }
}
