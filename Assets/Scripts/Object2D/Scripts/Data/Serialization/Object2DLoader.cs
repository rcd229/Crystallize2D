using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Util.Serialization;
using System.Linq;

public class Object2DLoader : Loader2D {
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
        return LevelsDirectoryPath + stage + "/" + LocalDirectoryPath;
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

        var dir = GetDirectoryPath(stage);
        if (Directory.Exists(dir)) {
            foreach (var f in Directory.GetFiles(dir)) {
                objects.Add(Load(f));
            }
        }
        return objects;
    }

    public static List<Object2D>LoadAll()
    {
        var levels = GameLevel2DLoader.GetAllLevels().ToArray();
        var objects = new List<Object2D>();
        foreach (GameLevel2D level in levels)
        {
            objects.AddRange(LoadAll(level.levelname));
        }
        return objects;
    }

    public static void Delete(string stage, Object2D obj) {
        if (File.Exists(GetFilePath(stage, obj))) {
            File.Delete(GetFilePath(stage, obj));
        }
    }
}
