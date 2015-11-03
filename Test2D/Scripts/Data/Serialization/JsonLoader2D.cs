using UnityEngine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Util.Serialization;

public abstract class JsonLoader2D<T> : Loader2D where T : IHasGuid {

    public abstract string LocalDirectoryPath { get; }

    public JsonLoader2D() {
        if (!Directory.Exists(LocalDirectoryPath)) {
            Directory.CreateDirectory(LocalDirectoryPath);
        }
    }

    protected string GetFilePath(T thing) {
        return GetFilePath(thing.Guid);
    }

    protected string GetFilePath(Guid guid) {
        return LocalDirectoryPath + guid.ToString() + ".json";
    }

    public void Delete(T thing) {
        if (File.Exists(GetFilePath(thing))) {
            File.Delete(GetFilePath(thing));
        }
    }

    public void Save(T thing) {
        var s = JsonConvert.SerializeObject(thing, Formatting.Indented);
        Serializer.SaveToFile(GetFilePath(thing), s);
    }

    public T Load(string filePath) {
        return JsonConvert.DeserializeObject<T>(Serializer.LoadFromFile(filePath));
    }

    public T Load(Guid guid) {
        return Load(GetFilePath(guid));
    }

    public List<T> LoadAll() {
        var things = new List<T>();
        foreach (var f in Directory.GetFiles(LocalDirectoryPath)) {
            things.Add(Load(f));
        }
        return things;
    }
}
