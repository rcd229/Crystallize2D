using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class TutorialSelector : IProcessBrancher {

    Dictionary<IProcessFactoryRef, IProcessFactoryRef> processToProcessDict;
    Dictionary<UIFactoryRef, IProcessFactoryRef> UIToProcessDict;

    public static TutorialSelector Instance {
        get {
            if (instance == null) {
                instance = new TutorialSelector();
            }
            return instance;
        }
    }
    private static TutorialSelector instance;
    private TutorialSelector() {
        processToProcessDict = new Dictionary<IProcessFactoryRef, IProcessFactoryRef>();
        UIToProcessDict = new Dictionary<UIFactoryRef, IProcessFactoryRef>();
    }


    #region IProcessSelector implementation
    //default. If tutorial is present then just use tutorial
    public IProcess SelectProcess<I, U, V>
        (ProcessFactoryRef<I, U> original,
         I input,
         ProcessExitCallback<U> originalCallback,
         ProcessExitCallback<V> alterCallback,
         IProcess parent) {
        IProcessFactoryRef p_ref;
        if (processToProcessDict.TryGetValue(original, out p_ref)) {
            ProcessFactoryRef<I, V> alternativeProcess = (ProcessFactoryRef<I, V>)p_ref;
            return alternativeProcess.Get(input, alterCallback, parent);
        } else {
            return original.Get(input, originalCallback, parent);
        }

    }
    //default, use tutorial if present
    public void SelectProcessOrUI<I, U, V>
        (UIFactoryRef<I, U> original,
         I input,
         Action<object, EventArgs<U>> originalCallback,
         ProcessExitCallback<V> alterCallback,
         IProcess parent) {
        IProcessFactoryRef p_ref;
        if (UIToProcessDict.TryGetValue(original, out p_ref)) {
            ProcessFactoryRef<I, V> alternativeProcess = (ProcessFactoryRef<I, V>)p_ref;
            alternativeProcess.Get(input, alterCallback, parent);
        } else {
            original.Get(input);
        }
    }
    #endregion

    public void SetAlternative<I, U, V>(ProcessFactoryRef<I, U> original, ProcessFactoryRef<I, V> alternative) {
        processToProcessDict[original] = alternative;
    }

    public void SetAlternative<I, U, V>(UIFactoryRef<I, U> original, ProcessFactoryRef<I, V> alternative) {
        UIToProcessDict[original] = alternative;
    }

    public IProcess SelectProcess<I, U, V>
        (ProcessFactoryRef<I, U> original,
         I input,
         ProcessExitCallback<U> originalCallback,
         ProcessExitCallback<V> alterCallback,
         IProcess parent,
         int tutorialID) {
        if (PlayerData.Instance.Tutorial.GetTutorialViewed(tutorialID)) {
            return original.Get(input, originalCallback, parent);
        } else {
            return SelectProcess(original, input, originalCallback, alterCallback, parent);
        }
    }

    public void SelectProcessOrUI<I, U, V>
        (UIFactoryRef<I, U> original,
         I input,
         Action<object, EventArgs<U>> originalCallback,
         ProcessExitCallback<V> alterCallback,
         IProcess parent,
         int tutorialID) {
        if (PlayerData.Instance.Tutorial.GetTutorialViewed(tutorialID)) {
            original.Get(input);
        } else {
            SelectProcessOrUI(original, input, originalCallback, alterCallback, parent);
        }
    }

}
