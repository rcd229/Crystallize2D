using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProficiencyPlayerData {

    public static int GetReviewExperienceForLevel(int level) {
        if (level == 1) {
            return 0;
        } else if (level == 2) {
            return 10;
        } else if (level == 3) {
            return 20;
        } else if (level == 4) {
            return 40;
        } else if (level == 5) {
            return 80;
        } else if (level == 6) {
            return 160;
        } else {
            return (level - 1).Squared() * 10;
        }
    }

    public static int GetReviewLevel(int xp) {
        int level = 0;
        while (xp >= 0) {
            level++;
            xp -= GetReviewExperienceForLevel(level + 1);
        }
        return level;
    }

    public static int GetNextLevelExperienceFromExperience(int xp) {
        return GetReviewExperienceForLevel(GetReviewLevel(xp) + 1);
    }

    public static int GetReviewLevelExperience(int xp) {
        int level = 0;
        while (xp >= 0) {
            level++;
            xp -= GetReviewExperienceForLevel(level + 1);
        }
        return xp + GetReviewExperienceForLevel(level + 1);
    }

    public int ReviewExperience { get; set; }
    public int Phrases { get; set; }
    public int Words { get; set; }

    public int Confidence { get; set; }
    public int ReserveConfidence { get; set; }

    public ProficiencyPlayerData() {
        Phrases = 0;
        Words = 3;

        Confidence = 10;
        ReserveConfidence = 0;
    }

    public int GetReviewLevel() {
        int level = 1;
        int xp = ReviewExperience;
        while (GetReviewExperienceForLevel(level + 1) <= xp) {
            level++;
            xp -= GetReviewExperienceForLevel(level);
        }
        return level;
    }

    public int GetReviewLevelExperience() {
        int level = 1;
        int xp = ReviewExperience;
        while (GetReviewExperienceForLevel(level + 1) <= xp) {
            level++;
            xp -= GetReviewExperienceForLevel(level);
        }
        return xp;
    }

    public void SetParametersForLevel() {
        var l = GetReviewLevel();
        if (l < 4) {
            Words = 2 + l;
        } else {
            Words = 5 + l / 4;
        }
        Phrases = 0 + (l + 2) / 4;
        
        var oldConfidence = PlayerData.Instance.Session.Confidence;
        Confidence = 8 + l * 2;
        if (oldConfidence > Confidence) {
            ReserveConfidence += oldConfidence - Confidence;
        }
    }

    public void RecalculateExperience() {
        int exp = 0;
        foreach (var r in PlayerData.Instance.Reviews.Reviews) {
            exp += r.Rank * r.Rank;
        }
        ReviewExperience = exp;
        SetParametersForLevel();
    }

}