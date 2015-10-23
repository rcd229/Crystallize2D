using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JobWordUI : MonoBehaviour, IInitializable<PhraseJobRequirementGameData> {

    public Text wordText;
    public Image wordImage;

    public void Initialize(PhraseJobRequirementGameData phraseReq) {
        bool fulfilled = phraseReq.IsFulfilled();
        string t = "";
        Color c = Color.white;

        //Debug.Log(phraseReq.Phrase.PhraseElements.Count);
        if(phraseReq.Phrase.IsWord){
            c = GUIPallet.Instance.GetColorForWordCategory(phraseReq.Phrase.Word.GetPhraseCategory());
            if (fulfilled) {
                t = PlayerDataConnector.GetText(phraseReq.Phrase.Word);//.GetPlayerText();
            } else {
                t = phraseReq.Phrase.Word.GetTranslation();
            }
        } else {
            if (fulfilled) {
                t = PlayerDataConnector.GetText(phraseReq.Phrase);
            } else {
                t = phraseReq.Phrase.Translation;
            }
        }

        if (fulfilled) {
            wordImage.color = c;
        } else {
            wordImage.color = Color.gray.SetTransparency(0.5f);
        }
        wordText.text = t;
    }

}