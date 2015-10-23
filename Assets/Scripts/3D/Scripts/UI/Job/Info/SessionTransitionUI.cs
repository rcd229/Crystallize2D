using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SessionTransitionUI : UIPanel, ITemporaryUI<string> {

    const string ResourcePath = "UI/SessionTransition";
    public static SessionTransitionUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<SessionTransitionUI>(ResourcePath);
    }

    public event System.EventHandler<EventArgs<object>> Complete;

    public Text nextDayText;

    float time = 0;
    float screenFillTime = 1f;
    float textTime = 0;// 1f;
    float waitTime = 0;//1f;

    public void Initialize(string label) {
        SetLabel(label);
    }

    public void SetLabel(string sessionLabel)
    {
        nextDayText.text = sessionLabel;
        MainCanvas.main.Add(transform, CanvasBranch.None);
    }

    void RaiseComplete() {
        GetComponent<Image>().fillAmount = 1f;
        nextDayText.GetComponent<CanvasGroup>().alpha = 1f;
        Complete.Raise(this, null);
    }

    void Start() {
        GetComponent<Image>().fillAmount = 0;
        nextDayText.GetComponent<CanvasGroup>().alpha = 0;
        CrystallizeEventManager.Input.OnLeftClick += HandleLeftClick;
    }

	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time <= screenFillTime) {
            GetComponent<Image>().fillAmount = time / screenFillTime;
            nextDayText.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if(time <= screenFillTime + textTime)
        {
            GetComponent<Image>().fillAmount = 1f;
            nextDayText.GetComponent<CanvasGroup>().alpha = (time - screenFillTime) / textTime;
        } else {
            GetComponent<Image>().fillAmount = 1f;
            nextDayText.GetComponent<CanvasGroup>().alpha = 1f;
        }

        nextDayText.GetComponent<CanvasGroup>().alpha = 0f;

        if (time >= screenFillTime + textTime + waitTime) {
            RaiseComplete();
        }

        if (time > screenFillTime && nextDayText.text == "") {
            RaiseComplete();
        }
	}

    void HandleLeftClick(object sender, System.EventArgs e) {
        if (time < screenFillTime + textTime) {
            time = screenFillTime + textTime;
        } else {
            RaiseComplete();
        }
    }

}
