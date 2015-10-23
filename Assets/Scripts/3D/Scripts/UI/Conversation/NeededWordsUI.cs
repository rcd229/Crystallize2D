using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/Element/NeededWords")]
public class NeededWordsUI : UIPanel, ITemporaryUI<List<PhraseSequence>, object>, IDebugMethods {

    List<PhraseSequence> phrases = new List<PhraseSequence>();

    public GameObject wordPrefab;
    public RectTransform wordParent;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(List<PhraseSequence> param1) {
        phrases = param1;
        MainCanvas.main.PushLayer();
        MainCanvas.main.Add(transform);
        UIUtil.GenerateChildren(param1, wordParent, CreateWord);
        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    void Input_OnEnvironmentClick(object sender, EventArgs e) {
        Close();
    }

    public override void Close() {
        MainCanvas.main.PopLayer();
        base.Close();
        Complete.Raise(this, null);
    }

    GameObject CreateWord(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetComponentInChildren<Text>().text = phrase.Word.GetTranslation();
        return instance;
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
        }
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(Unlock);
    }

    public string Unlock(string input) {
        foreach (var p in phrases) {
            PlayerDataConnector.CollectItem(p);
        }
        return "";
    }
    #endregion

}
