using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AnimatedCollectedItemUI : UIMonoBehaviour, ITemporaryUI<AnimatedCollectedItemArgs, PhraseSequence> {

    const float MaxSpeed = 2000f;
    const float PopInDuration = 0.25f;

    public Text phraseText;
    public Image backgroudImage;
    public AnimationCurve curve;

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    public PhraseSequence phrase;
    Vector2 target;
    bool isDragging = false;

    public void Initialize(AnimatedCollectedItemArgs param1) {
        phrase = param1.Phrase;
        isDragging = param1.IsDragging;
        if (phrase != null) {
            phraseText.text = PlayerDataConnector.GetText(param1.Phrase);
        //    if (param1.Phrase.IsWord) {
        //        backgroudImage.color = GUIPallet.Instance.GetColorForWordCategory(param1.Phrase.Word.GetPhraseCategory());
        //    }
        }

        target = param1.TargetPosition;

        MainCanvas.main.Add(transform);

        if (param1.SetPos) {
            transform.position = Input.mousePosition;
        }
    }

    public void Close() {
        Destroy(gameObject);
    }

    IEnumerator Start() {
        if (isDragging) {
            StartCoroutine(DragItem());
        }

        float t = 0.5f;
        while (t < 1f) {
            transform.localScale = curve.Evaluate(t) * Vector3.one;
            t += Time.deltaTime / PopInDuration;

            yield return null;
        }

        if (!isDragging) {
            StartCoroutine(MoveItem());  
        }
    }

    IEnumerator MoveItem() {
        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, target, MaxSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 1f) {
                Complete.Raise(this, new EventArgs<PhraseSequence>(phrase));
                Close();
                break;
            }

            yield return null;
        }

        //Close();
    }

    IEnumerator DragItem() {
        while (Input.GetMouseButton(0)) {
            transform.position = Input.mousePosition;

            yield return null;
        }

        if (!TryDrop()) {
            while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime;

                yield return null;
            }
        }

        Close();
    }

    bool TryDrop() {
        var raycastResults = new List<RaycastResult>();
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, raycastResults);
        //foreach (var r in raycastResults) {
        //    Debug.Log("Raycast: " + r.gameObject);
        //}
        
        foreach (var r in raycastResults) {
            var dropHandler = r.gameObject.GetInterface<IPhraseDropHandler>();
            if(dropHandler != null){
                dropHandler.HandlePhraseDropped(phrase);
                CrystallizeEventManager.UI.RaisePhraseDropped(this, new PhraseEventArgs(phrase));
                return true;
            }
        }

        CrystallizeEventManager.UI.RaisePhraseDropped(this, new PhraseEventArgs(phrase));
        return false;
    }

}
