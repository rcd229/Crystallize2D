using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewEntryPlayerData {

    public long TimeTicks { get; set; }
    public int Result { get; set; }

    public DateTime Time {
        get {
            return new DateTime(TimeTicks);
        }
    }

    public ReviewEntryPlayerData() {
        TimeTicks = System.DateTime.Now.Ticks;
        Result = 0;
    }

    public ReviewEntryPlayerData(int result) : this() {
        Result = result;
    }

}
