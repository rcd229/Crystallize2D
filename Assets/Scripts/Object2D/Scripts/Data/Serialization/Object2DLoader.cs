using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Util.Serialization;

public class Object2DLoader {
    public static string DirectoryPath { get { return Directory.GetParent(Application.dataPath) + "/CrystallizeData2D/"; } }
    public static string LocalDirectoryPath { get { return "Object/"; } }

    static JsonSerializerSettings settings = new JsonSerializerSettings();
    
    static Object2DLoader() {
        settings.TypeNameHandling = TypeNameHandling.All;
    }

    protected static string GetFilePath(string stage, Object2D obj) {
        return GetFilePath(stage, obj.Guid);
    }

    protected static string GetFilePath(string stage, Guid guid) {
        return GetDirectoryPath(stage) + guid.ToString() + ".json";
    }

    protected static string GetDirectoryPath(string stage) {
        return DirectoryPath + stage + "/" + LocalDirectoryPath;
    }

    public static void Save(string stage, Object2D obj) {
        var s = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        var fp = GetDirectoryPath(stage);
        if (!Directory.Exists(fp)) {
            Directory.CreateDirectory(fp);
        }
        Serializer.SaveToFile(GetFilePath(stage, obj), s);
    }

    public static Object2D Load(string filePath) {
        return JsonConvert.DeserializeObject<Object2D>(Serializer.LoadFromFile(filePath), settings);
    }

    public static List<Object2D> LoadAll(string stage) {
        var objects = new List<Object2D>();
        foreach (var f in Directory.GetFiles(GetDirectoryPath(stage))) {
            objects.Add(Load(f));
        }
        return objects;
    }

    public static void Delete(string stage, Object2D obj) {
        if (File.Exists(GetFilePath(stage, obj))) {
            File.Delete(GetFilePath(stage, obj));
        }
    }
}
