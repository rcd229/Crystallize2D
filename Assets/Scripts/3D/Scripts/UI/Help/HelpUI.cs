using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/Help")]
public class HelpUI : UIPanel, ITemporaryUI<object, object>{

    public static HelpUI Instance { get; private set; }

    public GameObject itemButtonPrefab;
    public RectTransform itemParent;
    public Text shopTitle;
    public Text descriptionTitle;
    public Text descriptionText;
    public Image iconImage;

    HelpData focusedItem;
    List<HelpData> items;
    List<GameObject> instances = new List<GameObject>();

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object param1) {
        Instance = this;
        CrystallizeEventManager.GetInstance();
        MainCanvas.main.PushLayer();
        MainCanvas.main.Add(transform);
        shopTitle.text = "Tutorial";
        Refresh();
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            MainCanvas.main.PopLayer();
        }
    }

    void Refresh() {
        focusedItem = null;
        descriptionTitle.text = "";
        descriptionText.text = "Click a topic to learn more about it.";
        iconImage.gameObject.SetActive(false);
        UIUtil.GenerateChildren(HelpData.GetValues(), instances, itemParent, CreateChild);
    }

    GameObject CreateChild(HelpData item) {
        var instance = Instantiate<GameObject>(itemButtonPrefab);
        instance.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        instance.GetComponentInChildren<Text>().text = item.Title;
        instance.GetComponent<UIButton>().OnClicked += ShopUI_OnClicked;
        instance.AddComponent<DataContainer>().Store(item);
        return instance;
    }

    void ShopUI_OnClicked(object sender, EventArgs e) {
        FocusItem(((Component)sender).GetComponent<DataContainer>().Retrieve<HelpData>());
    }

    void FocusItem(HelpData item) {
        descriptionTitle.text = item.Title;
        descriptionText.text = item.Content;
        if (item.Icon == null) {
            iconImage.gameObject.SetActive(false);
        } else {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = item.Icon.LoadResource();
        }

        focusedItem = item;
    }

    public override void Close() {
        Complete.Raise(this, new EventArgs<object>(null));
        base.Close();
    }

}
