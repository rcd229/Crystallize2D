using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DefaultPhrasePipelineBuilder {
    protected static PhraseSequence GetPhrase(string key, string set = "Default", bool isTest = false) {
        return PhrasePipeline.GetPhrase(set, key, isTest);
    }
}

public class PhrasePipelineBuilder {
    public bool IsTest { get; set; }
    public string SetKey { get; private set; }

    public PhrasePipelineBuilder(string setKey = "Default", bool isTest = false) {
        this.SetKey = setKey;
        this.IsTest = isTest;
    }

    public PhraseSequence GetPhrase(string key) {
        return PhrasePipeline.GetPhrase(SetKey, key, IsTest);
    }
}
