using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Predesk : MonoBehaviour {

    void Start() {
        gameObject.SetActive(BuyableFurniture.Desk.Availability != BuyableAvailability.Purchased);
    }

}
