using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DynamicProcessFactory<I, O> : ProcessFactory<I> {

    Type t;

    public DynamicProcessFactory(Type t) {
        this.t = t;
    }

    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        return (IProcess<I>)Activator.CreateInstance(t);
    }

}