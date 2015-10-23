using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class InputListenerProcess : IProcess<InputListenerArgs, InputListenerArgs> {

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(InputListenerArgs param1) {
        if (param1.InputType == InputType.EnvironmentClick) {
            CrystallizeEventManager.Input.OnEnvironmentClick += InputReceived;
        } else if (param1.InputType == InputType.LeftClick) {
            CrystallizeEventManager.Input.OnLeftClick += InputReceived;
        } else if (param1.InputType == InputType.RightClick) {
            CrystallizeEventManager.Input.OnRightClick += InputReceived;
        } else if (param1.InputType == InputType.Key) {
            var keyListener = KeyListener.GetInstance();
            keyListener.Initialize(((KeyInputListenerArgs)param1).Key);
            keyListener.Complete += InputReceived;
        }
    }

    void InputReceived(object sender, EventArgs e) {
        Exit();
    }

    public void ForceExit() {

    }

    void Exit() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= InputReceived;
        CrystallizeEventManager.Input.OnLeftClick -= InputReceived;
        CrystallizeEventManager.Input.OnRightClick -= InputReceived;
        var exit = OnExit;
        OnExit = null;
        exit.Raise(this, null);
    }

}
