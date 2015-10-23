using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobCollectionPlayerData : SerializableDictionary<JobID, JobPlayerData> {

    public JobPlayerData GetOrCreateItem(JobID id) {
        if (ContainsKey(id)) {
            return Get(id);
        } else {
            var i = new JobPlayerData(id);
            Add(i);
            return i;
        }
    }

}
