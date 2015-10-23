using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpBoxUI : UIMonoBehaviour {

    public Text helpText;

    bool open = false;

	// Use this for initialization
	void Start () {
        TutorialCanvas.main.RegisterGameObject("HelpBox", gameObject);
	    CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
	}

    void HandleUIRequested(object sender, UIRequestEventArgs args) {
        if (args is HelpMessageUIRequestEventArgs) {
            var t = ((HelpMessageUIRequestEventArgs)args).Text;
            if (t == "") {
                Close();
            } else {
                Open(t);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (open) {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.deltaTime);
        } else {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime);
            if (canvasGroup.alpha <= 0) {
                gameObject.SetActive(false);
            }
        }
	}

    public void Open(string message) {
        open = true;
        gameObject.SetActive(true);
        //Debug.Log("Opened.");

        if (helpText.text != message) {
            helpText.text = message;

            // TODO: don't want to play effects from this class
            var effect = GetComponentInChildren<FlashWaveUIEffect>();
            if (effect) {
                effect.Play();
            }
        }
    }

    public void Close() {
        //Debug.Log("Closed.");
        open = false;
    }

}
