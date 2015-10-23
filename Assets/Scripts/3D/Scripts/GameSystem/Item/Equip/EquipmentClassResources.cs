using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public enum AnimatorState {
    Stand,
    Run,
    Give,
    Use
}

public class EquipmentClassResources : ScriptableObject {

    [SerializeField]
    AnimationClip stand;
    [SerializeField]
    AnimationClip run;
    [SerializeField]
    AnimationClip give;
    [SerializeField]
    AnimationClip use;

    /*public EquipmentClassResources(AnimationClip stand, AnimationClip run, AnimationClip give) {
        this.stand = stand;
        this.run = run;
        this.give = give;
    }*/

    public void SetTo(GameObject target) {
        var animator = target.GetComponentInChildren<Animator>();

        var overrideController = new AnimatorOverrideController();
        if (animator.runtimeAnimatorController is AnimatorOverrideController) {
            //Debug.Log("is override");
            overrideController.runtimeAnimatorController = ((AnimatorOverrideController)animator.runtimeAnimatorController).runtimeAnimatorController;
        } else {
            overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        }

        SetOverride(overrideController, AnimatorState.Stand, stand);
        SetOverride(overrideController, AnimatorState.Run, run);
        SetOverride(overrideController, AnimatorState.Give, give);
        SetOverride(overrideController, AnimatorState.Use, use);

        animator.runtimeAnimatorController = overrideController;
        animator.Update(0);
    }

    void SetOverride(AnimatorOverrideController controller, AnimatorState state, AnimationClip clip) {
        //Debug.Log("setting " + clip + " to " + state);
        var a = AnimatorNameMap.Instance.GetAnimationForState(state.ToString());
        controller[a] = clip;
    }

}
