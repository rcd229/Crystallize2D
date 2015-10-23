using UnityEngine;
using System.Collections;

public class EmoticonPanelUI : MonoBehaviour {

    public bool closeOnSelect = true;

    void Start() {
        if (closeOnSelect) {
            CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
            CrystallizeEventManager.UI.OnBeginDragWord += HandleBeginDrag;

            TutorialCanvas.main.RegisterGameObject("EmoticonPanel", gameObject);

            Close();
        }
    }

    void HandleBeginDrag(object sender, PhraseEventArgs e) {
        Close();
    }

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is EmoticonPanelUIRequestEventArgs) {
            if (!gameObject.activeSelf) {
                Open();
            } else {
                Close();
            }
        }
    }

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void PlayHappyAnimation() {
        PlayAnimation(PersonAnimationType.Happy);
    }

    public void PlayConfusedAnimation() {
        PlayAnimation(PersonAnimationType.Confused);
    }

    public void PlayAngryAnimation() {
        PlayAnimation(PersonAnimationType.Angry);
    }

    public void PlayPointAnimation() {
        PlayAnimation(PersonAnimationType.Point);
    }

    public void PlayWaveAnimation() {
        PlayAnimation(PersonAnimationType.Wave);
    }

    public void PlayAlertAnimation() {
        PlayAnimation(PersonAnimationType.Alert);
    }

    public void PlayThanksAnimation() {
        PlayAnimation(PersonAnimationType.Thanks);
    }

    public void PlayAnimation(PersonAnimationType type) {
        CrystallizeEventManager.Environment.RaisePersonAnimationRequested(this, new PersonAnimationEventArgs(PlayerManager.Instance.PlayerGameObject, type));
        if (closeOnSelect) {
            Close();
        }
    }

}
