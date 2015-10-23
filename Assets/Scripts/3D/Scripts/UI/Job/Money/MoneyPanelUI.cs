using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MoneyPanelUI : UIPanel, ITemporaryUI<object, object> {

    const string ResourcePath = "UI/MoneyPanel";
    public static MoneyPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<MoneyPanelUI>(ResourcePath);
    }

    public Text moneyText;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object param1) {    }

    void Start() {
        Refresh();
        CrystallizeEventManager.PlayerState.OnMoneyChanged += PlayerState_OnMoneyChanged;
    }

    void PlayerState_OnMoneyChanged(object sender, EventArgs e) {
        Refresh();
    }

    void Refresh() {
        moneyText.text = PlayerData.Instance.Money.ToString() + " ¥";
    }

}
