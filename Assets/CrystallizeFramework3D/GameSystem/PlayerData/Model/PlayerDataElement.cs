using UnityEngine;
using System.Collections;
using System;

public class PlayerDataElement {
    public event EventHandler DataChanged;

    protected void RaiseDataChanged() {
        Debug.Log("data changed");
        DataChanged.Raise(this, EventArgs.Empty);
    }
}
