using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrentMoneyUIMonitor : MonoBehaviour {

    public Text text;
    public string currency;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        text.text = currency + PlayerData.Instance.Money;
    }
}
