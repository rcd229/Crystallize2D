using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;

public class ProcessFactory<I>  {
    public virtual IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        return null; 
    }
}

public class ProcessFactory<T, I> : ProcessFactory<I> where T : IProcess<I>, new() {
    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        return new T();
    }
}

public class ProcessFactory<T, I, O> : ProcessFactory<T, I> where T : IProcess<I, O>, new() { }