using UnityEngine;
using System;
using System.Collections;

public class PersonalPlayerData {

    public string Name { get; private set; }
    public long TotalPlayTimeTicks { get; set; }
    public TimeSpan TotalPlayTime {
        get { return new TimeSpan(TotalPlayTimeTicks); }
        set { TotalPlayTimeTicks = value.Ticks; }
    }
    public DateTime StartPlayTime { get; set; }
    public int SurveysRequested { get; set; }

    public ContextData Context {
        get {
            var c = new ContextData();
            c.Set("name", new PhraseSequence(Name));
            return c;
        }
    }

    public PersonalPlayerData() {
        Name = "Player";
    }

    public void SetName(string name) {
        Name = name;
    }

}
