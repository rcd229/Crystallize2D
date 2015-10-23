using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UniqueNameGameData : ISerializableDictionaryItem<string> {

    public string Name { get; set; }
    public string Key { get { return Name; } }

    public UniqueNameGameData() {
        Name = "";
    }

}
