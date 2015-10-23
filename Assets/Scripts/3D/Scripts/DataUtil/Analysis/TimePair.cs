using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimePair {

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int numberCorrect { get; set; }
    public int numberIncorrect { get; set; }

    public TimePair(DateTime t1, DateTime t2)
    {
        Start = t1;
        End = t2;
        numberCorrect = 0;
        numberIncorrect = 0;
    }

    public Boolean WithInRange(DateTime t1)
    {
        if (t1 > Start && t1 < End)
        {
            return true;
        }
        return false;
    }

    public Boolean shareTime(TimePair t1)
    {
        if (t1.Start < Start && t1.End > Start)
        {
            return true;
        }
        if (t1.Start < End && t1.End > End)
        {
            return true;
        }
        return false;
    }
}
