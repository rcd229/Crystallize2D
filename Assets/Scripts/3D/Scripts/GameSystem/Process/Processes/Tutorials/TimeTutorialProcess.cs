using UnityEngine;
using System.Collections;

public class TimeTutorialProcess : IProcess<string, object> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI ui;

    public void ForceExit() {
        Exit();
    }

    public void Initialize(string param1) {
        var r = GameObject.FindGameObjectWithTag("ClockUI");
        ui = UILibrary.HighlightBox.Get(new UITargetedMessageArgs(r.GetComponent<RectTransform>(), "View the time here"));
        ProcessLibrary.ListenForInput.Get(new InputListenerArgs(InputType.LeftClick), InputCallback, this);
    }

    void InputCallback(object obj, object args) {
        Exit();
    }

    void Exit() {
        ui.CloseIfNotNull();
        OnExit.Raise(this, null);
    }
}