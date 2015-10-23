using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneShop : SceneGuid<ShopID>, IInteractableSceneObject {

    public Shop Shop { get { return Shop.GetShop(ID); } }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        DataLogger.LogTimestampedData("ShopNPC", Guid.ToString());
        ProcessLibrary.SceneShop.Get(Shop, callback, parent);
    }

    public bool HasNew() {
        foreach (var i in Shop.GetShop(ID).GetInitArgs().Items) {
            if (!i.Viewed) {
                return true;
            }
        }
        return false;
    }

    public override ShopID ID {
        get { return new ShopID(Guid); }
    }

    void Start() {
        gameObject.GetOrAddComponent<IndicatorComponent>().Initialize(
            Shop.GetInitArgs().Title, new OverheadIcon(IconType.ShoppingCart),
            new MapIndicator(MapResourceType.ShopNPC), HasNew());
    }
}
