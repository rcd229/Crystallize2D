using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SessionPlayerData {

    public int BaseMoney { get; set; }
    public int ReducedMoney {
        get {
            return Mathf.RoundToInt(BaseMoney * (float)(MaxMistakes - Mistakes) / MaxMistakes);
        }
    }

    int mistakes = 0;
    public int Mistakes { 
        get{
            return mistakes;
        }
        set {
            mistakes = value;
            UpdateTaskState();
        }
    }

    public int MaxMistakes { get; set; }
    public float RestQuality { get; set; }
    public string ChestItem { get; set; }
    public string LegsItem { get; set; }
    public List<PhraseSequence> TodaysCollectedWords { get; set; }
	public bool isPromotion {get;set;}
    public int Confidence { get; set; }
    public float[] Position { get; set; }
    public string Area { get; set; }

    public SessionPlayerData() {
        BuyableClothes.Initialize();
        ChestItem = BuyableClothes.DefaultChestItemName;
        LegsItem = BuyableClothes.DefaultLegsItemName;
        TodaysCollectedWords = new List<PhraseSequence>();
        Confidence = 10;
        Position = new float[3];
        Area = "";
    }

    public EarnedMoneyArgs GetEarnedMoney(IJobRef job) {
        var list = new List<ValueModifier>();
		ValueModifier cloths;
		if(job.GameDataInstance.formalLevel == JobFormalLevel.Versatile){
	        cloths = new ValueModifier("Versatility", PlayerDataConnector.GetVersatilityMultiplier());
		}
		//cloth is formal
		else {
			cloths = new ValueModifier("Formality", PlayerDataConnector.GetFormalityMultiplier());
		}
		if (cloths.Value != 1f) {
			list.Add(cloths);
		}
        var comfort = new ValueModifier("Rest Qlty", PlayerDataConnector.GetComfortMultiplier());
        if (comfort.Value != 1f) {
            list.Add(comfort);
        }
        var kana = new ValueModifier("Kana", PlayerDataConnector.GetKanaMultiplier());
        if (kana.Value != 1f) {
            list.Add(kana);
        }

		if(job.GameDataInstance.PromotionMistakes - Mistakes > 0 && isPromotion){
			var promotion = new ValueModifier("Promotion", PlayerDataConnector.GetPromotionMultiplier());
			list.Add(promotion);
		}

        return new EarnedMoneyArgs(BaseMoney, BaseMoney - ReducedMoney, list.ToArray());
    }

    public float GetScore() {
        return (float)(MaxMistakes - Mistakes) / MaxMistakes;
    }

    void UpdateTaskState() {
        TaskState.Instance.SetState("Salary", "¥ " + ReducedMoney);
    }

}
