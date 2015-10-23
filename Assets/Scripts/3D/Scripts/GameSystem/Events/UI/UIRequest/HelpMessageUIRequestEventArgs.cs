using UnityEngine;
using System.Collections;

public class HelpMessageUIRequestEventArgs : UIRequestEventArgs {

    public string Text { get; set; }

    public HelpMessageUIRequestEventArgs(GameObject menuParent, string text)
        : base(menuParent) {
        Text = text;
    }

}
