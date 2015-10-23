using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GreeterProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public const string Hello = "hello";
    public const string GoodMorning = "good morning";
    public const string GoodEvening = "good evening";
    public const string Huh = "what?";

    public ProcessExitCallback OnExit { get; set; }

    JobTaskRef task;
    IJobRef job;
    int correct = 0;
    int times = 5;
    ITemporaryUI<TimeSpan,object> clockUI;
    TimeSpan time;
    PhraseSequence targetPhrase;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        job = param1.Job;
        clockUI = UILibrary.Clock.Get(new TimeSpan());
        RandomizeTime();

        ProcessLibrary.MessageBox.Get("Choose the correct greeting based on the time.", MessageBoxCallback, this);
    }

    public void ForceExit() {
        Exit();
    }

    void MessageBoxCallback(object sender, object args){
        var convArgs = new ConversationArgs(task.Data.Actor.GetTarget(), null);
        convArgs.PlayImmediately = false;
        ProcessLibrary.BeginConversation.Get(convArgs, ConversationStarted, this);
    }

    void ConversationStarted(object sender, object state) {
        ProcessLibrary.ClockTutorial.Get("", OnTutorialExit, this);
    }

    void OnTutorialExit(object sender, object args) {
        var p = new PhraseSequence("?");
        var line = new DialogueActorLine();
        line.Phrase = p;
        var d = new DialogueSequence();
        var lineEle = new LineDialogueElement();
        lineEle.Line = line;
        d.Actors.Add(new SceneObjectGameData("Customer"));
        d.AddNewDialogueElement(lineEle);

        var convArgs = new ConversationArgs(task.Data.Actor.GetTarget(), d);
        convArgs.PlayImmediately = true;
        ProcessLibrary.ConversationSegment.Get(convArgs, ConversationContinued, this);
    }

    void ConversationContinued(object sender, object args) {
        var choices = new List<PhraseSequence>(new PhraseSequence[]{
            job.GameDataInstance.Lines.Get(Hello).Phrase,
            job.GameDataInstance.Lines.Get(GoodMorning).Phrase,
            job.GameDataInstance.Lines.Get(GoodEvening).Phrase,
            new PhraseSequence("?")
        }
        );
        var ui = UILibrary.PhraseSelector.Get(new PhraseSelectorInitArgs(choices));
        ui.Complete += ui_Complete;
    }

    void ui_Complete(object sender, EventArgs<PhraseSequence> e) {
        if (PhraseSequence.PhrasesEquivalent(e.Data, targetPhrase)) {
            var p = UILibrary.PositiveFeedback.Get("");
            correct++;
            p.Complete += FeedbackExit;
        } else {
            var p = UILibrary.NegativeFeedback.Get("");
            p.Complete += FeedbackExit;
        }
    }

    void FeedbackExit(object obj, EventArgs<object> args) {
        times--;
        if (times <= 0) {
            int money = (int)(2000 * correct * PlayerData.Instance.Session.RestQuality);
            PlayerDataConnector.AddMoney(money);
            string moneyString = string.Format("You made {0} yen today.", money);
            var ui = UILibrary.MessageBox.Get(moneyString);
            ui.Complete += HandleFinished;
        } else {
            RandomizeTime();
            ConversationStarted(this, null);
        }
    }

    void HandleFinished(object sender, EventArgs<object> obj) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    void RandomizeTime() {
        var hours = UnityEngine.Random.Range(0, 24);
        if (hours > 18) {
            hours = UnityEngine.Random.Range(0, 24);
        }
        time = new TimeSpan(hours, UnityEngine.Random.Range(0, 60), 0);
        clockUI.Initialize(time);

        targetPhrase = job.GameDataInstance.Lines.Get(GetPhraseForTime(time)).Phrase;
    }

    string GetPhraseForTime(TimeSpan time) {
        if (time.Hours < 11) {
            return GoodMorning;
        } else if (time.Hours < 18) {
            return Hello;
        } else {
            return GoodEvening;
        }
    }

}