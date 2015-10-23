using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneJobProcess : IProcess<IJobRef, object> {

    public ProcessExitCallback OnExit { get; set; }

    IJobRef job;

    public void Initialize(IJobRef job) {
        this.job = job;

        //var words = job.GetRemainingNeededWords();
        //var unlocked = job.PlayerDataInstance.Unlocked || job.GameDataInstance.QuestID == null;
        //if (unlocked) {
            var phrases = new List<PhraseSequence>();
            phrases.Add(new PhraseSequence("Yes"));
            phrases.Add(new PhraseSequence("No"));
            var args = new PhraseSelectorInitArgs(phrases, "Do this job?", false);
            args.UseTranslation = true;
            args.Center = true;
            UILibrary.PhraseSelector.Get(args, HandleJobConfirmed, this);
        //} else {
        //    ProcessLibrary.QuestConversation.Get(new QuestArgs(gameObject, NPC), callback, parent);
        //    //UILibrary.NeededWords.Get(words, HandleNeedWordsExit, this);
        //}
    }

    public void ForceExit() {
        
    }

    void HandleNeedWordsExit(object sender, EventArgs args) {
        Debug.Log("Needed words exit");
        Exit();
    }

    void HandleJobConfirmed(object sender, EventArgs<PhraseSequence> data) {
        if (data.Data.GetText().ToLower().Trim() == "yes") {
            // TODO: start job process
            ProcessLibrary.Job.Get(new DaySessionArgs("", job), HandleJobFinished, this);
        } else {
            Exit();
        }
    }

    void HandleJobFinished(object sender, object args) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
