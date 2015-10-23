using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public enum JobTaskType{
	Normal,
	Promotion
}

public class JobTaskRef  {

    public IJobRef Job { get; private set; }
    public JobTaskGameData Data { get; private set; }
    public int Variation { get; private set; }
	public JobTaskType JobTaskType {get; private set;}

    public int Index {
        get {
			if(JobTaskType == JobTaskType.Normal){
            	return Job.GameDataInstance.Tasks.IndexOf(Data);
			}
			else{
				return PromotionTaskData.PromotionIndex;
			}
        }
    }

    public bool IsPromotion {
        get {
            return Variation == PromotionTaskData.PromotionVariation;
        }
    }

    public bool IsPromotionFailed {
        get{
            if (!IsPromotion) {
                return false;
            }
            return PlayerData.Instance.Session.Mistakes == Job.GameDataInstance.PromotionMistakes;
        }
    }

    public JobTaskRef(IJobRef job, JobTaskGameData data, JobTaskType type = JobTaskType.Normal) {
        this.Job = job;
//        if (type == JobTaskType.Promotion && !(data is PromotionTaskData)) {
//            this.Data = PromotionTaskData.Get(data);
//        } else {
            this.Data = data;
//        }
        this.Variation = 0;
		this.JobTaskType = type;
    }

    public JobTaskRef(IJobRef job, JobTaskGameData data, int variation, JobTaskType type) : this(job, data, type) {
        this.Variation = variation;
    }

    // TODO: make safe
    public Transform GetClientTarget() {
        return SceneAreaManager.Instance.Get(TagLibrary.Area01).transform.Find("Client");
        //return GameObject.Find(Data.AreaName).transform.Find("Client");
    }

    public Vector3 GetHomePosition() {
        var t = Data;
        if(t is PromotionTaskData){
            t = ((PromotionTaskData)Data).child;
        }
        
        if (t.StartingPosition.IsEmptyOrNull()) {
            return default(Vector3);
        }

        var area = SceneAreaManager.Instance.Get(t.StartingPosition);
        if (area) {
            if (area.transform.Find(TagLibrary.Home)) {
                return area.transform.Find(TagLibrary.Home).position;
            } else {
                return area.transform.position + Vector3.up;
            }
        }
        return default(Vector3);
    }

}
