using UnityEngine;
using System.Collections;
using System;
using System.IO;


public class ClearPlayerMoney : MonoBehaviour {

	public string fileRoot = "/Users/Shiyu_Wang/Desktop/";
    public bool relativeToDataPath = false;
	public const string folderName = "PlayerData/";
	// Use this for initialization
	void Start () {
        var dir = GetDirectoryPath();

		foreach(var file in dir.GetFiles()){
			if(file.Name.EndsWith(".xml")){
				var xml = new XmlDataUtil(file.FullName);
				xml.clearMoney();
				xml.Save();
			}
		}
		Debug.Log("finished");
	}

    DirectoryInfo GetDirectoryPath() {
        var directory = new DirectoryInfo(fileRoot + folderName);
        if (relativeToDataPath) {
            directory = new DirectoryInfo(
                Directory.GetParent(Application.dataPath).Parent + "/" + fileRoot + "/" + folderName
                );
        }
        return directory;
    }
}
