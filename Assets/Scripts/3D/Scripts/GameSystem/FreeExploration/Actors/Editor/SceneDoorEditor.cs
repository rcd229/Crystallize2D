using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SceneDoor))]
public class SceneDoorEditor : NamedGuidEditor<SceneData> {
    protected override string ValueLabel { get { return "Area"; } }
    protected override IEnumerable<NamedGuid> IDs { get { return SceneData.GetNamedGuids(); } }
}
