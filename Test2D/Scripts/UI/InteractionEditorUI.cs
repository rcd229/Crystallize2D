using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public abstract class InteractionEditorUI<T> : MonoBehaviour, ITemporaryUI<T, object>
    where T : IHasPhraseName, IHasGuid {
    public GameObject interactionButtonPrefab;
    public GameObject lineInputPrefab;
    public Text thingGuid;
    public PhraseInputUI thingNameInput;
    public RectTransform interactionParent;
    public Text interactionGuid;
    public InputField interactionNameInput;
    public InputField interactionDescriptionInput;
    public RectTransform lineParent;

    public event EventHandler<EventArgs<object>> Complete;

    protected T target;

    List<DialogueSegment2D> interactions = new List<DialogueSegment2D>();

    DialogueSegment2D selected;
    List<GameObject> interactionResources = new List<GameObject>();
    List<GameObject> lineResources = new List<GameObject>();
    Dictionary<InputField, LineDialogueElement> lines = new Dictionary<InputField, LineDialogueElement>();

    public abstract void Save();
    public abstract InteractionHostType Type { get; }

    public void Initialize(T args1) {
        this.target = args1;
        thingGuid.text = target.Guid.ToString();
        thingNameInput.Initialize(target.Name);
        thingNameInput.OnPhraseChanged += NamePhraseChanged;
        interactions = InteractionLoader2D.Load(Type, target);
        Refresh();
    }

    void NamePhraseChanged(object sender, PhraseEventArgs e) {
        target.Name = e.Phrase;
    }

    public void Close() {
        Save();
        foreach (var i in interactions) {
            InteractionLoader2D.Save(Type, target.Guid, i);
        }
        Complete.Raise(this, null);
        Destroy(gameObject);
    }

    public void AddInteraction() {
        interactions.Add(new DialogueSegment2D());
        Refresh();
    }

    public void AddLine() {
        if (selected != null) {
            var line = new LineDialogueElement();
            line.Line.Phrase = new PhraseSequence("");
            selected.Dialogue.AddNewDialogueElement(line);
            Refresh();
        }
    }

    void Refresh() {
        UIUtil.GenerateChildren(interactions, interactionResources, interactionParent, GenerateInteraction);
        if (selected != null) {
            lines.Clear();
            UIUtil.GenerateChildren(selected.Dialogue.Elements.Items.Where(i => i is LineDialogueElement).Cast<LineDialogueElement>(), lineResources, lineParent, GenerateLine);
        }
    }

    GameObject GenerateInteraction(DialogueSegment2D dialogue) {
        var instance = Instantiate<GameObject>(interactionButtonPrefab);
        instance.GetComponentInChildren<Text>().text = dialogue.Name;
        instance.GetOrAddComponent<UIButton>().OnClicked += ThingEditorUI_OnClicked;
        instance.GetOrAddComponent<DataContainer>().Store(dialogue);
        return instance;
    }

    void ThingEditorUI_OnClicked(object sender, EventArgs e) {
        selected = (sender as Component).GetComponent<DataContainer>().Retrieve<DialogueSegment2D>();
        interactionGuid.text = selected.Guid.ToString();
        interactionNameInput.text = selected.Name;
        interactionDescriptionInput.text = selected.Description;
        Refresh();
    }

    GameObject GenerateLine(LineDialogueElement line) {
        var instance = Instantiate<GameObject>(lineInputPrefab);
        instance.GetComponentInChildren<Text>().text = "line:";
        instance.GetComponentInChildren<PhraseInputUI>().Initialize(line.Line.Phrase); //.text = line.Line.Phrase.PhraseElements[0].Text;
        instance.GetComponentInChildren<PhraseInputUI>().OnPhraseEntered += InteractionEditorUI_OnPhraseEntered;
        instance.AddComponent<DataContainer>().Store(line);
        //lines[instance.GetComponentInChildren<InputField>()] = line;
        return instance;
    }

    void InteractionEditorUI_OnPhraseEntered(object sender, PhraseEventArgs e) {
        var line = (sender as Component).transform.parent.GetComponent<DataContainer>().Retrieve<LineDialogueElement>();
        line.Line.Phrase = e.Phrase;
    }

    void Update() {
        if (selected != null) {
            interactionGuid.text = selected.Guid.ToString();
            selected.Name = interactionNameInput.text;
            selected.Description = interactionDescriptionInput.text;

            foreach (var l in lines) {
                l.Value.Line.Phrase.PhraseElements[0].Text = l.Key.text;
            }
        } else {
            interactionGuid.text = "Click an interaction to edit";
        }
    }
}
