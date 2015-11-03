using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class UniqueID {

    public static bool operator ==(UniqueID id1, UniqueID id2) {
        if (Equals(id1, null)) {
            return false;
        }

        if (Equals(id2, null)) {
            return false;
        }

        return id1.guid == id2.guid; 
    }
    public static bool operator !=(UniqueID id1, UniqueID id2) { 
        return !(id1 == id2); 
    }

    public Guid guid { get; private set; }

    public UniqueID() { guid = Guid.NewGuid(); }
    public UniqueID(Guid id) { guid = id; }
    public UniqueID(string id) { guid = new Guid(id); }

    public override bool Equals(object obj) {
        if (obj == null) {
            return false;
        } if (obj is UniqueID) {
            return ((UniqueID)obj).guid == guid;
        } else if (obj is Guid) {
            return (Guid)obj == guid;
        }
        return false;
    }

    public override int GetHashCode() {
        return guid.GetHashCode();
    }

}
