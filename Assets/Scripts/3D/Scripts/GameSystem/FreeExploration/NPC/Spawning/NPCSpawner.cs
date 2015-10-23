using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NPCSpawner : MonoBehaviour {

    int npcCount = 10;
    List<GameObject> tempNPCs = new List<GameObject>();

    INPCSpawner defaultSpawner;

    void Start() {
        defaultSpawner = new ConversationNPCSpawner();
        PlayerProximity.Instance.ProximityChanged += Instance_ProximityChanged;
    }

    void Instance_ProximityChanged(object sender, ProximityArgs e) {
        for (int i = 0; i < tempNPCs.Count; i++) {
            if (!tempNPCs[i]) {
                tempNPCs.RemoveAt(i);
                i--;
            } else {
                var pos = tempNPCs[i].transform.position.XZToVector2();
                if (!e.PlayerArea.Contains(pos) && !tempNPCs[i].GetComponent<Pin>()) {
                    Destroy(tempNPCs[i]);
                    tempNPCs.RemoveAt(i);
                    i--;
                }
            }
        }

        if (e.NewCells.Count == 0) {
            return;
        }


        var newCells = new List<Rect>(e.NewCells);
        int tries = 0;
        while (tries < 100
            && tempNPCs.Count < npcCount
            && newCells.Count > 0) {
            var index = UnityEngine.Random.Range(0, newCells.Count);
            var cell = newCells[index];
            Vector3 point;
            if (cell.ScatterPoint(50f, out point)) {
                GameObject npc = null;
                var context = new SpawnNPCContext(point, tempNPCs);
                if (UnityEngine.Random.value < 0.5f) {
                    npc = defaultSpawner.SpawnNPC(context);
                } else {
                    var availableSpawners = GameData.Instance.Spawn.SpawnableNPCs.Where(s => s.CanSpawn(context)).ToList();
                    availableSpawners.Add(defaultSpawner);
                    var selectedSpawner = availableSpawners.GetRandom();
                    //Debug.Log("Spawning with: " + selectedSpawner);
                    npc = selectedSpawner.SpawnNPC(context);
                }
                MaterialFadeIn.Get(npc);
                npc.transform.position = point;
                tempNPCs.Add(npc);
            } else {
                //Debug.Log("Failed to place NPC: " + cell);
            }

            newCells.RemoveAt(index);
            tries++;
        }
    }

}
