using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TimeSessionArgs {

    public string LevelName { get; set; }

    public TimeSessionArgs(string levelName) {
        this.LevelName = levelName;
    }

}
