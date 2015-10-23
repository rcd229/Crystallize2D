using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/Element/HighlightBox")]
public class DragBoxTutorialUI : UIMonoBehaviour, ITemporaryUI<UITargetedMessageArgs, object> {

    public Text infoText;
    public RectTransform target;
    public GameObject confirmButton;

    public event System.EventHandler<EventArgs<object>> Complete;

    public void Initialize(UITargetedMessageArgs param1) {
        MainCanvas.main.Add(transform, CanvasBranch.Tutorial);
        infoText.text = param1.Message;
        target = param1.Target;

        if (!param1.RequireConfirmation && confirmButton) {
            confirmButton.SetActive(false);
        } else {
            CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
        }
    }

    void Input_OnEnvironmentClick(object sender, System.EventArgs e) {
        Close();
    }

    public void Initialize(RectTransform target, string text) {
        infoText.text = text;
        this.target = target;
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (target) {
            rectTransform.pivot = target.pivot;
            rectTransform.position = target.position;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target.rect.height);
        }
	}

    public void Close() {
        Destroy(gameObject);
        Complete.Raise(this, null);
    }

}
