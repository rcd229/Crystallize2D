using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Reflection;

public class DialoguePipeline : IContainsStaticPhrases {

    public void Initialize() {
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
            var allDialogueContainers = from t in a.GetTypes()
                                        where t.HasAttribute<StaticDialoguesAttribute>()
                                        select t;
            foreach(var t in allDialogueContainers) {
                foreach(var fp in t.GetStaticFieldAndPropertyValues<DialogueSequence>()) {
                    Debug.Log("Dialogue: " + fp);
                }
            }
        }
    }

}
