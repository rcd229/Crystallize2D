using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Loader2D {
    public static string DirectoryPath {
        get {
            return Directory.GetParent(Application.dataPath) + "/CrystallizeData2D/";
        }
    }

    public static string LevelsDirectoryPath {
        get {
            return DirectoryPath + "Levels/";
        }
    }

    public static string TileDirectoryPath(SpriteLayer layer, string levelname) {
        string path = LevelsDirectoryPath + levelname + "/" + layer + "/";

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        return path;
    }
}
