using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InsertProcessFactory<I, O> : ProcessFactory<I> {

    IProcessGetter inserted;
    ProcessFactory<I> original;

    public InsertProcessFactory(IProcessGetter inserted, ProcessFactory<I> original) {
        this.inserted = inserted;
        this.original = original;
    }

    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        return new InsertProcess<I, O>(inserted, original);
    }

}
