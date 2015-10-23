using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class SceneGuid<T> : MonoBehaviour where T : UniqueID {
    
    [SerializeField]
    string guid = "";

    public Guid Guid {
        get {
            if (guid == "") {
                return Guid.Empty;
            } else {
                return new Guid(guid);
            }
        }
        set { 
            guid = value.ToString(); 
        }
    }

    public abstract T ID { get; }

}
