using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class OrderedSelectorGameData : JobTaskSelectorGameData {

    public List<int> Tasks { get; set; }

    public OrderedSelectorGameData()
        : base(typeof(OrderedSelectorProcess)) {
            Tasks = new List<int>();
    }

    public OrderedSelectorGameData(IEnumerable<int> tasks) : this() {
        Tasks = new List<int>(tasks);
    }

}
