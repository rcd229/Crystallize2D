using System;
using UnityEngine;
using System.Collections.Generic;
using Util;
using System.Linq;

public class Tile2DSceneResourceManager : MonoBehaviour {
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
        foreach (var l in currentLevel.layers) {
            foreach (var t in l.Value.GetTiles()) {
                if (t.type != 0) {
                    var tileInstance = TileSpriteManager.Instance.GetInstance(t.type - 1, l.Key, t.position.ToVector2());
                    resources.Add(tileInstance);
                }
            }
        }
    }

    public void createTileAtMousePos(Vector3 mousePos, SpriteLayer layer, int type) {
        var tileInstance = TileSpriteManager.Instance.GetInstance(type, layer, mousePos);
        currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), (byte)(type + 1));
        resources.Add(tileInstance);
    }

    public void removeTileAtMousePos(Vector3 mousePos, SpriteLayer layer) {
        currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), 0);
        LoadLevel(currentLevel);
    }

    public void setCurrentLevel(GameLevel2D level)
    {
        currentLevel = level;
    }

}
