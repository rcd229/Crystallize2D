using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueBuilder;

public class ContainsDialogueBase {
    protected static LineElement Line(string phraseKey) { return new LineElement(phraseKey); }
    protected static LineElement Line(int actorIndex, string phraseKey) { return new LineElement(actorIndex, phraseKey); }

    protected static EnglishLineElement EnglishLine(string phraseKey) { return new EnglishLineElement(0, phraseKey); }

    //protected static AnimationElement Animation(string animation) { return new AnimationElement(new GestureDialogueAnimation(animation)); }
    //protected static AnimationElement Animation(int actorIndex, string animation) { return new AnimationElement(actorIndex, new GestureDialogueAnimation(animation)); }

    protected static AnimationElement Animation(DialogueAnimation animation) { return new AnimationElement(animation); }
    protected static AnimationElement Animation(int actorIndex, DialogueAnimation animation) { return new AnimationElement(actorIndex, animation); }

    //protected static AnimationElement Animation(EmoticonType animation) { return new AnimationElement(new EmoticonDialogueAnimation(animation)); }
    //protected static AnimationElement Animation(int actorIndex, EmoticonType animation) { return new AnimationElement(actorIndex, new EmoticonDialogueAnimation(animation)); }

    protected static BranchElement Branch(params PromptResponsePair[] branches) { return new BranchElement(false, branches); }
    protected static BranchElement EnglishBranch(params PromptResponsePair[] branches) { return new BranchElement(true, branches); }

    protected static EventElement Event(IDialogueEvent dialogueEvent) { return new EventElement(dialogueEvent); }
    //protected static EventElement Confidence(int confidence) { return new EventElement(ActionDialogueEvent.Get(PlayerDataConnector.AddConfidence, confidence)); }

    protected static MessageElement Message(string message) { return new MessageElement(message); }

    protected static ExitDialogueElement ExitDialogue { get { return ExitDialogueElement.Instance; } }

    protected static PromptResponsePair Prompted(string promptKey, params Element[] elements) { return new PromptResponsePair(promptKey, false, elements); }
    protected static PromptResponsePair EnglishPrompted(string promptKey, params Element[] elements) { return new PromptResponsePair(promptKey, true, elements); }

    static void AddElements(DialogueSequenceBuilder builder, IEnumerable<Element> elements) {
        foreach (var e in elements) {
            AddActors(builder, e);
            if (e is LineElement) {
                var le = (LineElement)e;
                builder.AddLine(le.phraseKey, le.actor);
            } else if (e is EnglishLineElement) {
                var ele = (EnglishLineElement)e;
                builder.AddEnglishLine(ele.line);
            } 
            //else if (e is AnimationElement) {
            //    var ae = (AnimationElement)e;
            //    builder.AddAnimation(ae.animation, ae.actor);
            //} 
            else if (e is EventElement) {
                var ee = (EventElement)e;
                builder.AddEvent(ee.Event);
            } 
            //else if (e is MessageElement) {
            //    var me = (MessageElement)e;
            //    builder.AddMessage(me.message);
            //} 
            else if (e is ExitDialogueElement) {
                builder.Break();
            } else if (e is BranchElement) {
                var be = (BranchElement)e;
                int index = 0;
                BranchRef[] branchRefs = new BranchRef[0];
                if (be.isEnglish) {
                    branchRefs = be.branches.Select(b => new BranchRef(new PhraseSequence(b.promptKey))).ToArray();
                    builder.AddEnglishBranch(branchRefs);
                } else {
                    branchRefs = be.branches.Select(
                        b => b.isEnglish ? new BranchRef(new PhraseSequence(b.promptKey)) : new BranchRef(builder.GetPhrase(b.promptKey))
                        ).ToArray();
                    builder.AddJapaneseBranch(branchRefs);
                }
                foreach (var b in be.branches) {
                    builder.SetOpenLink(branchRefs[index]);
                    index++;
                    AddElements(builder, b.responseElements);
                    builder.OpenBranch();
                }
                builder.CloseBranches();
            }
        }
    }

    static void AddActors(DialogueSequenceBuilder builder, Element element) {
        while (builder.dialogue.Actors.Count < element.actor) {
            builder.AddActor("Actor" + (builder.dialogue.Actors.Count + 1));
        }
    }

    public static DialogueSequence BuildDialogue(params Element[] elements) {
        return BuildDialogue("Default", elements);
    }

    public static DialogueSequence BuildDialogue(bool isTest, params Element[] elements) {
        return BuildDialogue("Default", isTest, elements);
    }

    public static DialogueSequence BuildDialogue(string key, params Element[] elements) {
        return BuildDialogue(key, false, elements);
    }

    public static DialogueSequence BuildDialogue(string key, bool isTest, params Element[] elements) {
        var b = new DialogueSequenceBuilder(key);
        b.IsTest = isTest;
        AddElements(b, elements);
        return b.Build();
    }
}
