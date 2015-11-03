using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteLayer
{
    Path = 0,
    Building = 1,
    Door = 2,
    Environment = 3,
    Player = 4
}

public class TileSpriteManager : MonoBehaviour
{
    public GameObject prefab;
    private Sprite[] pathSprites;
    private Sprite[] buildingSprites;
    private Sprite[] doorSprites;
    private Sprite[] envirSprites;

    void Awake()
    {
        pathSprites = (Sprite[])Resources.LoadAll<Sprite>("Path");
        buildingSprites = (Sprite[])Resources.LoadAll<Sprite>("Building");
        doorSprites = (Sprite[])Resources.LoadAll<Sprite>("Door");
        envirSprites = (Sprite[])Resources.LoadAll<Sprite>("Environment");
        
    }

    public Texture2D getTexture(int type, SpriteLayer layer)
    {
        if (layer == SpriteLayer.Path)
        {
            return pathSprites[type].texture;
        }
        else if (layer == SpriteLayer.Building)
        {
            return buildingSprites[type].texture;
        }
        else if (layer == SpriteLayer.Door)
        {
            return doorSprites[type].texture;
        }
        else
        {
            return envirSprites[type].texture;
        }
    }

    public Sprite GetSprite(int id, int layer) {
        Sprite sprite;
        Sprite[] sprites = getAllSprites(layer);
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

    public Sprite[] getAllSprites(int layer)
    {
        Sprite[] sprites;
        if (layer == 0)
        {
            sprites = pathSprites;
        }
        else if (layer == 1)
        {
            sprites = buildingSprites;
        }
        else if (layer == 2)
        {
            sprites = doorSprites;
        }
        else
        {
            sprites = envirSprites;
        }
        return sprites;

    }

    public GameObject GetInstance(int type, SpriteLayer layer, Vector2 position)
    {
        var spriteInstance = Instantiate<GameObject>(prefab);
        spriteInstance.GetComponent<SpriteRenderer>().sprite = GetSprite(type, (int)layer);
        spriteInstance.GetComponent<SpriteRenderer>().sortingLayerName = layer.ToString();
        spriteInstance.transform.position = position;

        //add box collider if tile is in Environment layer so that player cannot walk over this tile
        if (layer == SpriteLayer.Building || layer == SpriteLayer.Door || layer == SpriteLayer.Environment)
        {
            spriteInstance.AddComponent<BoxCollider2D>();
            if (layer == SpriteLayer.Door)
            {
                spriteInstance.GetComponent<BoxCollider2D>().isTrigger = true;
                spriteInstance.AddComponent<Door2DTrigger>();
            }
        }

        return spriteInstance;

    }

    public int GetListLength(int layer)
    {
        if (layer == 0)
        {
            return pathSprites.Length;
        }
        else if (layer == 1)
        {
            return buildingSprites.Length;
        }
        else if (layer == 2)
        {
            return doorSprites.Length;
        }
        else
        {
            return envirSprites.Length;
        }
    }
}
