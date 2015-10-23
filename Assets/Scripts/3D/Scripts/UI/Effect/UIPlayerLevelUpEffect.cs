using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerLevelUpEffect : UIMonoBehaviour {

	public UIRaysEffect rays;

    public float duration = 3f;
    public bool canFade = false;

	IEnumerator Start(){
		if (rays) {
			rays.gameObject.SetActive (false);
		}

        canvasGroup.alpha = 0;
        var s = 2.5f;
        transform.localScale = 2.5f * Vector3.one;
        while (canvasGroup.alpha < 1f) {
            canvasGroup.alpha += Time.deltaTime;
            s -= Time.deltaTime * 1.5f;
            transform.localScale = s * Vector3.one;
            yield return null;
        }

        transform.localScale = Vector3.one;

        if (duration == 0) {
            while (!canFade) {
                yield return null;
            }
        } else {
            float t = 0;
            while (t < duration - 1f) {
                t += Time.deltaTime;
                
                if (canFade) {
                    break;
                }

                yield return null;//new WaitForSeconds(duration - 1f);
            }
        }

        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
	}
	
	// Update is called once per frame
    //void Update () {
    //    time += Time.deltaTime;
		
    //    transform.localScale = Vector3.one;
    //    canvasGroup.alpha = 0;
    //    if (time < 1f) {
    //        transform.localScale = (1f + 1.5f * (1f - time)) * Vector3.one;
			
    //    } else if (time < duration - 1f) {
    //        if (rays) {
    //            rays.gameObject.SetActive (true);
    //        }
    //        canvasGroup.alpha = 1f;
    //        transform.localScale = Vector3.one;
    //    } else if (time < duration) {
    //        canvasGroup.alpha = duration - time;
    //    } else {
    //        Destroy(gameObject);
    //    }
    //}
}
