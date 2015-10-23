using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class Shop {

    static Dictionary<ShopID, Shop> shops;
    public static Shop GetShop(ShopID id) {
        if (shops.ContainsKey(id)) {
            return shops[id];
        }
        return null;
    }

    public static readonly Shop Kana = new Shop(ShopID.Kana, GetKanaShop);
    public static readonly Shop Clothes = new Shop(ShopID.Clothes, GetClothesShop);
    public static readonly Shop Furnature = new Shop(ShopID.Furnature, GetFurnatureShop);
    public static readonly Shop Words = new Shop(ShopID.Words, GetWordShop);

    static ShopInitArgs GetKanaShop() {
        return new ShopInitArgs("Kana shop", BuyableKana.Values.Where(i => i.Availability == BuyableAvailability.Available).Cast<IBuyable>());
    }

    static ShopInitArgs GetClothesShop() {
        return new ShopInitArgs("Clothes shop", BuyableClothes.GetValues().Where(i => i.Availability == BuyableAvailability.Available).Cast<IBuyable>());
    }

    static ShopInitArgs GetFurnatureShop() {
        return new ShopInitArgs("Furniture shop", BuyableFurniture.GetValues().Where(i => i.Availability == BuyableAvailability.Available).Cast<IBuyable>());
    }

    static ShopInitArgs GetWordShop() {
        return new ShopInitArgs("Word shop", BuyableWord.GetAvailableWords().Cast<IBuyable>().OrderBy(i => i.Cost).Take(15).ToList());
    }

    public ShopID ID { get; private set; }

    Func<ShopInitArgs> getArgs;

    public ShopInitArgs GetInitArgs() {
        if (getArgs == null) {
            Debug.LogError("no shop");
        }
        return getArgs();
    }

    Shop(ShopID guid, Func<ShopInitArgs> getArgs){
        this.ID = guid;
        this.getArgs = getArgs;

        if (shops == null) {
            shops = new Dictionary<ShopID, Shop>();
        }
        if (shops.ContainsKey(guid)) {
            Debug.LogError("Shops already contains guid " + guid);
        }
        shops[guid] = this;
    }

}
