using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util;

public class MapLoader2D : Loader2D {

    public static void Save(Vector2int mapLocation, byte[] bytes, SpriteLayer layer, string levelname) {
        using (var writer = new StreamWriter(TileDirectoryPath(layer, levelname) + mapLocation.ToString())) {
            writer.WriteLine(bytes.ConvertToHexString());
        }
    }

    public static byte[] Load(Vector2int mapLocation, SpriteLayer layer, string levelname) {
        if (File.Exists(TileDirectoryPath(layer, levelname) + mapLocation)) {
            string s;
            using (var reader = new StreamReader(TileDirectoryPath(layer, levelname) + mapLocation)) {
                s = reader.ReadLine();
            }
            return s.ConvertHexToByteArray();
        }
        else {
            return null;
        }
    }

    public static Dictionary<Vector2int, byte[]> LoadAll(SpriteLayer l, string levelname) {
        //create dictionary for layer l
        var dict = new Dictionary<Vector2int, byte[]>();

        //load each tile in layer l and place in dictionary
        foreach (var file in Directory.GetFiles(TileDirectoryPath(l, levelname))) {
            var name = Path.GetFileName(file);
            var stringSplit = name.Replace("(", "").Replace(")", "").Split(',');
            var vec = new Vector2int(int.Parse(stringSplit[0]), int.Parse(stringSplit[1]));
            dict[vec] = Load(vec, l, levelname);
        }
        return dict;
    }
}
