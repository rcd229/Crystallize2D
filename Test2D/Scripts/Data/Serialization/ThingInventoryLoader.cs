using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;
using Newtonsoft.Json;

public class ThingInventoryLoader {

    const string DefaultFileName = "Inventory2D.json";

    static ThingInventoryLoader _instance;
    public static ThingInventoryLoader Instance {
        get {
            if (_instance == null) { _instance = new ThingInventoryLoader(); }
            return _instance;
        }
    }

    static string DefaultFilePath {
        get {
            return Application.dataPath + "/../" + DefaultFileName;
        }
    }

    ThingInventoryData current;

    public ThingInventoryData Get() {
        if (current == null) { current = Load(); }
        return current;
    }

    public ThingInventoryData Load() {
        if (File.Exists(DefaultFilePath)) {
            return JsonConvert.DeserializeObject<ThingInventoryData>(Serializer.LoadFromFile(DefaultFilePath));
        } else {
            return new ThingInventoryData();
        }
    }

    public void Save() {
        if (current != null) {
            Serializer.SaveToFile(DefaultFilePath, JsonConvert.SerializeObject(current, Formatting.Indented));
        }
    }
}
