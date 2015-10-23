using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Builder : MonoBehaviour {
    [MenuItem("Build/Build server")]
    public static void BuildServer() {
        var levels = new string[] { "assets/crystallize/Scenes/Remote/Server.unity" };
        BuildPipeline.BuildPlayer(levels, "ServerBuild/crystallize.exe", BuildTarget.StandaloneWindows, BuildOptions.ShowBuiltPlayer);
    }
}
