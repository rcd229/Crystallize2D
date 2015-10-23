using UnityEngine;
using System.Collections;

public class PersonAnimationPlayer : MonoBehaviour {

    Animator targetAnimator;

    public GameObject confusedParticlePrefab;
    public GameObject angryParticlePrefab;
    public GameObject happyParticlePrefab;
    public GameObject pointParticlePrefab;
    public GameObject alertParticlePrefab;

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.Environment.OnPersonAnimationRequested += HandlePersonAnimationRequested;
	}

    void HandlePersonAnimationRequested(object sender, PersonAnimationEventArgs args) {
        var a = args.TargetObject.GetComponentInChildren<Animator>();
        switch (args.AnimationType) {
            case PersonAnimationType.Alert:
                PlayAlertEffect(a);
                break;

            case PersonAnimationType.Happy:
                PlayHappyEffect(a);
                break;

            case PersonAnimationType.Angry:
                PlayAngryEffect(a);
                break;

            case PersonAnimationType.Confused:
                PlayConfusedEffect(a);
                break;

            case PersonAnimationType.Point:
                PlayPointEffect(a);
                break;

            case PersonAnimationType.Wave:
                PlayWaveEffect(a);
                break;

            case PersonAnimationType.Thanks:
                PlayThanksEffect(a);
                break;
        }

        //Debug.Log("Animation for " + args.TargetObject);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, args);
    }

    void PlayConfusedEffect(Animator animator) {
        AttachParticle(animator.transform, confusedParticlePrefab);
        animator.CrossFade("Questioning", 0.5f, 2);
    }

    void PlayAngryEffect(Animator animator) {
        AttachParticle(animator.transform, angryParticlePrefab);
        animator.CrossFade("Frustrated", 0.1f);
    }

    void PlayHappyEffect(Animator animator) {
        AttachParticle(animator.transform, happyParticlePrefab, 0.2f);
        animator.CrossFade("Clap", 0.1f, 2);
    }

    void PlayPointEffect(Animator animator) {
        //AttachParticle(animator.transform, pointParticlePrefab);
        animator.CrossFade("PointOnce", 0.25f);
    }

    void PlayWaveEffect(Animator animator) {
        animator.CrossFade("SingleWave", 0.1f);
    }

    void PlayThanksEffect(Animator animator) {
        animator.CrossFade("Bow", 0.1f);

        StartCoroutine(WaitAndReturnToBase(animator, 1.5f));
    }

    void PlayAlertEffect(Animator animator) {
        AttachParticle(animator.transform, alertParticlePrefab);
    }

    void AttachParticle(Transform target, GameObject prefab, float wait = 0) {
        StartCoroutine(WaitAndCreate(target, prefab, wait));
    }

    IEnumerator WaitAndReturnToBase(Animator animator, float wait) {
        yield return new WaitForSeconds(wait);

        animator.CrossFade("Stand", 0.1f);
    }

    IEnumerator WaitAndCreate(Transform target, GameObject prefab, float wait) {
        yield return new WaitForSeconds(wait);
        
        var go = Instantiate(prefab) as GameObject;
        go.transform.SetParent(target);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
    }

}
