using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PreviousJobRequirementGameData : JobRequirementGameData {

    public JobID JobID { get; set; }

    public PreviousJobRequirementGameData()
        : base() {
            JobID = new JobID(Guid.NewGuid());
    }

    public PreviousJobRequirementGameData(JobID jobID)
        : this() {
        JobID = jobID;
    }

    public override bool IsFulfilled() {
        var j = new IDJobRef(JobID);
        return j.PlayerDataInstance.Promoted;
    }

}
