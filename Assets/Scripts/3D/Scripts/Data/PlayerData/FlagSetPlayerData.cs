using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class FlagSetPlayerData : UniqueNameGameData {

    public HashSet<string> Flags { get; set; }

    public FlagSetPlayerData()
        : base() {
            Flags = new HashSet<string>();
    }

    public void Add(string flag) {
        Flags.Add(flag);
    }

    public void Remove(string flag) {
        Flags.Remove(flag);
    }

    public bool Contains(string flag) {
        return Flags.Contains(flag);
    }

}
