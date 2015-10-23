using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverhearAndPracticeProcess : IProcess<JobTaskRef, JobTaskExitArgs> { 

    public ProcessExitCallback OnExit { get; set; }

    JobTaskRef task;
    TalkToActorsInitArgs talkArgs;

    int count = 0;
    int targetCount = 0;
    List<GameObject> actors = new List<GameObject>();

    ITemporaryUI highlightBox;

    List<PhraseSequence> promotionPhrases = new List<PhraseSequence>();

    public void Initialize(JobTaskRef task) {
        this.task = task;
        if (task.Variation == PromotionTaskData.PromotionVariation) {
            InitializePromotion();
            blackout_Complete(null, null);
        } else {
            //TaskState.Instance.SetInstructions("Listen to the conversation.");
            TaskState.Instance.SetHidden(true);

            var person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
            StringMap actorMap;
            actors = DialogueActorUtil.GetActorsForTargets(task.Data.Dialogue.Actors, out actorMap);
            var convArgs = new ConversationArgs(person, task.Data.Dialogue);
            convArgs.ActorMap = actorMap;

            ProcessLibrary.Conversation.Get(convArgs, ConversationExit, this);
        }
    }

    void InitializePromotion() {
        var area = SceneAreaManager.Instance.Get(task.Data.AreaName); //GameObject.Find(task.Data.AreaName);
        var home = area.transform.Find(TagLibrary.Home);
        if (home != null) {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = home.position;
        } else {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = area.transform.position - Vector3.up;
        }

        foreach (var t in task.Job.GameDataInstance.Tasks) {
            if (t is LearnAndReviewTaskGameData) {
                promotionPhrases.Add(((LearnAndReviewTaskGameData)t).Phrase);
            }
        }
        promotionPhrases.Shuffle();
    }

    void ConversationExit(object sender, object obj) {
        var blackout = UILibrary.BlackScreen.Get(null);
        blackout.Complete += blackout_Complete;
    }

    void blackout_Complete(object sender, EventArgs<object> e) {
        actors.DestroyAndClear();

        if (task.Variation == PromotionTaskData.PromotionVariation) {
            targetCount = promotionPhrases.Count;
        } else {
            targetCount = ProgressUtil.GetTargetCount(task.Job.PlayerDataInstance.Days);
        }

        var sceneTargets = SceneAreaUtil.ScatterTargets(task.Data.AreaName, targetCount, default(Vector3));
        talkArgs = TalkToActorsInitArgs.GetConversation(sceneTargets.Count, GetConversationArgs, UpdateState);
        var area = SceneAreaManager.Instance.Get(task.Data.AreaName); // GameObject.Find(task.Data.AreaName);
        var home = area.transform.Find(TagLibrary.Home);
        if (home != null) {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = home.position;
        } else {
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = area.transform.position - Vector3.up;
        }
        DialogueActorUtil.GenerateActorsForTargets(sceneTargets);

        TaskState.Instance.SetHidden(false);
        TaskState.Instance.SetInstructions("Talk to all the people in the area.");
        TaskState.Instance.SetState("Conversations", string.Format("{0}/{1}", 0, targetCount));

        if (task.Variation == PromotionTaskData.PromotionVariation) {
            SessionTextExited(null, null);
        } else {
            CoroutineManager.Instance.WaitAndDo(() => {
                var text = UILibrary.ActivityText.Get("Practice time!");
                text.Complete += SessionTextExited;
            },
            new WaitForSeconds(0.5f));
        }
    }

    void SessionTextExited(object sender, EventArgs<object> e) {
        ProcessLibrary.TalkToActors.Get(talkArgs, TalkExit, this);
    }

    void TalkExit(object sender, object args) {
        count++;
        UpdateState(count);
        if (count >= targetCount) {
            Exit();
        } else {
            if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.StatusPanel)) {
                var messageBox = UILibrary.MessageBox.Get("Talk to all the people in the area to continue.");
                PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.StatusPanel);
                messageBox.Complete += messageBox_Complete;
            } else {
                SessionTextExited(null, null);
            }
        }
    }

    void messageBox_Complete(object sender, EventArgs<object> e) {
        PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.StatusPanel);
        var statusPanel = GameObject.Find(TagLibrary.TaskStatus);
        if (statusPanel) {
            highlightBox = UILibrary.HighlightBox.Get(new UITargetedMessageArgs(statusPanel.GetComponent<RectTransform>(), "See your progress here"));
            ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.LeftClick), TutorialComplete, this);
        } else {
            SessionTextExited(null, null);
        }
    }

    void TutorialComplete(object sender, object args) {
        highlightBox.Close();
        SessionTextExited(null, null);
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    PlayerConversationInitArgs GetConversationArgs(GameObject target) {
        if (task.IsPromotion) {
            return GetPromotionConversationArgs(target);
        } else {
            return GetNormalConversationArgs(target);
        }
    }

    PlayerConversationInitArgs GetNormalConversationArgs(GameObject target) {
        int c = count + ProgressUtil.GetExtraChoices(task.Job.PlayerDataInstance.Repetitions);
        var p = ((LearnAndReviewTaskGameData)task.Data).Phrase;// talkArgs.Phrases[UnityEngine.Random.Range(0, talkArgs.Phrases.Count)];
        if (UnityEngine.Random.value < GetAlternateProbability() && count > 1) {
            var allPhrases = DialogueMap.GetLearnedDialoguePhrases();
            if (allPhrases.Count > 0) {
                p = allPhrases[UnityEngine.Random.Range(0, allPhrases.Count)];
            }
        }
        var results = DialogueMap.GetMap().GetItemPrompt(p);

        //Debug.Log(p.GetText() + "; " + p.Translation);
        var d = new DialogueSequence(target.name, new PhraseSequence("NULL"));
        if (results.Count > 0) {
            d = results[UnityEngine.Random.Range(0, results.Count)];
        }
        var arg = new PlayerConversationInitArgs(target, p, d, c);
        arg.EmptySlots = 1 + ProgressUtil.GetAmountForTiers(task.Job.PlayerDataInstance.GetRepetitions(task.Index),
                                                      3, 5, 6, 6, 6, 6);
        return arg;
    }

    PlayerConversationInitArgs GetPromotionConversationArgs(GameObject target) {
        int c = count + 4;
        var p = promotionPhrases[count];
        var results = DialogueMap.GetMap().GetItemPrompt(p);
        var d = new DialogueSequence(target.name, new PhraseSequence("NULL"));
        if (results.Count > 0) {
            d = results[UnityEngine.Random.Range(0, results.Count)];
        }
        var args = new PlayerConversationInitArgs(target, p, d, c);
        args.EmptySlots = 100;
        return args;
    }

    public void UpdateState(int count) {
        PlayerDataConnector.AddRepetitionToJob(task.Job, task);
        TaskState.Instance.SetState("Conversations", string.Format("{0}/{1}", count, targetCount));
    }

    float GetAlternateProbability() {
        var amt = ProgressUtil.GetAmountForTiers(task.Job.PlayerDataInstance.GetRepetitions(task.Index), 2, 3, 3, 3);
        return (5f + amt) / 10f;
    }

}