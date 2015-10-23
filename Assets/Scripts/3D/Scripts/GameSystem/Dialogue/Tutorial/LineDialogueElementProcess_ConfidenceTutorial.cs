using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LineDialogueElementProcess_ConfidenceTutorial : LineDialogueElementProcess {
    protected override IEnumerator<SubProcess> AwaitNext(DialogueState args) {
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Confidence, true);

        var line = args.GetElement<LineDialogueElement>();
        var targetItem = PlayerDataConnector.GetNeededWords(line.Line.Phrase).FirstOrDefault();
        if (targetItem == null) {
            Debug.LogError("No unknown words found.");
            yield return Get(base.AwaitNext(args));
            yield break;
        }

        CrystallizeEventManager.UI.OnSpeechBubbleOpen += Continue;
        yield return Wait();
        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= Continue;
        var speechBubble = SpeechBubbleUI.LastSpeechBubble;

        yield return Get(ProcessLibrary.MessageBox, "When people say words to you that you don't know, you confidence will decrease.");

        var helpUIArgs = new UITargetedMessageArgs(speechBubble.GetWord(targetItem.Word), "Unknown word", true);
        yield return Get(UILibrary.HighlightBox, helpUIArgs);

        helpUIArgs = new UITargetedMessageArgs(GameObject.FindGameObjectWithTag(TagLibrary.ConfidenceTarget).GetComponent<RectTransform>(), "Confidence", true);
        yield return Get(UILibrary.HighlightBox, helpUIArgs);

        yield return Get(ProcessLibrary.MessageBox, "You can regain lost confidence by collecting the unknown words.");

        while (!PlayerDataConnector.ContainsLearnedItem(targetItem)) {
            helpUIArgs = new UITargetedMessageArgs(speechBubble.GetWord(targetItem.Word), "Drag from here");
            var speechBubbleBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);

            CrystallizeEventManager.UI.OnWordDragged += Continue;
            yield return Wait();
            CrystallizeEventManager.UI.OnWordDragged -= Continue;
            speechBubbleBoxUI.CloseIfNotNull();

            helpUIArgs = new UITargetedMessageArgs(CollectUI.GetInstance().GetComponent<RectTransform>(), "...to here");
            var inventoryBoxUI = UILibrary.HighlightBox.Get(helpUIArgs);

            CrystallizeEventManager.UI.OnPhraseDropped += Continue;
            yield return Wait();
            CrystallizeEventManager.UI.OnPhraseDropped -= Continue;
            inventoryBoxUI.CloseIfNotNull();
        }

        helpUIArgs = new UITargetedMessageArgs(GameObject.FindGameObjectWithTag(TagLibrary.ConfidenceTarget).GetComponent<RectTransform>(), "Confidence restored", true);
        yield return Get(UILibrary.HighlightBox, helpUIArgs);

        yield return Get(base.AwaitNext(args));
    }
}
