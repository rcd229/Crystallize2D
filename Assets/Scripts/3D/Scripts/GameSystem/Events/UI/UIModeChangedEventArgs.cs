using UnityEngine;
using System.Collections;

public class UIModeChangedEventArgs : System.EventArgs {

    public UIMode Mode { get; set; }

    public UIModeChangedEventArgs(UIMode mode)
    {
        this.Mode = mode;
    }

}
