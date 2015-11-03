using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIComponents2D : MonoBehaviour {
    public GameObject labeledGroup;
    public GameObject phraseLineInput;
    public GameObject closeButton;

    public IUIComponent<T> GetInstance<T>(Transform parent, GameObject prefab, string label = "", T initialValue = default(T)) {
        var inst = GameObject.Instantiate<GameObject>(prefab);
        var ui = inst.GetInterface<IUIComponent<T>>();
        if (!label.IsEmptyOrNull()) {
            var labelGroup = Instantiate<GameObject>(labeledGroup);
            labelGroup.GetComponent<UIComponent>().SetLabel(label);
            inst.transform.SetParent(labelGroup.transform, false);
            labelGroup.transform.SetParent(parent, false);
        } else {
            inst.transform.SetParent(parent, false);
        }
        ui.Value = initialValue;
        return ui;
    }
}
