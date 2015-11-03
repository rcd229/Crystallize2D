using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SelectorProcessFactory<I> : ProcessFactory<I>, IProcessModifier<I> {

    ProcessFactory<I> defaultFactory;
    IProcessSelector<I> selector;
    IProcessModifier<I> modifier;

    public SelectorProcessFactory(ProcessFactory<I> defaultFactory, IProcessSelector<I> selector) {
        this.defaultFactory = defaultFactory;
        this.selector = selector;

        if (selector is IProcessModifier<I>) {
            modifier = (IProcessModifier<I>)selector;
        }
    }

    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        var f = selector.SelectProcess(defaultFactory, inputArgs);
        if (f == null) {
            return defaultFactory.GetInstance(inputArgs, parent);
        } else {
            return f.GetInstance(inputArgs, parent);
        }
    }

    public void ModifyProcess(IProcess<I> process) {
        if (modifier != null) {
            modifier.ModifyProcess(process);
        }
    }

}
