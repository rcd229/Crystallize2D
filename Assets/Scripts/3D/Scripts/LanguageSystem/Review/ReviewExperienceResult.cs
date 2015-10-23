using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewExperienceArgs : System.EventArgs {

    public int ExperienceGained { get; set; }
    public bool LeveledUp { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentLevelExperience { get; set; }
    public int NextLevelExperience { get; set; }

    public ReviewExperienceArgs(int xp, int currentLevel, bool lvlUp, int cxp, int nxp) {
        ExperienceGained = xp;
        LeveledUp = lvlUp;
        CurrentLevel = currentLevel;
        CurrentLevelExperience = cxp;
        NextLevelExperience = nxp;  
    }

    public string ToMessageBoxString() {
        var s = string.Format("You gained {0} review experience!", ExperienceGained);
        if(LeveledUp){
            s += string.Format("\n\nLevel up!!! Reached review Lvl.{0}.\n", CurrentLevel);
        }
        s += string.Format("\n{0}/{1} xp to review Lvl.{2}.", CurrentLevelExperience, NextLevelExperience, CurrentLevel + 1);
        return s;
    }

}
