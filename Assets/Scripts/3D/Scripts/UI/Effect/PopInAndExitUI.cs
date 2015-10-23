using UnityEngine;
using System;
using System.Collections;

public class PopInAndExitUI : UIMonoBehaviour, ITemporaryUI<string, object> {

    const float PopInDuration = 0.25f;

    public AnimationCurve curve;

    public event EventHandler<EventArgs<object>> Complete;

    public virtual void Initialize(string param1) {

    }

    public void Close() {
        Exit();
        Destroy(gameObject);
    }

    IEnumerator Start() {
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;

        float t = 0;
        while (t < 1f) {
            transform.localScale = curve.Evaluate(t) * Vector3.one;
            t += Time.deltaTime / PopInDuration;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (t = 0; t < 1f; t += 2f * Time.deltaTime) {
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        Close();
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    void Exit() {
        Complete.Raise(this, null);
    }

}