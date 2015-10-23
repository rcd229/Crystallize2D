using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NightSessionArgs : TimeSessionArgs {

    public HomeRef Home { get; set; }

    public NightSessionArgs(string level, HomeRef home) : base(level) {
        this.Home = home;
    }

}
