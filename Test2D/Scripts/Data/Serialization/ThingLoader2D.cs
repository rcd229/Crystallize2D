using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Util.Serialization;

public class ThingLoader2D : JsonLoader2D<ThingInstance2D> {

    public static readonly ThingLoader2D Instance = new ThingLoader2D();

    public override string LocalDirectoryPath { get { return DirectoryPath + "Things/"; } }

    public void Save(SceneThing2D sceneThing) {
        sceneThing.Thing.Position = (Vector2)sceneThing.transform.position;
        Save(sceneThing.Thing);
    }

    public void Delete(SceneThing2D sceneThing) {

    }

    public void SaveAll() {
        foreach (var sceneThing in ResourceCollection.GetOrCreate<SceneThing2D>()) {
            Save(sceneThing);
        }
    }

}
