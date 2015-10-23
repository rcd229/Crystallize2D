using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[ResourcePath("UI/Indicator/TaskStatus")]
public class TaskStatusUI : UIMonoBehaviour, ITemporaryUI<string, object>{

    public Text moneyText;
    public Text statusText;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(string param1) {
        statusText.text = param1;
        //GetComponentInChildren<Text>().text = param1;
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Update() {
        moneyText.text = "Earnings: ¥ " + PlayerData.Instance.Session.ReducedMoney;
    }

}