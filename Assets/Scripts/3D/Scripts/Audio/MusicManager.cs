using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

    const float BaseVolume = 0.7f;

    static MusicManager _instance;
    static MusicType _currentMusic;
    static Coroutine _currentRoutine;

    public static void FadeToMusic(MusicType music) {
        if (_instance && music != _currentMusic) {
            _currentMusic = music;
            if (_currentRoutine != null) {
                _instance.StopCoroutine(_currentRoutine);
            }

            var clip = music.LoadResource();
            _currentRoutine = _instance.StartCoroutine(_instance.FadeToMusicCoroutine(clip));
        }
    }

    void Awake() {
        if (_instance) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    IEnumerator FadeToMusicCoroutine(AudioClip newMusic) {
        var a = GetComponent<AudioSource>();

        for (float t = 0; t < 1f; t += Time.deltaTime) {
            a.volume = BaseVolume * (1f - t);
            yield return null;
        }

        a.clip = newMusic;
        a.Play();

        for (float t = 0; t < 1f; t += Time.deltaTime) {
            a.volume = BaseVolume * t;
            yield return null;
        }
        a.volume = BaseVolume;

        _currentRoutine = null;
    }

}
