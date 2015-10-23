using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseSelectorInitArgs {
    public List<PhraseSequence> Phrases { get; set; }
    public bool CheckLearned { get; set; }
    public bool UseTranslation { get; set; }
    public bool Center { get; set; }
    public string PromptText { get; set; }

    public PhraseSelectorInitArgs(List<PhraseSequence> phrases, string promptText = "What will you say?", bool check = true) {
        this.Phrases = phrases;
        this.CheckLearned = check;
        this.UseTranslation = true;
        this.PromptText = promptText;
        this.Center = false;
    }
}

[ResourcePath("UI/PhraseSelectionPanel")]
public class PhraseSelectionPanelUI : UIPanel, ITemporaryUI<PhraseSelectorInitArgs, PhraseSequence> {

    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    public Text text;
    PhraseSelectorInitArgs args;
    List<GameObject> instances = new List<GameObject>();
    bool checkLearned = true;

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    public void Initialize(PhraseSelectorInitArgs param1) {
        text.text = param1.PromptText;
        checkLearned = param1.CheckLearned;

        if (param1.Center) {
            transform.position = new Vector2(Screen.width * 0.5f, Screen.height * .5f);
        }

        Debug.Log("Phrases: " + param1.Phrases.Count);
        foreach (var p in param1.Phrases) {
            Debug.Log("Phrase is " + p);
            //Debug.Log("phrase")
        }

        this.args = param1;
        PlayerState_OnWordCollected(null, null);
        CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        //Debug.Log("word collected");
        UIUtil.GenerateChildren(args.Phrases, instances, buttonParent, CreateInstance);
    }

    void OnDestroy() {
        if (CrystallizeEventManager.main) {
            CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
        }
    }

    GameObject CreateInstance(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.AddComponent<DataContainer>().Store(phrase);
        bool held = false;
        //if (checkLearned) {
        if (phrase.ComparableElementCount == 0) {
            held = true;
        } else {
            bool isWord = phrase.IsWord;
            if (isWord) {
                if (PlayerData.Instance.WordStorage.ContainsFoundWord(phrase.Word)) {
                    held = true;
                }
            } else {
                if (PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
                    held = true;
                } else if (PlayerDataConnector.CanSelectPhrase(phrase)) {
                    held = true;
                }
            }
        }

        if(phrase.GetText().Trim() == "?"){
            instance.GetComponentInChildren<Text>().text = "<i>stay silent</i>";
            instance.GetComponentInChildren<Text>().color = GUIPallet.Instance.defaultTextColor;
            instance.GetComponent<UIButton>().OnClicked += PhraseSelectionPanelUI_OnClicked;
        } else if (held) {
            if (PlayerDataConnector.NeedToConstructPhrase(phrase)) {
                instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetTranslation(phrase);
            } else {
                instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase, true);
            }
            instance.GetComponentInChildren<Text>().color = GUIPallet.Instance.defaultTextColor;
            instance.GetComponent<UIButton>().OnClicked += PhraseSelectionPanelUI_OnClicked;
        } else {
            instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetTranslation(phrase);
            instance.GetComponentInChildren<Text>().color = GUIPallet.Instance.lightTextColor;
            instance.GetComponent<UIButton>().OnClicked += PhraseSelectionPanelUI_OnClicked; // UnfinishedPhrase_OnClicked;
        }
        return instance;
    }

    //void UnfinishedPhrase_OnClicked(object sender, EventArgs e) {
    //    var p = ((Component)sender).GetComponent<DataContainer>().Retrieve<PhraseSequence>();
    //    UILibrary.NeededWords.Get(PlayerDataConnector.GetNeededWords(p));
    //}

    void PhraseSelectionPanelUI_OnClicked(object sender, EventArgs e) {
        Exit(((Component)sender).GetComponent<DataContainer>().Retrieve<PhraseSequence>());
    }

    void Exit(PhraseSequence phrase) {
        Close();
        Complete.Raise(this, new EventArgs<PhraseSequence>(phrase));
    }

}
