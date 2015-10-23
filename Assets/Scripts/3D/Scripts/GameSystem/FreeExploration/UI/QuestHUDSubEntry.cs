using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class QuestHUDSubEntry : MonoBehaviour, IInitializable<QuestHUDSubItem> {

    public RectTransform checkMark;
    public Text text;

    public void Initialize(QuestHUDSubItem args1) {
        checkMark.gameObject.SetActive(args1.IsComplete);
        text.text = args1.Text;
    }
}
