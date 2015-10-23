using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Linq;

public class QuestProgress : ISerializableDictionaryItem<QuestTypeID>, ISetableKey<QuestTypeID> {

    public QuestInstanceID InstanceID { get; set; }
    public QuestTypeID TypeID { get; set; }
    public string State { get; set; }
    public bool Viewed { get; set; }
    public bool Finished { get { return FinishedTimes > 0; } }
    public int FinishedTimes { get; set; }

    GeneratedQuestData _genData;
    public GeneratedQuestData GeneratedQuestData {
        get { return _genData; }
        set { _genData = value; }
    }

    public QuestTypeID Key {
        get {
            return TypeID;
        }
    }

    public QuestProgress() {
        State = "";
        Viewed = false;
        FinishedTimes = 0;
    }

    public void SetKey(QuestTypeID guid) {
        TypeID = guid;
    }

}

public class QuestPlayerData : GuidSerializableDictionary<QuestTypeID, QuestProgress> {

    public HashSet<Guid> Flags { get; private set; }

    public QuestPlayerData()
        : base() {
        Flags = new HashSet<Guid>();
    }

    //public bool AddFinishedQuest(Guid quest) {
    //    if (!QuestID.GetIDs().Where(s => s.Guid == quest).Any()) {
    //        Debug.LogError("Guid is not one of quest guids");
    //        return false;
    //    }

    //    if (FinishedQuest.Contains(quest)) {
    //        return false;
    //    } else {
    //        FinishedQuest.Add(quest);
    //        return true;
    //    }
    //}

    //public bool AddViewedQuest(Guid quest) {
    //    if (!QuestID.GetIDs().Where(s => s.Guid == quest).Any()) {
    //        Debug.LogError("Guid is not one of quest guids");
    //        return false;
    //    }

    //    if (ViewedQuest.Contains(quest)) {
    //        return false;
    //    } else {
    //        ViewedQuest.Add(quest);
    //        return true;
    //    }
    //}

    public bool AddFlag(Guid flag) {
        return Flags.Add(flag);
    }

    public bool RemoveFlag(Guid flag) {
        return Flags.Remove(flag);
    }
}
