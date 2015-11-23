using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[ResourcePath("UI/SelectionPanel")]
public class PhraseSelectionPanelUI : SingletonUI<List<BranchDialogueElementLink>, int, PhraseSelectionPanelUI> {
    public GameObject buttonPrefab;

    public override void Initialize(List<BranchDialogueElementLink> args1) {
        int c = 0;
        foreach (var link in args1) {
            var inst = Instantiate(buttonPrefab);
            var index = c;
            inst.transform.SetParent(transform);
            inst.GetComponent<UIButton>().OnClicked += (s, e) => Select(index);
            inst.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(link.Prompt); 
            c++;
        }
    }

    public void Select(int selected) {
        Complete.Raise(this, new EventArgs<int>(selected));
        Close();
    }
}
