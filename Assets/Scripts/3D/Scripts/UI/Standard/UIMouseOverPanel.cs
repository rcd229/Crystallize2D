using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIMouseOverPanel : MonoBehaviour {

    const string ResourcePath = "UI/MouseOverPanel";
    static UIMouseOverPanel _instance;
    public static UIMouseOverPanel GetInstance() {
        if (!_instance) {
            _instance = GameObjectUtil.GetResourceInstance<UIMouseOverPanel>(ResourcePath);
            MainCanvas.main.Add(_instance.transform);
        }
        return _instance;
    }

    public void Initialize(string text, Vector2 position) {
        transform.SetAsLastSibling();
        GetComponentInChildren<Text>().text = text;
        transform.position = position;
    }

    public void Close() {
        Destroy(gameObject);
    }
}
