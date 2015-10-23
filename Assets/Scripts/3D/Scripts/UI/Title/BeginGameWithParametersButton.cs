using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BeginGameWithParametersButton : MonoBehaviour, IPointerClickHandler {

    public string level = "Tutorial_Level01";
    public InputField nameInput;
    public ColorToggleButton toggleButton;
    public bool multiplayerFlag = false;

    public void OnPointerClick(PointerEventData eventData) {
        var n = nameInput.text;
        if (n == "") {
            n = "No name";
        }

        if (multiplayerFlag) {
            PlayerData.Instance.Flags.SetFlag(FlagPlayerData.IsMultiplayer, true);
        }

        var we = new PhraseSequenceElement(PhraseSequenceElementType.Text, n);
        we.AddTag("name");
        PlayerData.Instance.PersonalData.SetName(n);
        PlayerData.Instance.WordStorage.AddFoundWord(we);
        PlayerDataLoader.SaveLocal();

        Application.LoadLevel(level);
    }

}
