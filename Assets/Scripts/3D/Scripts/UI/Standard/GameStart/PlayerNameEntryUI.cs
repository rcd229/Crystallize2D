using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerNameEntryUI : UIPanel, ITemporaryUI<object, PlayerData> {

    const string ResourcePath = "UI/PlayerNameInputUI";
    public static PlayerNameEntryUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<PlayerNameEntryUI>(ResourcePath);
    }

    public Button LoadGame;
    public Button Cancel;
    public InputField PlayerName;
    public Text errorMessage;

    public event EventHandler<EventArgs<PlayerData>> Complete;
    public event EventHandler<EventArgs<object>> Canceled;

    public void Initialize(object param1) {
        Debug.Log("player loader initialized");
        errorMessage.enabled = false;
        LoadGame.onClick.AddListener(HandleOnClicked);
        Cancel.onClick.AddListener(HandleCancel);
    }

    void HandleCancel() {
        Canceled.Raise(this, null);
    }

    void HandleOnClicked() {
        Debug.Log("load button clicked");
        LoadGame.enabled = false;
        string name = PlayerName.text;
        CrystallizeNetwork.Client.RequestPlayerDataFromServer(name, HandlePlayerDataResponse);
        //RPCFunctions.Instance.RequestPlayerDataFromServer(name);
    }

    void HandlePlayerDataResponse(PlayerData data) {
        if (data == null) {
            errorMessage.enabled = true;
            errorMessage.gameObject.SetActive(true);
            LoadGame.enabled = true;
        } else {
            Debug.Log("Loaded player data from server: " + data.PersonalData.Name);
            PlayerData.Initialize(data);
            Exit(PlayerData.Instance);
        }
    }

    void Exit(PlayerData data) {
        Complete.Raise(this, new EventArgs<PlayerData>(data));
    }
}
