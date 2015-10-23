using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

[CustomEditor(typeof(SceneShop))]
public class SceneShopEditor : NamedGuidEditor<ShopID> {
    protected override string ValueLabel {
        get { return "Shop"; }
    }
}
