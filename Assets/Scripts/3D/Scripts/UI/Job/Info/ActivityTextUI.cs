using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/Element/ActivityText")]
public class ActivityTextUI : UIMonoBehaviour, ITemporaryUI<string,object> {

    public Text primaryText;
    public Text secondaryText;

    public void Close() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
        Destroy(gameObject);
        Complete.Raise(this, new EventArgs<object>(null));
    }

    public void Initialize(string param1) {
        bool clickToExit = true;
        if (param1.Length > 0 && param1[0] == '-') {
            clickToExit = false;
            param1 = param1.Substring(1, param1.Length - 1);
        }

        if (param1.Contains("\n")) {
            var split = param1.Split('\n');
            primaryText.text = split[0];
            secondaryText.text = split[1];
        } else {
            primaryText.text = param1;
            secondaryText.text = "";
        }

        if (clickToExit) {
            CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;
        }
    }

    void Input_OnLeftClick(object sender, System.EventArgs e) {
        Close();
    }

    public event System.EventHandler<EventArgs<object>> Complete;

    IEnumerator Start() {
        yield return StartCoroutine(UIUtil.FadeInAndOutRoutine(canvasGroup, 1f, 2f, 0.5f));

        Close();
    }

    void OnDisabled() {
        if (gameObject) {
            Destroy(gameObject);
        }
    }

}