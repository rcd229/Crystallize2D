using UnityEngine;
using System;
using System.Collections;

public interface ILockEvent {

    event EventHandler OnUnlock;

}
