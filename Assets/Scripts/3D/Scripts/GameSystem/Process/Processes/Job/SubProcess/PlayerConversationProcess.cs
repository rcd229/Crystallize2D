using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerConversationInitArgs {
    public static PlayerConversationInitArgs YesNoArgs(GameObject actor, DialogueSequence dialogue, bool isCorrect) {
        var args = new PlayerConversationInitArgs(actor, null, dialogue, 0, isCorrect);
        args.ExitOnClose = false;
        return args;
    }

    public GameObject Actor { get; set; }
    public PhraseSequence PlayerPhrase { get; set; }
    public DialogueSequence Dialogue { get; set; }
    public StringMap ActorMap { get; set; }
    public int ExtraChoiceCount { get; set; }
    public bool IsCorrect { get; set; }
    public bool ExitOnClose { get; set; }
	public int EmptySlots{get; set;}

    public PlayerConversationInitArgs(GameObject actor, PhraseSequence phrase, DialogueSequence dialogue, int choiceCount, bool isCorrect = false) {
        Actor = actor;
        PlayerPhrase = phrase;
        Dialogue = dialogue;
        ExtraChoiceCount = choiceCount;
        ExitOnClose = true;
        IsCorrect = isCorrect;
    }
}

public class PlayerConversationExitArgs {
    public bool IsCorrect { get; set; }

    public PlayerConversationExitArgs(bool isCorrect = true) {
        IsCorrect = isCorrect;
    }
}

public class PlayerConversationProcess : IProcess<PlayerConversationInitArgs, PlayerConversationExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

    PlayerConversationInitArgs args;
    PhraseConstructorArgs phraseConstructorArgs;

	int numTrials = 3;

    public void Initialize(PlayerConversationInitArgs param1) {
        Debug.Log("starting");
        args = param1;

        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.BeginConversation.Get(convArgs, BeginDialogueCallback, this);
    }

    public void ForceExit() {
        Debug.Log("Forcing exit");
        Exit();
    }

    void BeginDialogueCallback(object sender, object e) {
        //var known
        var allChoices = PlayerDataConnector.BufferWithExtraWords(
            PlayerDataConnector.GetWordChoices(args.PlayerPhrase), 
            args.ExtraChoiceCount + args.PlayerPhrase.ComparableElementCount);
        var comparable = from w in args.PlayerPhrase.PhraseElements
                         where w.IsDictionaryWord
                         select new PhraseSequence(w);
        var subChoices = CollectionExtensions.RandomSubsetWithValues(
            allChoices,
            comparable,
            args.PlayerPhrase.ComparableElementCount + args.ExtraChoiceCount);
        //foreach (var c in subChoices) {
        //    Debug.Log(c.GetText());
        //}
        phraseConstructorArgs = new PhraseConstructorArgs(args.PlayerPhrase, subChoices, args.EmptySlots);
        var ui = UILibrary.PhraseConstructor.Get(phraseConstructorArgs);
        ui.Complete += PhraseConstructed;
    }

    void PhraseConstructed(object sender, EventArgs<List<PhraseSequence>> e) {
        var p = e.Data.Flatten();
        if (p.FulfillsTemplate(args.PlayerPhrase)){ //PhraseSequence.PhrasesEquivalent(args.PlayerPhrase, p)) {
			DataLogger.LogTimestampedData("SaidCorrectPhrase", p.GetText());
			var pos = UILibrary.PositiveFeedback.Get("");
            pos.Complete += pos_Complete;
        } else {
			DataLogger.LogTimestampedData("SaidWrongPhrase", p.GetText());
            UILibrary.NegativeFeedback.Get("", neg_Complete, this);
            phraseConstructorArgs.EnteredPhrase = e.Data;
			if(p.ComparableElementCount > 1){
				numTrials --;
			}
        }
    }

    void neg_Complete(object sender, EventArgs<object> e) {
		if(numTrials <= 0){
			var ui = UILibrary.MessageBox.Get("The correct sentence is " + args.PlayerPhrase.GetText(JapaneseTools.JapaneseScriptType.Romaji));
			ui.Complete += pos_Complete;
		} else{
	        var ui = UILibrary.PhraseConstructor.Get(phraseConstructorArgs);
	        ui.Complete += PhraseConstructed;
		}
    }

    void pos_Complete(object sender, EventArgs<object> e) {
        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.ConversationSegment.Get(convArgs, DialogueCallback, this);
    }

    void DialogueCallback(object sender, object e) {
        var convArgs = new ConversationArgs(args.Actor, args.Dialogue, null, false);
        convArgs.ActorMap = new StringMap();
        convArgs.ActorMap.Set(new StringMapItem("[default]", args.Actor.name));
        ProcessLibrary.EndConversation.Get(convArgs, EndDialogueCallback, this);
    }

    void EndDialogueCallback(object sender, object e) {
        if (args.ExitOnClose) {
            args.Actor.AddComponent<PersonExitAnimation>();
        }
        Exit();
    }

    void Exit() {
        Debug.Log("exiting");
        OnExit.Raise(this, new PlayerConversationExitArgs());
    }

}