using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConversationCameraProcess : IProcess<ConversationArgs, object> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<ConversationArgs, object> instance;

    public void Initialize(ConversationArgs args) {
        instance = args.Dialogue.Camera.CameraController.Get(args); //UILibrary.ConversationCamera.Get(param1);
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        instance.CloseIfNotNull();
    }

}
