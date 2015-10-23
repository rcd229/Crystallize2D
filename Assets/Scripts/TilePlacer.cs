using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class TilePlacer : MonoBehaviour {

    public TileSpriteManager tileManager;
    private GameObject tile;
    private int tileIndex;
    private List<GameObject> pathList;
    private List<GameObject> envirList;
    private int type;
    private SpriteLayer layer;
    private Rect tileRect;


    // Use this for initialization
    void Start () {

        tileIndex = 0;
        pathList = new List<GameObject>();
        envirList = new List<GameObject>();
        type = 0;
        layer = (SpriteLayer)0;
        tileRect = new Rect(0, 0, 100, 100);
        IEnumerable<TileObject> pathTiles = TileMap2D.pathInstance.GetTiles();
        IEnumerable<TileObject> envirTiles = TileMap2D.envirInstance.GetTiles();
        foreach (TileObject t in pathTiles)
        {
            if (t.type != 0)
            {
                var tileInstance = tileManager.GetInstance(t.type - 1, TileMap2D.pathInstance.layer, t.position.ToVector2());
                pathList.Add(tileInstance);
            }
            
        }
        foreach (TileObject t in envirTiles)
        {
            if (t.type != 0)
            {
                var tileInstance = tileManager.GetInstance(t.type - 1, TileMap2D.envirInstance.layer, t.position.ToVector2());
                envirList.Add(tileInstance);
            }

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

            TileMap2D.pathInstance.SetValue(Vector2int.FromVector2(mousePos), (byte)(type+1));

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
            var p = pathList.Where(obj => obj.transform.position == mousePos).SingleOrDefault();
            var e = pathList.Where(obj => obj.transform.position == mousePos).SingleOrDefault();
            if (p != null) {
                Destroy(p);
                pathList.Remove(p);
            }
            if (e != null) {
                Destroy(e);
                envirList.Remove(e); 
            }
        }
    }
}
