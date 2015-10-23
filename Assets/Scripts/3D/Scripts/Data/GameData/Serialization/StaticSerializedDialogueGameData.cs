using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystallizeData {
    public abstract class StaticSerializedDialogueGameData : StaticNonserializedGameData {

        protected class BranchRef {
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

        protected DialogueSequence dialogue = new DialogueSequence();
        protected DialogueElement lastElement;
        protected DialogueElement thisElement;
        protected bool breakOnBranches = true;

        Dictionary<BranchDialogueElement, List<BranchRef>> branchMapping = new Dictionary<BranchDialogueElement, List<BranchRef>>();

        public DialogueSequence GetDialogue() {
            PrepareGameData();
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

        protected void AddActor(string name) {
            dialogue.Actors.Add(new SceneObjectGameData(name));
        }

        protected void Break() {
            lastElement = null;
        }

        protected int AddSubDialogue(DialogueSequence dialogue, int actorID = 0) {
            var e = this.dialogue.AddNewDialogueElement(dialogue);
            e.ActorIndex = actorID;
            ResolveNextID(e);
            return e.ID;
        }

        protected int AddLine(string phraseKey, int actorID = 0) {
            var e = dialogue.GetNewDialogueElement<LineDialogueElement>();
            e.ActorIndex = actorID;
            e.Line = new DialogueActorLine();
            e.Line.Phrase = GetPhrase(phraseKey);
            ResolveNextID(e);
            return e.ID;
        }

        protected int AddAnimation(DialogueAnimation animation, int actorID = 0) {
            var e = dialogue.GetNewDialogueElement<AnimationDialogueElement>();
            e.ActorIndex = actorID;
            e.Animation = animation;
            ResolveNextID(e);
            return e.ID;
        }

        protected int AddMessage(string message, int nextID = -10) {
            var e = dialogue.GetNewDialogueElement<MessageDialogueElement>();
            e.Message = message;
            ResolveNextID(e);
            return e.ID;
        }

        protected int AddBranch(params BranchRef[] branches) {
            return AddBranch(true, true, branches);
        }

        protected int AddJapaneseBranch(params BranchRef[] branches) {
            return AddBranch(false, false, branches);
        }

        protected int AddBranch(bool checkAvailable, bool displayTranslation, params BranchRef[] branches) {
            var e = dialogue.GetNewDialogueElement<BranchDialogueElement>();
            e.DisplayTranslation = displayTranslation;
            e.CheckAvailable = checkAvailable;
            branchMapping[e] = new List<BranchRef>(branches);
            ResolveNextID(e);
            if (breakOnBranches) {
                Break();
            }
            return e.ID;
        }

        protected void AddEvent(Action action) {
            AddEvent(ActionDialogueEvent.Get(action));
        }

        protected void AddEvent(int confidence) {
            AddEvent(ActionDialogueEvent.Get(PlayerDataConnector.AddConfidence, confidence));
        }

        protected void AddEvent(IDialogueEvent dEvent) {
            if (thisElement != null) {
                dialogue.AddEvent(thisElement.ID, dEvent);
            }
        }

        protected void AddEvents(params IDialogueEvent[] dEvents) {
            foreach (var e in dEvents) {
                AddEvent(e);
            }
        }

        void ResolveNextID(DialogueElement element) {
            if (lastElement != null) {
                lastElement.DefaultNextID = element.ID;
            }

            thisElement = element;
            lastElement = element;
        }
    }

}