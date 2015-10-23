using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DictionaryDataEditor : AssetModificationProcessor {
		
	static string[] OnWillSaveAssets(string[] assets){
		DictionaryData.SaveInstance ();
		
		return assets;
	}

    static List<DictionaryDataEntry> common = new List<DictionaryDataEntry>();

    [MenuItem("Crystallize/Dictionary/Search common entries")]
    public static void SearchCommonEntries() {
        common = DictionaryData.SearchDictionaryForCommon();
        var newCommon = common.Where(w => DictionaryData.Instance.GetEntryFromID(w.ID) != null);
        Debug.Log("Will add entries: " + newCommon.Count());
    }

    [MenuItem("Crystallize/Dictionary/Add common entries")]
    public static void AddCommonEntries() {
        foreach (var e in common) {
            DictionaryData.Instance.UpdateEntry(e);
        }
        Debug.Log("Dictionary now contains entries: " + DictionaryData.Instance.Entries.Count);
    }

    [MenuItem("Crystallize/Dictionary/Print entries")]
    public static void PrintEntries() {
        var s = "";
        foreach (var e in DictionaryData.Instance.Entries) {
            s += e.ID + "\t" + e.Kanji + "\t" + e.English[0] + "\n";
        }
        Util.Serialization.Serializer.SaveToFile(Application.dataPath + "/temp_dict.txt", s);
    }

}
