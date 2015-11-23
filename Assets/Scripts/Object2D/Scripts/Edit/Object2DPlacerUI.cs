using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Object2DPlacerUI : MonoBehaviour
{
    List<GameObject> objectList = new List<GameObject>();
    public GameObject tileprefab;

    // Use this for initialization
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        //Refresh();
    }

    void Refresh()
    {
        objectList.DestroyAndClear();
        var index = 0;
        foreach (GameObject gobj in Object2DSceneResourceManager.GetGraphicPrefabs())
        {
            var block = Instantiate<GameObject>(tileprefab);
            block.GetComponent<Image>().sprite = gobj.GetComponent<SpriteRenderer>().sprite;
            block.transform.SetParent(transform, false);
            block.AddComponent<DataContainer>().Store<int>(index);
            Debug.Log(index);
            block.AddComponent<UIButton>().OnClicked += Object2DPlacerUI_OnClicked;
            objectList.Add(block);
            index++;
        }
    }

    private void Object2DPlacerUI_OnClicked(object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
    {
        var ind = ((UIButton)sender).GetComponent<DataContainer>().Retrieve<int>();
        Debug.Log(ind);
        Object2DPlacer.placer.SetIndex(ind);
    }

    public void ScrollLeft()
    {
        transform.position += Screen.width * Vector3.right * 0.5f;
    }

    public void ScrollRight()
    {
        transform.position += Screen.width * Vector3.left * 0.5f;
    }
}
