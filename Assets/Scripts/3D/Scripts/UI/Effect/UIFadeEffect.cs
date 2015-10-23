using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFadeEffect : UIMonoBehaviour {

    public static UIFadeEffect Create() {
        var go = new GameObject("FadeEffect");
        MainCanvas.main.Add(go.transform, CanvasBranch.None);
        go.AddComponent<Image>();
        go.AddComponent<UIFadeEffect>();
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        go.transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        go.layer = LayerMask.NameToLayer("UI");

        return go.GetComponent<UIFadeEffect>();
    }

    public Color fadeColor = Color.black;
    public bool fadeIn = false;

    void OnEnable() {
        Initialize();
    }

    void OnDisable() {
        Initialize();
    }

    // Use this for initialization
    void Start() {
        Initialize();
    }

    void Initialize() {
        if (!canvasGroup) {
            gameObject.AddComponent<CanvasGroup>();
        }

        if (fadeIn) {
            canvasGroup.alpha = 0;
        } else {
            canvasGroup.alpha = 1f;
        }

        image.color = fadeColor;
    }

    // Update is called once per frame
    void Update() {
        if (fadeIn) {
            canvasGroup.alpha += Time.deltaTime;
        } else {
            canvasGroup.alpha -= Time.deltaTime;
        }
    }
}
