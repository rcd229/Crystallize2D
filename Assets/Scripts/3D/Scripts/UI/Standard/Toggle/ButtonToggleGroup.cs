using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ButtonToggleGroup : MonoBehaviour {

    public List<RectTransform> buttons;

    int selectedIndex = 0;
    public int SelectedIndex {
        get { return selectedIndex; }
        set {
            selectedIndex = value;
            Refresh();
        }
    }

    public Color selectedBackgroundColor = Color.white;
    public Color selectedTextColor = Color.black;
    public Color unselectedBackgroundColor = Color.gray;
    public Color unselectedTextColor = Color.gray;

    void Start() {
        Refresh();
        foreach (var b in buttons) {
            b.gameObject.GetOrAddComponent<UIButton>().OnClicked += ToggleGroup_OnClicked;
        }
    }

    void ToggleGroup_OnClicked(object sender, EventArgs e) {
        var obj = (sender as Component).GetComponent<RectTransform>();
        SelectedIndex = Mathf.Clamp(buttons.IndexOf(obj), 0, buttons.Count);
    }

    void Refresh() {
        for (int i = 0; i < buttons.Count; i++) {
            if (i == SelectedIndex) {
                buttons[i].GetComponent<Image>().color = selectedBackgroundColor;
                buttons[i].GetComponentInChildren<Text>().color = selectedTextColor;
            } else {
                buttons[i].GetComponent<Image>().color = unselectedBackgroundColor;
                buttons[i].GetComponentInChildren<Text>().color = unselectedTextColor;
            }
        }
    }

}
