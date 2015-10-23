using UnityEngine;
using System.Collections;

public class SpeechPanelUIRequestEventArgs : UIRequestEventArgs {

    public bool Open { get; set; }

    public SpeechPanelUIRequestEventArgs(bool open) : base(null) {
        Open = open;
    }

}
