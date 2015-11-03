using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Util.Serialization;

public class TriggerLoader2D : JsonLoader2D<TriggerData2D> {

    public static readonly TriggerLoader2D Instance = new TriggerLoader2D();

    public override string LocalDirectoryPath { get { return DirectoryPath + "Triggers/"; } }

    public void Save(SceneTrigger2D sceneTrigger) {
        sceneTrigger.Trigger.Position = (Vector2)sceneTrigger.transform.position;
        sceneTrigger.Trigger.Orientation = Mathf.RoundToInt(sceneTrigger.transform.rotation.eulerAngles.z);
        sceneTrigger.Trigger.Size = Mathf.RoundToInt(sceneTrigger.transform.localScale.x);
        Save(sceneTrigger.Trigger);
    }

    public void SaveAll() {
        foreach (var sceneThing in ResourceCollection.GetOrCreate<SceneTrigger2D>()) {
            Save(sceneThing);
        }
    }

}
