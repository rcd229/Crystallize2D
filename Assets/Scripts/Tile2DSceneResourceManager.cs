using System;
using UnityEngine;
using System.Collections.Generic;
using Util;
using System.Linq;

public class Tile2DSceneResourceManager : MonoBehaviour {
    const int CollidersPerTile = 2;

    List<GameObject> resources = new List<GameObject>();
    public GameLevel2D currentLevel = GameLevel2D.DefaultLevel;

    static Tile2DSceneResourceManager _instance;
    public static Tile2DSceneResourceManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("Tile2DInitializer").AddComponent<Tile2DSceneResourceManager>();
            }
            return _instance;
        }
    }

    public void LoadLevel(GameLevel2D level) {
        resources.DestroyAndClear();
        currentLevel = level;
        Debug.Log("current level: " + currentLevel.levelname);
        //var s = "";
        //var c = 0;
        foreach (var l in currentLevel.layers) {
            foreach (var t in l.Value.GetTiles()) {
                if (t.type != 0) {
                    var tileInstance = TileSpriteManager.Instance.GetInstance(t.type - 1, l.Key, t.position.ToVector2());
                    resources.Add(tileInstance);

                    if (l.Key == SpriteLayer.Building) {
                        //s += t.type + ";\t";
                        //c++;
                        var p = CollidersPerTile * t.position;
                        CollisionMap2D.Instance.SetValue(p, 1, false);
                        CollisionMap2D.Instance.SetValue(p + Vector2int.Right, 1, false);
                        CollisionMap2D.Instance.SetValue(p + Vector2int.Up, 1, false);
                        CollisionMap2D.Instance.SetValue(p + Vector2int.One, 1, false);
                    }
                }
            }
        }
        //Debug.Log(c + "\n" + s);
    }

    //public void createTileAtMousePos(Vector3 mousePos, SpriteLayer layer, int type) {
    //    var tileInstance = TileSpriteManager.Instance.GetInstance(type, layer, mousePos);
    //    currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), (byte)(type + 1));
    //    resources.Add(tileInstance);
    //}

    public void createTileAtMousePos(Vector3 mousePos, SpriteLayer layer, string type) {
        var tileInstance = TileSpriteManager.Instance.GetInstance(type, layer, mousePos);
        int id = TileSpriteManager.Instance.GetIDForName(type, layer) + 1;
        currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), (byte)(id + 1));
        resources.Add(tileInstance);
    }

    public void removeTileAtMousePos(Vector3 mousePos, SpriteLayer layer) {
        currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), 0);
        LoadLevel(currentLevel);
    }

}
