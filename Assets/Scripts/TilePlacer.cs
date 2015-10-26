using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class TilePlacer : MonoBehaviour {

    public TileSpriteManager tileManager;
    private GameObject tile;
    private List<GameObject> pathList = new List<GameObject>();
    private List<GameObject> buildingList = new List<GameObject>();
    private List<GameObject> doorList = new List<GameObject>();
    private List<GameObject> envirList = new List<GameObject>();
    private int type;
    private SpriteLayer layer;
    private GameLevel2D currentLevel;


    // Use this for initialization
    void Start () {
        LoadLevel(GameLevel2D.DefaultLevel);
    }

    void LoadLevel(GameLevel2D level)
    {
        pathList.DestroyAndClear();
        buildingList.DestroyAndClear();
        doorList.DestroyAndClear();
        envirList.DestroyAndClear();
        currentLevel = level;
        type = 0;
        layer = (SpriteLayer)0;
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

    void SwitchLevel()
    {
        if (currentLevel == GameLevel2D.DefaultLevel)
        {
            LoadLevel(GameLevel2D.TestLevel);
        }
        else
        {
            LoadLevel(GameLevel2D.DefaultLevel);
        } 
    }

    // Update is called once per frame
    void Update () {
        tileCreateDestroy();
	}

    //place tile onscreen
    void tileCreateDestroy()
    {
        var mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        mousePos.x = Mathf.RoundToInt(mousePos.x);
        mousePos.y = Mathf.RoundToInt(mousePos.y);

        var tListLength = tileManager.GetListLength((int)layer);

        if (Input.GetMouseButtonDown(0))
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

        if (Input.GetKeyDown(KeyCode.Tab)) {
            type = (type + 1) % tListLength;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            type = (type - 1) % tListLength;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            type = 0;
            layer = (SpriteLayer)(((int)layer + 1) % 4);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject t;
            if (layer == SpriteLayer.Path)
            {
                t = pathList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
                if (t != null)
                {
                    Destroy(t);
                    pathList.Remove(t);
                    currentLevel.layers[SpriteLayer.Path].SetValue(Vector2int.FromVector2(mousePos), 0);
                }
            }
            else if (layer == SpriteLayer.Building)
            {
                t = buildingList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
                if (t != null)
                {
                    Destroy(t);
                    buildingList.Remove(t);
                    currentLevel.layers[SpriteLayer.Building].SetValue(Vector2int.FromVector2(mousePos), 0);
                }
            }
            else if (layer == SpriteLayer.Door)
            {
                t = doorList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
                if (t != null)
                {
                    Destroy(t);
                    doorList.Remove(t);
                    currentLevel.layers[SpriteLayer.Door].SetValue(Vector2int.FromVector2(mousePos), 0);
                }
            }
            else
            {
                t = envirList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
                if (t != null)
                {
                    Destroy(t);
                    envirList.Remove(t);
                    currentLevel.layers[SpriteLayer.Environment].SetValue(Vector2int.FromVector2(mousePos), 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchLevel();
        }
    }
}
