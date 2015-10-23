using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ContextActionController : MonoBehaviour, ITemporaryUI<ExploreInitArgs, ExploreResultArgs>, IHasCanvasBranch {

    public static ContextActionController GetInstance() {
        return new GameObject("ContextActionController").AddComponent<ContextActionController>();
    }

    public event EventHandler<EventArgs<ExploreResultArgs>> Complete;

    string item;
    string requiredTag;
    ITemporaryUI<ContextActionArgs, object> statusUI;

    Coroutine coroutine;
    GameObject target;

    public CanvasBranch Branch { get { return CanvasBranch.Persistent; } }

    public void Initialize(ExploreInitArgs param1) {
        Debug.Log("Created new controller.");
        item = param1.ActiveItem;
        requiredTag = param1.RequiredTag;

        //statusUI = UILibrary.ContextActionStatus.Get(new ContextActionArgs("", false));
        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Input_OnEnvironmentClick(object sender, EventArgs e) {
        if (transform) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(DoUse());
        }
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
    }

    IEnumerator DoUse() {
        //Debug.Log(target);
        if (!IsValidTarget(target)) {
            SoundEffectManager.Play(SoundEffectType.Invalid);
        } else if (!PlayerManager.Instance.InInteractionRange(target.transform)) {
            statusUI = UILibrary.ContextActionStatus.Get(new ContextActionArgs("move closer", false, true));
            yield break;
        }

        var t = target;
        if (target && target.GetComponent<_SceneSayPhrase>()) {
            target.GetComponent<_SceneSayPhrase>().Say();
            yield break;
        }

        if (!item.IsEmptyOrNull()) {
            var inst = new EquipmentItemRef(item).SetTo(PlayerManager.Instance.PlayerGameObject);
            if (inst != null) {
                CoroutineManager.Instance.WaitAndDo(() => Destroy(inst), new WaitForSeconds(1f));
            }

            new EquipmentItemRef(item).Use(PlayerManager.Instance.PlayerGameObject, target);

            PlayerManager.LockMovement(this);
            yield return new WaitForSeconds(0.1f);
            PlayerManager.UnlockMovement(this);
        }

        if (IsValidTarget(t)) {
            Exit(new ExploreResultArgs(t));
        }
    }

    void Exit(ExploreResultArgs args) {
        statusUI.CloseIfNotNull();
        Close();
        Complete(this, new EventArgs<ExploreResultArgs>(args));

        if (target) {
            MouseOverEffect.Disable(target);
        }
    }

    bool IsValidTarget(GameObject target) {
        if (!target) { return false; }

        var interactable = target.GetInterface<IInteractableSceneObject>() as MonoBehaviour;
        if (interactable != null) { return interactable.enabled; }

        if (target.GetComponent<_SceneSayPhrase>()) { return true; }
        if (target.name.Contains("Observer")) { return false; }
        if (requiredTag.IsEmptyOrNull()) { return true; }
        return target.CompareTag(requiredTag);
    }

    void Update() {
        GameObject newTarget = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.attachedRigidbody) {
                var rb = hit.collider.attachedRigidbody.gameObject;
                rb = ActorParent.Get(rb);
                if (IsValidTarget(rb)) {
                    newTarget = rb;
                }
            }
        } 

        if (target != newTarget) {
            if (target != null) MouseOverEffect.Disable(target);
            if (newTarget != null) MouseOverEffect.Enable(newTarget);
            target = newTarget;
        }
    }

}
