using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

public class ScriptableObjectUtility : EditorWindow
{
	[MenuItem("Utils/Create ScriptableObject")]
	public static void Init(){
		GetWindow<ScriptableObjectUtility>();
	}

	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset (Type type)
	{
		ScriptableObject asset = ScriptableObject.CreateInstance(type);
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + type.ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

	public static List<T> GetAllAssets<T> (string path) where T : class{
		var list = new List<T> ();
		AddObjectsFromDirectoryRecursively<T> (Path.GetDirectoryName (Application.dataPath) + "/" + path,
		                                       path,
		                                       list);
		return list;
	}

	static void AddObjectsFromDirectoryRecursively<T>(string directory, string assetPath, List<T> conversationList) where T : class {
		var paths = Directory.GetFiles(directory);
		foreach(string p in paths){
			var obj = AssetDatabase.LoadAssetAtPath(assetPath + "/" + Path.GetFileNameWithoutExtension(p), typeof(UnityEngine.Object));
			
			//Debug.Log (obj);
			var conv = obj as T;
			if(conv != null){
				conversationList.Add(conv);
			}
		}
		
		var dirs = Directory.GetDirectories (directory);
		foreach (var dir in dirs) {
			AddObjectsFromDirectoryRecursively(dir, assetPath + "/" + Path.GetFileName(dir), conversationList);
		}
	}

	string fileName = "NewFile";
	Type[] types;
	string[] typeStrings;
	int typeIndex = 0;

	void OnEnable(){
		types = Assembly.GetAssembly(typeof(Extensions)).GetTypes().OrderBy((T) => T.Name)
			.Where(t => t.IsSubclassOf(typeof(ScriptableObject))).ToArray();
		typeStrings = new string[types.Count()];
		int i = 0;
		foreach(var t in types){
			typeStrings[i] = t.ToString();
			i++;
		}
	}

	void OnGUI(){
		///if(dict == null){
		fileName = GUILayout.TextField(fileName);
		
		
		typeIndex = EditorGUILayout.Popup(typeIndex, typeStrings);
		
		if(GUILayout.Button("Create")){
			CreateAsset(types[typeIndex]);
		}
	}
}
