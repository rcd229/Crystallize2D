using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum JobFormalLevel {
    Formal, Versatile
}

public class JobGameData : ISerializableDictionaryItem<JobID>, ISetableKey<JobID> {

    public JobID ID { get; set; }

    public string Name { get; set; }
    public string PhraseSetName { get; set; }
    public int Ordering { get; set; }
    public int Difficulty { get; set; }
    public int Money { get; set; }
    public int Tries { get; set; }
    public string HelpText { get; set; }
    public JobTaskSelectorGameData TaskSelector { get; set; }
    public List<PhraseSequence> AvailableWords { get; set; }
    public List<JobTaskGameData> Tasks { get; set; }
    public List<JobRequirementGameData> Requirements { get; set; }
    public PhraseMapGameData Lines { get; set; }
    public JobFormalLevel formalLevel { get; set; }
    public int PromotionReq { get; set; }
    public JobTaskGameData PromotionTask { get; set; }
    public int PromotionMistakes { get; set; }
    public QuestTypeID QuestID { get; set; }

    public bool Hide { get; set; }

    public JobID Key {
        get { return ID; }
    }

    public JobGameData() {
        ID = new JobID(Guid.NewGuid());
        Name = "";
        PhraseSetName = "";
        Difficulty = 0;
        Tries = 5;
        HelpText = "You need to learn the required words first.";
        AvailableWords = new List<PhraseSequence>();
        Tasks = new List<JobTaskGameData>();
        Requirements = new List<JobRequirementGameData>();
        TaskSelector = new JobTaskSelectorGameData();
        Lines = new PhraseMapGameData();
        formalLevel = JobFormalLevel.Versatile;
        PromotionReq = 1000;
        PromotionTask = new JobTaskGameData();
    }

    public void SetKey(JobID key) {
        ID = key;
    }

    public void AddRequirement(JobID jobID) {
        Requirements.Add(new PreviousJobRequirementGameData(jobID));
    }

    public void AddRequirement(PhraseSequence phrase) {
        Requirements.Add(new PhraseJobRequirementGameData(phrase));
    }

    public bool RequirementsFullfilled() {
        foreach (var r in Requirements) {
            if (!r.IsFulfilled()) {
                return false;
            }
        }
        return true;
    }

    public IEnumerable<PhraseJobRequirementGameData> GetRequirements() {
        return from r in Requirements
               where r is PhraseJobRequirementGameData
               select (PhraseJobRequirementGameData)r;
    }

    public IEnumerable<PhraseJobRequirementGameData> GetPhraseRequirements() {
        return from r in Requirements
               where r is PhraseJobRequirementGameData
               select (PhraseJobRequirementGameData)r;
    }

    public IEnumerable<PreviousJobRequirementGameData> GetJobRequirements() {
        return from r in Requirements
               where r is PreviousJobRequirementGameData
               select (PreviousJobRequirementGameData)r;
    }

    public QuestNPCItemData GetNPC() {
        if (QuestID == null) return null;
        var q = new QuestRef(QuestID);
        if (!(q.GameDataInstance.StateMachine is JobUnlockQuest)) return null;
        return ((JobUnlockQuest)q.GameDataInstance.StateMachine).GetNPC();
    }

}