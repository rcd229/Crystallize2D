using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class JobTaskExitArgs {

    public bool IsFailed { get; set; }

    public JobTaskExitArgs(bool isFailed) {
        this.IsFailed = isFailed;
    }

}
