using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[ResourcePath("UI/Element/DraggableWord")]
public class SpeechBubbleWordUI : MonoBehaviour, IInitializable<PhraseSequenceElement>, IPointerClickHandler, IBeginDragHandler, IDragHandler {

	PhraseSequenceElement word;

    public PhraseSequenceElement Word {
        get {
            return word;
        }
    }

    public void Initialize(PhraseSequenceElement word) {
        this.word = word;
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(word);
        
        
        var c = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());

        if (PlayerDataConnector.ContainsLearnedItem(word) || !word.IsDictionaryWord) {
            GetComponentInChildren<Text>().color = Color.black.Lighten(0.1f);
            GetComponent<Image>().enabled = false;
        } else {
            GetComponentInChildren<Text>().color = Color.black.Lighten(0.25f);
            GetComponent<Image>().enabled = true;
            CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
        }

        StartCoroutine(SlowUpdate());
    }

    IEnumerator SlowUpdate() {
        yield return null;
        yield return null;
        while (true) {
            if (GetComponent<RectTransform>().rect.width > 400f) {
                GetComponent<LayoutElement>().preferredWidth = 400f;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
        }
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        if (e.Phrase.IsWord && e.Word.WordID == word.WordID) {
            GetComponentInChildren<Text>().color = Color.black.Lighten(0.1f); //GUIPallet.Instance.defaultTextColor;
            GetComponent<Image>().enabled = false;
        } 
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            CrystallizeEventManager.UI.RaiseBeforeWordClicked(this, new PhraseClickedEventArgs(word, "Inventory"));
            CrystallizeEventManager.UI.RaiseWordClicked(this, new PhraseClickedEventArgs(word, "Inventory"));
        } else if(eventData.button == PointerEventData.InputButton.Right) {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new PhraseClickedEventArgs(word, "Dictionary"));
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        CrystallizeEventManager.UI.RaiseWordDragged(this, new PhraseClickedEventArgs(word, ""));
    }

    public void OnDrag(PointerEventData eventData) {
        
    }
}
