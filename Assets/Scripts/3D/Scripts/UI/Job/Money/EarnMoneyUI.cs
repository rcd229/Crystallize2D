using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/EarnMoney")]
public class EarnMoneyUI : MonoBehaviour, ITemporaryUI<EarnedMoneyArgs, object> {

    const string Yen = "¥ ";

    public GameObject elementPrefab;
    public Text baseText;
    public Text lostText;
    public Text totalEarnedText;
    public Text allTotalText;
    public RectTransform elementParent;

    int earned;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(EarnedMoneyArgs args) {
        baseText.text = args.BaseMoney.ToString();
        lostText.text = args.LostMoney.ToString();
        UIUtil.GenerateChildren(args.Modifiers, elementParent, CreateChild);

        totalEarnedText.text = Yen + 0;
        earned = args.GetValue();
        allTotalText.text = Yen + (PlayerData.Instance.Money - earned).ToString();
        Debug.Log("Initializing earn money");
    }

    public void Close() {
        Destroy(gameObject);
    }

	// Use this for initialization
	IEnumerator Start () {
        yield return null;

        float duration = 2f;
        for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
            totalEarnedText.text = Yen + (int)Mathf.Lerp(0, earned, t);
            allTotalText.text = Yen + (int)(PlayerData.Instance.Money - Mathf.Lerp(earned, 0, t));

            if (Input.GetMouseButtonDown(0)) {
                break;
            }

            yield return null;
        }
        totalEarnedText.text = Yen + earned;
        allTotalText.text = Yen + PlayerData.Instance.Money;

        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                break;
            }

            yield return null;
        }

        Complete.Raise(this, new EventArgs<object>(null));
        Close();
	}

    GameObject CreateChild(ValueModifier modifier) {
        var instance = Instantiate<GameObject>(elementPrefab);
        instance.transform.SetParent(elementParent, false);
        instance.GetComponent<EarnMoneyModifierUI>().Initialize(modifier);
        return instance;
    }

}
