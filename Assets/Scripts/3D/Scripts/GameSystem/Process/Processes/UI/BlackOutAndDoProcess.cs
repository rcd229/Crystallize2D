using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class BlackOutAndDoProcess : IProcess<IProcessGetter, object> {

    public ProcessExitCallback OnExit { get; set; }

    IProcessGetter getter;

    public void Initialize(IProcessGetter param1) {
        getter = param1;
        //Debug.Log("Blacking out: " + Time.time);
        BlackScreenUI.Instance.FadeOut(0.25f, FadeOutCallback);
    }

    public void ForceExit() {
        Exit();
    }

    void FadeOutCallback(object sender, object obj) {
        //Debug.Log("fade out finished.");
        CoroutineManager.Instance.WaitAndDo(
            () => getter.Get(SubProcessCallback, this),
            new WaitForSeconds(1f));
    }

    void SubProcessCallback(object sender, object obj) {
        //Debug.Log("sub process finished.");
        BlackScreenUI.Instance.FadeIn(0.25f, FadeInCallback);
    }

    void FadeInCallback(object sender, object obj) {
        //Debug.Log("exiting");
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
