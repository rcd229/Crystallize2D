using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpriteMapData {
    public int[] Indices { get; set; }
    public string[] Names { get; set; }

    public SpriteMapData() {
        Indices = new int[0];
        Names = new string[0];
    }

    public SpriteMapData(int[] indices, string[] names) {
        Indices = indices;
        Names = names;
    }
}

public class SpriteMap {
    public Map<int, string> map = new Map<int, string>();

    public SpriteMap() {    }

    public SpriteMap(SpriteMapData data) {
        for(int i = 0; i < data.Indices.Length; i++) {
            map.Add(data.Indices[i], data.Names[i]);
        }
    }

    public int GetIndex(string name) {
        if (map.Contains2(name)) {
            return map.Reverse[name];
        }
        return -1;
    }

    public string GetName(int index) {
        if (map.Contains1(index)) {
            return map.Forward[index];
        }
        return null;
    }

    public SpriteMapData GetData() {
        var keys = map.Keys1.ToArray();
        var vals = new string[keys.Length];
        for(int i = 0; i < keys.Length; i++) {
            vals[i] = map.Forward[keys[i]];
        }
        return new SpriteMapData(keys, vals);
    }
}
