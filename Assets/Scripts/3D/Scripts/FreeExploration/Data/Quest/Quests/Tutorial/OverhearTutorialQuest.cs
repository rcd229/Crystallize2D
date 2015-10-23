using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class OverhearTutorialQuest : BaseQuestGameData, IQuestStateMachine {

    public static readonly QuestTypeID QuestID = new QuestTypeID("3a855bf23c8941219a9e3ce7a3383ced");
    public static readonly string[] States = new string[] { "Root", "ListenMore", "Speak" };

    public override QuestTypeID ID { get { return QuestID; } }
    public override string QuestName { get { return "Overhear conversations tutorial"; } }
    public override bool IsRepeatable { get { return false; } }
    public override IQuestStateMachine StateMachine { get { return this; } }
    public string FirstState { get { return States[0]; } }

    NPCGroup Group1 { get { return StandardNPCGroup.HelloGroup; } }
    NPCGroup Group2 { get { return StandardNPCGroup.GoodbyeGroup; } }
    NPCGroup Group3 { get { return StandardNPCGroup.GoodMorningGroup; } }

    public string GetNextState(string state, string transition) {
        return transition;
    }

    public void UpdateSceneForState(QuestRef quest) {
        var s = quest.PlayerDataInstance.State;
        if(s == States[0]) {
            var target = NPCTarget.Get(Group1.guid);
            if (target) {
                var res = NPCManager.Instance.SpawnNPCGroup(target.transform, Group1);
                SceneResourceManager.Instance.SetResources(ID.guid, new GameObject[] { res });
            }
        } else if (s == States[1]) {
            var resources = new List<GameObject>();
            var t1 = NPCTarget.Get(Group1.guid);
            if (t1) {
                resources.Add(NPCManager.Instance.SpawnNPCGroup(t1.transform, Group1));
            }

            var t2 =  NPCTarget.Get(Group2.guid);
            if(t2){
                resources.Add(NPCManager.Instance.SpawnNPCGroup(t2.transform, Group2));
            }

            var t3 =  NPCTarget.Get(Group3.guid);
            if(t3){
                resources.Add(NPCManager.Instance.SpawnNPCGroup(t3.transform, Group3));
            }

            if (resources.Count < 3) {
                //Debug.LogError("Not enough targets found!");
            }

            SceneResourceManager.Instance.SetResources(ID.guid, resources);
        } else if (s == States[2]) {
            
        } else {
            SceneResourceManager.Instance.SetResources(ID.guid, null);
        }
    }
}
