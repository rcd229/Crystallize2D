using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface ITutorialData {
    bool GetTutorialViewed(string name);
    void SetTutorialViewed(string name);
}
