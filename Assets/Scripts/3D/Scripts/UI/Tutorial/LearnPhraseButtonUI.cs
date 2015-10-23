using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LearnPhraseButtonUI : MonoBehaviour, IPointerClickHandler, IInitializable<PhraseSequence>, IDragHandler, IBeginDragHandler {

    protected PhraseSequence phrase;

    public void Initialize(PhraseSequence param1) {
        phrase = param1;
        Refresh();
        CrystallizeEventManager.PlayerState.OnPhraseCollected += HandleSucceedCollectPhrase;
        CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
    }

    protected virtual void Refresh() {
        if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
            GetComponent<Image>().color = Color.gray;
        } else {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        Refresh();
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnPhraseCollected -= HandleSucceedCollectPhrase;
        CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
    }

    void HandleSucceedCollectPhrase(object sender, PhraseEventArgs e) {
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (phrase.IsWord) {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new PhraseClickedEventArgs(phrase, ""));
        } else {
            CrystallizeEventManager.UI.RaiseBeforePhraseClicked(this, new PhraseClickedEventArgs(phrase, ""));
            CrystallizeEventManager.UI.RaisePhraseClicked(this, new PhraseClickedEventArgs(phrase, ""));
        }
    }


    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData) {
        CrystallizeEventManager.UI.RaisePhraseDragged(this, new PhraseClickedEventArgs(phrase, ""));
    }

}
