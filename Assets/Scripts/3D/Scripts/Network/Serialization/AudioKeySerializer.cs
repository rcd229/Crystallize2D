using UnityEngine;
using System.Collections;

public class AudioKeySerializer : INetworkSerializer<AudioKey, string> {

    public string Serialize(AudioKey data) {
        if (data.IsMale) {
            return "m" + data.Text;
        } else {
            return "f" + data.Text;
        }
    }

    public AudioKey Deserialize(string data) {
        if (data[0] == 'm') {
            return new AudioKey(true, data.Substring(1, data.Length - 1));
        } else {
            return new AudioKey(false, data.Substring(1, data.Length - 1));
        }
    }

}
