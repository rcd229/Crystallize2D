using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

public interface IDialogueEvent : ISerializableDictionaryItem<int>, ISetableKey<int> {
    void RaiseEvent();
}

public abstract class BaseDialogueEvent : IDialogueEvent {

    public int Key { get { return ID; } }
    public int ID { get; set; }

    public BaseDialogueEvent() : this(-1) { }

    public BaseDialogueEvent(int id) {
        this.ID = id;
    }

    public abstract void RaiseEvent();


    public void SetKey(int key) {
        ID = key;
    }
}
