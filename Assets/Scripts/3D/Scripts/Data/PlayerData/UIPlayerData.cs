using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum HUDPartType {
    Collect,
    Confidence,
    Map,
    QuestStatus,
    ChatBox,
    Home
}

public class HUDPartArgs : EventArgs {
    public HUDPartType Part { get; private set; }
    public bool Value { get; private set; }
    public HUDPartArgs(HUDPartType part, bool val) {
        this.Part = part;
        this.Value = val;
    }
}

public class UIPlayerData {
    public HashSet<HUDPartType> DisabledParts = new HashSet<HUDPartType>();
    public HashSet<HUDPartType> InactiveParts = new HashSet<HUDPartType>();

    public bool MapOpen {
        get { return GetPartActive(HUDPartType.Map); }
        set { SetPartActive(HUDPartType.Map, value); }
    }
    public bool QuestStatusOpen {
        get { return GetPartActive(HUDPartType.QuestStatus); }
        set { SetPartActive(HUDPartType.QuestStatus, value); }
    }
    public bool ChatBoxOpen {
        get { return GetPartActive(HUDPartType.ChatBox); }
        set { SetPartActive(HUDPartType.ChatBox, value); }
    }

    public UIPlayerData() {
        MapOpen = true;
        QuestStatusOpen = true;
        ChatBoxOpen = true;
    }

    public bool GetPartEnabled(HUDPartType part) {
        return !DisabledParts.Contains(part);
    }

    public void SetPartEnabled(HUDPartType part, bool val) {
        if (val) {
            DisabledParts.Remove(part);
        } else {
            DisabledParts.Add(part);
        }
    }

    public bool GetPartActive(HUDPartType part) {
        return !InactiveParts.Contains(part);
    }

    public void SetPartActive(HUDPartType part, bool val) {
        if (val) {
            InactiveParts.Remove(part);
        } else {
            InactiveParts.Add(part);
        }
        //Debug.Log("Setting active: " + part + "; " + val + "; " + GetPartActive(part));
    }

}
