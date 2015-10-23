using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/SurveyPrompt")]
public class SurveyUI : MonoBehaviour, ITemporaryUI<object, object> {

    public GameObject buttons1;
    public GameObject buttons2;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object args1) {
        PlayerData.Instance.PersonalData.SurveysRequested++;
    }

    public void Close() {
        Destroy(gameObject);
        Complete.Raise(null, null);
    }

    public void Accept() {
        Application.OpenURL("https://cornell.qualtrics.com/SE/?SID=SV_8jNX52oa4tw20Fn");
        buttons1.SetActive(false);
        buttons2.SetActive(true);
    }

    public void Reject() {
        Close();
    }

   
}
