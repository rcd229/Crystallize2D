using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class StatusUI : UIPanel, ITemporaryUI<TaskState, object> {

    const string Yen = "¥ ";
    const string ResourcePath = "UI/Status";

    public static StatusUI GetInstance() {
        var inst = GameObjectUtil.GetResourceInstance<StatusUI>(ResourcePath);
        MainCanvas.main.Add(inst.transform);
        return inst;
    }

    public Text moneyText;
    public Text jobText;
    public RectTransform jobPanel;
    public Text keysText;
    public Text valuesText;
    public Text instructionsText;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(TaskState taskState) {
        moneyText.text = Yen + PlayerData.Instance.Money;
        jobText.text = taskState.Job;
        jobPanel.gameObject.SetActive(!taskState.Hidden);
        keysText.text = "";
        valuesText.text = "";
        foreach (var state in taskState.States) {
            if(keysText.text != ""){
                keysText.text += "\n";
                valuesText.text += "\n";
            }

            keysText.text += state.Key + ":";
            valuesText.text += state.Value;
        }
        instructionsText.text = string.Format("<i>{0}</i>", taskState.Instructions);
    }

    void Start() {
        Initialize(TaskState.Instance);
        TaskState.Instance.StateChanged += Instance_StateChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged += PlayerState_OnMoneyChanged;
    }

    //void Update() {
    //    if (Input.GetKeyDown(KeyCode.Alpha1)) {
    //        GameObjectUtil.GetResourceInstance("UI/Element/ConfidenceText");
    //    }
    //}

    void PlayerState_OnMoneyChanged(object sender, EventArgs e) {
        moneyText.text = Yen + PlayerData.Instance.Money;
    }

    void Instance_StateChanged(object sender, EventArgs<TaskState> e) {
        Initialize(e.Data);
    }

    void OnDestroy() {
        TaskState.Instance.StateChanged -= Instance_StateChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged -= PlayerState_OnMoneyChanged;
    }

}
