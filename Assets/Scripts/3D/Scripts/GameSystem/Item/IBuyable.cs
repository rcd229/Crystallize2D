using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public enum BuyableAvailability {
    Locked,
    Available,
    Purchased
}

public interface IBuyable {
    int Cost { get; }
    string Name { get; }
    string Description { get; }
    bool Viewed { get; set; }
    BuyableAvailability Availability { get; }

    void AfterBuyItem();
}
