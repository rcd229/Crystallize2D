using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class SceneShopProcess : IProcess<Shop, object> {

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(Shop shop) {
        var args = shop.GetInitArgs();
        if (args.Items.Where(i => i.Availability == BuyableAvailability.Available).Count() == 0) {
            var msg = UILibrary.MessageBox.Get("That person doesn't have anything else to sell right now.");
            msg.Complete += ShopComplete;
        } else {
            var shopUI = UILibrary.ShopPanel.Get(shop.GetInitArgs());
            shopUI.Complete += ShopComplete;
        }
    }

    void ShopComplete(object sender, EventArgs<object> e) {
        Exit();
    }

    public void ForceExit() {

    }

    void Exit() {
        IndicatorManager.SetIndicatorsChanged();
        OnExit.Raise(this, null);
    }

}
