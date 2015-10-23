using UnityEngine;
using System.Collections;

public class AppearanceGameDataPreset : AppearanceGameData {
	private string resourcePath = "Avatar/";

	//Note: if this is not null, this will overwrite other settings
	public string PrefabName {get;set;}

	public string ResourcePath {
		get {return resourcePath + PrefabName;}
	}

	//constructor that loads a complete avatar prefab rom file path. WILL BE ADDED TO PRESET
	public AppearanceGameDataPreset(string prefabPath){
		PrefabName = prefabPath;
		AppearancePresetLibrary.Add(this);
	}
}
