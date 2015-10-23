using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialProcessSelector<I> : IProcessSelector<I>, IProcessModifier<I> {

    ProcessFactory<I> factory;
    string tutorialName;

    public TutorialProcessSelector(string tutorialName, ProcessFactory<I> factory) {
        this.factory = factory;
        this.tutorialName = tutorialName;
    }

    public ProcessFactory<I> SelectProcess(ProcessFactory<I> defaultFactory, I args) {
        if (!PlayerData.Instance.Tutorial.GetTutorialViewed(tutorialName)) {
            return factory;
        }
        return null;
    }

    public void ModifyProcess(IProcess<I> process) {
        process.OnExit += process_OnExit;
    }

    void process_OnExit(object sender, object args) {
        // TODO: this is terrible
        if (tutorialName != "DragWords") {
            PlayerData.Instance.Tutorial.SetTutorialViewed(tutorialName);
        }
    }

}

public class TutorialProcessSelector<T, I> : TutorialProcessSelector<I> where T : IProcess<I>, new() {

    public TutorialProcessSelector(string tutorialName) : base(tutorialName, new ProcessFactory<T, I>()) { }

}
