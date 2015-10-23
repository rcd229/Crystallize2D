using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AudioArg {
    public string Text { get; private set; }
    public bool IsMale { get; private set; }
    public AudioArg(string t, bool m) {
        Text = t;
        IsMale = m;
    }
}

public class AudioLoader {

    public const string AudioDirectory = "Audio/";

    public static void SaveWavToLibrary(string text, bool isMale, AudioClip clip) {
        WavSerializer.Save(GetFullFilepath(text, isMale), clip);
    }

    public static string GetFilename(string text, bool isMale) {
        var prefix = "";
        if (isMale) {
            prefix = "m_";
        } else {
            prefix = "f_";
        }
        return prefix + RemoveInvalidCharacters(text) + ".wav";
    }

    public static string GetFullFilepath(string text, bool isMale) {
        var f = GetFilename(text, isMale);
        return Directory.GetParent(Application.dataPath) + "/" + AudioDirectory + f;
    }

    static string RemoveInvalidCharacters(string str) {
        foreach (var c in System.IO.Path.GetInvalidFileNameChars()) {
            str = str.Replace(c.ToString(), "");
        }
        return str;
    }

    public static void PlayAudio(string text, bool isMale) {
        if(File.Exists(GetFullFilepath(text, isMale))){
            //Debug.Log("Playing local audio.");
            CoroutineManager.Instance.StartCoroutine(LoadLocalAudio(text, isMale, PlayAudio));
        }
		else {
			//if(Network.isClient){
            if(CrystallizeNetwork.Connected){
	            Debug.Log("Playing remote audio from server");

                CrystallizeNetwork.Client.RequestAudioClipFromServer(isMale, text, HandleAudioResponse);
			}
			else{
				NeospeechAudioLoader.GetAudioClip(text, isMale, PlayAudio);
			}
        }
    }

    static void HandleAudioResponse(AudioKey audioKey) {
        CoroutineManager.Instance.StartCoroutine(LoadLocalAudio(audioKey.Text, audioKey.IsMale, PlayAudio));
    }

    static void PlayAudio(AudioClip clip) {
        AudioSource a = null;
        var voice = GameObject.FindGameObjectWithTag("Voice");
        if (voice) {
            a = voice.GetComponent<AudioSource>();
        } else {
            if (!Camera.main.GetComponent<AudioSource>()) {
                Camera.main.gameObject.AddComponent<AudioSource>();
            }
            a = Camera.main.GetComponent<AudioSource>();
        }
        a.clip = clip;
        a.Play();
    }

    public static IEnumerator LoadLocalAudio(string text, bool isMale, Action<AudioClip> callback) {
        var url = "file:///" + GetFullFilepath(text, isMale);
        var www = new WWW(url);

        yield return www;
        var clip = www.audioClip;
        callback(clip);
    }

}
