using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteLayer {
    Path = 0,
    Building = 1,
    Door = 2,
    Environment = 3,
    Player = 4
}

public class TileSpriteManager : MonoBehaviour {
    static TileSpriteManager _instance;
    public static TileSpriteManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("TileSpriteManager").AddComponent<TileSpriteManager>();
                _instance.prefab = Resources.Load<GameObject>("BaseTilePrefab");
            }
            return _instance;
        }
    }

    GameObject prefab;
    Sprite[] pathSprites;
    Sprite[] buildingSprites;
    Sprite[] doorSprites;
    Sprite[] envirSprites;

    void Awake() {
        pathSprites = Resources.LoadAll<Sprite>("Path");
        buildingSprites = Resources.LoadAll<Sprite>("Building");
        doorSprites = Resources.LoadAll<Sprite>("Door");
        envirSprites = Resources.LoadAll<Sprite>("Environment");
    }

    public Texture2D GetTexture(int type, SpriteLayer layer) {
        if (layer == SpriteLayer.Path) {
            return pathSprites[type].texture;
        }
        else if (layer == SpriteLayer.Building) {
            return buildingSprites[type].texture;
        }
        else if (layer == SpriteLayer.Door) {
            return doorSprites[type].texture;
        }
        else {
            return envirSprites[type].texture;
        }
    }

    public Sprite GetSprite(int id, int layer) {
        Sprite sprite;
        Sprite[] sprites = GetAllSprites(layer);
        if (id >= 0 && id < sprites.Length) {
            sprite = sprites[id];
        }
        else {
            Debug.Log("index out of range");
            id = 0;
            sprite = sprites[id];
        }
        return sprite;
    }

    public Sprite[] GetAllSprites(int layer) {
        Sprite[] sprites;
        if (layer == 0) {
            sprites = pathSprites;
        }
        else if (layer == 1) {
            sprites = buildingSprites;
        }
        else if (layer == 2) {
            sprites = doorSprites;
        }
        else {
            sprites = envirSprites;
        }
        return sprites;

    }

    public GameObject GetInstance(int type, SpriteLayer layer, Vector2 position) {
        var tileInstance = Instantiate(prefab);
        TransformPath.Add(tileInstance.transform, "Tiles");
        tileInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(type, (int)layer);
        tileInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
        tileInstance.transform.position = position;

        //add box collider if tile is in Environment layer so that player cannot walk over this tile
        //if (layer == SpriteLayer.Building || layer == SpriteLayer.Door || layer == SpriteLayer.Environment) {
        //    tileInstance.AddComponent<BoxCollider2D>();
        //}

        return tileInstance;
    }

    public int GetListLength(int layer) {
        if (layer == 0) {
            return pathSprites.Length;
        }
        else if (layer == 1) {
            return buildingSprites.Length;
        }
        else if (layer == 2) {
            return doorSprites.Length;
        }
        else {
            return envirSprites.Length;
        }
    }
}
