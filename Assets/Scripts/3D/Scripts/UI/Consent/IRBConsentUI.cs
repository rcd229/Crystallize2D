using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class IRBConsentUI : MonoBehaviour {

    public void Accept() {
        Destroy(gameObject);
    }

    public void Reject(){
        Application.Quit();
    }

}
