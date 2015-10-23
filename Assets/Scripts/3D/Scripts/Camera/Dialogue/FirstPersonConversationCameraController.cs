using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FirstPersonConversationCameraController : MonoBehaviour, ITemporaryUI<ConversationArgs, object> {

    const float Distance = 1.2f;
    const string TargetsPath = "Camera/DialogueTargets1";
    const string BubbleTargetPath = "UI/Element/DialogueSpeechBubbleTarget";

    public static FirstPersonConversationCameraController GetInstance() {
        return new GameObject("FirstPersonConversationCameraController").AddComponent<FirstPersonConversationCameraController>();
    }

    public event EventHandler<EventArgs<object>> Complete;

    ConversationArgs args;
    GameObject targets;
    GameObject speechBubbleTarget;
    GameObject playerObject;
    GameObject playerAppearance;

    bool quitting = false;

    public void Initialize(ConversationArgs args) {
        this.args = args;
    }

    public void Close() {
        BlackScreenUI.Instance.Initialize(0.1f, 0.5f, 0.1f);
        BlackScreenUI.Instance.Complete += Instance_Complete;
        //Instance_Complete(null, null);
    }

    void Instance_Complete(object sender, EventArgs<object> e) {
        if (gameObject) {
            Destroy(gameObject);

            MainCanvas.main.PopLayer(CanvasBranch.Default);
            WorldCanvas.Instance.gameObject.SetActive(true);
            //if(playerObject.gameObject)
            if (playerObject != null) {
                playerObject.SetActive(true);
            }

            Complete.Raise(this, null);
            Complete = null;
        }
    }

    void Start() {
        MainCanvas.main.PushLayer(CanvasBranch.Default);
        WorldCanvas.Instance.gameObject.SetActive(false);
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Home, false);

        targets = GameObjectUtil.GetResourceInstance(TargetsPath);
        targets.transform.position = args.Target.transform.position;
        targets.transform.rotation = args.Target.transform.rotation;

        var camTarget= targets.transform.Find("CameraTarget");
        OmniscientCamera.main.Suspend();
        OmniscientCamera.main.GetComponent<Camera>().fieldOfView = 45f;
        OmniscientCamera.main.SetPosition(camTarget.position);
        OmniscientCamera.main.SetRotation(camTarget.rotation);

        var plrTarget = targets.transform.Find("PlayerTarget");
        playerObject = PlayerManager.Instance.PlayerGameObject;
        playerObject.SetActive(false);
        //PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
        playerAppearance = AppearanceLibrary.CreateObject(PlayerData.Instance.Appearance.GetResourceData());
        playerAppearance.transform.position = plrTarget.position;
        playerAppearance.transform.rotation = plrTarget.rotation;

        speechBubbleTarget = GameObjectUtil.GetResourceInstance(BubbleTargetPath);
        MainCanvas.main.Add(speechBubbleTarget.transform);
    }

    void OnDestroy() {
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Home, true);

        if (OmniscientCamera.main) {
            OmniscientCamera.main.Resume();
            OmniscientCamera.main.GetComponent<Camera>().fieldOfView = 60f;
        }

        if (targets) {
            Destroy(targets);
        }

        if (playerAppearance) {
            Destroy(playerAppearance);
        }
    }

    void OnApplicationQuit() {
        Debug.Log("quitting");
        quitting = true;
    }

}