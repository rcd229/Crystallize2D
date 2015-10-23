using UnityEngine;
using System.Collections;

public class GameEventArgs : System.EventArgs {

    public GameEventType Type { get; private set; }

    public GameEventArgs(GameEventType type) {
        Type = type;
    }

}
