using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneData : UniqueIDData<SceneData> {

    public static readonly SceneData SchoolHallwayFromOutdoor = new SceneData(new Guid("7a028fb8885446bca32da2deb46c638d"), "Hallway from outdoor", "SchoolHallwaySession");
    public static readonly SceneData SchoolHallwayFromClassroom = new SceneData(new Guid("2e3d11a502ff4b42805237ab02c28cb2"), "Hallway from classroom", "SchoolHallwaySession");
    public static readonly SceneData SchoolClassroomFromHallway = new SceneData(new Guid("3a2bde64ad62473d8e36f70d5096fd2b"), "Classroom", "SchoolClassroomSession");
    public static readonly SceneData SchoolOutdoorFromHallway = new SceneData(new Guid("697f8e369a264c268fa82821a84562f8"), "Outdoor from hall", "OutdoorSchoolSession");
    public static readonly SceneData StreetFromSchool = new SceneData(new Guid("5e1876a7cff34a2eb5c126c74e2937f6"), "Street from school", "StreetSession");

    public readonly string SceneID;

    public SceneData(Guid guid, string name, string sceneID) : base(name, guid) {
        this.SceneID = sceneID;
    }

    public bool GetEnabled() {
        return !PlayerData.Instance.QuestData.Flags.Contains(guid);
    }

    public void SetEnabled(bool enabled) {
        if (enabled) {
            PlayerDataConnector.UnsetFlags(guid);
        } else {
            PlayerDataConnector.SetFlags(guid);
        }
    }
}
