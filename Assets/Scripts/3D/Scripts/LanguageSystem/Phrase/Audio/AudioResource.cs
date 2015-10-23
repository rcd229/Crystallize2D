using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioResource  {

    public int id;
    public AudioClip audioClip;

    public AudioResource(int id, AudioClip clip) {
        this.id = id;
        this.audioClip = clip;
    }

}
