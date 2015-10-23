using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SystemButtonPanelUI : MonoBehaviour {

    public CanvasGroup gameSaved;
    public RectTransform reviewText;
    public GameObject homeButton;
    public GameObject chatButton;
    public GameObject questButton;
    public GameObject mapButton;

    GameObject collectUI;
    GameObject confidenceUI;

    Dictionary<HUDPartType, GameObject> buttons = new Dictionary<HUDPartType, GameObject>();

    IEnumerator Start() {
        yield return null;

        buttons[HUDPartType.Home] = homeButton;
        buttons[HUDPartType.ChatBox] = chatButton;
        buttons[HUDPartType.QuestStatus] = questButton;
        buttons[HUDPartType.Map] = mapButton;
        collectUI = GameObject.FindGameObjectWithTag(TagLibrary.CollectUI);
        Debug.Log(collectUI);
        confidenceUI = GameObject.FindGameObjectWithTag(TagLibrary.PlayerStatusUI);

        foreach (HUDPartType val in Enum.GetValues(typeof(HUDPartType))) {
            var partEnabled = PlayerDataConnector.GetHUDPartEnabled(val);
            //Debug.Log(val + "; " + partEnabled + "; " + PlayerDataConnector.GetHUDPartActive(val));
            SetHUDEnabled(val, partEnabled);
            if (partEnabled) {
                SetHUDActive(val, PlayerDataConnector.GetHUDPartActive(val));
            }
            //Debug.Log(val + "; " + PlayerDataConnector.GetHUDPartEnabled(val) + "; " + PlayerDataConnector.GetHUDPartActive(val));
        }

        CrystallizeEventManager.UI.OnHUDPartStateChanged += HandleHUDChanged;
    }

    void HandleHUDChanged(object sender, HUDPartArgs args) {
        SetHUDEnabled(args.Part, args.Value);
    }

    void SetHUDEnabled(HUDPartType part, bool val) {
        //Debug.Log("Setting: " + part + "; " + val);
        if (buttons.ContainsKey(part)) {
            buttons[part].SetActive(val);
        }
        SetHUDActive(part, val);
    }

    void SetHUDActive(HUDPartType part, bool val) {
        PlayerDataConnector.SetHUDPartActive(part, val);
        switch (part) {
            case HUDPartType.ChatBox:
                if (ChatBoxUI.Instance != null) {
                    if (val) {
                        ChatBoxUI.Instance.gameObject.SetActive(true);
                    } else {
                        ChatBoxUI.Instance.gameObject.SetActive(false);
                    }
                }
                break;

            case HUDPartType.Map:
                //Debug.Log("Map: " + val + "; " + Map.Instance);
                if (val && !Map.Instance) {
                    UILibrary.MiniMap.Get("");
                } if(!val && Map.Instance) {
                    Map.Instance.Close();
                }
                break;

            case HUDPartType.QuestStatus:
                if (val && !QuestHUD.Instance) {
                    UILibrary.QuestHUD.Get(null);
                } else if(!val && QuestHUD.Instance) {
                    QuestHUD.Instance.Close();
                }
                break;

            case HUDPartType.Collect:
                collectUI.SetActive(val);
                break;

            case HUDPartType.Confidence:
                confidenceUI.SetActive(val);
                break;
        }
    }

    void OnEnable() {
        PlayerState_AvailableReviewsChanged(null, new ReviewStateArgs());
        CrystallizeEventManager.PlayerState.AvailableReviewsChanged += PlayerState_AvailableReviewsChanged;
    }

    void OnDisable() {
        CrystallizeEventManager.PlayerState.AvailableReviewsChanged -= PlayerState_AvailableReviewsChanged;
    }

    void PlayerState_AvailableReviewsChanged(object sender, ReviewStateArgs e) {
        if (reviewText && reviewText.GetComponentInChildren<Text>()) {
            if (e.Reviews.Count > 0) {
                reviewText.gameObject.SetActive(true);
                reviewText.GetComponentInChildren<Text>().text = e.Reviews.Count.ToString();
            } else {
                reviewText.gameObject.SetActive(false);
            }
        }
    }

    public void ReturnHome() {
        CrystallizeEventManager.UI.RaiseGoHomeClicked(this, EventArgs.Empty);
    }

    public void OpenMap() {
        SetHUDActive(HUDPartType.Map, !PlayerDataConnector.MapOpenStatus);
    }

    public void OpenTaskStatus() {
        SetHUDActive(HUDPartType.QuestStatus, !PlayerDataConnector.QuestStatusPanelOpenStatus);
    }

    public void ToggleChatBox() {
        SetHUDActive(HUDPartType.ChatBox, !PlayerDataConnector.ChatBoxOpenStatus);
    }

    public void Save() {
        PlayerDataLoader.Save();
        StartCoroutine(SaveRoutine());
    }

    public void OpenMainMenu() {
        UISystem.main.OpenMainMenu();
    }

    public void OpenHelpMenu() {
        if (!HelpUI.Instance) {
            UILibrary.Help.Get(null);
        }
    }

    IEnumerator SaveRoutine() {
        gameSaved.alpha = 1f;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(UIUtil.FadeOutRoutine(gameSaved));
    }

}
