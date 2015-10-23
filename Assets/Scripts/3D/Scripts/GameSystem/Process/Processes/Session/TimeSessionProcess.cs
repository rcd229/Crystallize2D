using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class TimeSessionProcess {
    public static readonly ProcessFactoryRef<string, object> TransitionFactory = new ProcessFactoryRef<string,object>();
}

public abstract class TimeSessionProcess<I, O> : IProcess<I, O> where I : TimeSessionArgs {

    public ProcessExitCallback OnExit { get; set; }

    protected abstract string TimeName {
        get;
    }

    string nextLevel = "";

    public virtual void Initialize(I input) {
        BeforeInitialize(input);
        Run(input);
    }

    public virtual void ForceExit() {
        //Exit();
    }

    protected void Run(I args) {
        nextLevel = SelectNextLevel(args);
        var s = "";
        if (!(this is DayProcess)) {
            s = PlayerData.Instance.Time.GetFormattedString();
        } 
        TimeSessionProcess.TransitionFactory.Get(s, TransitionCompleteCallback, this);
    }

    protected virtual string SelectNextLevel(I args) {
        return args.LevelName;
    }

    protected virtual void BeforeInitialize(I input) { }

    protected abstract void AfterLoad();

    protected void Exit() {
        Exit(default(O));
    }

    protected void Exit(O args) {
        CrystallizeEventManager.OnLoadComplete -= HandleLoadComplete;
        OnExit(this, args);
    }

    void SetAmbient(string timeName) {
        var amb = GameObject.Find(timeName);
        if (amb) {
            foreach (Transform t in amb.transform) {
                t.gameObject.SetActive(true);
            }
        }
    }

    void TransitionCompleteCallback(object sender, object args) {
        CrystallizeEventManager.OnLoadComplete += HandleLoadComplete;
        Application.LoadLevel(nextLevel);
    }

    void HandleLoadComplete(object sender, System.EventArgs args) {
        CrystallizeEventManager.OnLoadComplete -= HandleLoadComplete;
        SetAmbient(TimeName);
        BlackScreenUI.GetInstance().FadeIn(1f, null);
        AfterLoad();
    }

}
