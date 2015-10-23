using UnityEngine;
using System.Collections;

public class RemoteDataTest : MonoBehaviour {

    IEnumerator Start() {
        string url = @"https://onedrive.live.com/download?cid=03E2EBA4775CF689&resid=3E2EBA4775CF689%2183402&authkey=ABs6Bac7hAtVNPM";
        var www = new WWW(url);
        Debug.Log("Retrieving");

        yield return www;

        Debug.Log(www.text);
    }

}