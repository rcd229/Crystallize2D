using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneDoor : SceneGuid<SceneData>, IInteractableSceneObject {

    public override SceneData ID {
        get { return SceneData.Get(Guid); }
    }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        ProcessLibrary.ChangeScene.Get(ID, callback, parent);
    }

    void Start() {
        CrystallizeEventManager.PlayerState.QuestFlagChanged += PlayerState_QuestFlagChanged;
        SetEnabledForFlag();
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.PlayerState.QuestFlagChanged -= PlayerState_QuestFlagChanged;
        }
    }

    void PlayerState_QuestFlagChanged(object sender, EventArgs<object> e) {
        SetEnabledForFlag();
    }

    void SetEnabledForFlag() {
        enabled = ID.GetEnabled();
    }

}
