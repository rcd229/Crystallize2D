using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SkipSessionButtonUI : UIPanel, ITemporaryUI<object, object> {

    const string ResourcePath = "UI/SkipSessionButton";
    public static SkipSessionButtonUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<SkipSessionButtonUI>(ResourcePath);
    }

    public event EventHandler<EventArgs<object>> Complete;

    public UIButton button;

    public void Initialize(object param1) {
        button.OnClicked += button_OnClicked;
    }

    void button_OnClicked(object sender, EventArgs e) {
        RaiseComplete();
    }

    void RaiseComplete() {
        Complete.Raise(this, null);
    }

}
