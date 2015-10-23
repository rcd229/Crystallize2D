#define LEGACY_NETWORK

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class NetworkMonitor : MonoBehaviour {

    public Text text;

#if LEGACY_NETWORK
    void Update() {
        var s = "Server started: " + Network.isServer; //Network.isServer;
        s += "\nInternal: " + Network.player.ipAddress;
        s += "\nExternal: " + Network.player.externalIP;
        s += "\nConnections: " + Network.connections.Length;//Network.connections.Length;
        foreach (var player in Network.connections) {
            s += "\nPing [" + player.ipAddress + "]: " + Network.GetAveragePing(player);
        }

        foreach (var td in CrystallizeNetwork.Server.TransformTable.Transforms) {
            s += "\nTransform " + td.ConnectionID + ": \t" + td.Position + "; \t" + td.Rotation;
        }

        text.text = s;
    }
#else
    void Update() {
        var s = "Server started: " + NetworkServer.active; //Network.isServer;
        s += "\nInternal: " + Network.player.ipAddress;
        s += "\nExternal: " + Network.player.externalIP;
        s += "\nConnections: " + NetworkServer.connections.Count;//Network.connections.Length;
        int bytes;
        int messages;
        NetworkServer.GetStatsIn(out messages, out bytes);
        s += "\nIn (msgs): " + messages;
        s += "\nIn (bytes): " + bytes;
        int bufferedMsgs;
        int bufferedBytes;
        NetworkServer.GetStatsOut(out messages, out bufferedMsgs, out bytes, out bufferedBytes);
        s += "\nOut (msgs): " + messages;
        s += "\nOut (buffered msgs): " + bufferedMsgs;
        s += "\nOut (bytes): " + bytes;
        s += "\nOut (bytes per second): " + bufferedBytes;

        foreach (var td in CrystallizeNetwork.Server.PlayerTransformTable.Transforms) {
            s += "\nTransform " + td.ConnectionID + ": \t" + td.Position + "; \t" + td.Rotation;
        }
        
        text.text = s;
    }
#endif

}