using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TaskEntryPlayerData {

    public int TaskID { get; set; }

    public TaskEntryPlayerData() {
        TaskID = -1;
    }

    public TaskEntryPlayerData(int id) {
        TaskID = id;
    }

}
