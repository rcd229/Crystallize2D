using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/Network/ChatBox")]
public class ChatBoxUI : MonoBehaviour, ITemporaryUI<object, object> {

    public static ChatBoxUI Instance { get; private set; }

    public GameObject wordPrefab;
    public GameObject popupPrefab;
    public RectTransform wordParent;
    public RectTransform popupParent;
    public Text text;
    public InputField input;
    public ButtonToggleGroup chatModeGroup;

    List<GameObject> wordInstances = new List<GameObject>();
    List<GameObject> popupInstances = new List<GameObject>();
    //List<PhraseSequence> allWords = new List<PhraseSequence>();
    List<PhraseSequenceElement> words = new List<PhraseSequenceElement>();

    Dictionary<char, PhraseSequence> phrases = new Dictionary<char, PhraseSequence>();
    bool quitting = false;
    int childCount = 0;

    void Awake() {
        Instance = this;
        gameObject.SetActive(PlayerDataConnector.ChatBoxOpenStatus);
    }

    void Start() {
        ChatLog.Instance.ChatLogChanged += Instance_ChatLogChanged;

        //allWords = new List<PhraseSequence>(PhraseSetCollectionGameData.Default.AggregateAllWords());
    }

    void OnDestroy() {
        ChatLog.Instance.ChatLogChanged -= Instance_ChatLogChanged;
        PlayerManager.UnlockMovement(this);
    }

    void Update() {
        if (input.isFocused) {
            PlayerManager.LockMovement(this);
            input.GetComponent<Image>().color = Color.white;
        } else {
            PlayerManager.UnlockMovement(this);
            input.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            input.Select();
            input.ActivateInputField();
        }
    }

    void Instance_ChatLogChanged(object sender, EventArgs e) {
        var s = "";
        foreach (var l in ChatLog.Instance.ChatLines) {
            s += l + "\n";
        }
        s = s.Substring(0, s.Length - 1);
        text.text = s;
    }

    public void TextChanged(string text) {
        if (text.Length > 0 && phrases.ContainsKey(text[text.Length - 1])) {
            AddWord(phrases[text[text.Length - 1]]);
            input.text = "";
            return;
        }

        if (text.Length == 0) {
            UpdateWords(null);
        } else {
            var allWords = from r in PlayerData.Instance.Reviews.Reviews
                           select r.Item;
            var words = from w in allWords 
                        where w.GetText(JapaneseTools.JapaneseScriptType.Romaji).StartsWith(text)
                        orderby w.GetText(JapaneseTools.JapaneseScriptType.Romaji).Length
                        select w;
            UpdateWords(words.Take(5));
        }
    }

    public void EnterLine(string text) {
        if (words.Count > 0 || text.Length > 0) {
            if(text.Length > 0){
                words.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, text));
            }
            var phrase = new PhraseSequence();
            phrase.PhraseElements = words;
            ChatLog.Instance.EnterLine(PlayerData.Instance.PersonalData.Name + ": " + PlayerDataConnector.GetText(phrase), chatModeGroup.SelectedIndex);
            ChatLog.Instance.OpenSpeechBubble(phrase);
            Debug.Log(PlayerManager.Instance.PlayerGameObject);
            UILibrary.SpeechBubble.Get(new ChatSpeechBubbleUIInitArgs(phrase, PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>()));
            DataLogger.LogTimestampedData("Chat", phrase.GetText());

            words = new List<PhraseSequenceElement>();
            wordInstances.DestroyAndClear();
        } else if (text.Length == 0) {
            PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        }
        input.text = "";
    }

    void OnApplicationQuit() {
        quitting = true;
    }

    void AddWord(PhraseSequence word) {
        words.Add(word.Word);
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(word);
        instance.transform.SetParent(wordParent, false);
        wordInstances.Add(instance);
    }

    void UpdateWords(IEnumerable<PhraseSequence> words) {
        if (quitting) {
            return;
        }

        phrases = new Dictionary<char, PhraseSequence>();
        if (words == null) {
            popupInstances.DestroyAndClear();
            return;
        }

        childCount = 1;
        popupParent.gameObject.SetActive(popupInstances.Count > 0);
        UIUtil.GenerateChildren(words, popupInstances, popupParent, GenerateWordChild);
        for (int i = 0; i < popupInstances.Count; i++) {
            popupInstances[i].transform.SetAsFirstSibling();
        }
    }

    GameObject GenerateWordChild(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(popupPrefab);
        instance.transform.Find("NumberText").GetComponent<Text>().text = childCount + ":";
        phrases[childCount.ToString()[0]] = phrase;
        childCount++;
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        return instance;
    }

    public void OpenEmoticon(int type) {
        UILibrary.Emoticon.Get(new EmoticonInitArgs(PlayerManager.Instance.PlayerGameObject.transform, EmoticonType.Get(type)));
        if (CrystallizeNetwork.Connected) {
            CrystallizeNetwork.Client.SendEmoteToAll(type);
        }
    }

    public void Close() {
        Destroy(gameObject);
    }

    public void Initialize(object param1) {
        //throw new NotImplementedException();
    }

    public event EventHandler<EventArgs<object>> Complete;
}
