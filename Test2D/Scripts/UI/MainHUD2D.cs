using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MainHUD2D : MonoBehaviour {

    enum HUDMode {
        Play,
        Edit
    }

    public static MainHUD2D Instance { get; private set; }

    public static void SetPlay() {
        if (Instance) Instance.SetMode(HUDMode.Play);
    }

    public static void SetEdit() {
        if (Instance) Instance.SetMode(HUDMode.Edit);
    }

    public GameObject playHUD;
    public GameObject editHUD;

    void Awake() {
        Instance = this;
    }

    void Start() {
        SetMode(HUDMode.Play);
    }

    void SetMode(HUDMode mode) {
        switch (mode) {
            case HUDMode.Play:
                playHUD.SetActive(true);
                editHUD.SetActive(false);
                break;
            case HUDMode.Edit:
                playHUD.SetActive(false);
                editHUD.SetActive(true);
                break;
        }
    }
}
