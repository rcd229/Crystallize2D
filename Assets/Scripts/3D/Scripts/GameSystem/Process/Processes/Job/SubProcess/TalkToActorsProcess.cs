using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TalkToActorsInitArgs {
    public static TalkToActorsInitArgs GetYesNo(Func<GameObject, PlayerConversationInitArgs> getArgs) {
        return new TalkToActorsInitArgs(1, getArgs, ProcessLibrary.YesNoConversation);
    }

    public static TalkToActorsInitArgs GetConversation(int repeatCount, Func<GameObject, PlayerConversationInitArgs> getArgs, Action<int> updateCompleted) {
        return new TalkToActorsInitArgs(repeatCount, getArgs, ProcessLibrary.PlayerConversation);
    }

    public int RepeatCount { get; set; }
    public Func<GameObject, PlayerConversationInitArgs> GetConversationArgs { get; set; }
    public ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs> InteractionFactory { get; set; }

    public TalkToActorsInitArgs(int repeatCount, Func<GameObject, PlayerConversationInitArgs> getArgs, ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs> interactionFactory) {
        RepeatCount = repeatCount;
        InteractionFactory = interactionFactory;
        GetConversationArgs = getArgs;
    }

}

public class TalkToActorExitArgs {
    public PlayerConversationExitArgs PlayerConversationArgs { get; set; }

    public TalkToActorExitArgs(PlayerConversationExitArgs convExitArgs) {
        PlayerConversationArgs = convExitArgs;
    }
}

public class TalkToActorsProcess : IProcess<TalkToActorsInitArgs, TalkToActorExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

    TalkToActorsInitArgs args;
    ITemporaryUI messageUI;
    List<GameObject> tutorialInstances;

    List<GameObject> disabledForMovementTutorial = new List<GameObject>();

    public void Initialize(TalkToActorsInitArgs param1) {
        args = param1;

        new EquipmentItemRef(TagLibrary.Greet).SetTo(PlayerManager.Instance.PlayerGameObject);

        BeginMovement();
        //if (!PlayerData.Instance.Tutorial.GetTutorialViewed("Camera")) {
        //    DoCameraTutorial();
        //} else {
            
        //}
    }

    void BeginMovement() {
        BeginExplore();

        //ProcessLibrary.MovementTutorial.Get(null, movement_Complete, this);
    }

    void DisableExtraUIStuff() {
        var collect = GameObject.FindObjectOfType<CollectUI>();
        if (collect) {
            collect.gameObject.SetActive(false);
            disabledForMovementTutorial.Add(collect.gameObject);
        }
        var status = GameObject.Find(TagLibrary.TaskStatus);
        if (status) {
            status.gameObject.SetActive(false);
            disabledForMovementTutorial.Add(status.gameObject);
        }
    }

    // TODO: put this in the tutorial system
    void movement_Complete(object sender, object e) {
        foreach (var g in disabledForMovementTutorial) {
            g.SetActive(true);
        }
        InitializeMessages();
    }

    public void ForceExit() {
        Exit(null);
    }

    void InitializeMessages() {
        // TODO: move tutorial out
        if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.People)
            && args.RepeatCount > 1) {
            tutorialInstances = new List<GameObject>();
            var prefab = Resources.Load<GameObject>("Tutorial/DownArrow");
            foreach (var p in GameObject.FindGameObjectsWithTag(TagLibrary.Actor)) {
                var instance = GameObject.Instantiate<GameObject>(prefab);
                instance.transform.position = p.transform.position + 2.5f * Vector3.up;
                instance.transform.parent = p.transform;
                tutorialInstances.Add(instance);
            }
        }
        // end tutorial

        messageUI = UILibrary.Message.Get("-Click on people to talk to them.");
        messageUI.CloseOnPlayerMove();
    }

    void BeginExplore() {
        ProcessLibrary.Explore.Get(ExploreInitArgs.ActorArgs(), ExploreCallback, this);
    }

    void ExploreCallback(object sender, ExploreResultArgs args) {
        // TODO: move tutorial out
        if (tutorialInstances != null) {
            PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.People);
            foreach (var i in tutorialInstances) {
                GameObject.Destroy(i);
            }
            tutorialInstances = null;
        }
        // end tutorial
        messageUI.CloseIfNotNull();

        if (args.Target) {
            CoroutineManager.Instance.StartCoroutine(AnimationAndContinue(args.Target));
        }
    }

    // TODO: animation subprocess
    IEnumerator AnimationAndContinue(GameObject target) {
        var rb = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
        var f = rb.transform.position - target.transform.position;
        f.y = 0;
        rb.transform.forward = -f;
        rb.velocity = Vector3.zero;
        rb.GetComponentInChildren<Animator>().CrossFade(TagLibrary.Bow, 0.1f);

        target.transform.forward = f;

        yield return new WaitForSeconds(1f);

        target.GetComponentInChildren<Animator>().CrossFade(TagLibrary.Bow, 0.1f);

        yield return new WaitForSeconds(1f);

        StartConversation(target);
    }

    void StartConversation(GameObject target) {
        var cArgs = args.GetConversationArgs(target);
        //Debug.Log("talk to act got: " + cArgs.IsCorrect);
        args.InteractionFactory.Get(cArgs, EndConversationCallback, this);
        //ProcessLibrary.PlayerConversation.Get(GetArgs(target), EndConversationCallback, this);
    }

    void EndConversationCallback(object sender, PlayerConversationExitArgs e) {
        DataLogger.LogTimestampedData("PersonTalkedTo");
        if (e.IsCorrect) {
            Exit(e);
        } else {
            BeginExplore();
        }
    }

    void Exit(PlayerConversationExitArgs exitArgs) {
        OnExit.Raise(this, new TalkToActorExitArgs(exitArgs));
    }



}