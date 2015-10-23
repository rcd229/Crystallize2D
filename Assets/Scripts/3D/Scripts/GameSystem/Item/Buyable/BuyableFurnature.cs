using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuyableFurniture : FlagBuyableItem {

    static List<BuyableFurniture> values;

    //public static readonly BuyableFurnature Apartment = Furnature(1000, "Small apartment", "Apartment with a low down payment. It's small, but it's better than the park bench.", 5, "NULL");
    public static readonly BuyableFurniture Bed = Furniture(new Guid("ed6fafee102d439785fd535e9f1ec360"), 
        2000, "Second-hand Bed", "A sketchy bed.", 1, "Bed");//, Apartment);
    public static readonly BuyableFurniture AreaLamp = Furniture(new Guid("24575cea79fe4d45bb2ed50888d75baa"), 
        4000, "Area lamp", "A bright area light.", 1, "Lamp1", Bed);
    public static readonly BuyableFurniture ClothLamp = Furniture(new Guid("b2c74f4827e9487f9c3980db7e9049aa"), 
        4000, "Cloth lamp", "A light for your room with a cloth lampshade.", 1, "Lamp2", Bed);
    public static readonly BuyableFurniture Sofa = Furniture(new Guid("91171b1f3f5b4e5c9ff58ea3ab32fd2d"), 
        8000, "Second-hand sofa", "An old sofa.", 1, "Sofa", Bed);
    public static readonly BuyableFurniture Chair = Furniture(new Guid("b4323b4439084d94a10c807c5ebe4fc6"),
        8000, "Office chair", "An old office chair.", 1, "OfficeChair", Bed);
    public static readonly BuyableFurniture Desk = Furniture(new Guid("259756a03713490a89acb03d638aae03"), 
        10000, "Desk", "A good place to study.", 1, "Table", Chair);
    public static readonly BuyableFurniture Table = Furniture(new Guid("21c8601433c44c8b8e97b2390b2486cc"), 
        10000, "Coffee table", "Table to place in front of your sofa.", 1, "Desk", Sofa);
    public static readonly BuyableFurniture Books = Furniture(new Guid("fbae9f055c3348b180b77a3d9d108f75"), 
        12000, "Books", "Light reading material.", 1, "Books", Desk);
    public static readonly BuyableFurniture FruitBowl = Furniture(new Guid("4120366130e54060a8b741ae652fec81"), 
        12000, "Fruit bowl", "Bowl of fake fruit.", 1, "FruitBowl", Table);
    public static readonly BuyableFurniture AirConditioner = Furniture(new Guid("9ecdea0bda2d4435a11eec72c5e9b20e"), 
        20000, "Air conditioner", "Keep cool during the hot Japanese summer.", 1, "AirConditioner", Desk);

    static BuyableFurniture Furniture(Guid guid, int cost, string name, string description, int comfort, string gameObjectName, FlagBuyableItem prereq = null) {
        return new BuyableFurniture(guid, cost, name, description + "\nComfort +" + (comfort * 10) + "%", comfort, gameObjectName, prereq);
    }

    public static IEnumerable<BuyableFurniture> GetValues() {
        return values;
    }

    public static BuyableFurniture GetRandomAvailable() {
        return GetValues().Where(i => i.Availability == BuyableAvailability.Available).GetRandomFromEnumerable();
    }

    public string GameObjectName { get; private set; }
    public int Comfort { get; private set; }

    protected override string FlagKey { get { return "Home"; } }

    BuyableFurniture(Guid guid, int cost, string name, string description, int comfort, string gameObjectName, FlagBuyableItem prereq = null)
        : base(guid, cost, name, description + "\n\n<i>This item can increase comfort in your home. Making your home more comfortable can improve your rest quality and improve your ability to work.</i>", prereq) {
        Comfort = comfort;
        GameObjectName = gameObjectName;

        if (values == null) {
            values = new List<BuyableFurniture>();
        }
        values.Add(this);
    }

}
