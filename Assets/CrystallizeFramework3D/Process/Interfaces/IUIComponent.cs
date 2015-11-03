using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IUIComponent<T> {
    T Value { get; set; }
}