using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class EquipmentItemRef {

    public static void Unset(GameObject target) {
        var items = target.GetComponentsInChildren<Transform>().Where(t => t.CompareTag("Item"));
        foreach (var i in items) {
            GameObject.Destroy(i);
        }
    }

    public string ItemName { get; set; }

    public EquipmentItemGameData GameDataInstance {
        get {
            return GameData.Instance.Equipment.Items.Get(ItemName);
        }
    }

    public EquipmentClassResources ClassResources {
        get {
            return GameData.Instance.Equipment.Classes.Get(GameDataInstance.Class).GetResources();
        }
    }

    public EquipmentItemRef(string itemName) {
        ItemName = itemName;
    }

    public GameObject SetTo(GameObject actor) {
        if (ItemName == null) {
            return null;
        }

        //Debug.Log("setting res");
        //ClassResources.SetTo(actor);

        if (!GameDataInstance.HasPrefab()) {
            return null;
        }

        var instance = GameObject.Instantiate<GameObject>(GameDataInstance.GetPrefab());
        var bone = actor.transform.FindBone("Item_L");
        instance.tag = "Item";
        instance.transform.SetParent(bone);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        return instance;
    }

    public void Use(GameObject actor, bool effect = true) {
        var a = actor.GetComponentInChildren<Animator>();
        a.CrossFade("Use", 0.1f);
        
        if (ItemName == null) {
            return;
        }

        if (GameDataInstance.Effect == null) {
            return;
        }

        if (effect) {
            if (!GameDataInstance.Effect.IsEmptyOrNull()) {
                CoroutineManager.Instance.WaitAndDo(() => {
                    var go = EffectLibrary.GetEffect(GameDataInstance.Effect);
                    go.transform.position = actor.transform.position;
                    go.transform.rotation = actor.transform.rotation;
                },
                new WaitForSeconds(0.25f));
            }
        }
    }

}
