using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueBuilder;

public abstract class ContainsDialogueUniqueIDData<T> : UniqueIDData<T> where T : ContainsDialogueUniqueIDData<T> {
    protected static DialogueSetBuilder builderSet = new DialogueSetBuilder("NPCGroup");

    public ContainsDialogueUniqueIDData(string name, Guid id)
        : base(name, id) { }

    protected DialogueSequence BuildDialogue(params Element[] elements) {
        var b = builderSet.GetDialogueBuilder();
        AddElements(b, elements);
        return b.Build();
    }

    void AddElements(DialogueSequenceBuilder builder, IEnumerable<Element> elements) {
        foreach (var e in elements) {
            AddActors(builder, e);
            if (e is LineElement) {
                var le = (LineElement)e;
                builder.AddLine(le.phraseKey, le.actor);
            } else if (e is EnglishLineElement) {
                var ele = (EnglishLineElement)e;
                builder.AddEnglishLine(ele.line);
            } else if (e is AnimationElement) {
                var ae = (AnimationElement)e;
                builder.AddAnimation(ae.animation, ae.actor);
            } else if (e is EventElement) {
                var ee = (EventElement)e;
                builder.AddEvent(ee.Event);
            } else if (e is BranchElement) {
                var be = (BranchElement)e;
                var branchRefs = be.branches.Select(b => new BranchRef(builder.GetPhrase(b.promptKey))).ToArray();
                int index = 0;
                builder.AddJapaneseBranch(branchRefs);
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

    void AddActors(DialogueSequenceBuilder builder, Element element) {
        while (builder.dialogue.Actors.Count < element.actor) {
            builder.AddActor("Actor" + (builder.dialogue.Actors.Count + 1));
        }
    }

}
