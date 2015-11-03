using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Object2DDefaultEditor: MonoBehaviour, IInitializable<Object2D> {
    public void Initialize(Object2D args1) {
        var controls = GetComponent<Object2DEditorControls>();

        controls.AddDropDown(Object2D.GetTypes(), GetIndex(Object2D.GetTypes(), args1.GetType()), i => SelectType(args1, i));
        controls.AddInputField(() => args1.Name, s => SetName(args1, s));
        controls.AddButton(() => Delete(args1), "Delete");
        controls.AddButton(Close, "Close");
    }

    int GetIndex(IEnumerable<object> objs, object obj) {
        int i = 0;
        foreach (var o in objs) {
            if(o == obj) {
                return i;
            }
            i++;
        }
        return 0;
    }

    void SetName(Object2D obj, string name) {
        obj.Name = name;
        Object2DSceneResourceManager.Instance.Save(obj);
    }

    void Delete(Object2D obj) {
        Object2DSceneResourceManager.Instance.Delete(obj);
        Close();
    }

    void SelectType(Object2D obj, int type) {
        var newObj = (Object2D)Activator.CreateInstance(Object2D.GetTypes().GetSafely(type));
        obj.CopyTo(newObj);
        Object2DSceneResourceManager.Instance.Create(newObj);
        Object2DEditorResourceManager.Instance.OpenEditorFor(newObj);
    }

    void Close() {
        Destroy(gameObject);
    }
}