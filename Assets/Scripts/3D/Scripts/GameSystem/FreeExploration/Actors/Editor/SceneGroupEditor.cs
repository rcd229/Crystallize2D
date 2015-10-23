using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SceneNPCGroup))]
public class SceneGroupEditor : NamedGuidEditor<NPCGroup> {

    protected override string ValueLabel { get { return "Group"; } }
    protected override IEnumerable<NamedGuid> IDs { get { return NPCGroup.GetNamedGuids(); } }

}
