using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RemoteServer : MonoBehaviour {

    //public const int Port = 3389;

    //public GameObject rpcFunctionsPrefab;

    void Start() {
        //var config = new ConnectionConfig();
        //var topo = new HostTopology(config, 50);
        //NetworkTransport.AddHost(topo);
        //5987
        //Network.InitializeServer(50, Port, !Network.HavePublicAddress());
        //MasterServer.RegisterHost("Crystallize", "c2");
    }

    void OnServerInitialized() {
        Debug.Log("server intialized");
        //Network.Instantiate(rpcFunctionsPrefab, Vector3.zero, Quaternion.identity, 0);
    }

}