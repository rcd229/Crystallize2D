using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlaySoundEffect : MonoBehaviour {

    public int soundEffectID = 0;

    void Start() {
        var sound = SoundEffectType.PositiveFeedback;
        SoundEffectManager.Play(SoundEffectType.Get<SoundEffectType>(soundEffectID));
    }

}
