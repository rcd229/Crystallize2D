using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneBoss : SceneGuid<JobID>, IInteractableSceneObject {

    public IJobRef Job { get { return new IDJobRef(ID); } }

    public override JobID ID {
        get { return new JobID(Guid); }
    }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        var unlocked = Job.PlayerDataInstance.Unlocked || Job.GameDataInstance.QuestID == null;
        DataLogger.LogTimestampedData("JobNPC", ID.guid.ToString());
        if (unlocked) {
            ProcessLibrary.SceneJob.Get(Job, callback, parent);
        } else {
            ProcessLibrary.QuestConversation.Get(new QuestArgs(gameObject, Job.GameDataInstance.GetNPC()), callback, parent);
        }
    }

	public bool hasNew(){
		return Job.PlayerDataInstance.Repetitions == 0;
	}

    void Start() {
        gameObject.GetOrAddComponent<IndicatorComponent>().Initialize(
            Job.GameDataInstance.Name + " Boss", new OverheadIcon(IconType.Briefcase),
            new MapIndicator(MapResourceType.BossNPC), hasNew());
    }
}
