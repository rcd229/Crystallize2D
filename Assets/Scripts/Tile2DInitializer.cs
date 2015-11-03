using System;
using UnityEngine;

public class Tile2DInitializer : MonoBehaviour
{
    private List<GameObject> pathList = new List<GameObject>();
    private List<GameObject> buildingList = new List<GameObject>();
    private List<GameObject> doorList = new List<GameObject>();
    private List<GameObject> envirList = new List<GameObject>();
    public TileSpriteManager tileManager;
    public GameLevel2D currentLevel;

    static Tile2DInitializer _instance;
    public static Tile2DInitializer Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("Tile2DInitializer").AddComponent<Tile2DInitializer>();
            }
            return _instance;
        }
    }

    void Start()
    {
        LoadLevel(GameLevel2D.DefaultLevel);
    }

    void LoadLevel(GameLevel2D level)
    {
        pathList.DestroyAndClear();
        buildingList.DestroyAndClear();
        doorList.DestroyAndClear();
        envirList.DestroyAndClear();
        currentLevel = level;
        foreach (var l in currentLevel.layers)
        {
            foreach (var t in l.Value.GetTiles())
            {
                if (t.type != 0)
                {

                    var tileInstance = tileManager.GetInstance(t.type - 1, l.Key, t.position.ToVector2());
                    if (l.Value.layer == SpriteLayer.Path) { pathList.Add(tileInstance); }
                    else if (l.Value.layer == SpriteLayer.Building) { buildingList.Add(tileInstance); }
                    else if (l.Value.layer == SpriteLayer.Door) { doorList.Add(tileInstance); }
                    else { envirList.Add(tileInstance); }
                }
            }
        }
    }

    public void createTileAtMousePos(Vector3 mousepos, SpriteLayer layer, int type)
    {
        var tileInstance = tileManager.GetInstance(type, layer, mousePos);
        currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), (byte)(type + 1));

        if (layer == SpriteLayer.Path)
        {

            pathList.Add(tileInstance);
        }
        else if (layer == SpriteLayer.Building)
        {
            buildingList.Add(tileInstance);
        }
        else if (layer == SpriteLayer.Door)
        {
            doorList.Add(tileInstance);
        }
        else
        {
            envirList.Add(tileInstance);
        }

    }

}
