using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class QuestHUDEntry : MonoBehaviour, IInitializable<QuestHUDItem> {

    public Text title;
    public RectTransform subEntryParent;
    public GameObject subEntryPrefab;

    public void Initialize(QuestHUDItem args) {
        title.text = args.Title;
        foreach (var subItem in args.SubItems) {
            var instance = Instantiate<GameObject>(subEntryPrefab);
            instance.GetInterface<IInitializable<QuestHUDSubItem>>().Initialize(subItem);
            instance.transform.SetParent(subEntryParent, false);
        }
    }
}
