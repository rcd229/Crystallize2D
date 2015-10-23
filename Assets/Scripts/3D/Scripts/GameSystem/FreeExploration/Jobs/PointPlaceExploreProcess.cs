using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PointPlaceExploreProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

    GameObject target;
    JobTaskRef task;
    PointPlaceTaskData taskData;

    //learning point is question
    IEnumerable<QATaskGameData.QALine> qa;
    QATaskGameData.QALine currentQA;
    LinkedList<QATaskGameData.QALine> availableQuestions;
    List<GameObject> StreetGOs = new List<GameObject>();

    int variation;

    const string tagToUse = "place_to_point";
    const string targetName = "AskerTarget";

    public void Initialize(JobTaskRef param1) {
        task = param1;
        taskData = (PointPlaceTaskData)(task.Data);
        variation = task.IsPromotion ? 10 : task.Variation;
        target = GameObject.Find(param1.Data.Actor.Name);
        //find and put environment targets that contains the phrase
        foreach (var streetTarget in taskData.StreetTarget) {
            GameObject st = GameObject.Find(streetTarget);
            var instance = PropType.GetPropForTarget(PropType.ExclaimationMark, st.transform);
            st.tag = tagToUse;
            var env_phrase = st.GetComponentInChildren<EnvironmentPhrase>();
            env_phrase.phrase = new ScenePhraseSequence();
            env_phrase.phrase.Set(task.Job.GetPhrase(streetTarget));
            StreetGOs.Add(instance);
        }
        //TODO temporary. Disable the region restriction
        //		var area = GameObject.FindObjectOfType<SceneAreaSettings>();
        //		if (area != null){
        //			area.gameObject.SetActive(false);
        //		}

        //		SceneAreaManager.Instance.Get(TagLibrary.Area03);
        //		GameObject.Find("Targets").transform.Find("Area01").gameObject.SetActive(false);
        FollowPlayer follow = target.GetComponent<FollowPlayer>();
        if (follow) {
            follow.enabled = true;
        } else {
            target.AddComponent<FollowPlayer>();
        }
        //		target = new SceneObjectRef(taskData.Actor).GetSceneObject();
        //		remainingCount = GetTaskCount ();

        availableQuestions = new LinkedList<QATaskGameData.QALine>();
        var qas = taskData.GetQAs().ToArray();
        for (int i = 0; i < qas.Length; i++) {
            if (i > variation + 1)
                break;
            availableQuestions.AddLast(qas[i]);
        }
        qa = taskData.GetQAs();
        StartTask(null, null);
    }

    void StartTask(object obj, object arg) {
        //start by giving a random query
        getNewQuery();
        StartQuestion();
    }

    void StartQuestion() {
        StartQuestion(null, null);
    }
    void StartQuestion(object obj, object e) {
        ProcessLibrary.BeginConversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext(), true), HandleEndAskingQuestion, this);
    }

    void HandleEndAskingQuestion(object a, object b) {
        ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(target, taskData.Dialogue, false), HandleQuestionExit, this);
    }

    void HandleQuestionExit(object sender, object arg) {
        ProcessLibrary.Explore.Get(new ExploreInitArgs(null, tagToUse, "point to this place", "not a valid place"), HandleAnswerFeedBack, this);
    }

    void HandleAnswerFeedBack(object sender, ExploreResultArgs e) {
        var text = e.Target.GetComponentInChildren<EnvironmentPhrase>().phrase.Get();
        if (PhraseSequence.PhrasesEquivalent(text, currentQA.Answer)) {
            var ui = UILibrary.PositiveFeedback.Get("");
            ui.Complete += HandleFeedBackComplete;
        } else {
            var ui = UILibrary.NegativeFeedback.Get("");
            ui.Complete += HandleFeedBackComplete;
        }
    }

    void HandleFeedBackComplete(object sender, EventArgs<object> arg) {
        target.GetComponentInSelfOrChild<DialogueActor>().SetPhrase(null);
        Exit();
    }

    void getNewQuery() {
        if (availableQuestions.Count == 0) {
            currentQA = qa.ToArray()[UnityEngine.Random.Range(0, Math.Min(qa.Count(), variation + 2))];
        } else {
            currentQA = availableQuestions.ElementAt(UnityEngine.Random.Range(0, availableQuestions.Count));
            availableQuestions.Remove(currentQA);
        }
    }

    ContextData getNewContext() {
        ContextData c = new ContextData();
        c.Set("place", currentQA.Question);
        return c;
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        Debug.Log("Guide Job Exited");
        foreach (var go in StreetGOs) {
            GameObject.Destroy(go);
        }
        OnExit.Raise(this, null);
    }
}
