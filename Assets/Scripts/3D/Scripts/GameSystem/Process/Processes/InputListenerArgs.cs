using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum InputType {
    EnvironmentClick,
    LeftClick,
    RightClick,
    Key
}

public class InputListenerArgs {
    public InputType InputType { get; set; }
    public InputListenerArgs(InputType inputType) { this.InputType = inputType; }
}

public class KeyInputListenerArgs : InputListenerArgs {
    public KeyCode Key { get; set; }
    public KeyInputListenerArgs(KeyCode key) : base(InputType.Key) { this.Key = key; }
}
