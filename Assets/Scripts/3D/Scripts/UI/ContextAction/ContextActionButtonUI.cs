using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ContextActionButtonUI : UIPanel, ITemporaryUI<string, string> {

    const string ResourcePath = "UI/ContextActionButton";
    public static ContextActionButtonUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<ContextActionButtonUI>(ResourcePath);
    }

    public UIButton button;

    public event EventHandler<EventArgs<string>> Complete;

    public void Initialize(string param1) {
        button.GetComponentInChildren<Text>().text = param1;
        button.OnClicked += button_OnClicked;
    }

    void button_OnClicked(object sender, EventArgs e) {
        RaiseComplete();
    }

    void RaiseComplete() {
        var s = "";
        if (RegionManager.Instance) {
            if (RegionManager.Instance.CurrentRegion) {
                s = RegionManager.Instance.CurrentRegion.name;
            }
        }
        Complete.Raise(this, new EventArgs<string>(s));
    }

    void Update() {
        if (RegionManager.Instance) {
            if (RegionManager.Instance.CurrentRegion) {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
            } else {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
            }
        }
    }
}
