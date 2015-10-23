using UnityEngine;
using System;
using System.Collections;

public class InputEvents : GameEvents {

    public event EventHandler OnLeftClick;
    public void RaiseLeftClick(object sender, EventArgs e) { OnLeftClick.Raise(sender, e); }
    public event EventHandler OnRightClick;
    public void RaiseRightClick(object sender, EventArgs e) { OnRightClick.Raise(sender, e); }
    public event EventHandler OnEnvironmentClick;
    public void RaiseEnvironmentClick(object sender, EventArgs e) { OnEnvironmentClick.Raise(sender, e); }

}