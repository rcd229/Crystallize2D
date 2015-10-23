using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class BedSelector : MonoBehaviour {

    public static BedSelector Instance { get; private set; }

    void Start() {
        Instance = this;
        foreach (var bed in GetComponentsInChildren<BedInstance>()) {
            foreach (Transform c in bed.transform) {
                c.gameObject.SetActive(false);
            }
        }
    }

    public void SetBed(int bedID) {
        foreach (var bed in GetComponentsInChildren<BedInstance>()) {
            if (bed.id == bedID) {
                foreach (Transform c in bed.transform) {
                    c.gameObject.SetActive(true);
                }
            } else {
                foreach (Transform c in bed.transform) {
                    c.gameObject.SetActive(false);
                }
            }
        }
    }

}
