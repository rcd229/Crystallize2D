using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UnlockableFurnatureSetter : MonoBehaviour {

    void Start() {
        foreach (var f in BuyableFurniture.GetValues()) {
            var go = transform.Find(f.GameObjectName);
            if (go) {
                if (f.Availability == BuyableAvailability.Purchased) {
                    go.gameObject.SetActive(true);
                } else {
                    go.gameObject.SetActive(false);
                }
            }
        }
    }

}
