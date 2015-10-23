using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CrystallizeData{
	public abstract class StaticSerializedTargetGameData : StaticGameData {

	}

//	public abstract class StaticSerializedTargetGameData<T> : StaticSerializedTargetGameData where T : TargetGameData, new(){
//
//		protected T target = new T();
//
//		protected virtual void SetParent(string parent){
//			target.Parent = parent;
//		}
//
//		protected virtual void SetPrefab(string prefabName){
//			target.AppearanceData.PrefabName = prefabName;
//			AddRequiredResource(target.AppearanceData.ResourcePath, prefabName);
//		}
//
//		protected void Set(string parent, string prefab){
//			SetParent(parent);
//			SetPrefab(prefab);
//		}
//
//		void AddRequiredResource(string path, string name) {
//			GameDataInitializer.AddRequiredResource(path + name);
//		}
//	}
}