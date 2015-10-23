using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Region : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Set region");
        if (other.IsPlayer()) {
            if (RegionManager.Instance) {
                RegionManager.Instance.SetRegion(this);
            }

            CrystallizeEventManager.Environment.RaiseTriggerEntered(this, new GameObjectArgs(gameObject));
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.IsPlayer()) {
            if (RegionManager.Instance) {
                RegionManager.Instance.SetRegion(null);
            }

            CrystallizeEventManager.Environment.RaiseTriggerExited(this, new GameObjectArgs(gameObject));
        }
    }

}
