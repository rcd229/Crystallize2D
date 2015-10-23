using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(SceneNPC))]
public class SceneNPCEditor : NamedGuidEditor<NPCID> {

    protected override string ValueLabel { get { return "NPC"; } }

}
