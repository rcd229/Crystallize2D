using UnityEngine;
using System.Collections;

public class TargetLoadUtil  {
	public string Parent{get;set;}
	public AppearanceGameData AppearanceData{get;set;}
	public string ParentTag{get;set;}

	public TargetLoadUtil(){
		Parent = "";
		AppearanceData = AppearanceLibrary.GetRandomAppearance();
	}

	public TargetLoadUtil(string parent, string prefab){
		Parent = parent;
		AppearanceData = AppearancePresetLibrary.Get(prefab);
		if(AppearanceData == null){
			Debug.LogError("prefab dosent exist in library");
		}
	}

	public TargetLoadUtil(string parent, AppearanceGameData prefab){
		Parent = parent;
		AppearanceData = prefab;
	}

	public TargetLoadUtil(string parent, string prefab, string parentTag) : this(parent, prefab){
		ParentTag = parentTag;
	}

	public TargetLoadUtil(string parent, AppearanceGameData prefab, string parentTag) : this(parent, prefab){
		ParentTag = parentTag;
	}
	
	public virtual GameObject Instantiate(){
		GameObject childInstance = AppearanceLibrary.CreateObject(AppearanceData);
		GameObject parent = GameObject.Find(Parent);
		SetToDefaultLocalPosition(childInstance.transform, parent.transform);
		return parent;
	}

	void SetToDefaultLocalPosition(Transform child, Transform parent){
		child.SetParent(parent);
		child.localPosition = Vector3.zero;
		child.localRotation = Quaternion.identity;
	}
}
