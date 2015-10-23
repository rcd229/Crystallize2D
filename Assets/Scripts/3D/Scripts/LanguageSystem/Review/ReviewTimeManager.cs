using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewTimeManager {

    public static TimeSpan offset = new TimeSpan();

    public static DateTime GetTime() {
        return DateTime.Now + offset;
    }

    public static void AddTime(int days, int hours, int minutes) {
        offset += new TimeSpan(days, hours, minutes, 0);
    }

}
