using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/ChoiceMenu")]
public class ChoiceMenu2D : MonoBehaviour, ITemporaryUI<List<PhraseSequence>, PhraseSequence> {

    public static ChoiceMenu2D GetInstance() {
        return GameObjectUtil.GetResourceInstanceFromAttribute<ChoiceMenu2D>();
    }

    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    public void Initialize(List<PhraseSequence> args1) {
        UIUtil.GenerateChildren(args1, buttonParent, GenerateChild);
    }

    public void Close() {
        Destroy(gameObject);
    }

    GameObject GenerateChild(PhraseSequence phrase) {
        var inst = Instantiate<GameObject>(buttonPrefab);
        inst.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        inst.GetOrAddComponent<UIButton>().OnClicked += ChoiceMenu2D_OnClicked;
        inst.GetOrAddComponent<DataContainer>().Store(phrase);
        return inst;
    }

    void ChoiceMenu2D_OnClicked(object sender, EventArgs e) {
        var p = (sender as Component).GetComponent<DataContainer>().Retrieve<PhraseSequence>();
        Complete.Raise(this, new EventArgs<PhraseSequence>(p));
        Close();
    }

}
