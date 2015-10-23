using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MorningSessionArgs : TimeSessionArgs {

    public HomeRef Home { get; set; }

    public MorningSessionArgs(string level, HomeRef home) : base(level) {
        Home = home;
    }

}
