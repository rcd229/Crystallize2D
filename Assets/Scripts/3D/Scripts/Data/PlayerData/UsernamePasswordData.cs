using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util.Serialization;

public class UsernamePasswordTable {
    public List<UsernamePasswordPair> LoginList { get; set; }

    public UsernamePasswordTable() {
        LoginList = new List<UsernamePasswordPair>();
    }

    public void AddEntry(UsernamePasswordPair entry) {
        LoginList.Add(entry);
    }

    public bool CheckPassword(UsernamePasswordPair Entry) {
        foreach (var pair in LoginList) {
            if (Entry.Username == pair.Username) {
                if (Entry.Password == pair.Password) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }
}
