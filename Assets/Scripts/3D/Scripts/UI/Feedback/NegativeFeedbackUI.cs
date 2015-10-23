using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/NegativeFeedback")]
public class NegativeFeedbackUI : UIPanel, ITemporaryUI<string, object> {

    const string Yen = "¥ ";

    public Text currentMoneyText;
    public Text lostMoneyText;

    int moneyBefore = 0;
    int moneyAfter = 0;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(string param1) {
        SoundEffectManager.Play(SoundEffectType.NegativeFeedback);

        moneyBefore = PlayerData.Instance.Session.ReducedMoney;
        currentMoneyText.text = Yen + moneyBefore.ToString();

        PlayerData.Instance.Session.Mistakes++;
        moneyAfter = PlayerData.Instance.Session.ReducedMoney;
        lostMoneyText.text = "-" + Yen + (moneyBefore - moneyAfter).ToString();
        currentMoneyText.gameObject.SetActive(false);
        lostMoneyText.gameObject.SetActive(false);

        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;
        if (PlayerData.Instance.Session.isPromotion || param1 == ".") {
            StartCoroutine(WaitAndCloseCoroutine());
        } else {
            StartCoroutine(DropMoneyCoroutine());
        }
    }

    IEnumerator WaitAndCloseCoroutine() {
        yield return new WaitForSeconds(1f);

        Close();
    }

    IEnumerator DropMoneyCoroutine() {
        yield return new WaitForSeconds(.75f);

        currentMoneyText.gameObject.SetActive(true);
        currentMoneyText.GetComponent<CanvasGroup>().alpha = 0;
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f) {
            currentMoneyText.GetComponent<CanvasGroup>().alpha = t;

            yield return null;
        }
        currentMoneyText.GetComponent<CanvasGroup>().alpha = 1f;

        yield return new WaitForSeconds(0.5f);

        currentMoneyText.text = Yen + moneyAfter.ToString();
        lostMoneyText.gameObject.SetActive(true);
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            lostMoneyText.rectTransform.position += Vector3.down * 60f * Time.deltaTime;
            lostMoneyText.GetComponent<CanvasGroup>().alpha = 1f - t;
            yield return null;
        }
        lostMoneyText.gameObject.SetActive(false);

        Close();
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
    }

    public override void Close() {
        Complete.Raise(this, new EventArgs<object>(null));
        base.Close();
    }

}
