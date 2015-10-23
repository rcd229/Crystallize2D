using UnityEngine;
using System.Collections;

public class UIBounceInAndFade : UIMonoBehaviour {

    const float PopInDuration = 0.25f;
    const float FadeDuration = 0.5f;

    public float riseSpeed = 300f;
    public AnimationCurve curve;

    protected float direction = 1f;

    IEnumerator Start() {
        float t = 0;
        while (t < 1f) {
            transform.localScale = curve.Evaluate(t) * Vector3.one;
            t += Time.deltaTime / PopInDuration;

            yield return null;
        }

        riseSpeed = 0;
        yield return new WaitForSeconds(0.5f);

        for (t = 0; t < 1f; t += Time.deltaTime / FadeDuration) {
            canvasGroup.alpha = 1f - t;

            yield return null;
        }

        Destroy(gameObject);
    }

    void Update() {
        transform.position += direction * Vector3.up * riseSpeed * Time.deltaTime;
    }

}