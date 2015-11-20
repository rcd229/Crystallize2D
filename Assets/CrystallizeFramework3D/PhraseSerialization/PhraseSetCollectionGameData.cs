using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class PhraseSetCollectionGameData {

    const string FileExtension = ".txt";
    const string ResourcePath = "PhraseSets/";
    const string EditorFilePath = "/Resources/" + ResourcePath;
    const string DefaultPhraseSet = "Default";

    static Dictionary<string, PhraseSetGameData> instances = new Dictionary<string, PhraseSetGameData>();

    public static PhraseSetGameData Default {
        get {
            return GetOrCreateItem(DefaultPhraseSet);
        }
    }

    static string GetResourcePath(string file) {
        return ResourcePath + file;
    }

    static string GetEditorDirectory() {
        return Application.dataPath + EditorFilePath;
    }

    static string GetEditorDataPath(string file) {
        return Application.dataPath + EditorFilePath + file + FileExtension;
    }

    public static void SaveAll() {
        foreach (var item in instances.Keys) {
            SaveItem(item);
        }
        //Debug.Log("Saved PhraseSets to [" + GetEditorDirectory() + "]");
    }

    public static void LoadAll() {
        bool loaded = false;
        if (!Directory.Exists(GetEditorDirectory())) {
            Directory.CreateDirectory(GetEditorDirectory());
        }

        foreach (var f in Directory.GetFiles(GetEditorDirectory(), "*.txt")) {
            var name = Path.GetFileNameWithoutExtension(f);
            if (!instances.ContainsKey(name)) {
                instances[name] = LoadItem(name);
                loaded = true;
            }
        }
        if (loaded) {
            //Debug.Log("Loaded PhraseSets from [" + GetEditorDirectory() + "]");
        }
    }

    public static IEnumerable<PhraseSetGameData> GetPhraseSets() {
        return instances.Values;
    }

    static void SaveItem(string key) {
        if (instances.ContainsKey(key) && instances[key].Phrases.Count > 0) {
            if (Application.isEditor) {
                Serializer.SaveToXml<PhraseSetGameData>(GetEditorDataPath(key), instances[key], false);
				//Debug.Log("saved " + key);
            } else {
                Debug.LogWarning("Is player. (not implemented)");
            }
        }
    }

    static PhraseSetGameData LoadItem(string file) {
        PhraseSetGameData data = null;
        if (Application.isEditor) {
            data = Serializer.LoadFromXml<PhraseSetGameData>(GetEditorDataPath(file), false);
        } else {
            var text = Resources.Load<TextAsset>(GetResourcePath(file));
            if (text != null) {
                data = Serializer.LoadFromXmlString<PhraseSetGameData>(text.text);
            } 
        }
        return data;
    }

    public static bool HasItem(string key) {
        if (!instances.ContainsKey(key)) {
            instances[key] = LoadItem(key);
            if (instances[key] == null) {
                return false;
            }
        }
        return true;
    }

    public static PhraseSetGameData GetOrCreateItem(string key) {
        if (!instances.ContainsKey(key)) {
            instances[key] = LoadItem(key);
            if (instances[key] == null) {
                instances[key] = new PhraseSetGameData(key);
            }
        } else if (instances[key] == null) {
            instances[key] = new PhraseSetGameData(key);
        }
        return instances[key];
    }

}