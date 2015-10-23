using UnityEngine;
using System.Collections;

public class WordCollector : IProcess<PhraseSequenceElement, PhraseSequenceElement> {

    public static WordCollector GetInstance() {
        return new WordCollector();
    }

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(PhraseSequenceElement word) {
        if (PlayerData.Instance.WordStorage.ContainsFoundWord(word)) {
            PlayerData.Instance.WordStorage.AddFoundWord(word);

            CrystallizeEventManager.PlayerState.RaiseWordCollected(this, new PhraseEventArgs(word));
        }
        OnExit.Raise(this, new ProcessExitEventArgs<PhraseSequenceElement>(word));
    }

    public void ForceExit() {
        
    }
}