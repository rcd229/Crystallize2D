using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

[CustomEditor(typeof(SceneConnectionTarget))]
public class SceneConnectionTargetEditor : NamedGuidEditor<SceneData> {
    protected override string ValueLabel { get { return "Area"; } }
    protected override IEnumerable<NamedGuid> IDs { get { return SceneData.GetNamedGuids(); } }
}
