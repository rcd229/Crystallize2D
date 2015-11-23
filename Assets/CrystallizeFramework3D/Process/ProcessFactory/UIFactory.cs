using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIFactory {
    public ITemporaryUI<I, O> Get<T, I, O>(I input, EventHandler<EventArgs<O>> callback, IProcess parent)
        where T : Component, ITemporaryUI<I, O> {
        var fact = new UIFactoryRef<I, O>();
        fact.Set<T>();
        return fact.Get(input, callback, parent);
    }
}

public class UIFactory<I, O> {

    Func<ITemporaryUI<I, O>> getter;

    public UIFactory(Func<ITemporaryUI<I, O>> getter) {
        this.getter = getter;
    }

    public ITemporaryUI<I, O> GetInstance() {
        return getter();
    }

}
