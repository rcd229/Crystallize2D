using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AnimationClipImporter : AssetPostprocessor {

    public void OnPostprocessModel(GameObject obj) {
        ModelImporter modelImporter = (ModelImporter)assetImporter;
        if (modelImporter.clipAnimations.Length == 0) {
            modelImporter.clipAnimations = SetupDefaultClips(modelImporter.importedTakeInfos);
        }
    }

    ModelImporterClipAnimation[] SetupDefaultClips(TakeInfo[] importedTakeInfos) {
        ModelImporterClipAnimation[] clips = new ModelImporterClipAnimation[importedTakeInfos.Length];
        for (int i = 0; i < importedTakeInfos.Length; i++) {
            TakeInfo takeInfo = importedTakeInfos[i];
            ModelImporterClipAnimation mica = new ModelImporterClipAnimation();
            mica.name = takeInfo.defaultClipName;
            mica.takeName = takeInfo.name;
            mica.keepOriginalPositionY = true;
            mica.keepOriginalPositionXZ = true;
            mica.keepOriginalOrientation = true;
            mica.lockRootHeightY = true;
            mica.lockRootPositionXZ = true;
            mica.lockRootRotation = true;
            clips[i] = mica;
        }

        return clips;
    }
}
