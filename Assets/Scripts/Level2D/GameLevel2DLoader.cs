using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class GameLevel2DLoader : Loader2D
{

    public static IEnumerable<GameLevel2D> GetAllLevels()
    {
        var path = DirectoryPath;
        foreach(var p in Directory.GetDirectories(path)) {
            Debug.Log("Path: " + p + "; " + new DirectoryInfo(p).Name);
        }
        return from p in Directory.GetDirectories(path) select new GameLevel2D(new DirectoryInfo(p).Name);
    }
}
