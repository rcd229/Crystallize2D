using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IProcessFactoryRef { }

public class ProcessFactoryRef<I, O> : IProcessFactoryRef {

    public ProcessFactory<I> Factory { get; set; }

    public void Set<T>() where T : IProcess<I, O>, new() {
        Factory = new ProcessFactory<T, I, O>();
    }

    public void Set(Type t) {
        Factory = new DynamicProcessFactory<I, O>(t);
    }

    public void SetTutorial<T>(string name) where T : IProcess<I, O>, new() {
        Factory = new ProcessFactory<EmptyProcess<I, O>, I, O>();
        this.AddTutorial<T>(name);
    }

    public void InsertTutorial(IProcessGetter getter, string name) {
        var inserted = new InsertProcessFactory<I, O>(getter, Factory);
        AddSelector(new TutorialProcessSelector<I>(name, inserted));
    }

    public void InsertTutorial<T>(string name) where T : IProcess<object, object>, new() {
        var r = new ProcessFactoryRef<object,object>();
        r.Set<T>();
        var inserted = new InsertProcessFactory<I, O>(new ProcessGetter<object, object>(r, null), Factory);
        AddSelector(new TutorialProcessSelector<I>(name, inserted));
    }

    public void AddTutorial<T>(string name) where T : IProcess<I>, new() {
        AddSelector(new TutorialProcessSelector<T, I>(name));
    }

    public void AddSelector(IProcessSelector<I> selector) {
        Factory = new SelectorProcessFactory<I>(Factory, selector);
    }

    public void Set(Action<I> action) {
        Factory = new ActionProcessFactory<I>(action);
    }

}
