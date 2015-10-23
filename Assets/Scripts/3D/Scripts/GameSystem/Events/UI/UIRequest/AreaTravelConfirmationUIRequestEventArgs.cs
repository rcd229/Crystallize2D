using UnityEngine;
using System.Collections;
using System;

public class AreaTravelConfirmationUIRequestEventArgs : UIRequestEventArgs {

    public AreaGameData Area { get; private set; }

    public AreaTravelConfirmationUIRequestEventArgs(GameObject menuParent, AreaGameData area)
        : base(menuParent) {
        this.Area = area;
    }

}
