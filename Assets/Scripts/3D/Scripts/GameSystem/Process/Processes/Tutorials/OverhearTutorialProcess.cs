using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class OverhearTutorialProcess : ExploreProcess {

    ITemporaryUI contextStatus; 

    public override void Initialize(ExploreInitArgs args) {
        base.Initialize(args);
        contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click the people to listen", true, false));
    }

    protected override void BeforeExit() {
        contextStatus.CloseIfNotNull();
    }

}
