using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialFadeInUIEffect : UIMonoBehaviour {

    public bool reverse;

	// Use this for initialization
	void Start () {
        transform.SetParent(TutorialCanvas.main.transform);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        rectTransform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        if (reverse) {
            GetComponent<Image>().fillAmount = 0;
        } else {
            GetComponent<Image>().fillAmount = 1f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (reverse) {
            GetComponent<Image>().fillAmount += Time.deltaTime;

            if (GetComponent<Image>().fillAmount >= 1f) {
                //Destroy(gameObject);
            }
        } else {
            GetComponent<Image>().fillAmount -= Time.deltaTime;

            if (GetComponent<Image>().fillAmount <= 0) {
                Destroy(gameObject);
            }
        }
	}

}
