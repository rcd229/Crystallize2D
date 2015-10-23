using UnityEngine;
using System.Collections;
using System.Linq;

public class PhraseEvaluator {

	public static float GetPhraseLevel(int wordID){
        var r = ReviewManager.main.reviewLog.GetReview(wordID);
        if (r == null) {
            return 1f;
        } else {
            return r.Level;
        }
	}

	public static Color GetColor(PhraseReviewData reviewData){
		if (!reviewData.IsLive (ReviewManager.main.simulatedTime)) {
			return Color.gray;
		}
		
		if (reviewData.Level == 0) {
			return GUIPallet.Instance.stageColors [4];
		} else {
			return GUIPallet.Instance.stageColors [(reviewData.Level - 1) % 4];
		}
	}

}
