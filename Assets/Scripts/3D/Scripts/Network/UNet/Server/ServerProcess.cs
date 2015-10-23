#define LEGACY_NETWORK

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class ServerProcess : MonoBehaviour {

    const int ConnectionCount = 64;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start() {
#if LEGACY_NETWORK
        //!Network.HavePublicAddress()
        Network.InitializeServer(ConnectionCount, CrystallizeNetwork.LegacyPort, true);
        MasterServer.RegisterHost("Crystallize", "crystallize0");
#endif

        while (!CrystallizeNetwork.ServerStarted) {
            yield return null;
        }

#if LEGACY_NETWORK
        Debug.Log("Server started.");
        Network.Instantiate(Resources.Load<GameObject>("RPCMessager"), Vector3.zero, Quaternion.identity, 0);
#endif

        StartCoroutine(PlayerTransformUpdate());

        while (true) {
            yield return new WaitForSeconds(60f);
            CrystallizeNetwork.Server.Logger.Flush();
        }
    }

    IEnumerator PlayerTransformUpdate() {
        while (true) {
            CrystallizeNetwork.Server.BroadcastTransforms();
            yield return new WaitForSeconds(0.05f);
        }
    }

    void OnApplicationQuit() {
        if (CrystallizeNetwork.Server != null) {
            CrystallizeNetwork.Server.Close();
        }
    }

}
