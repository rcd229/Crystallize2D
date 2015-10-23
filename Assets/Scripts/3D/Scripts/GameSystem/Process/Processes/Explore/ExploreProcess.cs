using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ExploreProcess : IProcess<ExploreInitArgs, ExploreResultArgs> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI playerController;
    //ITemporaryUI tutorial;
    ITemporaryUI<ExploreInitArgs, ExploreResultArgs> contextController;

    public virtual void Initialize(ExploreInitArgs args) {
        BeginMovement(args);
        BeginAction(args);
    }

    protected virtual void BeforeExit() {    }

    protected void BeginMovement(ExploreInitArgs args) {
        playerController = UILibrary.PlayerController.Get(args);
    }

    protected void BeginAction(ExploreInitArgs args) {
        contextController = UILibrary.ContextActionController.Get(args);
        contextController.Complete += contextController_Complete;
    }

    void contextController_Complete(object sender, EventArgs<ExploreResultArgs> e) {
        Exit(e.Data);
    }

    public void ForceExit() {
        playerController.CloseIfNotNull();
    }

    void Exit(ExploreResultArgs args) {
        BeforeExit();
        playerController.Close();
        OnExit.Raise(this, args);
    }

}
