using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PointPlaceQuestSpawnPoint : MonoBehaviour {

    IEnumerator Start() {
        var spawnContext = new SpawnNPCContext(transform.position, new List<GameObject>());
        var spawner = new PointPlaceQuest01();
        if (PlayerData.Instance.QuestData.Flags.Contains(NPCQuestFlag.PointPlaceUnlockedFlag) 
            && spawner.CanSpawn(spawnContext)) {
            var npc = spawner.SpawnNPC(spawnContext);

            yield return null;

            npc.GetComponent<Rigidbody>().rotation = transform.rotation;
            //Debug.Log(npc.transform.rotation.eulerAngles + "; " + transform.rotation.eulerAngles);
        }

//		PlayerProximity.Instance.ProximityChanged += HandleProximityChanged;;
    }

//    void HandleProximityChanged (object sender, ProximityArgs e)
//    {
//		PlayerManager.Instance.PlayerGameObject;
//		var npc = NPCManager.Instance.SpawnNPC();
//		npc.AddComponent<SetAnimation>().Set(TagLibrary.Sit);
//    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        GameObjectUtil.DoTargetGizmo(transform, "Point place spawn", Color.red);
    }
#endif

}
