using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface ICompleteable<T>  {
    event EventHandler<EventArgs<T>> Complete;
}

public interface ICompleteable : ICompleteable<object> {}
