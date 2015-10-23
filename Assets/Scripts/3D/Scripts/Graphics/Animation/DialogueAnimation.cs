using UnityEngine;
using System;
using System.Collections;

public abstract class DialogueAnimation {

    public event EventHandler OnComplete;
    public abstract void Play(GameObject actor);
    public abstract DialogueAnimation GetInstance();

    protected void Exit() {
        OnComplete.Raise(this, EventArgs.Empty);
    }

}