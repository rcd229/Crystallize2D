using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class TilePlacer : MonoBehaviour {

    public TileSpriteManager tileManager;
    private GameObject tile;
    private List<GameObject> pathList = new List<GameObject>();
    private List<GameObject> envirList = new List<GameObject>();
    private int type;
    private SpriteLayer layer;
    private Rect tileRect;
    private GameLevel2D currentLevel;


    // Use this for initialization
    void Start () {
        LoadLevel(GameLevel2D.DefaultLevel);
    }

    void LoadLevel(GameLevel2D level)
    {
        pathList.DestroyAndClear();
        envirList.DestroyAndClear();
        currentLevel = level;
        type = 0;
        layer = (SpriteLayer)0;
        tileRect = new Rect(0, 0, 100, 100);
        foreach (var l in currentLevel.layers)
        {
            foreach (var t in l.Value.GetTiles())
            {
                if (t.type != 0)
                {

                    var tileInstance = tileManager.GetInstance(t.type - 1, l.Key, t.position.ToVector2());
                    pathList.Add(tileInstance);
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

        var x = 11 + (int)mousePos.x;
        var y = -(int)(mousePos.y) + 5;

        var tListLength = tileManager.GetListLength((int)layer);

        if (Input.GetMouseButtonDown(0))
        {
            var tileInstance = tileManager.GetInstance(type, layer, mousePos);
            currentLevel.layers[layer].SetValue(Vector2int.FromVector2(mousePos), (byte)(type + 1));

            if (layer == SpriteLayer.Path)
            {
               
                pathList.Add(tileInstance);
            }
            else
            {
                envirList.Add(tileInstance);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            type = (type + 1) % tListLength;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            type = 0;
            layer = (SpriteLayer)(((int)layer + 1) % 3);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var p = pathList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
            var e = envirList.Where(obj => obj.transform.position == mousePos).FirstOrDefault();
            if (p != null) {
                Destroy(p);
                pathList.Remove(p);
                currentLevel.layers[SpriteLayer.Path].SetValue(Vector2int.FromVector2(mousePos), 0);
            }
            if (e != null) {
                Destroy(e);
                envirList.Remove(e);
                currentLevel.layers[SpriteLayer.Environment].SetValue(Vector2int.FromVector2(mousePos), 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchLevel();
        }
    }
}
