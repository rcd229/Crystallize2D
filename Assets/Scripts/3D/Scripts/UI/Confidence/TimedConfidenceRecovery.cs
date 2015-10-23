using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TimedConfidenceRecovery : MonoBehaviour {

    void OnEnable() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        while (true) {
            if (PlayerData.Instance.Session.Confidence <= 0) {
                PlayerData.Instance.Session.Confidence = 1;
            }

            if (PlayerData.Instance.Proficiency.ReserveConfidence > 0 && PlayerData.Instance.Session.Confidence < PlayerData.Instance.Proficiency.Confidence) {
                PlayerData.Instance.Session.Confidence += 1;
                PlayerData.Instance.Proficiency.ReserveConfidence -= 1;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

}
