using UnityEngine;
using System.Collections;

public class GestureDialogueAnimation : DialogueAnimation {

    const float MinWait = 0.05f;

    public string Animation { get; set; }
    public float Wait { get; set; }

    Animator animator;

    public GestureDialogueAnimation() {
        Animation = "";
    }

    public GestureDialogueAnimation(string animation, float wait = 0.5f) {
        Animation = animation;
        Wait = Mathf.Max(MinWait, wait);
    }

    public GestureDialogueAnimation(AnimatorState animation, float wait = 0.5f) : this(animation.ToString(), wait) { }

    public override DialogueAnimation GetInstance() {
        return new GestureDialogueAnimation(Animation, Wait);
    }

    public override void Play(GameObject actor) {
        animator = actor.GetComponentInChildren<Animator>();

        CoroutineManager.Instance.StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence() {
        animator.CrossFade(Animation, 0.1f);

        yield return new WaitForSeconds(Wait);

        Exit();
    }

}