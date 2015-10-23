using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CrystallizeNetworkScene : MonoBehaviour {

    const string ResourcePath = "Network/NetworkPlayer";
    const float MaxSayDistance = 30f;

    public static CrystallizeNetworkScene Instance { get; private set; }

    Dictionary<int, GameObject> playerGameObjects = new Dictionary<int, GameObject>();

    IEnumerator Start() {
        Instance = this;

        yield return new WaitForSeconds(0.1f);

        BlackScreenUI.Instance.FadeIn(1f, null);
        //UILibrary.ChatBox.Get(null);

        while (!CrystallizeNetwork.Connected) {
            yield return null;
        }
        CrystallizeNetwork.Client.SendPlayerDataToServer();

        CrystallizeNetwork.Client.RequestAllAvatarsFromServer();
        CrystallizeEventManager.Network.OnNetworkSpeechBubbleRequested += Network_OnNetworkSpeechBubbleRequested;
        CrystallizeEventManager.Network.OnNetworkEmoteRequested += Network_OnNetworkEmoteRequested;

        while (true) {
            UpdatePlayers();
            yield return null;
        }
    }

    void OnDisabled() {
        CrystallizeEventManager.Network.OnNetworkSpeechBubbleRequested -= Network_OnNetworkSpeechBubbleRequested;
        CrystallizeEventManager.Network.OnNetworkEmoteRequested -= Network_OnNetworkEmoteRequested;
    }

    void Network_OnNetworkEmoteRequested(object sender, NetworkEmoteArgs e) {
        if (DistanceToPlayer(e.PlayerID) < MaxSayDistance) {
            UILibrary.Emoticon.Get(new EmoticonInitArgs(playerGameObjects[e.PlayerID].transform, EmoticonType.Get(e.EmoteType)));
        }
    }

    void Network_OnNetworkSpeechBubbleRequested(object sender, NetworkSpeechBubbleRequestedEventArgs e) {
        if (DistanceToPlayer(e.PlayerID) < MaxSayDistance) {
            playerGameObjects[e.PlayerID].GetComponent<DialogueActor>().SetPhrase(e.GetPhraseSequence(), false, SpeechBubbleOpened);
        }
    }

    void SpeechBubbleOpened(GameObject speechBubble) {
        CoroutineManager.Instance.WaitAndDo(() => CloseSpeechBubble(speechBubble), new WaitForSeconds(10f));
    }

    void CloseSpeechBubble(GameObject speechBubble) {
        if (speechBubble) {
            Destroy(speechBubble);
        }
    }

    void UpdatePlayers() {
        CrystallizeNetwork.Client.SendPositionDataToServer();
        HashSet<int> removed = new HashSet<int>(playerGameObjects.Keys);
        foreach (var td in CrystallizeNetwork.Client.TransformTable.Transforms) {
            if (td.ConnectionID != CrystallizeNetwork.ConnectionID) {
                UpdatePlayer(td);
            }
            removed.Remove(td.ConnectionID);
        }

        foreach (var r in removed) {
            if (playerGameObjects.ContainsKey(r) && playerGameObjects[r]) {
                Destroy(playerGameObjects[r]);
                playerGameObjects.Remove(r);
                Debug.Log("removing: " + r);
            }
        }
    }

    void UpdatePlayer(TransformData data) {
        Debug.Log("adding player for: " + data.ConnectionID);
        if (!playerGameObjects.ContainsKey(data.ConnectionID)) {
            playerGameObjects[data.ConnectionID] = GameObjectUtil.GetResourceInstance(ResourcePath);
            playerGameObjects[data.ConnectionID].GetComponent<CrystallizeNetworkPlayer>().playerID = data.ConnectionID;
        }
        var go = playerGameObjects[data.ConnectionID];
        go.transform.position = data.Position;
        go.transform.rotation = data.Rotation;
    }

    public float DistanceToPlayer(int playerID) {
        if (!playerGameObjects.ContainsKey(playerID)) {
            return float.MaxValue;
        }

        return Vector3.Distance(PlayerManager.Instance.PlayerGameObject.transform.position, playerGameObjects[playerID].transform.position);
    }

}
