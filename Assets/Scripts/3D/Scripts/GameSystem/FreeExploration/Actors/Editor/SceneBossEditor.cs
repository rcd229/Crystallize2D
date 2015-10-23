using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(SceneBoss))]
public class SceneBossEditor : NamedGuidEditor<JobID> {
    protected override string ValueLabel { get { return "Job"; } }
}
