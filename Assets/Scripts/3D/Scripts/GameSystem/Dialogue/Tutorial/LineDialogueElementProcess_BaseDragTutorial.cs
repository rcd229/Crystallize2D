using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class LineDialogueElementProcess_BaseDragTutorial : LineDialogueElementProcess {
    protected abstract string InitialText { get; }
    protected abstract PhraseSequence GetTargetItem(DialogueState args);
    protected abstract RectTransform GetDragStartTarget(SpeechBubbleUI speechBubble, PhraseSequence phrase);
    protected abstract void HookDragEvent();
    protected abstract void UnhookDragEvent();

    protected override IEnumerator<SubProcess> AwaitNext(DialogueState args) {
        var line = args.GetElement<LineDialogueElement>();
        var targetItem = GetTargetItem(args);
        if (targetItem == null) {
            yield return Get(base.AwaitNext(args));
            yield break;
        }
        
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += Continue;
        yield return Wait();
        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= Continue;
        var speechBubble = SpeechBubbleUI.LastSpeechBubble;

        yield return Get(ProcessLibrary.MessageBox, InitialText);

        while (!PlayerDataConnector.ContainsLearnedItem(targetItem)) {
            var helpUIArgs = new UITargetedMessageArgs(GetDragStartTarget(speechBubble, targetItem), "Drag from here");
            var speechBubbleBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);

            HookDragEvent();
            yield return Wait();
            UnhookDragEvent();
            speechBubbleBoxUI.CloseIfNotNull();

            helpUIArgs = new UITargetedMessageArgs(CollectUI.GetInstance().GetComponent<RectTransform>(), "...to here");
            var inventoryBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);

            CrystallizeEventManager.UI.OnPhraseDropped += Continue;
            yield return Wait();
            CrystallizeEventManager.UI.OnPhraseDropped -= Continue;
            inventoryBoxUI.CloseIfNotNull();
        }

        yield return Get(ProcessLibrary.MessageBox, "Click anywhere to continue the dialogue.");
        yield return Get(base.AwaitNext(args));
    }
}
