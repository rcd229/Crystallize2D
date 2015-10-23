#define LEGACY_NETWORK

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CrystallizeNetwork {
    public const int LegacyPort = 7890;
    //public const int LegacyPort = 339;
    public const int UNetPort = 7891;
    public const int TransportPort = 7892;

    public static ICrystallizeClient Client { get; private set; }
    public static ICrystallizeServer Server { get; private set; }

    static CrystallizeNetwork() {
        if (!GameSettings.Instance.MasterServerIP.IsEmptyOrNull()) {
            MasterServer.ipAddress = GameSettings.Instance.MasterServerIP;
        }
    }

    public static bool ServerStarted {
        get {
            if (Server == null) {
                return false;
            }

            return Server.IsStarted;
        }
    }

    public static bool Connected {
        get {
            if (Client == null) {
                return false;
            }

            return Client.Connected;
        }
    }

    public static int ConnectionID { get { return Client.ConnectionID; } }

    public static void InitializeServer() {
#if LEGACY_NETWORK
        Server = new RPCServer();
#else
        Server = new CrystallizeServer();
#endif

        Server.Initialize();
        new GameObject("ServerProcess").AddComponent<ServerProcess>();
    }

    public static void InitializeClient() {
        InitializeClient(GameSettings.Instance.ServerIP);
    }

    public static void InitializeClient(string ip) {
        if (Client != null) {
            Client.Close();
        }

#if LEGACY_NETWORK
        Client = new RPCClient(ip);
#else
        Client = new CrystallizeClient(ip);
#endif

        Client.Initialize();
    }

    public static void InitializeClient(HostData hostData) {
        if (Client != null) {
            Client.Close();
        }

        Client = new RPCClient(hostData);
        Client.Initialize();
    }

    public static ConnectionConfig GetConfig(out byte reliableChannelID, out byte unreliableChannelID) {
        var config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableFragmented);
        unreliableChannelID = config.AddChannel(QosType.UnreliableSequenced);
        return config;
    }
}
