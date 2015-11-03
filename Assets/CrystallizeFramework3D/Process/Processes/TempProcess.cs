using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TempProcess<I, O> : IProcess<I, O> {

    public ProcessExitCallback OnExit { get; set; }

    NotImplementedYetUI panel;

    public void Initialize(I data) {
        panel = NotImplementedYetUI.GetInstance();
        panel.SetProcessText(data);
        InputEventManager.Instance.Events.OnLeftClick += HandleLeftClick;
    }

    void HandleLeftClick(object sender, EventArgs e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        panel.Close();
        InputEventManager.Instance.Events.OnLeftClick -= HandleLeftClick;
        OnExit(this, null);
    }

}
