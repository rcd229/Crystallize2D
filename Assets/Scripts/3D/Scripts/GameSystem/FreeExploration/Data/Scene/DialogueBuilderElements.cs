using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DialogueBuilder {
    public abstract class Element {
        public readonly int actor;
        public Element(int actor = 0) { this.actor = actor; }
        public override string ToString() { return "Actor" + actor; }
    }

    public class LineElement : Element {
        public readonly string phraseKey;
        public LineElement(string phraseKey) : this(0, phraseKey) { }
        public LineElement(int actor, string phraseKey) : base(actor) { this.phraseKey = phraseKey; }
        public override string ToString() { return phraseKey; }
    }

    public class EnglishLineElement : Element {
        public readonly string line;
        public EnglishLineElement(int actor, string line) : base(actor) { this.line = line; }
        public override string ToString() { return line; }
    }

    public class MessageElement : Element {
        public readonly string message;
        public MessageElement(string message) {
            this.message = message;
        }
    }

    public class AnimationElement : Element {
        public readonly DialogueAnimation animation;
        public AnimationElement(DialogueAnimation animation) : this(0, animation) { }
        public AnimationElement(int actor, DialogueAnimation animation) : base(actor) { this.animation = animation; }
        public override string ToString() { return animation.ToString(); }
    }

    public class BranchElement : Element {
        public readonly PromptResponsePair[] branches;
        public readonly bool isEnglish;
        public BranchElement(bool isEnglish, params PromptResponsePair[] branches)
            : base() {
            this.branches = branches;
            this.isEnglish = isEnglish;
        }
        public override string ToString() { return branches[0].promptKey; }
    }

    public class EventElement : Element {
        public readonly IDialogueEvent Event;
        public EventElement(IDialogueEvent Event) : base() { this.Event = Event; }
    }

    public class ExitDialogueElement : Element {
        public static ExitDialogueElement Instance = new ExitDialogueElement();
    }

    public class PromptResponsePair {
        public readonly string promptKey;
        public readonly Element[] responseElements;
        public readonly bool isEnglish;
        public PromptResponsePair(string promptKey, bool isEnglish, params Element[] responseKeys) {
            this.promptKey = promptKey;
            this.responseElements = responseKeys;
            this.isEnglish = isEnglish;
        }

        public PromptResponsePair(string promptKey, bool isEnglish, params string[] responseKeys)
            : this(promptKey, isEnglish, responseKeys.Select(k => new LineElement(k)).ToArray()) { }
    }
}
