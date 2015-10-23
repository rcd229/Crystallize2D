using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/Equipment")]
public class EquipmentUI : UIPanel, ITemporaryUI<object, object>, IDebugMethods {

    public GameObject buttonPrefab;
    public RectTransform chestParent;
    public RectTransform legsParent;
    public Text statsText;

    public event EventHandler<EventArgs<object>> Complete;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(object param1) {
        Refresh();
        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    void Refresh() {
        instances.DestroyAndClear();

        foreach (var item in BuyableClothes.GetValues()) {
            if (item.Availability == BuyableAvailability.Purchased) {
                if (item.PartType == 0) {
                    CreateChild(item, chestParent);
                } else {
                    CreateChild(item, legsParent);
                }
            }
        }

        statsText.text = "";
        statsText.text += string.Format("{0}%", Mathf.RoundToInt(PlayerDataConnector.GetComfortMultiplier() * 100f));
        statsText.text += "\n";
        statsText.text += string.Format("{0}%", Mathf.RoundToInt(PlayerDataConnector.GetVersatilityMultiplier() * 100f));
        statsText.text += "\n";
        statsText.text += string.Format("{0}%", Mathf.RoundToInt(PlayerDataConnector.GetFormalityMultiplier() * 100f));
    }

    void CreateChild(BuyableClothes item, RectTransform parent) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = item.Name;
        if (PlayerData.Instance.Session.ChestItem == item.Name || PlayerData.Instance.Session.LegsItem == item.Name) {
            instance.GetComponentInChildren<Text>().color = GUIPallet.Instance.defaultTextColor;
                //.GetComponent<Image>().color = Color.yellow.Lighten(0.3f);
        }
        instance.transform.SetParent(parent, false);
        instance.AddComponent<DataContainer>().Store(item);
        instance.GetComponent<UIButton>().OnClicked += EquipmentUI_OnClicked;
        instances.Add(instance);
    }

    void EquipmentUI_OnClicked(object sender, EventArgs e) {
        var item = ((Component)sender).GetComponent<DataContainer>().Retrieve<BuyableClothes>();
        if (item.PartType == 0) {
            PlayerData.Instance.Appearance.TopType = item.MeshType;
            PlayerData.Instance.Appearance.TopMaterial = item.MaterialType;
            PlayerData.Instance.Session.ChestItem = item.Name;
        } else {
            PlayerData.Instance.Appearance.BottomType = item.MeshType;
            PlayerData.Instance.Appearance.BottomMaterial = item.MaterialType;
            PlayerData.Instance.Session.LegsItem = item.Name;
        }

        GameObject.FindObjectOfType<AvatarConstructor>().UpdateAvatar();
        Refresh();
    }

    public override void Close() {
        Complete.Raise(this, null);
        base.Close();
    }

    void Input_OnEnvironmentClick(object sender, EventArgs e) {
        Close();
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(UnlockAllClothes);
    }

    public string UnlockAllClothes(string s) {
        foreach (var c in BuyableClothes.GetValues()) {
            c.AfterBuyItem();
        }
        Refresh();
        return "all clothes unlocked.";
    }
    #endregion
}
