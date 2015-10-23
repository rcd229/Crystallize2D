using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ActionProcessFactory<I> : ProcessFactory<I> {

    Action<I> action;

    public ActionProcessFactory(Action<I> action) {
        this.action = action;
    }

    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        return new ActionProcess<I>(action);
    }

}
