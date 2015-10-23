using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class TaskState {

    static TaskState _instance;
    public static TaskState Instance {
        get {
            if (_instance == null) {
                _instance = new TaskState("NULL");
            }
            return _instance;
        }
    }

    public static void Initialize(string job) {
        _instance = new TaskState(job);
    }

    public static void Clear() {
        _instance = null;
    }

    public string Job { get; private set; }
    public string Instructions { get; private set; }
    public List<StringMapItem> States { get; private set; }
    public bool Hidden { get; private set; }

    public event EventHandler<EventArgs<TaskState>> StateChanged;
	public event EventHandler<EventArgs<object>> OtherStateChanged;

    TaskState(string job) {
        Job = job;
        Instructions = "";
        States = new List<StringMapItem>();
        Hidden = false;
    }

    public void SetInstructions(string instructions) {
        Instructions = instructions;
        RaiseStateChanged();   
    }

    public void SetState(string key, string value) {
		SetStateWithoutRaise(key, value);
        OtherStateChanged.Raise(this, null);
    }

	public void SetStateWithoutRaise(string key, string value){
		var item = States.Where(s => s.Key == key).FirstOrDefault();
		if (item == null) {
			item = new StringMapItem(key, value);
			States.Add(item);
		} else {
			item.Value = value;
		}
		RaiseStateChanged();
	}
	
	public void SetHidden(bool isHidden) {
        Hidden = isHidden;
        RaiseStateChanged();
    }

    void RaiseStateChanged() {
        StateChanged.Raise(this, new EventArgs<TaskState>(this));
    }

}
