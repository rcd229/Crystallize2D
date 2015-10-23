using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AffirmationPhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {
    public const string Sure = "Sure";

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return new string[]{Sure}; }
}
