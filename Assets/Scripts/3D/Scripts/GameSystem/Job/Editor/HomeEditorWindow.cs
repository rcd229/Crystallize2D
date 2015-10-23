using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeEditorWindow : GameDataDictionaryEditorWindow<HomeGameData> {

    [MenuItem("Crystallize/Game Data/Homes")]
    static void Open() {
        GetWindow<HomeEditorWindow>();
    }

    protected override DictionaryCollectionGameData<HomeGameData> Dictionary {
        get { return GameData.Instance.Homes; }
    }
}
