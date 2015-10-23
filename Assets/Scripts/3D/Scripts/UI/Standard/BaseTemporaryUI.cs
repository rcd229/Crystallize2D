using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class BaseTemporaryUI<I, O> : MonoBehaviour, ITemporaryUI<I, O> {

    public virtual event EventHandler<EventArgs<O>> Complete;

    public virtual void Initialize(I param1) {    }

    public void Close() {
        Destroy(gameObject);
    }

}
