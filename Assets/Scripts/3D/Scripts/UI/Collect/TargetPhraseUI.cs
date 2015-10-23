using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TargetPhraseUI : MonoBehaviour, IInitializable<PhraseSequence>, IPhraseDropHandler {

    const float FlashUpDuration = 0.2f;
    const float FlashDownDuration = 0.5f;

    public Text translationText;
    public Text phraseText;

    PhraseSequence phrase;

    public void Initialize(PhraseSequence phrase) {
        if (phrase.IsWord) {
            translationText.text = phrase.Word.GetTranslation();
        } else {
            translationText.text = phrase.Translation;
        }
        phraseText.text = "???";
        this.phrase = phrase;
    }

    public void HandlePhraseDropped(PhraseSequence phrase) {
        if (PhraseSequence.PhrasesEquivalent(phrase, this.phrase)) {
            //var args = new PhraseEventArgs(phrase);
            PlayerDataConnector.TryCollectItem(phrase);
            //if (phrase.IsWord) {
            //    CrystallizeEventManager.PlayerState.RaiseCollectWordRequested(this, args);
            //} else {
            //    CrystallizeEventManager.PlayerState.RaiseCollectPhraseRequested(this, args);
            //}
        } else {
            FlashRed();
        }
    }

    void FlashRed() {
        StartCoroutine(FlashRedCoroutine());
    }

    IEnumerator FlashRedCoroutine() {
        var images = GetComponentsInChildren<Image>();
        var colors = new Dictionary<Image, Color>();
        foreach (var i in images) {
            colors[i] = i.color;
        }

        for (float t = 0; t < 1f; t += Time.deltaTime / FlashUpDuration) {
            foreach (var i in images) {
                i.color = Color.Lerp(colors[i], Color.red, t);
            }
            yield return null;
        }

        yield return null;

        for (float t = 0; t < 1f; t += Time.deltaTime / FlashDownDuration) {
            foreach (var i in images) {
                i.color = Color.Lerp(Color.red, colors[i], t);
            }

            yield return null;
        }

        foreach (var i in images) {
            i.color = colors[i];
        }
    }

}