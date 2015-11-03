using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IPanel : ICloseable {
    void SetVisible(bool visible);
    void SetInteractive(bool interactive);
}
