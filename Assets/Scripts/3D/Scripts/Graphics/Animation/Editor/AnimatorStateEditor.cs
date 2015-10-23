using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AnimatorStateEditor : UnityEditor.AssetModificationProcessor {

    static string[] OnWillSaveAssets(string[] paths) {
        CheckAnimator();
        return paths;
    }

    static void CheckAnimator() {
        var c = Resources.Load<AnimatorController>("FemaleController");
        var d = new Dictionary<string, string>();
        foreach (var s in c.layers[0].stateMachine.states) {
            if (s.state.motion != null) {
                d[s.state.name] = s.state.motion.name;
            }
            //Debug.Log(s.state.name);
        }

        var subD = new Dictionary<string, string>();
        foreach (var v in Enum.GetValues(typeof(AnimatorState))) {
            var s = v.ToString();
            if (!d.ContainsKey(s.ToString())) {
                Debug.LogError("Base animator [" + c.name + "] missing state [" + s + "]");
            } else {
                subD[s] = d[s];
            }
        }
        AnimatorNameMap.Instance.SetNames(subD);
        ///Debug.Log("Set names: " + subD.Count);
        EditorUtility.SetDirty(AnimatorNameMap.Instance);
    }

}
