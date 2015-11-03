using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IProcessGetter {
    IProcess Get(ProcessExitCallback callback, IProcess parent);
}

public class ProcessGetter<I, O> : IProcessGetter {

    ProcessFactoryRef<I, O> factoryRef;
    I input;

    public ProcessGetter(ProcessFactoryRef<I, O> factoryRef, I input) {
        this.factoryRef = factoryRef;
        this.input = input;
    }

    public IProcess Get(ProcessExitCallback callback, IProcess parent) {
        return factoryRef.GetNullOutput(input, callback, parent);
    }

}
