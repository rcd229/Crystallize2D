using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TutorialProcessFactory<T, I, O> : ProcessFactory<I>
    where T : IProcess<I>, ISettableParent, new() {

    // two factory refrences, one for the original and one for the tutorial
    ProcessFactory<I> defaultFactory;

    // store tutorial name
    string tutorialName;

    // better contructor
    public TutorialProcessFactory(ProcessFactory<I> factory, string tutorialName) {
        this.defaultFactory = factory;
        this.tutorialName = tutorialName;
    }

    public override IProcess<I> GetInstance(I inputArgs, IProcess parent) {
        //		if(PlayerData.Instance.Tutorial.GetTutorialViewed(tutorialName)){
        if (PlayerData.Instance.Tutorial.GetTutorialViewed(tutorialName)) {
            return defaultFactory.GetInstance(inputArgs, parent);
        } else {
            var tempProcess = new T();
            tempProcess.SetParent(parent);
            return tempProcess;
        }
    }

}
