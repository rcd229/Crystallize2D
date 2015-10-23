using UnityEngine;
using System.Collections;
using System;

public class AreaUnlockConfirmationUIRequestEventArgs : UIRequestEventArgs {

    public AreaGameData Area { get; private set; }

    public AreaUnlockConfirmationUIRequestEventArgs(GameObject menuParent, AreaGameData area) : base(menuParent) {
        this.Area = area;
    }

}
