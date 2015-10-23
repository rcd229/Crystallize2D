using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobPlayerData : ISerializableDictionaryItem<JobID>, ISetableKey<JobID> {

    public JobID JobID { get; set; }
    public bool Shown { get; set; }
    public bool Unlocked {get; set;}
	public bool Promoted {get;set;}
    public float ExperiencePoints { get; set; }
    public int Days { get; set; }
    public List<TaskEntryPlayerData> Tasks { get; set; }

    public JobID Key {
        get { return JobID; }
    }

    public int Repetitions {
        get {
            return Tasks.Count;
        }
    }

    public JobPlayerData() {
        JobID = new JobID(Guid.NewGuid());
        Shown = false;
        Unlocked = false;
        ExperiencePoints = 0;
        Tasks = new List<TaskEntryPlayerData>();
    }

    public JobPlayerData(JobID jobID) : this() {
        JobID = jobID;
    }

    public void AddTask(JobTaskRef task) {
        var i = task.Index;
		if(i == PromotionTaskData.PromotionIndex){
			Debug.Log("logging a promotion task");
			Tasks.Add(new TaskEntryPlayerData(i));
		}
        else if (i != -1) {
            Tasks.Add(new TaskEntryPlayerData(i));
        }else {
            Debug.LogError("JobGameData must contain task.");
        }
    }

    public int UniqueViewedTaskCount() {
        var h = new HashSet<int>();
        if (Tasks == null) {
            Debug.Log(JobID);
        }
        foreach (var t in Tasks) {
            if (!h.Contains(t.TaskID)) {
                h.Add(t.TaskID);
            }
        }
        return h.Count;
    }

    public bool ViewedTask(int taskID) {
        foreach (var t in Tasks) {
            if (t.TaskID == taskID) {
                return true;
            }
        }
        return false;
    }

    public bool ViewedAllTasks(IJobRef job) {
        return UniqueViewedTaskCount() >= job.GameDataInstance.Tasks.Count;
    }

    public int GetRepetitions(int taskID) {
        int count = 0;
        foreach (var rep in Tasks) {
            if (rep.TaskID == taskID) {
                count++;
            }
        }
        return count;
    }

    public void SetKey(JobID key) {
        JobID = key;
    }

}
