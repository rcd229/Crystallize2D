using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IDebugMethods {
    IEnumerable<NamedMethod> GetMethods();
}
