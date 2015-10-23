using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IEventArgs<T> {
    T Data { get; }
}
