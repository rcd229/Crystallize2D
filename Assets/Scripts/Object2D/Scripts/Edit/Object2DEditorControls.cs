using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Object2DEditorControls: MonoBehaviour {
    // TODO: add entries here
    public GameObject labelContainer;
    public GameObject buttonPrefab;
    public GameObject inputFieldPrefab;
    public GameObject inputAreaPrefab;
    public GameObject dropdownPrefab;
    public GameObject labelPrefab;

    Transform currentContainer;

    public void AddLabel(string text, bool withContainer = true) {
        var instance = GetInstance(labelPrefab);
        SetLabel(instance, text);

        if (withContainer) {
            var container = GetInstance(labelContainer).transform;
            instance.transform.SetParent(container);
            currentContainer = container;
        }
    }

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

    public void AddInputArea(Func<string> getValue, Action<string> endEdit) {
        var instance = GetInstance(inputAreaPrefab);
        CoroutineManager.Instance.WaitAndDo(() => instance.GetComponent<InputField>().text = getValue());
        instance.GetComponent<InputField>().onEndEdit.AddListener((s) => endEdit(s));
    }

    public void AddInputArea(string val, Action<string> endEdit) {
        var instance = GetInstance(inputAreaPrefab);
        CoroutineManager.Instance.WaitAndDo(() => instance.GetComponent<InputField>().text = val);
        instance.GetComponent<InputField>().onEndEdit.AddListener((s) => endEdit(s));
    }

    public void AddDropDown<T>(IEnumerable<T> values, int value, Action<int> onValueChanged) {
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
        if (currentContainer) {
            instance.transform.SetParent(currentContainer);
            currentContainer = null;
        } else {
            instance.transform.SetParent(transform);
        }
        return instance;
    }

    void SetLabel(GameObject instance, string label) {
        instance.GetComponentInChildren<Text>().text = label;
    }
}