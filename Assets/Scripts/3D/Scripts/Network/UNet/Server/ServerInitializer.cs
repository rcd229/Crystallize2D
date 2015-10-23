using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ServerInitializer : MonoBehaviour {

    void Start() {
        CrystallizeNetwork.InitializeServer();
    }

}
