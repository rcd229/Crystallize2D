using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public static class ItemReview {
    public static TimeSpan GetIntervalForRank(int rank) {
        if (rank == 0) {
            return new TimeSpan(0, 0, 1);
        } else if (rank == 1) {
            return new TimeSpan(0, 5, 0);
        } else if (rank == 2) {
            return new TimeSpan(0, 10, 0);
        } else {
            return new TimeSpan(0, 20 * (rank - 2).Squared(), 0, 0);
        }
    }

    public static int GetLevelForRank(int rank) {
        if (rank < 3) {
            return 0;
        } else if (rank < 5) {
            return 1;
        } else {
            return 2;
        }
    }
}

public class ItemReviewPlayerData<T> {

    [XmlElement("Phrase")]
    public T Item { get; set; }
    public int Rank { get; set; }
    public List<ReviewEntryPlayerData> Entries { get; set; }

    public ItemReviewPlayerData() {
        Item = default(T);
        Rank = 0;
        Entries = new List<ReviewEntryPlayerData>();
    }

    public bool NeedsReview(){
        if (Entries.Count == 0) {
            return true;
        }
        return (Entries.Last().Time + ItemReview.GetIntervalForRank(Rank)) <= ReviewTimeManager.GetTime();
    }

    public void AddEntry(int result) {
        Entries.Add(new ReviewEntryPlayerData(result));
        if (result == 0) {
            Rank = 0;
        } else if (result == 1) {
            Rank++;
        } else if (result == 2) {
            Rank += 2;
        }
        //Debug.Log(Item.GetText() + ": rank " + Rank);
    }

    public int GetLevel() {
        return ItemReview.GetLevelForRank(Rank);
    }

    public virtual string GetText() {
        return Item.ToString();
    }

}
