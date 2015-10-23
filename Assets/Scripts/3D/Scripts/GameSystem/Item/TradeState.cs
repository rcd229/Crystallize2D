using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TradeState {

    public bool IsReady { get; set; }
    public int Money { get; set; }
    public List<int> Items { get; set; }

    public TradeState(bool isReady) {
        IsReady = isReady;
    }

    public TradeState(int money, List<int> items) {
        Money = money;
        Items = items;
    }

    public override bool Equals(object obj) {
        if (!(obj is TradeState)) {
            return false;
        }

        var ts = (TradeState)obj;
        return IsContentSame(ts) && IsReady == ts.IsReady;
    }

    public override int GetHashCode() {
        return IsReady.GetHashCode() + Money.GetHashCode() + Items.Count.GetHashCode();
    }

    public bool IsContentSame(TradeState other) {
        if (other == null) {
            return false;
        }

        if (this.Money != other.Money) {
            return false;
        }

        if (this.Items.Count != other.Items.Count) {
            return false;
        }

        for (int i = 0; i < Items.Count; i++) {
            if (Items[i] != other.Items[i]) {
                return false;
            }
        }

        return true;
    }

}
