using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectedWordUI : MonoBehaviour, IInitializable<PhraseSequence> {

    public Text translationText;
    public Text wordText;
    public Image wordImage;

    public void Initialize(PhraseSequence word) {
        translationText.text = word.Word.GetTranslation();
        wordText.text = PlayerDataConnector.GetText(word.Word);//.GetPlayerText();

        //Color c = GUIPallet.Instance.GetColorForWordCategory(word.Word.GetPhraseCategory());
        //c = Color.Lerp(c, Color.white, 0.5f);
        //wordImage.color = c;
    }

}