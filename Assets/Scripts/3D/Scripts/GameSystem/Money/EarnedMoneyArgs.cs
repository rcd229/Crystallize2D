using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EarnedMoneyArgs  {

    public int BaseMoney { get; set; }
    public int LostMoney { get; set; }
    public List<ValueModifier> Modifiers { get; set; }

    public EarnedMoneyArgs(int baseMoney, int lostMoney, params ValueModifier[] modifiers) {
        BaseMoney = baseMoney;
        LostMoney = lostMoney;
        Modifiers = new List<ValueModifier>(modifiers);
    }

    public int GetValue() {
        var earned = (float)(BaseMoney - LostMoney);
        foreach (var m in Modifiers) {
            earned *= m.Value;
        }
        return Mathf.RoundToInt(earned);
    }

}
