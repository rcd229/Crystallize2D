using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ResourcePath("UI/TextDisplayImage")]
public class TextDisplayUI : MonoBehaviour {
    List<string> textList = new List<string>();
    int index = 0;
    static TextDisplayUI _instance;
    public static bool open = false; //make other options for this
    public static TextDisplayUI Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstanceFromAttribute<TextDisplayUI>();
                MainCanvas.main.Add(_instance.transform);
            }
            return _instance;
        }
    }

    public void Display(PhraseSequence phrase) {
        open = true;
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
    }

    public void Close() {
        Destroy(gameObject);
        open = false;
    }

    // Use this for initialization
    public void Play(IEnumerable<string> strings) {
        gameObject.AddComponent<UIButton>().OnClicked += TextDisplayUI_OnClicked;
        textList.Clear();
        textList.AddRange(strings);
        Refresh();
    }

    void Update() {
        //Refresh();
    }

    void Refresh() {
        if (index < textList.Count) {
            GetComponentInChildren<Text>().text = textList[index];
        } else {
            Destroy(gameObject);
        }
    }

    private void TextDisplayUI_OnClicked(object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e) {
        index++;
        Refresh();
    }
}
