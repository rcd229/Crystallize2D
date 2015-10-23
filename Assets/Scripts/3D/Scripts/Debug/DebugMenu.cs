using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/DebugMenu")]
public class DebugMenu : UIPanel, ITemporaryUI<object, object> {

    public GameObject buttonPrefab;
    public InputField argInput;
    public RectTransform buttonParent;
    public Text output;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object obj) {
        var methods = new List<NamedMethod>();
        methods.AddRange(GlobalDebugMethods.Instance.GetMethods());
        methods.AddRange(DebugMenuListener.GetMethods());

        UIUtil.GenerateChildren(methods, buttonParent, CreateButtonChild);
    }

	public override void Close() {
        Destroy(gameObject);
    }

    GameObject CreateButtonChild(NamedMethod method) {
        var instance = GameObject.Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = method.Name;
        instance.GetComponent<UIButton>().OnClicked += (s, e) => Output(method.Method(argInput.text));
        return instance;
    }

    void Output(string s) {
        if (s == null) {
            output.text = "NULL";
        } else {
            output.text = s;
        }
    }

}
