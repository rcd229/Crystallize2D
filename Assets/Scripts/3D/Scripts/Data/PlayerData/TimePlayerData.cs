using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TimePlayerData {

    public static string GetSessionString(int session) {
        if (session == (int)TimeSessionType.Day) {
            return "day time";
        }
        
        if(Enum.IsDefined(typeof(TimeSessionType), session)){
            return ((TimeSessionType)session).ToString().ToLower();
        }
        return "UNDEFINED";
    }

    public int Day { get; set; }
    public int Session { get; set; }

    public TimePlayerData() { }

    public string GetFormattedString() {
        return string.Format("Day {0}: {1}", Day + 1, GetSessionString(Session));
    }

}
