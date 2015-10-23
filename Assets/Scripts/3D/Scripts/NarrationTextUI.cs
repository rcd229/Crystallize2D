using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/NarrationText")]
public class NarrationTextUI : MonoBehaviour, ITemporaryUI<List<PhraseSequence>, object> {

    static NarrationTextUI _instance;

    public GameObject linePrefab;
    public RectTransform lineParent;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(List<PhraseSequence> args1) {
        if (_instance) Destroy(_instance.gameObject);
        _instance = this;

        UIUtil.GenerateChildren(args1, lineParent, GenerateChild);
        StartCoroutine(FadeIn());
    }

    public void Close() {
        Destroy(gameObject);
    }

    GameObject GenerateChild(PhraseSequence line) {
        var instance = Instantiate<GameObject>(linePrefab);
        instance.GetComponent<IInitializable<PhraseSequence>>().Initialize(line);
        return instance;
    }

    IEnumerator FadeIn() {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.GetOrAddComponent<CanvasGroup>().alpha = 0;
        }

        for (int i = 0; i < transform.childCount; i++) {
            for (float t = 0; t < 1f; t += Time.deltaTime) {
                transform.GetChild(i).GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
            transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;

            yield return new WaitForSeconds(1f);
        }
    }

}
