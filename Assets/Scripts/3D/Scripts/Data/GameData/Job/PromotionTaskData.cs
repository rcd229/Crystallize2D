using UnityEngine;
using System.Collections;

public class PromotionTaskData : JobTaskGameData {

	public const int PromotionIndex = -100;
	public const int PromotionVariation = -100;

    public const bool UsePromotion = true;

	public JobTaskGameData child{get;set;}

	public static PromotionTaskData Get(JobTaskGameData c){
		var ret = new PromotionTaskData();
		ret.child = c;
		ret.SceneName = c.SceneName;
		ret.ProcessType = new ProcessTypeRef(typeof(PromotionTaskProcess));
		return ret;
	}
}
