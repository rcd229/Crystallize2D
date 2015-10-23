using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeShopUI : UIPanel, ITemporaryUI<object, object> {
    const string ResourcePath = "UI/HomeShop";
    public static HomeShopUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<HomeShopUI>(ResourcePath);
    }

    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(object param1) {
        CrystallizeEventManager.PlayerState.OnHomesChanged += PlayerState_OnHomesChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged += PlayerState_OnHomesChanged;
        Refresh();
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnHomesChanged -= PlayerState_OnHomesChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged -= PlayerState_OnHomesChanged;
    }

    void PlayerState_OnHomesChanged(object sender, EventArgs e) {
        Refresh();
    }

    void Refresh() {
        var children = new List<HomeRef>();
        foreach (var h in GameData.Instance.Homes.Items) {
            var hr = new HomeRef(h.ID);
            if (!hr.PlayerDataInstance.Unlocked) {
                children.Add(hr);
            }
        }
        UIUtil.GenerateChildren(children, instances, buttonParent, GetChild);
    }

    GameObject GetChild(HomeRef home) {
        var i = Instantiate<GameObject>(buttonPrefab);
        var cost = home.GameDataInstance.InitialCost;
        i.GetComponentInChildren<Text>().text = string.Format("{0} ({1} ¥)", home.GameDataInstance.Name, cost);
        if (cost > PlayerData.Instance.Money) {
            i.GetComponent<Image>().color = Color.gray;
        } else {
            i.AddComponent<DataContainer>().Store(home);
            i.GetComponent<UIButton>().OnClicked += HomeShopUI_OnClicked;
        }
        return i;
    }

    void HomeShopUI_OnClicked(object sender, EventArgs e) {
        var h = ((Component)sender).GetComponent<DataContainer>().Retrieve<HomeRef>();
        PlayerDataConnector.AddMoney(-h.GameDataInstance.InitialCost);
        PlayerDataConnector.UnlockHome(h);
        Refresh();
    }

    public event EventHandler<EventArgs<object>> Complete;

}
