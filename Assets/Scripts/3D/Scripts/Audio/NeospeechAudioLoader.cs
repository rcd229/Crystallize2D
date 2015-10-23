using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NeospeechAudioLoader : MonoBehaviour {

    public static void GetAudioClip(string text, bool isMale, Action<AudioClip> callback){
        CoroutineManager.Instance.StartCoroutine(GetAndPlayAudioClip(text, isMale, callback));
    }

    static IEnumerator GetAndPlayAudioClip(string text, bool isMale, Action<AudioClip> callback) {
        var voiceID = "302";
        if (isMale) {
            voiceID = "301";
        }
        var url = "https://tts.neospeech.com/rest_1_1.php?method=ConvertSimple"
            + "&email=gabeculbertson@gmail.com&accountId=d586c6ba1a&loginKey=LoginKey"
            + "&loginPassword=00d986d2622ae23175c7&voice=" + voiceID
            + "&outputFormat=FORMAT_WAV&sampleRate=16&text=\"" + text + "\"";
		//debug setting
		//if(GameSettings.Instance.IsDebug){
			url = "https://tts.neospeech.com/rest_1_1.php?method=ConvertSimple"
				+ "&email=wsywsy108108@gmail.com&accountId=fa97e66262&loginKey=LoginKey"
				+ "&loginPassword=1a15dd62ab2e39c786fa&voice=" + voiceID
				+ "&outputFormat=FORMAT_WAV&sampleRate=16&text=\"" + text + "\"";
		//}
		//
        var www = new WWW(url);

        yield return www;

        var convNumber = GetElement("conversionNumber", www.text);
        Debug.Log(www.text + "; " + convNumber);

        url = "https://tts.neospeech.com/rest_1_1.php?method=GetConversionStatus"
                 + "&email=gabeculbertson@gmail.com&accountId=d586c6ba1a&loginKey=LoginKey"
                 + "&conversionNumber=" + convNumber;

		//debug settings
		//if(GameSettings.Instance.IsDebug){
			url = "https://tts.neospeech.com/rest_1_1.php?method=GetConversionStatus"
				+ "&email=wsywsy108108@gmail.com&accountId=fa97e66262&loginKey=LoginKey"
					+ "&conversionNumber=" + convNumber;
		//}
		//
        var statusCode = "0";
        int count = 0;
        while (statusCode != "4") {
            www = new WWW(url);

            yield return www;

            if (count > 5) {
                if (GetElement("resultCode", www.text) == "-5") {
                    Debug.Log("Not enough phrases for the day.");
                }
				yield break;
            }

            statusCode = GetElement("statusCode", www.text);
            Debug.Log(www.text + "; " + statusCode);
            count++;

            yield return new WaitForSeconds(0.1f);
        }


        url = GetElement("downloadUrl", www.text); //"http://media.neospeech.com/audio/ws/2015-07-16/d586c6ba1a/result-30987669.wav";
        www = new WWW(url);

        yield return www;

        var clip = www.GetAudioClip(false, false, AudioType.WAV);
        AudioLoader.SaveWavToLibrary(text, isMale, clip);
        callback(clip);
    }

    static string GetElement(string key, string response) {
        var regex = new Regex(key + "=\"(.*?)\"");
        var match = regex.Match(response);
        return match.Groups[1].Value;
    }

}
