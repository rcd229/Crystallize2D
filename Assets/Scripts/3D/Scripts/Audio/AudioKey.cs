using UnityEngine;
using System.Collections;

public class AudioKey {
    public bool IsMale { get; set; }
    public string Text { get; set; }

    public AudioKey() {
        IsMale = false;
        Text = "";
    }

    public AudioKey(bool isMale, string text) {
        IsMale = isMale;
        Text = text;
    }

    public string ToKeyString() {
        return new AudioKeySerializer().Serialize(this);
    }
}
