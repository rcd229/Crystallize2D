using UnityEngine;
using System.Collections;

public class SetAnimation : MonoBehaviour {

    public string animationFlag;
    public bool isState = true;
    public Animator animator;

    // Use this for initialization
    void Start() {
        Set(animationFlag);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (!animator) {
                animator = GetComponentInChildren<Animator>();
            }
            animator.CrossFade(animationFlag, 0.1f);

            CoroutineManager.Instance.WaitAndDo(() => {
                var go = EffectLibrary.GetEffect(EffectLibrary.DustCloud);
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
            },
            new WaitForSeconds(0.75f));
        }
    }

    public void Set(string animation) {
        this.animationFlag = animation;

        if (!animator) {
            animator = GetComponentInChildren<Animator>();
        }

        if (isState) {
            animator.Play(animationFlag);
        } else {
            animator.SetBool(animationFlag, true);
        }
    }


}
