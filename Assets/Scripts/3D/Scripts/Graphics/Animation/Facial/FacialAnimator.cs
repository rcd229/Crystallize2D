using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public enum FacialAnimationType {
    None,
    Annoyed,
    Angry,
    Happy,
    Sad
}

public class FacialAnimator : MonoBehaviour {

    const int Smile = 2;
    const int OMouth = 12;
    const int EMouth = 13;
    const int AMouth = 14;
    const float Speed = 4f;

    static int[] Mouths = new int[] { OMouth, EMouth, AMouth };

    public SkinnedMeshRenderer face;

    int currentMouth = 0;
    int nextMouth = 0;

    float t = 0;

    Coroutine coroutine;

    public void SetFacialExpression(FacialAnimationType type) {
        if(!face){
            return;
        }

        switch (type) {
            case FacialAnimationType.Happy:
                face.SetBlendShapeWeight(Smile, 100f);
                break;
        }
    }

    public void TalkForSeconds(float seconds) {
        if (!face) {
            return;
        }

        enabled = true;
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(TalkForSecondsCoroutine(seconds));
    }

    IEnumerator TalkForSecondsCoroutine(float seconds) {
        yield return new WaitForSeconds(seconds);

        foreach (var m in Mouths) {
            face.SetBlendShapeWeight(m, 0);
        }
        enabled = false;
        coroutine = null;
    }

    void Start() {
        if (!GetComponent<DialogueActor>().IsMale()) {
            //GetComponent<DialogueActor>().OnSpeechTextChanged += FacialAnimator_OnSpeechTextChanged;
            face = transform.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => r.name == "Body").FirstOrDefault();
        }
    }

    void FacialAnimator_OnSpeechTextChanged(object sender, PhraseEventArgs e) {
        if (e.Phrase != null) {
            var text = e.Phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
            TalkForSeconds(text.Length / 10f);
        }
    }

    void Update() {
        t += 100f * Time.deltaTime * Speed;

        if (t < 100f) {
            face.SetBlendShapeWeight(Mouths[currentMouth], 100f - t);
            face.SetBlendShapeWeight(Mouths[nextMouth], t);
        } else {
            t = 0;
            face.SetBlendShapeWeight(Mouths[currentMouth], 0);
            face.SetBlendShapeWeight(Mouths[nextMouth], 100f);

            currentMouth = nextMouth;
            while (nextMouth == currentMouth) {
                nextMouth = UnityEngine.Random.Range(0, Mouths.Length);
            }
        }
    }

}
