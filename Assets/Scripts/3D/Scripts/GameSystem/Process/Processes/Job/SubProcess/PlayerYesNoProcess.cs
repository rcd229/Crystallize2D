using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerYesNoProcess : IProcess<PlayerConversationInitArgs, PlayerConversationExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

    PlayerConversationInitArgs args;
    PhraseConstructorArgs phraseConstructorArgs;
    bool correct;

    public void Initialize(PlayerConversationInitArgs param1) {
        args = param1;
        //Debug.Log("initalized with " + args.IsCorrect);

        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.BeginConversation.Get(convArgs, BeginDialogueCallback, this);
    }

    public void ForceExit() {
        Exit(false);
    }

    void BeginDialogueCallback(object sender, object e) {
        var convArgs = new ConversationArgs(args.Actor, new DialogueSequence("[default]", new PhraseSequence("?")), null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.ConversationSegment.Get(convArgs, InitialSegmentCallback, this);
    }

    void InitialSegmentCallback(object sender, object e) {
        var phrases = new List<PhraseSequence>();
        phrases.Add(new PhraseSequence("Yes"));
        phrases.Add(new PhraseSequence("No"));

        var ui = UILibrary.PhraseSelector.Get(new PhraseSelectorInitArgs(phrases, "Is this the right person?", false));
        ui.Complete += PhraseConstructed;
    }

    void PhraseConstructed(object sender, EventArgs<PhraseSequence> e) {
        //Debug.Log(e.Data.GetText() + "; " + args.IsCorrect);
        if (e.Data.GetText().Trim() == "Yes") {
            if (args.IsCorrect) {
                DataLogger.LogTimestampedData("YesNoResult", "1");
                var pos = UILibrary.PositiveFeedback.Get("");
                pos.Complete += pos_Complete;
            } else {
                DataLogger.LogTimestampedData("YesNoResult", "0");
                var neg = UILibrary.NegativeFeedback.Get("");
                //phraseConstructorArgs.EnteredPhrase = e.Data;
                neg.Complete += DialogueCallback;
            }
        } else {
            DialogueCallback(this, this);
        }
    }

    //void neg_Complete(object sender, EventArgs<object> e) {
    //    var ui = UILibrary.PhraseConstructor.Get(phraseConstructorArgs);
    //    ui.Complete += PhraseConstructed;
    //}

    void pos_Complete(object sender, EventArgs<object> e) {
        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        correct = true;
        ProcessLibrary.ConversationSegment.Get(convArgs, DialogueCallback, this);
    }

    void DialogueCallback(object sender, object e) {
        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.EndConversation.Get(convArgs, EndDialogueCallback, this);
    }

    void EndDialogueCallback(object sender, object e) {
        //args.Actor.AddComponent<PersonExitAnimation>();
        Exit(correct);
    }

    void Exit(bool correct) {
        OnExit.Raise(this, new PlayerConversationExitArgs(correct));
    }

}