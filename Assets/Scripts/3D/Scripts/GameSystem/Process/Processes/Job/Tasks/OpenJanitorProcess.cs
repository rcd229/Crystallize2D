using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class OpenJanitorProcess : JanitorProcess {

    const string JanitorBossName = "JanitorBoss";

    public override GameObject GetActor() {
        return GameObject.Find(JanitorBossName);
    }

    public override int GetTotalTaskCount() {
        return 3;
    }

}