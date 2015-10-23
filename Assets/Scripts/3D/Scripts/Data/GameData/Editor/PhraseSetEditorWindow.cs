using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseSetEditorWindow : EditorWindow {

    const string NeedTranslation = "Need translation";

    static HashSet<string> allKeys = new HashSet<string>();
    static Dictionary<string, PhraseSequence> keySequences = new Dictionary<string, PhraseSequence>();

    static void UpdateKeySequences() {
        PhraseSetCollectionGameData.LoadAll();
        var sets = PhraseSetCollectionGameData.GetPhraseSets();

        foreach (var p in PhraseSetCollectionGameData.GetOrCreateItem("Default").Phrases) {
            UpdateKeySequence(p.Translation, p);
        }

        foreach (var set in sets) {
            if (!GameDataInitializer.phraseSets.ContainsKey(set.Name)) {
                continue;
            }

            var keys = GameDataInitializer.phraseSets[set.Name];
            for (int i = 0; i < set.Phrases.Count; i++) {
                if (i >= keys.Count) {
                    break;
                }

                if (!keySequences.ContainsKey(keys[i])) {
                    UpdateKeySequence(keys[i], set.Phrases[i]);
                }
            }
        }
    }

    static void UpdateKeySequence(string key, PhraseSequence phrase) {
        if (!phrase.IsEmpty) {
            keySequences[key] = phrase;
        }
    }

    static void GetAllKeys() {
        PhraseSetCollectionGameData.LoadAll();
        foreach (var k in GameDataInitializer.phraseSets.Keys) {
            foreach (var s in GameDataInitializer.phraseSets[k]) {
                if (!allKeys.Contains(s)) {
                    allKeys.Add(s);
                }
            }
        }
    }



    [MenuItem("Crystallize/Game Data/Phrase sets")]
    public static void Open() {
        var window = GetWindow<PhraseSetEditorWindow>();
        window.Initialize();
    }

    List<string> setNames;
    Vector2 scroll;
    string filterString = "";
    PhraseSetGameData target;
    bool showSets = true;

    bool initialized = false;

    PhraseSetGameData untranslated = new PhraseSetGameData();

    void Initialize() {
        if (!initialized) {
            //setNames = GameDataInitializer.phraseSets.Keys.ToList();
            setNames = new List<string>();
            setNames.Add("Default");
            setNames.Add(NeedTranslation);

            foreach (var p in PhraseSetCollectionGameData.Default.Phrases) {
                if (p.PhraseElements.Count == 0) {
                    untranslated.Phrases.Add(p);
                }
            }

            initialized = true;
        }

        UpdateKeySequences();
        //Debug.Log(setNames.Length);
        //setNames = GameData.Instance.PhraseSets.Items.Select((ps) => ps.Name).ToArray();
    }

    void OnGUI() {
        bool needRepaint = false;
        Initialize();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        filterString = EditorGUILayout.TextField("Filter", filterString);

        if (showSets) {
            var filtered = (from n in setNames
                            where n.ToLower().Contains(filterString.ToLower())
                            orderby n
                            select n);


            foreach (var n in filtered) {
                //Debug.Log(n);
                if (GUILayout.Button(n)) {
                    if (n == NeedTranslation) {
                        target = untranslated;
                    } else {
                        target = PhraseSetCollectionGameData.GetOrCreateItem(n);
                        if (n == "Default") {
                            InitializeDefault();
                        }
                    }
                    filterString = "";
                    needRepaint = true;
                }
            }
        }

        if (target != null) {
            if (target.Name == "Default") {
                DrawDefaultPhraseSet(target);
            } else {
                DrawPhraseSet(target);
            }
        }

        EditorGUILayout.EndScrollView();

        if (Event.current.type == EventType.Repaint) {
            var old = showSets;
            if (filterString == "" && target == null) {
                showSets = true;
            } else if (filterString != "") {
                showSets = true;
            } else {
                //Debug.Log(filterString.Length + "; " + (target == null));
                showSets = false;
            }

            if (old != showSets) {
                needRepaint = true;
            }
        }

        if (needRepaint) {
            Repaint();
        }
    }

    void InitializeDefault() {
        GetAllKeys();

        var pSet = PhraseSetCollectionGameData.GetOrCreateItem("Default");
        foreach (var k in allKeys) {
            if (!ContainsKey(pSet, k)) {
                PhraseSequence p = null;
                if (keySequences.ContainsKey(k)) {
                    p = new PhraseSequence(keySequences[k]);
                } else {
                    p = new PhraseSequence();
                }
                p.Translation = k;
                pSet.Phrases.Add(p);
            }
        }
    }

    void DrawPhraseSet(PhraseSetGameData phraseSet) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(phraseSet.Name);

        //var keys = GameDataInitializer.phraseSets[phraseSet.Name];
        //for (var i = 0; i < keys.Count; i++) {
        foreach (var p in phraseSet.Phrases){
            //var p = phraseSet.GetOrCreatePhrase(i);

            //bool isDefault = true;
            //if (keySequences.ContainsKey(keys[i])) {
            //    if (!PhraseSequence.PhrasesEquivalent(keySequences[keys[i]], p)) {
            //        isDefault = false;
            //    }
            //}

            //if (p.Translation != keys[i]) {
            //    isDefault = false;
            //}

            //if (p.IsEmpty && keySequences.ContainsKey(keys[i])) {
            //    p.PhraseElements = new List<PhraseSequenceElement>(keySequences[keys[i]].PhraseElements);
            //}

            //if (p.Translation.IsEmptyOrNull()) {
            //    p.Translation = keys[i];
            //}

            bool isDefault = true;
            //var p = phraseSet.GetPhrase(keys[i], out isDefault);
            var c = GUI.color;
            if (!isDefault) {
                GUI.color = new Color(0.9f, 0.7f, 0.7f);
            }
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUI.color = c;

            //EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(36f));
            //EditorGUILayout.LabelField(keys[i], GUILayout.Width(240f));
            EditorGUILayout.LabelField(p.Translation, GUILayout.Width(240f));
            EditorUtilities.DrawPhraseSequence(p);
            EditorGUILayout.EndHorizontal();
        }

        //if (GUILayout.Button("Commit to default")) {
        //    for (var i = 0; i < keys.Count; i++) {
        //        var p = phraseSet.GetOrCreatePhrase(i);
        //        if (keySequences.ContainsKey(keys[i])) {
        //            if (!PhraseSequence.PhrasesEquivalent(keySequences[keys[i]], p)) {
        //                var dp = PhraseSetCollectionGameData.Default.GetPhrase(keys[i]);
        //                dp.PhraseElements = new List<PhraseSequenceElement>(p.PhraseElements);
        //            }
        //        }
        //    }
        //}

        //if (GUILayout.Button("Reset to default")) {
        //    foreach (var p in phraseSet.Phrases) {
        //        p.PhraseElements.Clear();
        //        p.Translation = "";
        //    }
        //}

        EditorGUILayout.EndVertical();
    }

    string defaultFilterText = "";
    void DrawDefaultPhraseSet(PhraseSetGameData phraseSet) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(phraseSet.Name);

        defaultFilterText = EditorGUILayout.TextField("Filter", defaultFilterText);
        for (var i = 0; i < phraseSet.Phrases.Count; i++) {
            var p = phraseSet.Phrases[i];
            if (defaultFilterText != "") {
                if (!p.Translation.ToLower().Contains(defaultFilterText.ToLower())) {
                    continue;
                }
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(32f));
            EditorGUILayout.LabelField(p.Translation, GUILayout.Width(240f));
            EditorUtilities.DrawPhraseSequence(p);
            if (GUILayout.Button("up", GUILayout.Width(24f))) {
                UpdatePhraseInAllPhraseSets(p.Translation, p);
                break;
            }

            if (GUILayout.Button("-", GUILayout.Width(24f))) {
                phraseSet.Phrases.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Clean")) {
            Clean(phraseSet);
        }

        EditorGUILayout.EndVertical();
    }

    static IEnumerable<string> GetAllCompiledPhraseSetKeys() {
        return GameDataInitializer.phraseSets.Keys;
    }

    static bool ContainsCompiledPhraseSet(string setKey) {
        return GameDataInitializer.phraseSets.ContainsKey(setKey);
    }

    static List<string> GetCompiledPhraseSetKeys(string setKey) {
        return GameDataInitializer.phraseSets[setKey];
    }

    void UpdatePhraseInAllPhraseSets(string key, PhraseSequence phrase) {
        foreach (var setKey in GetAllCompiledPhraseSetKeys()) {
            var keys = GetCompiledPhraseSetKeys(setKey);
            for (int i = 0; i < keys.Count; i++) {
                if (keys[i] == key) {
                    PhraseSetCollectionGameData.GetOrCreateItem(setKey).SetPhrase(i, phrase);
                }
            }
        }
    }

    void Clean(PhraseSetGameData defaultPhraseSet) {
        var keys = new HashSet<string>();
        foreach (var setKey in GetAllCompiledPhraseSetKeys()) {
            foreach (var k in GetCompiledPhraseSetKeys(setKey)) {
                if (!keys.Contains(k)) {
                    keys.Add(k);
                }
            }
        }

        int removed = 0;
        for (int i = 0; i < defaultPhraseSet.Phrases.Count; i++) {
            if (!keys.Contains(defaultPhraseSet.Phrases[i].Translation)) {
                defaultPhraseSet.Phrases.RemoveAt(i);
                removed++;
                i--;
            }
        }

        Debug.Log("Removed " + removed + " phrases.");
    }

    void ResetAllPhraseSetsToDefault() {
        foreach (var ps in PhraseSetCollectionGameData.GetPhraseSets()) {
            if (ps != PhraseSetCollectionGameData.Default) {

            } else {
                Debug.Log("found default");
            }
        }
    }

    bool ContainsKey(PhraseSetGameData phraseSet, string key) {
        foreach (var p in phraseSet.Phrases) {
            if (p.Translation == key) {
                return true;
            }
        }
        return false;
    }

}