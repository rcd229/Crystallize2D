using UnityEngine;
using System.Collections;

public class TradeStateEventArgs : System.EventArgs {

    public TradeState State { get; set; }

    public TradeStateEventArgs(TradeState state) {
        State = state;
    }

}
