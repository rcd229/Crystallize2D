using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShopID : UniqueID {

    public static readonly ShopID Kana = new ShopID("b9ca23fa6cb44d549d40784719de695f");
    public static readonly ShopID Clothes = new ShopID("bc2f4b78a0d04995b7ba8b10b9ad8bc7");
    public static readonly ShopID Furnature = new ShopID("ae44e203fd41488ba06305910d325d1c");
    public static readonly ShopID Words = new ShopID("bb8011c8035f4176a87dacb6d9d40199");

    public ShopID() : base() { }
    public ShopID(string id) : base(id) { }
    public ShopID(Guid id) : base(id) { }

}
