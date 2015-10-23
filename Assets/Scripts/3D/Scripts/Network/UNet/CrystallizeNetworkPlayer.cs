using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CrystallizeNetworkPlayer : MonoBehaviour {

    public int playerID = -1;

    void Start() {
        Debug.Log("requesting avatar for: " + playerID);
        CrystallizeNetwork.Client.RequestAvatarFromServer(playerID, HandleAvatarLoaded);
    }

    void HandleAvatarLoaded(PlayerAvatarData avatar) {
        Debug.Log("got avatar for: " + playerID);
        GetComponent<FloatingNameHolder>().SetName(avatar.Name);
        GetComponentInChildren<AvatarConstructor>().SetAvatar(avatar.Appearance);
    }

}
