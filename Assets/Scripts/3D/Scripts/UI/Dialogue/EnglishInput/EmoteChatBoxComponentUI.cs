using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmoteChatBoxComponentUI : MonoBehaviour {

    void Start() {
        if (GameSettings.Instance.ExperimentModule != GameSettings.InterdependenceModule) {
            gameObject.SetActive(false);
        }
    }

    public void PlayHappyAnimation() {
        PlayAnimation(PersonAnimationType.Happy);
    }

    public void PlayWaveAnimation() {
        PlayAnimation(PersonAnimationType.Wave);
    }

    public void PlayThanksAnimation() {
        PlayAnimation(PersonAnimationType.Thanks);
    }

    public void PlayAnimation(PersonAnimationType type) {
        CrystallizeEventManager.Environment.RaisePersonAnimationRequested(this, new PersonAnimationEventArgs(PlayerManager.Instance.PlayerGameObject, type));
    }

}
