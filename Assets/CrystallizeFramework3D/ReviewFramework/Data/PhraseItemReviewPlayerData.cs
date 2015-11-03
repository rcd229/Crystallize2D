using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlType("ItemReviewPlayerData")]
public class PhraseItemReviewPlayerData : ItemReviewPlayerData<PhraseSequence>  {

    public PhraseItemReviewPlayerData() : base() { }

    public PhraseItemReviewPlayerData(PhraseSequence phrase)
        : base() {
        Item = phrase;
    }

    public override string GetText() {
        return Item.GetText();
    }

}
