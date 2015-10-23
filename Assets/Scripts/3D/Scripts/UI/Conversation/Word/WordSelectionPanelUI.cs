using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordSelectionPanelUI : MonoBehaviour, ITemporaryUI<PhraseSequenceElement, PhraseSequenceElement> {

    const string ResourcePath = "UI/WordSelectionPanel";

    public static WordSelectionPanelUI GetInstance() {
        var instance = Instantiate<GameObject>(Resources.Load<GameObject>(ResourcePath));
        var panel = instance.GetComponent<WordSelectionPanelUI>();
        panel.Initialize();
        MainCanvas.main.Add(panel.transform);
        return panel;
    }

    public Transform wordParent;
    public GameObject wordButtonPrefab;

    IWordDropHandler source;
    Dictionary<UIButton, PhraseSequenceElement> buttonWords = new Dictionary<UIButton, PhraseSequenceElement>();

    public event EventHandler<EventArgs<PhraseSequenceElement>> Complete;

    public bool IsOpen {
        get {
            return gameObject;
        }
    }

    public void Initialize() {
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        foreach (var id in PlayerData.Instance.WordStorage.FoundWords) {
            var word = new PhraseSequenceElement(id, 0);
            if (word == null) {
                continue;
            }

            var instance = Instantiate(wordButtonPrefab) as GameObject;
            instance.transform.SetParent(wordParent);
            instance.GetComponentInChildren<Image>().color = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
            instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(word);//.GetPlayerText();
            instance.GetComponent<UIButton>().OnClicked += HandleClicked;
            buttonWords[instance.GetComponent<UIButton>()] = word;
        }

        TutorialCanvas.main.RegisterGameObject("WordSelector", gameObject);

        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
    }

	public void Initialize (IWordDropHandler wordContainer) {
        source = wordContainer;

        Initialize();
	}

    public void Initialize(PhraseSequenceElement element) {
        Initialize();
    }

    public void Close() {
        Exit(null);
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        Exit(null);
    }

    void HandleClicked(object sender, System.EventArgs e) {
        if (!(sender is UIButton)) {
            return;
        }

        if (!buttonWords.ContainsKey((UIButton)sender)) {
            return;
        }

        if (source != null) {
            source.AcceptDrop(new WordContainer(buttonWords[(UIButton)sender]));
        }

        Exit(new EventArgs<PhraseSequenceElement>(buttonWords[(UIButton)sender]));
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Exit(null);
        }
    }

    public void Exit(EventArgs<PhraseSequenceElement> args) {
        if (IsOpen) {
            Complete.Raise(this, args);
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        TutorialCanvas.main.UnregisterGameObject("WordSelector");
        CrystallizeEventManager.Environment.OnActorDeparted -= HandleActorDeparted;
    }

}
