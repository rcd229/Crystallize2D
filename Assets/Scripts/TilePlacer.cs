using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class TilePlacer : MonoBehaviour {

    private GameObject tile;
    private int type;
    private SpriteLayer layer;
    public static TilePlacer placer;


    void Awake()
    {
        placer = this;
    }

    // Use this for initialization
    void Start () {
        layer = (SpriteLayer)0;
        type = 0;
    }

    //void SwitchLevel()
    //{
    //    if (Tile2DInitializer.Instance.currentLevel == GameLevel2D.DefaultLevel)
    //    {
    //        Tile2DInitializer.Instance.LoadLevel(GameLevel2D.TestLevel);
    //    }
    //    else
    //    {
    //        Tile2DInitializer.Instance.LoadLevel(GameLevel2D.DefaultLevel);
    //    } 
    //}

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

        var tListLength = TileSpriteManager.Instance.GetListLength((int)layer);

        //get current level
        var dropdown = from thing in GetComponentsInParent<GameObject>() where thing.name == "CurrentLevel" select thing;

        if (Input.GetMouseButtonDown(0) && !UIUtil.MouseOverUI())
        {
            Tile2DSceneResourceManager.Instance.createTileAtMousePos(mousePos, layer, type);
        }

        if (Input.GetMouseButtonDown(1) && !UIUtil.MouseOverUI()) {
            Tile2DSceneResourceManager.Instance.removeTileAtMousePos(mousePos, layer);
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
    }

    public SpriteLayer getLayer()
    {
        return layer;
    }

    public void setType(int t)
    {
        type = t;
    }
}
