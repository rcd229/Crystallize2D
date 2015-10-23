using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueTutorialProcessSelector : IProcessSelector<DialogueState> {

    const string DragWordsTutorial = "DragWords";
    const string DragPhrasesTutorial = "DragPhrases";
    const string ConfidenceTutorial = "ConfidenceTutorial";

    public ProcessFactory<DialogueState> SelectProcess(ProcessFactory<DialogueState> defaultFactory, DialogueState inputArgs) {
        var line = inputArgs.GetElement<LineDialogueElement>();
        if (line == null) { return defaultFactory; }

        var factory = defaultFactory;
        var canDragWords = PlayerDataConnector.ContainsUncollectedItem(line.Line.Phrase);
        if (TryTutorial<LineDialogueElementProcess_DragWordsTutorial>(canDragWords, DragWordsTutorial, out factory)) {
            return factory;
        }

        var canDragPhrase = PlayerDataConnector.CanLearn(line.Line.Phrase, false) 
            && !line.Line.Phrase.IsWord
            && !line.Line.Phrase.HasContextData;
        if (TryTutorial<LineDialogueElementProcess_DragPhrasesTutorial>(canDragPhrase, DragPhrasesTutorial, out factory)) {
            return factory;
        }

        var reducesConfidence = PlayerDataConnector.ConfidenceCost(line.Line.Phrase) > 0
            && inputArgs.GetTarget().GetComponent<DialogueActor>().canReduceConfidence;
        if (TryTutorial<LineDialogueElementProcess_ConfidenceTutorial>(reducesConfidence, ConfidenceTutorial, out factory)) {
            return factory;
        }

        return defaultFactory;
    }

    bool TryTutorial<T>(bool condition, string key, out ProcessFactory<DialogueState> factory) where T : IProcess<DialogueState, DialogueState>, new() {
        factory = null;
        if (condition && !PlayerDataConnector.GetTutorialViewed(key)) {
            PlayerDataConnector.SetTutorialViewed(key);
            factory = new ProcessFactory<T, DialogueState, DialogueState>();
            return true;
        }
        return false;
    }

}
