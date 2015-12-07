using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SpriteLayer {
    Path = 0,
    Building = 1,
    Door = 2,
    Environment = 3,
    Player = 4
}

public class TileSpriteManager : MonoBehaviour {
    class SpriteData {
        public Sprite Sprite { get; set; }
        public int Index { get; set; }
    }

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

    SpriteMap map;

    GameObject prefab;
    Dictionary<SpriteLayer, Dictionary<int, SpriteData>> sprites = new Dictionary<SpriteLayer, Dictionary<int, SpriteData>>();
    //Dictionary<int, SpriteData> pathSprites = new Dictionary<int, SpriteData>();
    //Sprite[] pathSprites;
    //Sprite[] buildingSprites;
    //Sprite[] doorSprites;
    //Sprite[] envirSprites;
    Transform tileParent;

    void Awake() {
        map = SpriteMapLoader.Load();

        //pathSprites = LoadSprites("Path"); //Resources.LoadAll<Sprite>("Path");
        //buildingSprites = Resources.LoadAll<Sprite>("Building");
        //doorSprites = Resources.LoadAll<Sprite>("Door");
        //envirSprites = Resources.LoadAll<Sprite>("Environment");
        sprites[SpriteLayer.Path] = LoadSprites("Path");
        sprites[SpriteLayer.Building] = LoadSprites("Building");
        sprites[SpriteLayer.Door] = LoadSprites("Door");
        sprites[SpriteLayer.Environment] = LoadSprites("Environment");
        tileParent = TransformPath.Get("Tiles");
    }

    Dictionary<int, SpriteData> LoadSprites(string path) {
        var dict = new Dictionary<int, SpriteData>();
        var sprites = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < sprites.Length; i++) {
            var key = map.GetOrCreateIndex(sprites[i].name);
            dict.Add(key, new SpriteData() { Sprite = sprites[i], Index = i });
        }
        SpriteMapLoader.Save(map);
        return dict;
    }

    Texture2D GetTexture(int type, SpriteLayer layer) {
        return GetSpriteDataForLayer(layer)[type].Sprite.texture;

        //if (layer == SpriteLayer.Path) {
        //    return pathSprites[type].Sprite.texture;
        //} else if (layer == SpriteLayer.Building) {
        //    return buildingSprites[type].texture;
        //} else if (layer == SpriteLayer.Door) {
        //    return doorSprites[type].texture;
        //} else {
        //    return envirSprites[type].texture;
        //}
    }

    public Sprite GetSprite(int id, SpriteLayer layer) {
        Sprite sprite;
        Sprite[] sprites = GetAllSprites(layer);
        if (id >= 0 && id < sprites.Length) {
            sprite = sprites[id];
        } else {
            Debug.Log("index out of range");
            id = 0;
            sprite = sprites[id];
        }
        return sprite;
    }

    public Sprite[] GetAllSprites(SpriteLayer layer) {
        //if (layer == 0) {
        //    sprites = pathSprites.Values.Select(e => e.Sprite).ToArray();
        //} else if (layer == 1) {
        //    sprites = buildingSprites;
        //} else if (layer == 2) {
        //    sprites = doorSprites;
        //} else {
        //    sprites = envirSprites;
        //}
        return (from s in GetSpriteDataForLayer(layer)
                orderby s.Value.Index
                select s.Value.Sprite).ToArray();
    }

    //public GameObject GetInstance(int type, SpriteLayer layer, Vector2 position) {
    //    var tileInstance = Instantiate(prefab);
    //    tileInstance.transform.parent = tileParent;
    //    tileInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(type, (int)layer);
    //    tileInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
    //    tileInstance.transform.position = position;

    //    //add box collider if tile is in Environment layer so that player cannot walk over this tile
    //    //if (layer == SpriteLayer.Building || layer == SpriteLayer.Door || layer == SpriteLayer.Environment) {
    //    //    tileInstance.AddComponent<BoxCollider2D>();
    //    //}

    //    return tileInstance;
    //}

    public string GetNameForIndex(int index, SpriteLayer layer) {
        return (from s in sprites[layer]
                where s.Value.Index == index
                select s.Value.Sprite.name).FirstOrDefault();
    }

    public int GetIDForName(string name, SpriteLayer layer) {
        return (from s in sprites[layer]
                where s.Value.Sprite.name == name
                select s.Key).FirstOrDefault();
    }

    public GameObject GetInstance(string type, SpriteLayer layer, Vector2 position) {
        var tileInstance = Instantiate(prefab);
        tileInstance.transform.parent = tileParent;
        tileInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(map.GetIndex(type), layer);
        tileInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
        tileInstance.transform.position = position;
        return tileInstance;
    }

    public GameObject GetInstance(int id, SpriteLayer layer, Vector2 position) {
        var tileInstance = Instantiate(prefab);
        tileInstance.transform.parent = tileParent;
        tileInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(id, layer);
        tileInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
        tileInstance.transform.position = position;
        return tileInstance;
    }

    public int GetListLength(SpriteLayer layer) {
        //if (layer == 0) {
        //    return pathSprites.Count;
        //} else if (layer == 1) {
        //    return buildingSprites.Length;
        //} else if (layer == 2) {
        //    return doorSprites.Length;
        //} else {
        //    return envirSprites.Length;
        //}
        return GetSpriteDataForLayer(layer).Count;
    }

    Dictionary<int, SpriteData> GetSpriteDataForLayer(SpriteLayer layer) {
        if (!sprites.ContainsKey(layer)) {
            throw new Exception("Sprite layer is not for tiles");
        } else {
            return sprites[layer];
        }
    }
}
