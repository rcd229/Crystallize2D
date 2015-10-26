using UnityEngine;
using UnityEditor;
using System.Collections;

public class PhraseSetSaver : UnityEditor.AssetModificationProcessor {

    public static string[] OnWillSaveAssets(string[] paths) {
        PhraseSetCollectionGameData.SaveAll();
        return paths;
    }

}
