using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IPhraseResources {
    string SetKey { get; }
    IEnumerable<string> GetPhraseKeys();
}
