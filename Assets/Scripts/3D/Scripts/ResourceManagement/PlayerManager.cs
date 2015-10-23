using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Util.Serialization;

public class PlayerManager : MonoBehaviour {

    static PlayerManager _instance;
    public static PlayerManager Instance {
        get {
            if (_instance != null && _instance.quitting) {
                return null;
            }

            if (!_instance) {
                Debug.Log("making player manager");
                _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }

    public static bool MovementLocked {
        get {
            return Instance.movementLocks.Count != 0;
        }
    }

    public static void LockMovement(object lockObject) {
        Instance.movementLocks.Add(lockObject);
    }

    public static void UnlockMovement(object lockObject) {
        if (_instance && !_instance.quitting) {
            _instance.movementLocks.Remove(lockObject);
        }
    }

    //public event EventHandler OnLevelChanged;

    int previousPlayerLevel = 0;
    GameObject _playerGameObject;
    HashSet<object> movementLocks = new HashSet<object>();

    bool quitting = false;

    GameObject lastTarget;
    HashSet<GameObject> targets = new HashSet<GameObject>();

    public GameObject PlayerGameObject {
        get {
            if (!_playerGameObject) {
                _playerGameObject = GameObject.FindGameObjectWithTag("Player");
            }
            return _playerGameObject;
        }
        set {
            _playerGameObject = value;
        }
    }

    public GameObject OtherGameObject {
        get {
            if (PlayerID == 0) {
                return GameObject.FindGameObjectWithTag("OtherPlayer");
            } else {
                return GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    public float InteractionRange {
        get {
            return 3f;
        }
    }

    public int OtherPlayerLevelID { get; set; }

    public int PlayerCount { get; set; }

    public int PlayerID {
        get {
            if (!PlayerGameObject) {
                return -1;
            }

            if (PlayerGameObject.CompareTag("Player")) {
                return 0;
            }
            return 1;
        }
    }

    public GameObject TriggerTarget {
        get {
            if (lastTarget) {
                return lastTarget;
            }
            if (targets.Count > 0) {
                return targets.First();
            }
            return lastTarget;
        }
    }

    void Awake() {
        PlayerCount = 1;
        OtherPlayerLevelID = -1;
    }

    void Start() {
        //PlayerData.Instance.LevelData.SetLevelState(Application.loadedLevelName, LevelState.Unlocked);
        //Debug.Log(playerData.LevelData.GetLevelState(Application.loadedLevelName));

        //CrystallizeEventManager.UI.OnInteractiveDialogueOpened += HandleOnInteractiveDialogueOpened;
        //CrystallizeEventManager.UI.OnInteractiveDialogueClosed += HandleOnInteractiveDialogueClosed;

        CrystallizeEventManager.Environment.OnTriggerEntered += Environment_OnTriggerEntered;
        CrystallizeEventManager.Environment.OnTriggerExited += Environment_OnTriggerExited;
    }

    void OnDestroy() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.Environment.OnTriggerEntered -= Environment_OnTriggerEntered;
            CrystallizeEventManager.Environment.OnTriggerExited -= Environment_OnTriggerExited;
        }
    }

    void Environment_OnTriggerEntered(object sender, GameObjectArgs e) {
        var t = lastTarget;
        if (!targets.Contains(e.Target)) {
            targets.Add(e.Target);
        }
        lastTarget = e.Target;

        if (t != lastTarget) {
            CrystallizeEventManager.Environment.RaiseEnvironmentTargetChanged(this, new GameObjectArgs(lastTarget));
        }
    }

    void Environment_OnTriggerExited(object sender, GameObjectArgs e) {
        //Debug.Log("Exited");
        var t = lastTarget;
        if (targets.Contains(e.Target)) {
            targets.Remove(e.Target);
            if (lastTarget == e.Target && targets.Count > 0) {
                lastTarget = targets.First();
            } else {
                lastTarget = null;
            }
        }
        if (t != lastTarget) {
            CrystallizeEventManager.Environment.RaiseEnvironmentTargetChanged(this, new GameObjectArgs(lastTarget));
        }
    }

    //void HandleOnInteractiveDialogueOpened(object sender, EventArgs e) {
    //    State = PlayerState.Conversation;
    //}

    //void HandleOnInteractiveDialogueClosed(object sender, EventArgs e) {
    //    State = PlayerState.Exploring;
    //}

    //void Update() {
    //    if (previousPlayerLevel != PlayerData.Instance.InventoryState.Level) {
    //        previousPlayerLevel = PlayerData.Instance.InventoryState.Level;

    //        if (OnLevelChanged != null) {
    //            OnLevelChanged(this, EventArgs.Empty);
    //        }
    //    }
    //}

    public GameObject GetPlayerGameObject(int playerID) {
        if (playerID == 0) {
            return GameObject.FindGameObjectWithTag("Player");
        } else {
            return GameObject.FindGameObjectWithTag("OtherPlayer");
        }
    }

    public int GetPlayerID(GameObject go) {
        if (go.CompareTag("Player")) {
            return 0;
        } else if (go.CompareTag("OtherPlayer")) {
            return 1;
        }
        return -1;
    }

    public bool InInteractionRange(Transform target) {
        if (!target) {
            return false;
        }
        return Vector3.Distance(PlayerGameObject.transform.position, target.position) < InteractionRange;
    }

    void OnApplicationQuit() {
        quitting = true;
    }

}
