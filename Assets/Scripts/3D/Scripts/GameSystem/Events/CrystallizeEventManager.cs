using UnityEngine;
using System;
using System.Collections;

public partial class CrystallizeEventManager : MonoBehaviour {

    static bool quitting = false;
    static CrystallizeEventManager _main;

    public static CrystallizeEventManager main {
        get {
            if (!_main && !quitting) {
                UnityEngine.Debug.Log("Producing new event manager.");
                var go = new GameObject("CrystallizeEventSystem");
                _main = go.AddComponent<CrystallizeEventManager>();

                if (OnInitialized != null) {
                    OnInitialized(_main, EventArgs.Empty);
                }
            }
            return _main;
        }
    }

    public static bool Alive {
        get {
            return _main;
        }
    }

    public static CrystallizeEventManager GetInstance() {
        return main;
    }

    public static PlayerStateEvents PlayerState { get { return main.playerState; } }
    public static EnvironmentEvents Environment { get { return main.environment; } }
    public static UIEvents UI { get { return main.ui; } }
    public static NetworkEvents Network { get { return main.network; } }
    public static DebugEvents Debug { get { return main.debug; } }
    public static InputEvents Input { get { return main.input; } }

    public static EventHandler OnInitialized;
    public static EventHandler OnLoadComplete;
    public static EventHandler OnQuit;

    public event EventHandler OnLoad;
    public void RaiseLoad(object sender, EventArgs e) { OnLoad.Raise(sender, e); }
    public event EventHandler OnSave;
    public void RaiseSave(object sender, EventArgs e) { OnSave.Raise(sender, e); }

    PlayerStateEvents playerState;
    EnvironmentEvents environment;
    UIEvents ui;
    NetworkEvents network;
    DebugEvents debug;
    InputEvents input;

    void Awake() {
        playerState = new PlayerStateEvents();
        environment = new EnvironmentEvents();
        ui = new UIEvents();
        network = new NetworkEvents();
        debug = new DebugEvents();
        input = new InputEvents();

        quitting = false;
    }

    void OnApplicationQuit() {
        quitting = true;

        if (OnQuit != null) {
            OnQuit(this, EventArgs.Empty);
            OnQuit = null;
        }
    }

    IEnumerator Start() {
        yield return null;

        if (OnLoadComplete != null) {
            OnLoadComplete(this, EventArgs.Empty);
        }
    }

}
