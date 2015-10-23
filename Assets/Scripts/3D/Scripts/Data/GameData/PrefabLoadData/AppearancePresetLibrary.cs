using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AppearancePresetLibrary : MonoBehaviour {

	static Dictionary<int, AppearanceGameData> appearances = new Dictionary<int, AppearanceGameData>();
	static int count = 0;
	static List<string> PathsToCheck = new List<string>();

	public static readonly AppearanceGameData FemaleBase = new AppearanceGameDataPreset("FemaleBase");

	public static void Add(AppearanceGameData data){
		int key = count;
		appearances[key] = data;
		if(data is AppearanceGameDataPreset){
			var castData = (AppearanceGameDataPreset) data;
			AddRequiredResource(castData.ResourcePath);
		}
		count++;
	}

	[ResourcePathMethod]
	public static IEnumerable<string> GetRecourcePaths() {
		return PathsToCheck;
	}
	
	/// <summary>
	/// Get the appearance from the path.
	/// </summary>
	/// <param name="keystring">Keystring.</param>
	public static AppearanceGameData Get(int key){
		AppearanceGameData ret;
		appearances.TryGetValue(key, out ret);
		return ret;
	}

	public static AppearanceGameData Get(string name){
		var answerList = from appearance in appearances.Values
						where appearance is AppearanceGameDataPreset && ((AppearanceGameDataPreset) appearance).PrefabName == name
						select appearance;

		return answerList.FirstOrDefault();
	}

	public static GameObject GetAndCreate(int key){
		var data = Get (key);
		if(data == null){
			return null;
		}
		else{
			return AppearanceLibrary.CreateObject(data);
		}
	}
	
	static void AddRequiredResource(string path) {
		GameDataInitializer.AddRequiredResource(path);
		PathsToCheck.Add(path);
	}

	public static AppearanceGameData GetRandomPreset(){
		return appearances.Values.ToArray()[UnityEngine.Random.Range(0, appearances.Count)];
	}
}
