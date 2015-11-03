using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class TileObject
{
    public Vector2int position;
    public byte type;

    public TileObject(Vector2int pos, byte t)
    {
        position = pos;
        type = t;
    }
}

public class TileMap2D {

    public const int MapSize = 16;

    public static Vector2int GetReducedPoint(Vector2int point) {
        return new Vector2int(Mathf.FloorToInt((float)point.x / MapSize), Mathf.FloorToInt((float)point.y / MapSize));
    }

    Dictionary<Vector2int, byte[]> maps = new Dictionary<Vector2int, byte[]>();
    public SpriteLayer layer;
    public string levelname;

    public TileMap2D()
    {
    }

    public TileMap2D (SpriteLayer ly, string ln) {
            levelname = ln;
            layer = ly;
            maps = MapLoader2D.LoadAll(layer, levelname);
    }

    public IEnumerable<Vector2int> GetPositions(Vector2int areaPosition) {
        var positions = new List<Vector2int>();
        var m = GetMapFromReducedPoint(areaPosition);
        for (int i = 0; i < m.Length; i++) {
            if (m[i] > 0) {
                var x = i / MapSize;
                var y = i - (x * MapSize);
                positions.Add(MapSize * areaPosition + new Vector2int(x, y));
            }
        }
        return positions;
    }

    public IEnumerable<Vector2int> GetPositions() {
        var positions = new List<Vector2int>();
        foreach (var m in maps) {
            for (int i = 0; i < m.Value.Length; i++) {
                if (m.Value[i] > 0) {
                    var x = i / MapSize;
                    var y = i - (x * MapSize);
                    positions.Add(MapSize * m.Key + new Vector2int(x, y));
                }
            }
        }
        return positions;
    }

    public void SetValue(Vector2int position, byte value) {
        var map = GetMap(position);
        map[GetLocalPosition(position)] = value;
        MapLoader2D.Save(GetReducedPoint(position), map, layer, levelname);
    }

    byte[] GetMap(Vector2int point) {
        return GetMapFromReducedPoint(GetReducedPoint(point));
    }

    byte[] GetMapFromReducedPoint(Vector2int reducedPoint) {
        if (!maps.ContainsKey(reducedPoint)) {
            var m = MapLoader2D.Load(reducedPoint, layer, levelname);
            if (m == null) {
                maps[reducedPoint] = new byte[MapSize * MapSize];
                var newMap = maps[reducedPoint];
                for (int i = 0; i < newMap.Length; i++) { newMap[i] = 0; }
            } else {
                maps[reducedPoint] = m;
            }
        }
        return maps[reducedPoint];
    }

    int GetLocalPosition(Vector2int point) {
        var p = point - MapSize * GetReducedPoint(point);
        return p.x * MapSize + p.y;
    }


    public List<TileObject> GetTiles()
    {
        var tiles = new List<TileObject>();
        foreach (var m in maps)
        {
            for (int i = 0; i < m.Value.Length; i++)
            {
                    var x = i / MapSize;
                    var y = i - (x * MapSize);
                    TileObject t = new TileObject((MapSize * m.Key + new Vector2int(x, y)), m.Value[i]);
                    tiles.Add(t);
            }
        }
        return tiles;
    }


}
