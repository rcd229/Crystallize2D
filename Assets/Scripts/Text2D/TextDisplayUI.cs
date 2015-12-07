using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ResourcePath("UI/TextDisplayImage")]
public class TextDisplayUI : MonoBehaviour {
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


    //List<string> textList = new List<string>();
    //int index = 0;
    public GameObject phrasePrefab;

    GameObject phraseInstance;

    public void Display(PhraseSequence phrase) {
        open = true;
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        //GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        if (phraseInstance) {
            Destroy(phraseInstance);
        }

        phraseInstance = Instantiate(phrasePrefab);
        phraseInstance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        phraseInstance.transform.SetParent(transform, false);
    }

    public void Close() {
        Destroy(gameObject);
        open = false;
    }

    // Use this for initialization
    //public void Play(IEnumerable<string> strings) {
    //    gameObject.AddComponent<UIButton>().OnClicked += TextDisplayUI_OnClicked;
    //    textList.Clear();
    //    textList.AddRange(strings);
    //    Refresh();
    //}

    //void Update() {
    //    //Refresh();
    //}

    //void Refresh() {
    //    if (index < textList.Count) {
    //        GetComponentInChildren<Text>().text = textList[index];
    //    } else {
    //        Destroy(gameObject);
    //    }
    //}

    //private void TextDisplayUI_OnClicked(object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e) {
    //    index++;
    //    Refresh();
    //}
}
