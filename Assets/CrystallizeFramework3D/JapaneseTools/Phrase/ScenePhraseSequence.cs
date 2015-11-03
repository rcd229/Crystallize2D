using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using Util.Serialization;

[System.Serializable]
public class ScenePhraseSequence {

    [SerializeField]
    string xmlString = "";

    public string CachedText { get; set; }

    public PhraseSequence Get() {
        if(xmlString != ""){
            return Serializer.LoadFromXmlString<PhraseSequence>(xmlString);
        } else {
            return new PhraseSequence();   
        }
    }

    public void Set(PhraseSequence obj) {
        CachedText = obj.GetText();
        xmlString = Serializer.SaveToXmlString<PhraseSequence>(obj);
    }

}
