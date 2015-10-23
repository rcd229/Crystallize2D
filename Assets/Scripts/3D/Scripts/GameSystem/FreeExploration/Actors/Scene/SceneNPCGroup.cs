using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneNPCGroup : SceneGuid<NPCGroup>, IInteractableSceneObject, IActorParent, IHeadOverride {

    NPCGroup group;
    public override NPCGroup ID { 
        get {
            if (group == null) {
                group = NPCGroup.Get(Guid);
            }
            return group; 
        } 
    }
    
    public Vector3 HeadPosition {
        get { return 1.25f * Vector3.up; }
    }

    public void Initialize(NPCGroup group) {
        this.Guid = group.guid;
        this.group = group;
    }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        if (ID == null) {
            callback(this, null);
        } else {
            var actors = GetComponentsInChildren<DialogueActor>();
            var targets = new GameObject[actors.Length];
            for(int i = 0; i < targets.Length; i++){
                targets[i] = actors[i].gameObject;
            }
            var convArgs = new ConversationArgs(targets, ID.Dialogue);
            ProcessLibrary.Conversation.Get(convArgs, callback, parent);
        }
    }
}
