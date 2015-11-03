using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("AnimatorNameMap")]
public class AnimatorNameMap : ScriptableObject {

    static AnimatorNameMap _instance;
    public static AnimatorNameMap Instance {
        get {
            if (!_instance) {
                _instance = Resources.Load<AnimatorNameMap>("AnimatorNameMap");
            }
            return _instance;
        }
    }

    [Serializable]
    class StateAnimation {
        public string animation;
        public string state;

        public StateAnimation(){}

        public StateAnimation(string a, string s){
            animation = a;
            state = s;
        }
    }

    [SerializeField]
    List<StateAnimation> stateAnimations = new List<StateAnimation>();

    public void SetNames(Dictionary<string, string> names) {
        if (Application.isPlaying) {
            Debug.LogError("Cannot change NameMap at run time.");
            return;
        }
        stateAnimations = new List<StateAnimation>();
        foreach (var kv in names) {
            //Debug.Log("Adding: " + kv.Key);
            stateAnimations.Add(new StateAnimation(kv.Value, kv.Key));
        }
    }

    public string GetAnimationForState(string state) {
        foreach (var sa in stateAnimations) {
            if (sa.state == state) {
                return sa.animation;
            }
        }
        return null;
    }

}
