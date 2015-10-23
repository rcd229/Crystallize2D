using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class EnglishInputSpeechBubbleUI : MonoBehaviour {

    public InputField input;

    public bool OpenedManually { get; set; }

    public bool IsOpen {
        get {
            return gameObject.activeSelf;
        }
    }

    void Start() {
        TutorialCanvas.main.RegisterGameObject("EnglishInput", gameObject);
        UISystem.main.OnInputEvent += HandleInputEvent;

        Close();
    }

    void Update() {
        if (!EventSystem.current.alreadySelecting) {
            EventSystem.current.SetSelectedGameObject(input.gameObject, null);
        }
        input.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    void HandleInputEvent(object sender, UIInputEventArgs e) {
        if (!IsOpen && e.KeyCode == KeyCode.Space) {
            if (!UISystem.main.ContainsCenterPanel()) {
                Open();
                OpenedManually = true;
            }
        }

        if (IsOpen && e.KeyCode == KeyCode.Return) {
            Confirm();
        }
    }

    public void Open() {
        gameObject.SetActive(true);
        UISystem.main.AddCenterPanel(this);
        PlayerController.LockMovement(this);
    }

    public void Confirm() {
        var pe = new PhraseSequenceElement(PhraseSequenceElementType.Text, input.text);
        var p = new PhraseSequence();
        p.Add(pe);
        PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(p);
        Close();
    }

    public void Close() {
        UISystem.main.RemoveCenterPanel(this);
        PlayerController.UnlockMovement(this);
        gameObject.SetActive(false);
    }

}
