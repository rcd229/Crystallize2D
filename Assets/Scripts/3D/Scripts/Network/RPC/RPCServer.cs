using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class RPCServer : ICrystallizeServer {

    public ServerLogger Logger { get; private set; }
    public LeaderBoard LeaderBoard { get; private set; }
    public PlayerTransformTable TransformTable {
        get { return RPCMessager.Instance.TransformTable; }
    }

    public bool IsStarted {
        get {
            return Network.isServer;
        }
    }

    public void Initialize() {
        Logger = new ServerLogger();
        LeaderBoard = new LeaderBoard();
    }

    public void Close() {
        Network.Disconnect();
    }

    public void BroadcastTransforms() { }

}
