using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteLayer
{
    Path = 0,
    Environment = 1,
    Player = 2
}

public class TileSpriteManager : MonoBehaviour
{
    public GameObject prefab;
    private Sprite[] pathSprites;
    //private Sprite[] buildingSprites;
    private Sprite[] envirSprites;
    

    void Awake()
    {
        pathSprites = (Sprite[])Resources.LoadAll<Sprite>("Path");
        envirSprites = (Sprite[])Resources.LoadAll<Sprite>("Environment");
        //buildingSprites = (Sprite[])Resources.LoadAll<Sprite>("Building");
    }

    public Texture2D getTexture(int type, SpriteLayer layer)
    {
        if (layer == SpriteLayer.Path)
        {
            return pathSprites[type].texture;
        }
        else
        {
            return envirSprites[type].texture;
        }
    }

    public Sprite GetSprite(int id, int layer) {
        Sprite sprite;
        Sprite[] sprites;
        if (layer == 0)
        {
            sprites = pathSprites;
        }
        else
        {
            sprites = envirSprites;
        }
        if (id >=0 && id < sprites.Length) {
            sprite = sprites[id];
        }
        else
        {
            Debug.Log("index out of range");
            id = 0;
            sprite = sprites[id];
        }
        return sprite;
    }

    public GameObject GetInstance(int type, SpriteLayer layer, Vector2 position)
    {
        var spriteInstance = Instantiate<GameObject>(prefab);
        spriteInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(type, (int)layer);
        spriteInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
        spriteInstance.transform.position = position;

        //add box collider if tile is in Environment layer so that player cannot walk over this tile
        if ((int)layer == 1 || (int)layer == 2)
        {
            spriteInstance.AddComponent<BoxCollider2D>();
        }

        return spriteInstance;

    }

    public int GetListLength(int layer)
    {
        if (layer == 0)
        {
            return pathSprites.Length;
        }
        else
        {
            return envirSprites.Length;
        }
    }
}
