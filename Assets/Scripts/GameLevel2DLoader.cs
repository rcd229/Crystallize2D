using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class GameLevel2DLoader : Loader2D
{

    public IEnumerable<GameLevel2D> GetAllLevels()
    {
        var path = DirectoryPath;
        return from p in Directory.GetDirectories(path) select new GameLevel2D(Path.GetDirectoryName(p));
    }
}
