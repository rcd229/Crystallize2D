using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BuyableWord : IBuyable {

    const string ViewedSet = "WordViewed";

    public static IEnumerable<BuyableWord> GetAvailableWords() {
        Dictionary<int, BuyableWord> buyables = new Dictionary<int, BuyableWord>();
        foreach (var job in GameData.Instance.Jobs.Items) {
            var jref = new IDJobRef(job.ID);
            if (jref.PlayerDataInstance.Repetitions > 2) {
                int cost = jref.GameDataInstance.Money * 2;
                foreach (var word in jref.GameDataInstance.AvailableWords) {
                    if (!PlayerDataConnector.ContainsLearnedItem(word)) {
                        if(!buyables.ContainsKey(word.Word.WordID)){
                            buyables[word.Word.WordID] = new BuyableWord(word, cost);
                        } else if (buyables[word.Word.WordID].Cost > cost) {
                            buyables[word.Word.WordID].Cost = cost;
                        }
                    }
                }
            }
        }
        return buyables.Values;
    }


    public int Cost { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public BuyableAvailability Availability { get { return GetAvailability(); } }

    public bool Viewed {
        get { return PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedSet).Contains(phrase.GetText()); }
        set { PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedSet).Add(phrase.GetText()); }
    }

    PhraseSequence phrase;

    public BuyableWord(PhraseSequence phrase, int cost) {
        this.phrase = phrase;
        Cost = cost;
        if (phrase.IsWord) {
            Name = phrase.Word.GetTranslation();
        } else {
            Name = phrase.Translation;
        }
        Description = "The Japanese word for '" + Name + "'"
            + "\n\n<i>Purchasing this word will add it to your inventory. This can help you unlock more jobs.</i>";
    }

    public void AfterBuyItem() {
        if (phrase.IsWord) {
            PlayerDataConnector.CollectWord(phrase.Word);
        } else {
            PlayerDataConnector.CollectPhrase(phrase);
        }
    }

    public BuyableAvailability GetAvailability() {
        if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
            return BuyableAvailability.Purchased;
        } else {
            return BuyableAvailability.Available;
        }
    }

}
