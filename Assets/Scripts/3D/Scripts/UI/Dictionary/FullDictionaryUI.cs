using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JapaneseTools;

[ResourcePath("UI/Dictionary")]
public class FullDictionaryUI : MonoBehaviour, ITemporaryUI<string, object> {
    public GameObject buttonPrefab;
    public RectTransform entryParent;
    public Text labelText;
    public Text infoText;
    public InputField searchInput;

    public event EventHandler<EventArgs<object>> Complete;

    DictionaryDataEntry selectedEntry;
    List<GameObject> entryInstances = new List<GameObject>();

    public void Initialize(string args1) {
        MainCanvas.main.PushLayer();
        MainCanvas.main.Add(transform);
        searchInput.text = args1;
        PlayerManager.LockMovement(this);
        Refresh();
    }

    public void Close() {
        PlayerManager.UnlockMovement(this);
        Destroy(gameObject);
        MainCanvas.main.PopLayer();
    }

    public void Search() {
        Refresh();
    }

    public void AddSelected() {
        if (selectedEntry != null) {
            var word = new PhraseSequenceElement(selectedEntry.ID, 0);
            if (!PlayerDataConnector.ContainsLearnedItem(word)) {
                PlayerDataConnector.CollectItem(new PhraseSequence(word));
            }
        }
    }

    void Refresh() {
        var searchString = searchInput.text;
        IEnumerable<DictionaryDataEntry> searchResult = new DictionaryDataEntry[0];
        if (searchString.Contains("-j")) {
            searchString = searchString.Replace("-j", "").Trim();
            searchResult = from e in DictionaryData.Instance.Entries
                           where KanaConverter.Instance.ConvertToRomaji(e.Kana).StartsWith(searchString)
                           orderby e.Kana
                           select e;
        } else if (searchString.Contains("-e")) {
            searchString = searchString.Replace("-e", "").Trim();
            searchResult = from e in DictionaryData.Instance.Entries
                           where e.English.Any((s) => s.StartsWith(searchString))
                           orderby e.Kana
                           select e;
        } else {
            searchResult = from e in DictionaryData.Instance.Entries
                           where KanaConverter.Instance.ConvertToRomaji(e.Kana).StartsWith(searchString) || e.English.Any((s) => s.StartsWith(searchString))
                           orderby e.Kana
                           select e;
        }
        UIUtil.GenerateChildren(searchResult.Take(50), entryInstances, entryParent, GetChild);
    }

    GameObject GetChild(DictionaryDataEntry entry) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.AddComponent<DataContainer>().Store(entry);
        instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(new PhraseSequenceElement(entry.ID, 0));
        instance.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        instance.GetOrAddComponent<LayoutElement>().preferredWidth = 240f;
        instance.GetOrAddComponent<UIButton>().OnClicked += FullDictionaryUI_OnClicked;
        return instance;
    }

    void FullDictionaryUI_OnClicked(object sender, EventArgs e) {
        selectedEntry = (sender as Component).GetComponent<DataContainer>().Retrieve<DictionaryDataEntry>();
        var labels = "";
        var values = "";
        if (selectedEntry.Kanji != selectedEntry.Kana) {
            labels += "\nKanji";
            values += "\n" + Bold(selectedEntry.Kanji);
        }
        labels += "\nKana";
        values += "\n" + Bold(selectedEntry.Kana);
        labels += "\nRomaji";
        values += "\n" + Bold(KanaConverter.Instance.ConvertToRomaji(selectedEntry.Kana));
        labels += "\n";
        values += "\n";
        labels += "\nDefinitions";
        foreach (var eng in selectedEntry.English) {
            values += "\n" + eng;
        }
        labelText.text = labels;
        infoText.text = values;
    }

    string Bold(string value) {
        return string.Format("<b>{0}</b>", value);
    }

}
