using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.IO;

public class Loader2D {
    public static string DirectoryPath
    {
        get
        {
            return Directory.GetParent(Application.dataPath) + "/CrystallizeData2D/";
        }
    }

    public static string TileDirectoryPath(SpriteLayer layer) {
        string levelname = Application.loadedLevelName;
        string path = Directory.GetParent(Application.dataPath) + "/CrystallizeData2D/" + levelname + "/" + layer + "/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

    static Loader2D() {
    }
}
