using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameTimeProcess : MonoBehaviour {

    public static bool LoopDay = false;

    public static GameTimeProcess GetTestInstance() {
        GameObject go = new GameObject("TestTime");
        var gtp = go.AddComponent<GameTimeProcess>();
        go.AddComponent<DontDestroyOnLoad>();
        return gtp;
    }

    public static GameTimeProcess GetTestInstance(IJobRef job) {
        GameObject go = new GameObject("TestTime");
        var gtp = go.AddComponent<GameTimeProcess>();
        go.AddComponent<DontDestroyOnLoad>();
        gtp.testJob = job;
        return gtp;
    }

    public static readonly ProcessFactoryRef<MorningSessionArgs, DaySessionArgs> MorningFactory = new ProcessFactoryRef<MorningSessionArgs, DaySessionArgs>();
    public static readonly ProcessFactoryRef<DaySessionArgs, object> DayFactory = new ProcessFactoryRef<DaySessionArgs, object>();
    public static readonly ProcessFactoryRef<EveningSessionArgs, HomeRef> EveningFactory = new ProcessFactoryRef<EveningSessionArgs, HomeRef>();
    public static readonly ProcessFactoryRef<NightSessionArgs, MorningSessionArgs> NightFactory = new ProcessFactoryRef<NightSessionArgs, MorningSessionArgs>();

    public string overrideMorningSession = "";

    public string MorningSessionArea {
        get {
            if (overrideMorningSession.IsEmptyOrNull()) {
                return "HomeScene";
            } else {
                return overrideMorningSession;
            }
        }
    }

    public string DaySessionArea {
        get {
            return "DaySession";
        }
    }

    public string EveningSessionArea {
        get {
            return "StreetSession";
        }
    }

    public string NightSessionArea {
        get {
            return "HomeScene";
        }
    }

    public IJobRef testJob = null;
    IProcess currentProcess;

    void Start() {
        Debug.Log("Main process started");
        DontDestroyOnLoad(gameObject);
        ProcessInitializer.Running = true;

        ProcessInitializer.Initialize();
        ProcessInitializer.InstantiateNewSceneObjects();

        var jr = new IDJobRef(JobID.FreeExplore);
        var args = new DaySessionArgs("", jr);

        if (PlayerDataConnector.GetTutorialViewed(FreeExploreProcessSelector.SpeakExplore)) {
            currentProcess = MorningFactory.Get(new MorningSessionArgs(MorningSessionArea, new HomeRef(0)), MorningSessionCallback, null);
        } else {
            currentProcess = DayFactory.Get(args, DaySessionCallback, null);
        }
    }

    void OnLevelWasLoaded(int level) {
        //Debug.Log("Making scene objects");
        ProcessInitializer.InstantiateNewSceneObjects();
        CrystallizeEventManager.UI.OnGoHomeClicked += UI_OnGoHomeClicked;
    }

    void UI_OnGoHomeClicked(object sender, EventArgs e) {
        PlayerData.Instance.Session.Position = PlayerManager.Instance.PlayerGameObject.transform.position.ToFloatArray();
        PlayerData.Instance.Session.Area = Application.loadedLevelName;
        
        if (currentProcess != null) {
            currentProcess.ForceExit();
        }
        NightSessionCallback(sender, new MorningSessionArgs(NightSessionArea, new HomeRef(0)));
        //EveningSessionCallback(null, null);
    }

    // for testing only
    DaySessionArgs _dayArgs;
    public void MorningSessionCallback(object sender, DaySessionArgs args) {
        _dayArgs = args;
        //Debug.Log("Morning exited:" + args);

        if (args == null) {
            PlayerData.Instance.Time.Session = (int)TimeSessionType.Evening;
            currentProcess = EveningFactory.Get(new EveningSessionArgs(EveningSessionArea), EveningSessionCallback, null);
        } else {
            PlayerData.Instance.Time.Session = (int)TimeSessionType.Day;
            currentProcess = DayFactory.Get(args, DaySessionCallback, null);
        }
    }

    void DaySessionCallback(object sender, object args) {
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Evening;
        PlayerDataLoader.Save();

        if (LoopDay) {
            currentProcess = DayFactory.Get(_dayArgs, DaySessionCallback, null);
        } else {
            currentProcess = MorningFactory.Get(new MorningSessionArgs(MorningSessionArea, new HomeRef(0)), MorningSessionCallback, null);
            //currentProcess = EveningFactory.Get(new EveningSessionArgs(EveningSessionArea), EveningSessionCallback, null);
        }
    }

    void EveningSessionCallback(object sender, HomeRef args) {
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Night;
        currentProcess = NightFactory.Get(new NightSessionArgs(NightSessionArea, args), NightSessionCallback, null);
    }

    void NightSessionCallback(object sender, MorningSessionArgs args) {
        PlayerData.Instance.Time.Day++;
        if (GameSettings.Instance.Local) {
            PlayerDataLoader.SaveLocal();
        } else {
            PlayerDataLoader.Save();
        }
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Morning;
        currentProcess = MorningFactory.Get(args, MorningSessionCallback, null);
    }

}
