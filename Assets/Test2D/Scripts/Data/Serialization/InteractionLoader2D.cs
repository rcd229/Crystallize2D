using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Util.Serialization;

public enum InteractionHostType {
    Thing,
    Trigger
}

public class InteractionLoader2D : Loader2D {
    public static string ThingDirectoryPath { get { return ThingLoader2D.Instance.LocalDirectoryPath; } }
    public static string TriggerDirectoryPath { get { return TriggerLoader2D.Instance.LocalDirectoryPath; } }

    static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

    static string GetDirectory(InteractionHostType type) {
        if (type == InteractionHostType.Thing) return ThingDirectoryPath;
        else if (type == InteractionHostType.Trigger) return TriggerDirectoryPath;
        return "";
    }

    static string GetDirectoryPath(string dirPath, Guid guid){
        var dir = dirPath + guid.ToString();
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        return dir;
    }

    static string GetFilePath(InteractionHostType type, Guid guid, DialogueSegment2D interaction) {
        return GetDirectoryPath(GetDirectory(type), guid) + "/" + interaction.Guid.ToString() + ".json";
    }

    public static void Save(InteractionHostType type, Guid guid, DialogueSegment2D interaction) {
        var s = JsonConvert.SerializeObject(interaction, Formatting.Indented, settings);
        Serializer.SaveToFile(GetFilePath(type, guid, interaction), s);
    }

    public static DialogueSegment2D Load(string filePath) {
        return JsonConvert.DeserializeObject<DialogueSegment2D>(Serializer.LoadFromFile(filePath), settings);  
    }

    public static List<DialogueSegment2D> Load(InteractionHostType type, IHasGuid obj) {
        var interactions = new List<DialogueSegment2D>();
        foreach (var f in Directory.GetFiles(GetDirectoryPath(GetDirectory(type), obj.Guid))) {
            interactions.Add(Load(f));
        }
        return interactions;
    }

    public static List<DialogueSegment2D> Load(ThingInstance2D thing) {
        return Load(InteractionHostType.Thing, thing);
    }

    public static List<DialogueSegment2D> Load(TriggerData2D trigger) {
        return Load(InteractionHostType.Trigger, trigger);
    }
}
