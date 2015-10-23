using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DisclaimerUI : MonoBehaviour {

    public void Accept() {
        Application.LoadLevel("ChooseScene");
    }

}
