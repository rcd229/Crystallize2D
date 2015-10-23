using UnityEngine;
using System.Collections;

public class UIInputEventArgs : System.EventArgs {

    public KeyCode KeyCode { get; set; }

    public UIInputEventArgs(KeyCode keyCode) {
        KeyCode = keyCode;
    }

}
