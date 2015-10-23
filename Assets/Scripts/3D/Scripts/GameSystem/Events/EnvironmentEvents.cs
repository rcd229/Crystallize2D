using UnityEngine;
using System;
using System.Collections;

public class EnvironmentEvents : GameEvents {

    public event EventHandler OnActorApproached;
    public void RaiseActorApproached(object sender, EventArgs args) { OnActorApproached.Raise(sender, args); }
    public event EventHandler OnActorDeparted;
    public void RaiseActorDeparted(object sender, EventArgs args) { OnActorDeparted.Raise(sender, args); }

    public event EventHandler BeforeCameraMove;
    public void RaiseBeforeCameraMove(object sender, EventArgs args) { BeforeCameraMove.Raise(sender, args); }
    public event EventHandler AfterCameraMove;
    public void RaiseAfterCameraMove(object sender, EventArgs args) { AfterCameraMove.Raise(sender, args); }

    public event EventHandler BeforeSceneChange;
    public void RaiseBeforeSceneChange(object sender, EventArgs args) { BeforeSceneChange.Raise(sender, args); }

    public event EventHandler<PersonAnimationEventArgs> OnPersonAnimationRequested;
    public void RaisePersonAnimationRequested(object sender, PersonAnimationEventArgs args) { OnPersonAnimationRequested.Raise(sender, args); }

    public event EventHandler<GameObjectArgs> OnTriggerEntered;
    public void RaiseTriggerEntered(object sender, GameObjectArgs args) { OnTriggerEntered.Raise(sender, args); }

    public event EventHandler<GameObjectArgs> OnTriggerExited;
    public void RaiseTriggerExited(object sender, GameObjectArgs args) { OnTriggerExited.Raise(sender, args); }

    public event EventHandler<GameObjectArgs> OnEnvironmentTargetChanged;
    public void RaiseEnvironmentTargetChanged(object sender, GameObjectArgs args) { OnEnvironmentTargetChanged.Raise(sender, args); }

	public event EventHandler<GameObjectArgs> OnQuestNPCRemoved;
	public void RaiseQuestNPCRemoved(object sender, GameObjectArgs args) { OnQuestNPCRemoved.Raise(sender, args); }

    //public event SequenceRequestHandler<GameObject, object> OnConversationCameraRequested;
    //public void RequestConversationCamera(GameObject input, ProcessExitCallback<object> callback) {
    //    RequestSequence(input, OnConversationCameraRequested, callback);
    //}

}
