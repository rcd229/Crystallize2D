using UnityEngine;
using System.Collections;

public class ClientProcess : MonoBehaviour {

    private float reconnection_time = 1f;
    // TODO: reconnect on disconnect
    IEnumerator Start() {
        if (GameSettings.Instance.Local) {
            yield break;
        }

        if (GameSettings.Instance.ServerIP == "") {
            Debug.Log("Connecting through master server");
            while (!CrystallizeNetwork.Connected) {
                MasterServer.RequestHostList("Crystallize");

                Debug.Log("Trying to connect");
                yield return new WaitForSeconds(3f);
            }

            while (true) {
                if (reconnection_time < 60f) {
                    reconnection_time *= 2;
                }
                yield return new WaitForSeconds(reconnection_time);
                while (!CrystallizeNetwork.Connected) {
                    reconnection_time = 1f;
                    MasterServer.RequestHostList("Crystallize");

                    Debug.Log("Trying to reconnect");
                    yield return new WaitForSeconds(reconnection_time);
                }
            }
        } else {
            CrystallizeNetwork.InitializeClient();

            yield return new WaitForSeconds(5f);

            while (!CrystallizeNetwork.Connected) {
                CrystallizeNetwork.Client.Reconnect();
                //Debug.Log("Trying to connect: " + GameSettings.Instance.ServerIP + "; " + CrystallizeNetwork.LegacyPort);
                yield return new WaitForSeconds(1f);
            }

            while (true) {
                if (reconnection_time < 60f) {
                    reconnection_time *= 2;
                }
                yield return new WaitForSeconds(reconnection_time);
                while (!CrystallizeNetwork.Connected) {
                    reconnection_time = 1f;
                    CrystallizeNetwork.Client.Reconnect();
                    Debug.Log("Trying to reconnect");
                    yield return new WaitForSeconds(reconnection_time);
                }
            }
        }
    }

    void OnMasterServerEvent(MasterServerEvent msEvent) {
        if (CrystallizeNetwork.Connected) {
            return;
        }
        
        if (msEvent == MasterServerEvent.HostListReceived) {
            var hostList = MasterServer.PollHostList();
            if (hostList.Length > 0) {
                foreach (var h in hostList) {
                    Debug.Log(h.ip + "; " + h.useNat + "; " + h.port + "; " + h.gameType + "; "+ h.gameName);
                }

                CrystallizeNetwork.InitializeClient(hostList[0]);
            }
        }
    }

    void OnConnectedToServer() {
        StartCoroutine(SavePlayerDataOnConnection());
        Debug.Log("connected to server");
    }

    void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Could not connect to server: " + error);
    }

    IEnumerator SavePlayerDataOnConnection() {
        while (!CrystallizeNetwork.Connected) {
            yield return new WaitForSeconds(1f);
        }
        PlayerDataLoader.Save();
    }

    void OnApplicationQuit() {
        PlayerDataLoader.Save();
    }

}