using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GreeterProcess01 : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string Hello = "hello";
    public const string GoodMorning = "good morning";
    public const string GoodEvening = "good evening";
    public const string Huh = "what?";
    public const string AreYouAStudent = "are you a student?";

    public static ProcessFactoryRef<string, object> ClockTutorial = new ProcessFactoryRef<string, object>();

    public ProcessExitCallback OnExit { get; set; }

    JobTaskRef taskRef;
    GameObject actor;
    PhraseSequence targetPhrase;
    PhraseConstructorArgs phraseConstructorArgs;

    int count = 0;

    public void Initialize(JobTaskRef param1) {
        int count = 0;
        foreach (var s in StringExtensions.GetCountSet("Target", 1, 5, 2)) {
            var t = GameObject.Find(s);
            var actor = DialogueActorUtil.GetNewActor(AppearanceLibrary.GetRandomAppearance());
            actor.transform.position = t.transform.position;
            actor.transform.rotation = t.transform.rotation;
            actor.name = "Actor" + count;
            count++;
        }
        taskRef = param1;

        new EquipmentItemRef("Greet").SetTo(PlayerManager.Instance.PlayerGameObject);
        BeginExplore();
    }

    public void ForceExit() {
        Exit();
    }

    void BeginExplore() {
        ProcessLibrary.Explore.Get(ExploreInitArgs.ActorArgs(), ExploreCallback, this);
     }

    void ExploreCallback(object sender, ExploreResultArgs args) {
        if(args.Target){
            CoroutineManager.Instance.StartCoroutine(AnimationAndContinue(args.Target));
        }
    }

    IEnumerator AnimationAndContinue(GameObject target) {
        var rb = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
        var f = rb.transform.position - target.transform.position;
        f.y = 0;
        rb.transform.forward = -f;
        rb.velocity = Vector3.zero;
        rb.GetComponentInChildren<Animator>().CrossFade("Bow", 0.1f);

        target.transform.forward = f;

        yield return new WaitForSeconds(1f);

        target.GetComponentInChildren<Animator>().CrossFade("Bow", 0.1f);

        yield return new WaitForSeconds(1f);

        StartConversation(target);
    }

    void StartConversation(GameObject target) {
        ProcessLibrary.PlayerConversation.Get(GetArgs(target), EndConversationCallback, this);
    }

    void EndConversationCallback(object sender, PlayerConversationExitArgs args) {
        count++;
        if (count < 5) {
            BeginExplore();
        } else {
            Exit();
        }
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    PlayerConversationInitArgs GetArgs(GameObject target) {
        int c = 3 + count;
        var d = new DialogueSequence(target.name, new PhraseSequence("Hello there"));
        if (count > 0) {
            d = new DialogueSequence(target.name, new PhraseSequence("yes I am"));
        }

        var p = taskRef.Job.GetPhrase(Hello);
        if(count > 0){
            p =  PhraseSetCollectionGameData.Default.GetPhrase(AreYouAStudent);
        }

        return new PlayerConversationInitArgs(target, p, d, c);
    }

}