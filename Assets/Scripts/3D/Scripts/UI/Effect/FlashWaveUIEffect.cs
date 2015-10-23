using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FlashWaveUIEffect : MonoBehaviour {

    class FlashWave {

        public GameObject instance;
        RectTransform rectTransform;
        CanvasGroup canvasGroup;
        float time = 0;

        public FlashWave(GameObject instance) {
            this.instance = instance;
            this.rectTransform = instance.GetComponent<RectTransform>();
            this.canvasGroup = instance.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void Update() {
            time += Time.deltaTime;
            
            if(time < FlashWaveUIEffect.FlashInterval){
                canvasGroup.alpha = time / FlashWaveUIEffect.FlashInterval;
            } else if(time < FlashWaveUIEffect.FlashInterval + FadeTime) {
                var t = (time - FlashWaveUIEffect.FlashInterval) / FadeTime;
                canvasGroup.alpha = 1f - t;
                var r = rectTransform.rect;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r.width + FinalOffset * Time.deltaTime / FadeTime);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, r.height + FinalOffset * Time.deltaTime / FadeTime);
                //instance.transform.localScale = (1f + t * FinalScale) * Vector3.one;
            } else {
                instance.SetActive(false);
            }
        }
    
    }

    public const float FlashInterval = 0.2f;
    public const float FadeTime = 0.4f;
    public const float FinalScale = 0.25f;
    public const float FinalOffset = 32f;

    public GameObject imagePrefab;
    public Color flashColor;

    float timer = 0;
    int times = 2;

    List<FlashWave> waves = new List<FlashWave>();

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
    void Update() {
        if (times > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                AddWave();
                timer += FlashInterval;
                times--;
            }
        }

        foreach (var wave in waves) {
            wave.Update();
        }

        foreach (var wave in waves) {
            if (!wave.instance.activeSelf) {
                Destroy(wave.instance);
                waves.Remove(wave);

                if (waves.Count == 0) {
                    enabled = false;
                }

                break;
            }
        }
    }

    public void Play() {
        times = 3;
        enabled = true;
        timer = 0;
    }

    void AddWave() {
        var instance = Instantiate<GameObject>(imagePrefab);
        instance.transform.SetParent(transform.parent);
        var r = instance.transform.parent.GetComponent<RectTransform>().rect;
        instance.transform.localPosition = r.center;
        instance.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r.width);
        instance.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, r.height);
        var wave = new FlashWave(instance);
        waves.Add(wave);
    }

}
