using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ConversationPhraseUI : MonoBehaviour, IInitializable<PhraseSequence> {

    public PhraseSequence Phrase { get; set; }

    public void Initialize(PhraseSequence param1) {
        GetComponentInChildren<Text>().text = param1.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        Phrase = param1;
    }

}
