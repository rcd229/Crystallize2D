using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using Util.Serialization;

public class XmlDataUtil {

	string filepath;
	PlayerData data;

	public XmlDataUtil(string path){
		filepath = path;
		if(File.Exists(filepath)){
			data = Serializer.LoadFromXml<PlayerData>(filepath);
		}
		else{
			Debug.LogError(filepath + " does not exist");
		}
	}

	public void clearMoney(){
		data.Money = 0;
	}

	public void Save(){
		Serializer.SaveToXml<PlayerData>(filepath, data);
	}
}
