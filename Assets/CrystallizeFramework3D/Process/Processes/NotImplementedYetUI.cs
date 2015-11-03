using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NotImplementedYetUI : UIPanel {

    static string ResourcePath = "UI/NotImplementedYet";

    public static NotImplementedYetUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<NotImplementedYetUI>(ResourcePath);
    }

    void Start() {
        MainCanvas.main.Add(transform, CanvasBranch.None);
    }

    public void SetProcessText(object processOutType) {
        if (processOutType != null) {
            GetComponentInChildren<Text>().text = string.Format("This feature ({0}) isn't implemented yet.\nClick anywhere to continue.", 
                processOutType.GetType().ToString());
        }
    }

}
