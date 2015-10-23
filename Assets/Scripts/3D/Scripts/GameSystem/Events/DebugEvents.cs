using UnityEngine;
using System;
using System.Collections;

public class DebugEvents {

    public event EventHandler OnDebugTextRequested;
    public void RaiseDebugTextRequested(object sender, EventArgs args) { OnDebugTextRequested.Raise(sender, args); }

}
