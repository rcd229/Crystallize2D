using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IDebugListener {
    void AddContextMethods(IEnumerable<NamedMethod> methods);
    void RemoveContextMethods(IEnumerable<NamedMethod> methods);
}
