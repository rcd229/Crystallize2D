using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TilePlacerUI : MonoBehaviour {

    public GameObject tileprefab;
    SpriteLayer lastlayer;
    List<GameObject> tilelist = new List<GameObject>();

	// Use this for initialization
	void Start () {
        Refresh();
	}
	
	// Update is called once per frame
	void Update () {
        if (lastlayer != TilePlacer.placer.getLayer())
        {
            Refresh();
        }
	}

    void Refresh()
    {
        tilelist.DestroyAndClear();
        lastlayer = TilePlacer.placer.getLayer();
        var index = 0;
        foreach (Sprite sprite in TileSpriteManager.Instance.GetAllSprites((int)lastlayer))
        {
            var tile = Instantiate<GameObject>(tileprefab);
            tile.GetComponent<Image>().sprite = sprite;
            //Debug.Log(sprite.pivot);
            var npivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y/sprite.rect.height);
            //Debug.Log(npivot);
            if (npivot != new Vector2(0.5f,0.5f))
            {
                tile.GetComponent<Image>().color = Color.grey;
            }
            tile.transform.SetParent(transform, false);
            tile.AddComponent<DataContainer>().Store<int>(index);
            tile.AddComponent<UIButton>().OnClicked += TilePlacerUI_OnClicked;
            tilelist.Add(tile);
            index++;
        }
    }

    private void TilePlacerUI_OnClicked(object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
    {
        var ind = ((UIButton)sender).GetComponent<DataContainer>().Retrieve<int>();
        TilePlacer.placer.setType(ind);
    }

    public void ScrollLeft()
    {
        transform.position += Screen.width*Vector3.right*0.5f;
    }

    public void ScrollRight()
    {
        transform.position += Screen.width * Vector3.left*0.5f;
    }
}
