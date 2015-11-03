using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ProgressUtil {

    public static int GetRandomIndexForTiers(int reps, int difficulty, int targetDifficulty, params int[] tiers) {
        if (difficulty == targetDifficulty) {
            var exact = GetExactIndexForTiers(reps, tiers);
            if (exact != -1) {
                return exact;
            }
            return UnityEngine.Random.Range(0, ProgressUtil.GetAmountForTiers(reps, tiers));
        } else {
            return UnityEngine.Random.Range(0, tiers.Length + 1);
        }
    }

    public static int GetExactIndexForTiers(int reps, params int[] tiers) {
        var index = 0;
        var count = 0;
        foreach (var t in tiers) {
            if (reps == count) {
                return index;
            }
            index++;
            count += t;
        }
        return -1;
    }

    public static int GetAmountForTiers(int count, params int[] tiers) {
        int index = 0;
        while (index < tiers.Length) {
            count -= tiers[index];
            if (count < 0) {
                break;
            }
            index++;
        }
        return index;
    }

    public static int GetRandomIndexForTiers_(int count, params int[] tiers) {
        int origCount = count;
        int index = 0;
        if (count < tiers[0]) {
            return 0;
        }

        while (index < tiers.Length) {
            count -= tiers[index];
            index++;
            if (count == 0) {
                Debug.Log("exact: " + index + " orig: " + origCount);
                return index;
            } else if (count < 0) {
                Debug.Log("tier: " + (index+1) + " orig: " + origCount);
                return UnityEngine.Random.Range(0, index);
            }
        }
        Debug.Log("reached max: " + index + "; ");
        return UnityEngine.Random.Range(0, index + 1);
    }

    public static int GetTargetCount(int repetitions) {
        var c = repetitions;//task.Job.PlayerDataInstance.Repetitions;
        if (c == 0) {
            return 3;
        } else if (c < 3) {
            return 4;
        } else if (c < 8) {
            return 5;
        } else {
            return 6;
        }
    }

    public static int GetExtraChoices(int repetitions) {
        var c = repetitions;
        if (c == 0) {
            return 0;
        } else if (c < 3) {
            return 1;
        } else if (c < 5) {
            return 2;
        } else if (c < 10) {
            return 3;
        } else if (c < 15) {
            return 4;
        } else {
            return 5;
        }
    }

}
