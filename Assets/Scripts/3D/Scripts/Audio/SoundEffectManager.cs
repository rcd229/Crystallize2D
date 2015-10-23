using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour {

    static SoundEffectManager _instance;

    public static void Play(SoundEffectType sfx) {
        if (_instance) {
            var clip = sfx.LoadResource();
            _instance.GetComponent<AudioSource>().clip = clip;
            _instance.GetComponent<AudioSource>().Play();
        }
    }

    void Awake() {
        if (_instance) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

}
