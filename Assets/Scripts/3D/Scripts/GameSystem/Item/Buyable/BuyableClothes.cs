using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuyableClothes : FlagBuyableItem {

    public const string DefaultChestItemName = "White T-shirt";
    public const string DefaultLegsItemName = "Khaki shorts";

    static List<BuyableClothes> values;

    public static readonly BuyableClothes WhiteTShirt = TShirt(new Guid("68b01920350a4170b8726c5fe683fa54"), 0, 0, DefaultChestItemName, AppearanceShirt01Material.White);
    public static readonly BuyableClothes BlackTShirt = TShirt(new Guid("5caa6b586ebc4233b72506d80574dbf9"), 20000, 1, "Black T-shirt", AppearanceShirt01Material.Black);
    public static readonly BuyableClothes StripedTShirt = TShirt(new Guid("1a14c245e16041fd940666d5eb68bb21"), 40000, 2, "Striped T-shirt", AppearanceShirt01Material.Stripe, BlackTShirt);
    public static readonly BuyableClothes TotoroTShirt = TShirt(new Guid("a8467e70628345e4bf40a9143906175f"), 80000, 3, "Totoro T-shirt", AppearanceShirt01Material.Totoro, StripedTShirt);
    public static readonly BuyableClothes WhiteFancyShirt = FancyShirt(new Guid("c4bec43bc481433da538132fe4794161"), 20000, 1, "White collared shirt", AppearanceShirt02Material.White);
    public static readonly BuyableClothes BlackFancyShirt = FancyShirt(new Guid("33884c5633214109898d557bf35692a7"), 40000, 2, "Black collared shirt", AppearanceShirt02Material.Black, WhiteFancyShirt);
    public static readonly BuyableClothes StripedFancyShirt = FancyShirt(new Guid("cd6f61f0fd134cf8b014edfcda6a8433"), 80000, 3, "Striped collared shirt", AppearanceShirt02Material.Striped, BlackFancyShirt);

    public static readonly BuyableClothes KhakiShorts = Shorts(new Guid("f3e97b404a9747dea4f14b7f3d336151"), 0, 0, DefaultLegsItemName, AppearanceLegs01Material.Khaki);
    public static readonly BuyableClothes BlackShorts = Shorts(new Guid("187e91b61e3441b5b705894c20388221"), 20000, 1, "Black shorts", AppearanceLegs01Material.Black);
    public static readonly BuyableClothes RedShorts = Shorts(new Guid("038d0bab09f34c868c5bc628ee92d961"), 40000, 2, "Red shorts", AppearanceLegs01Material.Red, BlackShorts);
    public static readonly BuyableClothes JeansShorts = Shorts(new Guid("e8173d86c8104c40946681d8aefa6880"), 80000, 3, "Jeans shorts", AppearanceLegs01Material.Blue, RedShorts);
    public static readonly BuyableClothes JeansPants = Pants(new Guid("a5aa7c8a448c45fabe2ce3b846d12c8e"), 20000, 1, "Jeans", AppearanceLegs02Material.Jeans);
    public static readonly BuyableClothes KhakiPants = Pants(new Guid("329f5343a81347fb93682c07efc4ac07"), 40000, 2, "Khaki pants", AppearanceLegs02Material.Khaki, JeansPants);
    public static readonly BuyableClothes NavyPants = Pants(new Guid("138c49f264244297abe532885c6ced93"), 80000, 3, "Navy pants", AppearanceLegs02Material.Navy, KhakiPants);

    public static void Initialize() { }

    static BuyableClothes TShirt(Guid guid, int cost, int value, string name, AppearanceShirt01Material mat, BuyableClothes prereq = null) {
        string description = "A casual item to wear on your upper body.";
        description += "\nVersatility +" + (value * 10) + "%";
        return new BuyableClothes(guid, cost, value, 0, name, description, 0, 0, (int)mat, prereq);
    }

    static BuyableClothes FancyShirt(Guid guid, int cost, int value, string name, AppearanceShirt02Material mat, BuyableClothes prereq = null) {
        string description = "A classy item to wear on your upper body.";
        description += "\nFormality +" + (value * 10) + "%";
        return new BuyableClothes(guid, cost, 0, value, name, description, 0, 1, (int)mat, prereq);
    }

    static BuyableClothes Shorts(Guid guid, int cost, int value, string name, AppearanceLegs01Material mat, BuyableClothes prereq = null) {
        string description = "A casual item to wear on your legs.";
        description += "\nVersatility +" + (value * 10) + "%";
        return new BuyableClothes(guid, cost, value, 0, name, description, 1, 0, (int)mat, prereq);
    }

    static BuyableClothes Pants(Guid guid, int cost, int value, string name, AppearanceLegs02Material mat, BuyableClothes prereq = null) {
        string description = "A classy item to wear on your legs.";
        description += "\nFormality +" + (value * 10) + "%";
        return new BuyableClothes(guid, cost, 0, value, name, description, 1, 1, (int)mat, prereq);
    }

    static BuyableClothes GetItem(string name) {
        return values.Where(i => i.Name == name).FirstOrDefault();
    }

    public static IEnumerable<BuyableClothes> GetValues() {
        return values;
    }

    public static BuyableClothes GetItemWithParams(BodyPartType partType, int type, int material) {
        return (from c in GetValues()
                where c.PartType == (int)partType && c.MeshType == type && c.MaterialType == material
                select c).FirstOrDefault();
    }

    protected override string FlagKey { get { return "Clothes"; } }

    public int Versatility { get; private set; }
    public int Formality { get; private set; }
    public int PartType { get; private set; }
    public int MeshType { get; private set; }
    public int MaterialType { get; private set; }

    BuyableClothes(Guid guid, int cost, int versatilityValue, int formalityValue, string name, string description, int partType, int meshType, int materialType, BuyableClothes prereq = null)
        : base(guid, cost, name, description + "\n\n<i>Wearing appropriate clothes can help you earn more money when doing certain tasks.</i>", prereq) {
        //Debug.Log("Adding " + name);
        Versatility = versatilityValue;
        Formality = formalityValue;
        PartType = partType;
        MeshType = meshType;
        MaterialType = materialType;

        if (values == null) {
            values = new List<BuyableClothes>();
        }
        values.Add(this);
    }

}
