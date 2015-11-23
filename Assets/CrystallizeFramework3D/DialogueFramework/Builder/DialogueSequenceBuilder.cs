using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DialogueSetBuilder : PhrasePipelineBuilder {
    public string SetKey { get; private set; }

    public DialogueSetBuilder(string setKey) : base(setKey) {
        SetKey = setKey;
    }

    public DialogueSequenceBuilder GetDialogueBuilder() {
        var builder = new DialogueSequenceBuilder(SetKey);
        builder.IsTest = IsTest;
        return builder;
    }

    public DialogueSequence Get(params string[] lines) {
        var b = GetDialogueBuilder();
        foreach (var l in lines) {
            b.AddLine(l);
        }
        return b.Build();
    }
}

public class BranchRef {
    public PhraseSequence Prompt { get; set; }
    public int Index { get; set; }

    public BranchRef() {
        Index = -1;
    }

    public BranchRef(int index) {
        Index = index;
    }

    public BranchRef(PhraseSequence prompt)
        : this() {
        Prompt = prompt;
    }
}

public class DialogueSequenceBuilder : PhrasePipelineBuilder {
    public DialogueSequence dialogue = new DialogueSequence();
    public DialogueElement lastElement;
    public DialogueElement thisElement;

    public bool breakOnBranches = true;

    Dictionary<BranchDialogueElement, List<BranchRef>> branchMapping = new Dictionary<BranchDialogueElement, List<BranchRef>>();
    List<DialogueElement> openBranches = new List<DialogueElement>();
    BranchRef openLink;
    bool readyToCloseBranches = false;

    public DialogueSequenceBuilder(string setKey) : base(setKey) { }

    public DialogueSequence Build() {
        AfterPrepareGameData();
        return dialogue;
    }

    void AfterPrepareGameData() {
        foreach (var k in branchMapping.Keys) {
            k.Branches = new List<BranchDialogueElementLink>(
                branchMapping[k].Select((b) => new BranchDialogueElementLink(b.Index, b.Prompt))
                );
        }
    }

    public void AddActor(string name) {
        dialogue.Actors.Add(new SceneObjectGameData(name));
    }

    public void Break() {
        lastElement = null;
    }

    public void OpenBranch() {
        if (lastElement != null) {
            openBranches.Add(lastElement);
        }
        Break();
    }

    public void CloseBranches() {
        readyToCloseBranches = true;
    }

    public void CloseBranches(int index) {
        foreach (var b in openBranches) {
            b.DefaultNextID = index;
        }
        openBranches = new List<DialogueElement>();
        readyToCloseBranches = false;
    }

    public void SetOpenLink(BranchRef branch) {
        openLink = branch;
    }

    public int AddSubDialogue(DialogueSequence dialogue, int actorID = 0) {
        var e = this.dialogue.AddNewDialogueElement(dialogue);
        e.ActorIndex = actorID;
        ResolveNextID(e);
        return e.ID;
    }

    public int AddLine(string phraseKey, int actorID = 0) {
        var e = dialogue.GetNewDialogueElement<LineDialogueElement>();
        e.ActorIndex = actorID;
        e.Line = new DialogueActorLine();
        e.Line.Phrase = GetPhrase(phraseKey);
        ResolveNextID(e);
        return e.ID;
    }

    public int AddEnglishLine(string phraseKey, int actorID = 0) {
        var e = dialogue.GetNewDialogueElement<LineDialogueElement>();
        e.ActorIndex = actorID;
        e.Line = new DialogueActorLine();
        e.Line.Phrase = new PhraseSequence(phraseKey);
        ResolveNextID(e);
        return e.ID;
    }

    //public int AddAnimation(EmoticonType emoticon, int actorID = 0) {
    //    return AddAnimation(new EmoticonDialogueAnimation(emoticon), actorID);
    //}

    //public int AddAnimation(string gesture, int actorID = 0) {
    //    return AddAnimation(new GestureDialogueAnimation(gesture), actorID);
    //}

    //public int AddAnimation(DialogueAnimation animation, int actorID = 0) {
    //    var e = dialogue.GetNewDialogueElement<AnimationDialogueElement>();
    //    e.ActorIndex = actorID;
    //    e.Animation = animation;
    //    ResolveNextID(e);
    //    return e.ID;
    //}

    //public int AddMessage(string message, int nextID = -10) {
    //    var e = dialogue.GetNewDialogueElement<MessageDialogueElement>();
    //    e.Message = message;
    //    ResolveNextID(e);
    //    return e.ID;
    //}

    public int AddEnglishBranch(params BranchRef[] branches) {
        return AddBranch(true, true, false, branches);
    }

    public int AddJapaneseBranch(params BranchRef[] branches) {
        return AddBranch(false, false, true, branches);
    }

    public int AddBranch(bool checkAvailable, bool displayTranslation, bool includeDefault, params BranchRef[] branches) {
        var e = dialogue.GetNewDialogueElement<BranchDialogueElement>();
        e.DisplayTranslation = displayTranslation;
        e.CheckAvailable = checkAvailable;
        e.IncludeDefaultBranch = includeDefault;
        branchMapping[e] = new List<BranchRef>(branches);
        ResolveNextID(e);
        if (breakOnBranches) {
            Break();
        }
        return e.ID;
    }

    //public void AddEvent(Action action) {
    //    AddEvent(ActionDialogueEvent.Get(action));
    //}

    //public void AddEvent(int confidence) {
    //    AddEvent(ActionDialogueEvent.Get(PlayerDataConnector.AddConfidence, confidence));
    //}

    public void AddEvent(IDialogueEvent dEvent) {
        if (thisElement != null) {
            dialogue.AddEvent(thisElement.ID, dEvent);
        }
    }

    public void AddEvents(params IDialogueEvent[] dEvents) {
        foreach (var e in dEvents) {
            AddEvent(e);
        }
    }

    void ResolveNextID(DialogueElement element) {
        if (readyToCloseBranches) {
            CloseBranches(element.ID);
        }

        if (openLink != null) {
            openLink.Index = element.ID;
            openLink = null;
        }

        if (lastElement != null) {
            lastElement.DefaultNextID = element.ID;
        }

        thisElement = element;
        lastElement = element;
    }
}
