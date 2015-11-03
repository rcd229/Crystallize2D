using UnityEngine;
using System.Collections;

[System.Serializable]
public class PartnerObjectiveCompleteEventArgs : System.EventArgs {

    public int QuestID { get; set; }

    public PartnerObjectiveCompleteEventArgs(int questID) {
        QuestID = questID;
    }

}
