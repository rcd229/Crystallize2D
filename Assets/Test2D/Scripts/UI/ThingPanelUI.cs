using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ThingPanelUI : MonoBehaviour {
    public GameObject thingToolPrefab;
    public RectTransform tileTool;
    public RectTransform removeTool;
    public RectTransform addThingButton;
    public RectTransform itemParent;

    GameObject selected;

    Dictionary<GameObject, ThingTemplate2D> thingMap = new Dictionary<GameObject, ThingTemplate2D>();
    List<GameObject> itemInstances = new List<GameObject>();

    public void Initialize(object args) {
        tileTool.gameObject.GetOrAddComponent<UIButton>().OnClicked += ThingPanelUI_OnClicked;
        removeTool.gameObject.GetOrAddComponent<UIButton>().OnClicked += ThingPanelUI_OnClicked;
        addThingButton.gameObject.GetOrAddComponent<UIButton>().OnClicked += AddClicked;
        Refresh();
    }

    void Refresh() {
        thingMap.Clear();

        var templates = ThingInventoryLoader.Instance.Get().Things;
        UIUtil.GenerateChildren(templates, itemInstances, itemParent, CreateChild);
        addThingButton.SetAsLastSibling();
    }

    GameObject CreateChild(ThingTemplate2D thing) {
        var inst = Instantiate<GameObject>(thingToolPrefab);
        inst.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(thing.Name);
        inst.GetOrAddComponent<UIButton>().OnClicked += ThingPanelUI_OnClicked;
        thingMap[inst] = thing;
        return inst;
    }

    void ThingPanelUI_OnClicked(object sender, EventArgs<PointerEventData> e) {
        var c = sender as Component;
        if (e.Data.button == PointerEventData.InputButton.Left) {
            selected = c.gameObject;
        } else if(e.Data.button == PointerEventData.InputButton.Right) {
            if (thingMap.ContainsKey(c.gameObject)) {
                var thing = thingMap[c.gameObject];
                GameObjectUtil.GetResourceInstanceFromAttribute<ThingTemplateEditorUI>().Initialize(thing, null, null);
            }
        }
    }

    void AddClicked(object sender, EventArgs e) {
        var temp = new ThingTemplate2D();
        temp.Name = new PhraseSequence("New thing");
        ThingInventoryLoader.Instance.Get().Things.Add(temp);
        Refresh();
    }

    void Start() {
        Initialize(null);
    }

    void Update() {
        if (selected == tileTool.gameObject) {
            tileTool.GetComponent<Image>().enabled = true;
            //UpdateLeftClick(onMouse: TilePlacerUI2D.PlaceTile);
        } else {
            tileTool.GetComponent<Image>().enabled = false;
        }

        if (selected == removeTool.gameObject) {
            removeTool.GetComponent<Image>().enabled = true;
            //UpdateLeftClick(onMouse: TilePlacerUI2D.RemoveTile);
            UpdateLeftClick(onMouse: TilePlacerUI2D.RemoveThing);
        } else {
            removeTool.GetComponent<Image>().enabled = false;
        }

        var selIndex = itemInstances.IndexOf(selected);
        for (int i = 0; i < itemInstances.Count; i++) {
            if (selIndex == i) {
                itemInstances[i].GetComponent<Image>().enabled = true;
                UpdateLeftClick(onMouseDown: () => TilePlacerUI2D.PlaceThing(thingMap[itemInstances[i]]));
            } else {
                itemInstances[i].GetComponent<Image>().enabled = false;
            }
            var nameValue = PlayerDataConnector.GetText(thingMap[itemInstances[i]].Name);
            var nameText = itemInstances[i].GetComponentInChildren<Text>();
            if (nameText.text != nameValue) {
                nameText.text = nameValue;
                ThingInventoryLoader.Instance.Save();
            }
        }

        for (int i = 0; i < 9; i++) {
            var keyCode = (KeyCode)((int)KeyCode.Alpha1 + i);
            if (Input.GetKeyDown(keyCode)) {
                if (i == 0) {
                    selected = tileTool.gameObject;
                } else if (i == 1) {
                    selected = removeTool.gameObject;
                } else if(i - 2 < itemInstances.Count) {
                    selected = itemInstances[i - 2];
                }
            }
        }
    }

    void UpdateLeftClick(Action onMouseDown = null, Action onMouse = null, Action onMouseUp = null) {
        if (Input.GetMouseButtonDown(0)) onMouseDown.Raise();
        if (Input.GetMouseButton(0)) onMouse.Raise();
        if (Input.GetMouseButtonUp(0)) onMouseUp.Raise();
    }

}
