using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Linq;

public class DialoguePath {
    public string ClassName { get; set; }
    public string PropertyName { get; set; }

    public DialoguePath() { }

    public DialoguePath(string className, string propertyName) {
        ClassName = className;
        PropertyName = propertyName;
    }

    public override bool Equals(object obj) {
        if (!(obj is DialoguePath)) {
            return false;
        }
        var dp = (DialoguePath)obj;
        return ClassName == dp.ClassName && PropertyName == dp.PropertyName;
    }

    public override int GetHashCode() {
        return ClassName.GetHashCode() + 13 * PropertyName.GetHashCode();
    }

    public override string ToString() {
        return ClassName + ":" + PropertyName;
    }
}

public class DialoguePipeline : IContainsStaticPhrases {

    static Dictionary<string, DialogueSequence> dialogues = new Dictionary<string, DialogueSequence>();

    public static IEnumerable<string> GetDialoguePaths() {
        return dialogues.Keys;
    }

    public static DialogueSequence GetDialogue(string key) {
        if (dialogues.ContainsKey(key)) {
            return dialogues[key];
        } else {
            return null;
        }
    }

    public void Initialize() {
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
            var allDialogueContainers = from t in a.GetTypes()
                                        where t.HasAttribute<StaticDialoguesAttribute>()
                                        select t;
            foreach (var t in allDialogueContainers) {
                foreach (var fp in t.GetProperties()
                    .Where(p => typeof(DialogueSequence).IsAssignableFrom(p.PropertyType))) {
                    var d = (DialogueSequence)fp.GetValue(null, new object[0]);
                    dialogues.Add(new DialoguePath(t.GetType().ToString(), fp.Name).ToString(), d);
                    Debug.Log("Dialogue: " + fp);
                }
            }
        }
    }
}
