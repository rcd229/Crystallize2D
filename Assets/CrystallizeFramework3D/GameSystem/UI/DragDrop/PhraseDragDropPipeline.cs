using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseDragDropPipeline {
    const string SettingsPath = "PhraseDragDropSettings";

    static PhraseDragDropSettingsResource _settings;
    static PhraseDragDropSettingsResource Settings {
        get {
            if (_settings == null) {
                _settings = Resources.Load<PhraseDragDropSettingsResource>(SettingsPath);
            }
            return _settings;
        }
    }

    public static bool IsDragging { get; set; }

    public static GameObject BeginDrag(PhraseSequence phrase, Vector2 position) {
        return BeginDrag(phrase, position, phrase.IsWord);
    }

    public static GameObject BeginDrag(PhraseSequence phrase, Vector2 position, bool isWord) {
        var prefab = Settings.draggedPhrasePrefab;
        if (isWord) {
            prefab = Settings.draggedWordPrefab;
        }

        IsDragging = true;
        var instance = GameObject.Instantiate(prefab);
        Debug.Log(instance);
        instance.GetOrAddComponent<DraggedPhrase>().Initialize(phrase, position);
        instance.GetOrAddComponent<CanvasGroup>().interactable = false;
        instance.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
        return instance;
    }

    public static GameObject Click(PhraseSequence phrase, Vector2 position) {
        return Click(phrase, position, phrase.IsWord);
    }

    public static GameObject Click(PhraseSequence phrase, Vector2 position, bool isWord) {
        var prefab = Settings.draggedPhrasePrefab;
        if (isWord) {
            prefab = Settings.draggedWordPrefab;
        }

        var instance = GameObject.Instantiate(prefab);
        instance.AddComponent<DraggedPhrase>().Initialize(phrase, position);
        instance.GetOrAddComponent<CanvasGroup>().interactable = false;
        instance.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
        return instance;
    }
}