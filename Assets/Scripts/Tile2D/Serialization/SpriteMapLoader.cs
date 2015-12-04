using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Util.Serialization;

public class SpriteMapLoader : Loader2D {
    static string SpriteMapFilePath {
        get {
            return DirectoryPath + "/SpriteMaps/DefaultSpriteMap.json";
        }
    }

    public static void Save(SpriteMap map) {
        Serializer.SaveToFile(SpriteMapFilePath, JsonConvert.SerializeObject(map.GetData()));
    }

    public static SpriteMap Load() {
        if (File.Exists(SpriteMapFilePath)) {
            return new SpriteMap(JsonConvert.DeserializeObject<SpriteMapData>(Serializer.LoadFromFile(SpriteMapFilePath)));
        } else {
            return new SpriteMap();
        }
    } 
}
