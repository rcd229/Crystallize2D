using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TTSTest : MonoBehaviour {

    static byte[] GetBytes(string str) {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }


    IEnumerator Start() {
        var url = @"http://translate.google.com/translate_tts?ie=UTF-8&tl=en&q=hello+world";
        WWW www = new WWW(url );
        yield return www;
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = www.texture;
        //var clip = www.GetAudioClip(false, false, AudioType.MPEG);
        var bytes = www.text;

        string tempFile = Application.persistentDataPath + "/bytes.mp3";
        System.IO.File.WriteAllBytes(tempFile, GetBytes(bytes));

        WWW loader = new WWW("file://" + tempFile);
        yield return loader;
        if (!System.String.IsNullOrEmpty(loader.error))
            Debug.LogError(loader.error);

        AudioClip s1 = loader.GetAudioClip(false, false, AudioType.MPEG);
        //or
        //AudioClip s2 = loader.audioClip;


        //Debug.Log(www.text);
        //Debug.Log(clip);
        Camera.main.GetComponent<AudioSource>().clip = s1;
        Camera.main.GetComponent<AudioSource>().Play();
        //if (www.audioClip) {


        //} else {

        //}
    }

}
