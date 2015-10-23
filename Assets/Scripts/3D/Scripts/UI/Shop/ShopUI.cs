using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class ShopInitArgs {
    public string Title { get; set; }
    public List<IBuyable> Items {get; set;}

    public ShopInitArgs(string title, IEnumerable<IBuyable> items){
        Title = title;
        Items = new List<IBuyable>(items);
    }
}

[ResourcePath("UI/Shop")]
public class ShopUI : UIPanel, ITemporaryUI<ShopInitArgs, object>{

    public GameObject itemButtonPrefab;
    public RectTransform itemParent;
    public Text shopTitle;
    public Text descriptionText;
    public RectTransform buyButton;

    IBuyable focusedItem;
    List<IBuyable> items;
    List<GameObject> instances = new List<GameObject>();

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(ShopInitArgs param1) {
        shopTitle.text = param1.Title;
        items = new List<IBuyable>(param1.Items);

        Refresh();

        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    void Refresh() {
        focusedItem = null;
        descriptionText.text = "Click an item to see its description.";
        buyButton.gameObject.SetActive(false);
        var available = items.Where(i => i.Availability == BuyableAvailability.Available);
        if(available.Count() == 0){
            Close();
        }
        UIUtil.GenerateChildren(available, instances, itemParent, CreateChild);
    }

    void Input_OnEnvironmentClick(object sender, EventArgs e) {
        Close();
    }

    GameObject CreateChild(IBuyable item) {
        var instance = Instantiate<GameObject>(itemButtonPrefab);
        instance.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        instance.GetComponentInChildren<Text>().text = string.Format("¥{1}\t{0}", item.Name, item.Cost);
        instance.GetComponent<UIButton>().OnClicked += ShopUI_OnClicked;
        instance.AddComponent<DataContainer>().Store(item);
        instance.transform.Find("New").GetComponent<Image>().enabled = !item.Viewed;
        return instance;
    }

    void ShopUI_OnClicked(object sender, EventArgs e) {
        (sender as Component).transform.Find("New").GetComponent<Image>().enabled = false;
        FocusItem(((Component)sender).GetComponent<DataContainer>().Retrieve<IBuyable>());
    }

    void FocusItem(IBuyable item) {
        item.Viewed = true;
        descriptionText.text = string.Format("<b>{0}</b>\n{1}", item.Name, item.Description);
        buyButton.gameObject.SetActive(true);
        Destroy(buyButton.GetComponent<UIButton>());
        if (PlayerData.Instance.Money >= item.Cost) {
            buyButton.GetComponent<Image>().color = GUIPallet.Instance.defaultBackgroundColor;
            buyButton.GetComponentInChildren<Text>().color = GUIPallet.Instance.defaultTextColor;
            buyButton.gameObject.AddComponent<UIButton>().OnClicked += BuyButton_OnClicked;
        } else {
            buyButton.GetComponent<Image>().color = Color.gray;
            buyButton.GetComponentInChildren<Text>().color = Color.gray;
        }
        buyButton.GetComponentInChildren<Text>().text = string.Format("Buy (¥{0})", item.Cost);
        focusedItem = item;
    }

    void BuyButton_OnClicked(object sender, EventArgs e) {
        if (focusedItem != null) {
            if (items.Contains(focusedItem)) {
                items.Remove(focusedItem);
            }

            PlayerDataConnector.AddMoney(-focusedItem.Cost);

            SoundEffectManager.Play(SoundEffectType.Buy);
            focusedItem.AfterBuyItem();
            DataLogger.LogTimestampedData("Bought", focusedItem.Name);

            Refresh();
        }
    }

    public override void Close() {
        Complete.Raise(this, new EventArgs<object>(null));
        base.Close();
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
    }

}
