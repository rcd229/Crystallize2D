﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialProcessSelector {
    [FrameworkProperty]
    public static ITutorialData TutorialData { get; set; }
}

public class TutorialProcessSelector<I> : TutorialProcessSelector, IProcessSelector<I>, IProcessModifier<I> {

    ProcessFactory<I> factory;
    string tutorialName;

    public TutorialProcessSelector(string tutorialName, ProcessFactory<I> factory) {
        this.factory = factory;
        this.tutorialName = tutorialName;
    }

    public ProcessFactory<I> SelectProcess(ProcessFactory<I> defaultFactory, I args) {
        if (!TutorialData.GetTutorialViewed(tutorialName)) {
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
            TutorialData.SetTutorialViewed(tutorialName);
        }
    }

}

public class TutorialProcessSelector<T, I> : TutorialProcessSelector<I> where T : IProcess<I>, new() {

    public TutorialProcessSelector(string tutorialName) : base(tutorialName, new ProcessFactory<T, I>()) { }

}
