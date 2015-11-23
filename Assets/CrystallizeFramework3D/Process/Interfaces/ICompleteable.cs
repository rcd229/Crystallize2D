using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface ICompleteable<T>  {
    EventHandler<EventArgs<T>> Complete { get; set; }
}

public interface ICompleteable : ICompleteable<object> {}
