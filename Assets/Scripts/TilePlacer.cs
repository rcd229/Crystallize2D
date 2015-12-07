using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class TilePlacer : MonoBehaviour {

    private GameObject tile;
    //private int type;
    private string type;
    private SpriteLayer layer;
    public static TilePlacer placer;

    private int _index;
    private int Index {
        get { return _index; }
        set {
            _index = value;
            type = TileSpriteManager.Instance.GetNameForIndex(_index, layer);
        }
    }

    void Awake() {
        placer = this;
    }

    // Use this for initialization
    void Start() {
        layer = (SpriteLayer)0;
        Index = 0;

        CollisionMap2D.Instance.SetVisualsEnabled(true);
    }

    // Update is called once per frame
    void Update() {
        tileCreateDestroy();
    }

    //place tile onscreen
    void tileCreateDestroy() {

        var mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        mousePos.x = Mathf.RoundToInt(mousePos.x);
        mousePos.y = Mathf.RoundToInt(mousePos.y);

        var tListLength = TileSpriteManager.Instance.GetListLength(layer);

        if (Input.GetMouseButtonDown(0) && !UIUtil.MouseOverUI()) {
            Tile2DSceneResourceManager.Instance.createTileAtMousePos(mousePos, layer, type);
        }

        if (Input.GetMouseButtonDown(1) && !UIUtil.MouseOverUI()) {
            Tile2DSceneResourceManager.Instance.removeTileAtMousePos(mousePos, layer);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            Index = (Index + 1) % tListLength;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Index = (Index - 1) % tListLength;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            layer = (SpriteLayer)(((int)layer + 1) % 4);
            Index = 0;
        }
    }

    public SpriteLayer getLayer() {
        return layer;
    }

    public void SetType(string t) {
        type = t;
    }
}
