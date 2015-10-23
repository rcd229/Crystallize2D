using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AnimatedUIItem : UIMonoBehaviour, ITemporaryUI<AnimatedCollectedItemArgs, object> {

    const float MaxSpeed = 2000f;
    const float PopInDuration = 0.25f;

    public Text phraseText;
    public Image backgroudImage;
    public AnimationCurve curve;

    public PhraseSequence phrase;
    Vector2 target;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(AnimatedCollectedItemArgs param1) {
        phrase = param1.Phrase;
        if (phrase != null) {
            phraseText.text = PlayerDataConnector.GetText(param1.Phrase);
            if (param1.Phrase.IsWord) {
                backgroudImage.color = GUIPallet.Instance.GetColorForWordCategory(param1.Phrase.Word.GetPhraseCategory());
            }
        }

        target = param1.TargetPosition;

        MainCanvas.main.Add(transform);

        if (param1.SetPos) {
            transform.position = Input.mousePosition;
        }
    }

    IEnumerator Start() {
        float t = 0.5f;
        while (t < 1f) {
            transform.localScale = curve.Evaluate(t) * Vector3.one;
            t += Time.deltaTime / PopInDuration;

            yield return null;
        }

        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, target, MaxSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 1f) {
                Complete.Raise(this, new EventArgs<object>(null));
                Close();
                break;
            }

            yield return null;
        }
    }

    public void Close() {
        Destroy(gameObject);
    }

}
