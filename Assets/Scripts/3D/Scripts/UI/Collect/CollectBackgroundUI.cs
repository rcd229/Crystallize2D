using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CollectBackgroundUI : MonoBehaviour, IPhraseDropHandler {

    public EventHandler<PhraseEventArgs> OnPhraseDropped;

    public void HandlePhraseDropped(PhraseSequence phrase) {
        var args =  new PhraseEventArgs(phrase);
        OnPhraseDropped.Raise(this, args);
        PlayerDataConnector.TryCollectItem(phrase);
        //if (phrase.IsWord) {
        //    CrystallizeEventManager.PlayerState.RaiseCollectWordRequested(this, args);
        //} else {
        //    CrystallizeEventManager.PlayerState.RaiseCollectPhraseRequested(this, args);
        //}
    }

}
