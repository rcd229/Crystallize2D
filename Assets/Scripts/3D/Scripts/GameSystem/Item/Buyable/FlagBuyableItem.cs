using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class FlagBuyableItem : IBuyable {

    protected abstract string FlagKey { get; }

    string ViewedFlagKey { get { return FlagKey + "Viewed"; } }

    public Guid Guid { get; private set; }
    public int Cost { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public BuyableAvailability Availability { get { return GetAvailability(); } }

    public bool Viewed { 
        get{
            return PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedFlagKey).Contains(Guid.ToString());
        }
        set {
            PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedFlagKey).Add(Guid.ToString());
        }
    }

    List<FlagBuyableItem> prerequisites = new List<FlagBuyableItem>();

    protected FlagBuyableItem(Guid guid, int cost, string name, string description, FlagBuyableItem prereq = null) {
        this.Guid = guid;
        Cost = cost;
        Name = name;
        Description = description;

        if (prereq != null) {
            prerequisites.Add(prereq);
        }
    }

    public void AfterBuyItem() {
        PlayerData.Instance.Flags.GetOrCreateFlagSet(FlagKey).Add(Guid.ToString());

        if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.EquipClothes)) {
            UILibrary.MessageBox.Get("You can change your clothes at home by selecting the bathroom door.");
            PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.EquipClothes);
        }
    }

    public BuyableAvailability GetAvailability() {
        if (PlayerData.Instance.Flags.GetOrCreateFlagSet(FlagKey).Contains(Guid.ToString()) || Cost == 0) {
            return BuyableAvailability.Purchased;
        } else if (PrerequisitesFulfilled()) {
            return BuyableAvailability.Available;
        }
        return BuyableAvailability.Locked;
    }

    public bool PrerequisitesFulfilled() {
        foreach (var p in prerequisites) {
            if (p.Availability != BuyableAvailability.Purchased) {
                return false;
            }
        }
        return true;
    }

}
