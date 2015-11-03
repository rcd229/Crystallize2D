using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Object2DEditorControls: MonoBehaviour {
    // TODO: add entries here
    public GameObject buttonPrefab;
    public GameObject inputFieldPrefab;
    public GameObject dropdownPrefab;

    public void AddButton(Action onClick, string label = "New Button") {
        var instance = GetInstance(buttonPrefab);
        SetLabel(instance, label);
        instance.GetComponent<Button>().onClick.AddListener(() => onClick());
    }

    public void AddInputField(Func<string> getValue, Action<string> endEdit) {
        var instance = GetInstance(inputFieldPrefab);
        instance.GetComponent<InputField>().text = getValue();
        instance.GetComponent<InputField>().onEndEdit.AddListener((s) => endEdit(s));
    }

    public void AddDropDown(IEnumerable<object> values, int value, Action<int> onValueChanged) {
        var instance = GetInstance(dropdownPrefab);
        var dd = instance.GetComponent<Dropdown>();
        dd.options.Clear();
        dd.options.AddRange(values.Select(v => (new Dropdown.OptionData(v.ToString()))));
        // force the control to update (it doesn't when already 0)
        if (dd.value == 0) { dd.value = 1; }
        dd.value = value;
        dd.onValueChanged.AddListener(i => onValueChanged(i));
    }

    GameObject GetInstance(GameObject prefab) {
        var instance = Instantiate<GameObject>(prefab);
        instance.transform.SetParent(transform);
        return instance;
    }

    void SetLabel(GameObject instance, string label) {
        instance.GetComponentInChildren<Text>().text = label;
    }
}