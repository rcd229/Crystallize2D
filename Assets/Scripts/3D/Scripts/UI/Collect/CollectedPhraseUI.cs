using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectedPhraseUI : MonoBehaviour, IInitializable<PhraseSequence> {

    public Text translationText;
    public Text phraseText;

    public void Initialize(PhraseSequence phrase) {
        translationText.text = phrase.Translation;
        phraseText.text = PlayerDataConnector.GetText(phrase);
    }

}