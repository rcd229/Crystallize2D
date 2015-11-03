using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIFactory<I, O> {

    Func<ITemporaryUI<I, O>> getter;

    public UIFactory(Func<ITemporaryUI<I, O>> getter) {
        this.getter = getter;
    }

    public ITemporaryUI<I, O> GetInstance() {
        return getter();
    }

}
