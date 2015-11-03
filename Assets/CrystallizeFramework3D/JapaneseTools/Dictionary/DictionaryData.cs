using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Util.Serialization;
using Newtonsoft.Json;
using JapaneseTools;

public class DictionaryData {

    public const int AdditionalEntryStartID = 10000000;

    const string DictionaryFilePath = "/crystallize/Resources/JMDict_e/JMDict_e";
    const string EditorFilePath = "/crystallize/Resources/";
    const string FileName = "Dictionary";
    const string PlayerFilePath = "/PlayerGameData/";
    const string FileExtension = ".txt";

    static DictionaryData _instance;
    static XDocument _baseDictionaryXml;

    public static DictionaryData Instance {
        get {
            if (_instance == null) {
                LoadInstance();
            }
            return _instance;
        }
    }

    public static XDocument BaseDictionaryXml {
        get {
            if (!Application.isEditor) {
                throw new IOException("Must be in editor to access JMDict!");
            }

            if (_baseDictionaryXml == null) {
                string fileName = Application.dataPath + DictionaryFilePath;
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ProhibitDtd = false;

                using (XmlReader reader = XmlReader.Create(fileName, settings)) {
                    _baseDictionaryXml = XDocument.Load(reader);
                }
            }
            return _baseDictionaryXml;
        }
    }

    static string GetEditorDataPath() {
        return Application.dataPath + EditorFilePath + FileName + FileExtension;
    }

    static string GetPlayerDataPath() {
        return Application.dataPath + PlayerFilePath + FileName + FileExtension;
    }

    public static void LoadInstance() {
        if (Application.isEditor) {
            //Debug.Log("Loading Dictionary. Is editor.");
            _instance = Serializer.LoadFromXml<DictionaryData>(GetEditorDataPath(), false);
            if (_instance == null) {
                _instance = new DictionaryData();
            }
        } else {
            Debug.Log("Loading Dictionary. Is player.");
            if (File.Exists(GetPlayerDataPath())) {
                Debug.LogWarning("(not implemented)");
            } else {
                var text = Resources.Load<TextAsset>(FileName);
                if (text != null) {
                    _instance = Serializer.LoadFromXmlString<DictionaryData>(text.text);
                } else {
                    _instance = new DictionaryData();
                }
            }
        }
		_instance.PopulateIdMapping();
        _instance.LoadAuxiliaryData();
    }

	void PopulateIdMapping(){
		IdToEntryMapping = new Dictionary<int, int>();
		for (int i = 0; i < Entries.Count; i++){
			if(IdToEntryMapping.ContainsKey(Entries[i].ID)){
				IdToEntryMapping[Entries[i].ID] = i;
			}else{
				IdToEntryMapping.Add(Entries[i].ID, i);
			}
		}
	}

    public static void SaveInstance() {
        if (_instance == null) {
            Debug.Log("No changes to dictionary.");
            return;
        } else {
            if (Application.isEditor) {
                Serializer.SaveToXml<DictionaryData>(GetEditorDataPath(), _instance);
            } else {
                Debug.LogWarning("Is player. (not implemented)");
            }
        }

    }

    static List<DictionaryDataEntry> LoadEntriesFromKana(string kana) {
        var foundElements = new List<XElement>();
        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            foreach (var re in e.Elements("r_ele")) {
                if (re.Element("reb") != null) {
                    if (re.Element("reb").Value == kana) {
                        foundElements.Add(e);
                        break;
                    }
                }
            }
        }

        var list = new List<DictionaryDataEntry>();
        if (foundElements.Count == 0) {
            Debug.Log("Entry for " + kana + " not found.");
            return list;
        } else if (foundElements.Count > 1) {
            Debug.Log("Multiple matches found for " + kana + "; Displaying results.");
            foreach (var e in foundElements) {
                list.Add(GetDictionaryEntryFromXmlEntry(e));
            }
            return list;
        }
        list.Add(GetDictionaryEntryFromXmlEntry(foundElements[0]));
        return list;
    }

    public static List<DictionaryDataEntry> SearchDictionary(string searchString) {
        var kanaString = KanaConverter.Instance.ConvertToHiragana(searchString);
        var foundElements = new List<XElement>();
        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            foreach (var re in e.Elements("r_ele")) {
                if (re.Element("reb") != null) {
                    var kana = re.Element("reb").Value;
                    if (kana.Contains(kanaString)) {
                        foundElements.Add(e);
                        break;
                    } else if (KanaConverter.Instance.ConvertToRomaji(kana).Contains(searchString)) {
                        foundElements.Add(e);
                        break;
                    }
                }
            }
        }

        return GetDictionaryEntryFromXmlEntries(foundElements);
    }

    public static List<DictionaryDataEntry> SearchDictionaryForCommon() {
        var foundElements = new List<XElement>();
        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            if (IsCommon(e)) {
                foundElements.Add(e);
            }
        }

        return GetDictionaryEntryFromXmlEntries(foundElements);
    }

    static bool IsCommon(XElement element) {
        foreach (var k_ele in element.Elements("k_ele")) {
            foreach (var ke_pri in k_ele.Elements("ke_pri")) {
                if (ke_pri.Value == "ichi1") {
                    return true;
                }
            }
        }

        foreach (var r_ele in element.Elements("r_ele")) {
            foreach (var re_pri in r_ele.Elements("re_pri")) {
                if (re_pri.Value == "ichi1") {
                    return true;
                }
            }
        }

        return false;
    }

    public static List<DictionaryDataEntry> SearchDictionaryForID(int id) {
        var foundElements = new List<XElement>();
        var idString = id.ToString();
        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            if (e.Element("ent_seq").Value == idString) {
                foundElements.Add(e);
                break;
            }
        }

        return GetDictionaryEntryFromXmlEntries(foundElements);
    }

    public static List<DictionaryDataEntry> SearchDictionaryWithStartingRomaji(string searchString) {
        var foundElements = new List<XElement>();
        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            foreach (var re in e.Elements("r_ele")) {
                if (re.Element("reb") != null) {
                    var kanaString = KanaConverter.Instance.ConvertToRomaji(re.Element("reb").Value);
                    if (kanaString.StartsWith(searchString)) {
                        foundElements.Add(e);
                        break;
                    }
                }
            }
        }

        return GetDictionaryEntryFromXmlEntries(foundElements);
    }

    public static List<DictionaryDataEntry> SearchDictionaryExact(IEnumerable<string> searchStrings, List<DictionaryDataEntry> ambiguousEntries) {
        var localEntries = new HashSet<string>();
        foreach (var sea in searchStrings) {
            if (Instance.GetLocalEntryFromKanji(sea) != null) {
                localEntries.Add(sea);
            } else if (Instance.GetLocalEntryFromKana(sea) != null) {
                localEntries.Add(sea);
            }
        }
        Debug.Log(localEntries.Count + " entries have already been added.");

        var searchHash = new HashSet<string>(searchStrings);
        foreach (var l in localEntries) {
            searchHash.Remove(l);
        }
        var foundElements = new Dictionary<string, List<XElement>>();

        foreach (var e in BaseDictionaryXml.Root.Elements("entry")) {
            foreach (var re in e.Elements("k_ele")) {
                if (re.Element("keb") != null) {
                    var kanjiString = re.Element("keb").Value;
                    if (searchHash.Contains(kanjiString)) {
                        if (!foundElements.ContainsKey(kanjiString)) {
                            foundElements[kanjiString] = new List<XElement>();
                        }
                        foundElements[kanjiString].Add(e);
                    }
                }
            }

            foreach (var re in e.Elements("r_ele")) {
                if (re.Element("reb") != null) {
                    var kanaString = re.Element("reb").Value;
                    if (searchHash.Contains(kanaString)) {
                        if (!foundElements.ContainsKey(kanaString)) {
                            foundElements[kanaString] = new List<XElement>();
                        }
                        foundElements[kanaString].Add(e);
                        break;
                    }
                }
            }
        }

        foreach (var k in foundElements.Keys) {
            if (foundElements[k].Count > 1) {
                var toRemove = new Queue<XElement>();
                foreach (var e in foundElements[k]) {
                    bool include = false;
                    foreach (var r in e.Element("r_ele").Elements("re_pri")) {
                        if (r.Value.Contains("1")) {
                            include = true;
                        }
                    }

                    if (!include) {
                        toRemove.Enqueue(e);
                    }
                }

                if (foundElements[k].Count - toRemove.Count > 0) {
                    while (toRemove.Count > 0) {
                        foundElements[k].Remove(toRemove.Dequeue());
                    }
                }
            }
        }

        var list = new List<DictionaryDataEntry>();
        Debug.Log("Found " + foundElements.Count + " entries (missing " + (searchHash.Count - foundElements.Count) + ")");

        var s = "";
        foreach (var k in searchHash) {
            if (!foundElements.ContainsKey(k)) {
                s += k + " not found in dictionary.\n";
            }
        }
        Debug.Log(s);

        var highFreqEntryString = "";
        var highFreqCount = 0;
        foreach (var k in foundElements.Keys) {
            var l = foundElements[k];
            if (l.Count == 0) {
                highFreqEntryString += k + " contains no high freq entries.\n";
                highFreqCount++;
            } else if (l.Count == 1) {
                list.Add(GetDictionaryEntryFromXmlEntry(l[0]));
            } else {
                foreach (var e in l) {
                    ambiguousEntries.Add(GetDictionaryEntryFromXmlEntry(e));
                }
            }
        }
        Debug.Log(highFreqCount + " entries without high freq.\n" + highFreqEntryString);
        return list;
    }

    public static void UpdateAllDictionaryEntriesFromSource() {
        var ids = new List<int>();
        foreach (var e in Instance.Entries) {
            ids.Add(e.ID);
        }

        foreach (var id in ids) {
            var entry = SearchDictionaryForID(id)[0];
            Instance.UpdateEntry(entry);
        }

        var orderPos = _pos.OrderBy((s) => s);
        var orderStr = "";
        foreach (var pos in orderPos) {
            orderStr += pos + "\n";
        }
        Debug.Log(orderStr);

        var posString = "";
        var dict = new Dictionary<PartOfSpeech, List<string>>();
        foreach (var entry in Instance.Entries) {
            if (!dict.ContainsKey(entry.PartOfSpeech)) {
                dict[entry.PartOfSpeech] = new List<string>();
            }
            dict[entry.PartOfSpeech].Add(entry.Kanji);
        }

        foreach (var k in dict.Keys) {
            posString += k.ToString() + "\n";
            foreach (var e in dict[k]) {
                posString += e + "\n";
            }
        }
        Debug.Log(posString);
    }

    static HashSet<string> _pos = new HashSet<string>();
    static DictionaryDataEntry GetDictionaryEntryFromXmlEntry(XElement element) {
        var id = int.Parse(element.Element("ent_seq").Value);
        var kana = element.Element("r_ele").Element("reb").Value;
        var kanji = kana;
        if (element.Element("k_ele") != null) {
            kanji = element.Element("k_ele").Element("keb").Value;
        }
        var english = new List<string>();
        var pos = PartOfSpeech.Unclassified;
        var sense = element.Element("sense");
        if (sense.Element("pos") != null) {
            if (!_pos.Contains(sense.Element("pos").Value)) {
                _pos.Add(sense.Element("pos").Value);
            }
            pos = GetPartOfSpeechFromXml(sense.Element("pos").Value);
        }
        foreach (var en in sense.Elements("gloss")) {
            english.Add(en.Value);
        }
        return new DictionaryDataEntry(id, kanji, kana, english, pos);
    }

    static PartOfSpeech GetPartOfSpeechFromXml(string xml) {
        xml = xml.ToLower();

        if (xml.Contains("adject")) return PartOfSpeech.Adjective;
        if (xml.Contains("adverb")) return PartOfSpeech.Adverb;
        if (xml.Contains("conjunction")) return PartOfSpeech.Conjunction;
        if (xml.Contains("copula")) return PartOfSpeech.Copula;
        if (xml.Contains("counter")) return PartOfSpeech.Counter;
        if (xml.Contains("expression")) return PartOfSpeech.Expression;
        if (xml.Contains("godan")) return PartOfSpeech.GodanVerb;
        if (xml.Contains("ichidan")) return PartOfSpeech.IchidanVerb;
        if (xml.Contains("interjection")) return PartOfSpeech.Interjection;
        if (xml.Contains("noun")) return PartOfSpeech.Noun;
        if (xml.Contains("numeric")) return PartOfSpeech.Numeric;
        if (xml.Contains("particle")) return PartOfSpeech.Particle;
        if (xml.Contains("prefix")) return PartOfSpeech.Prefix;
        if (xml.Contains("suffix")) return PartOfSpeech.Suffix;
        if (xml.Contains("pronoun")) return PartOfSpeech.Pronoun;
        if (xml.Contains("suru")) return PartOfSpeech.SuruVerb;
        if (xml.Contains("kuru")) return PartOfSpeech.SuruVerb;

        return PartOfSpeech.Unclassified;
    }

    static List<DictionaryDataEntry> GetDictionaryEntryFromXmlEntries(IEnumerable<XElement> elements) {
        var list = new List<DictionaryDataEntry>();
        foreach (var e in elements) {
            list.Add(GetDictionaryEntryFromXmlEntry(e));
        }
        return list;
    }

	Dictionary<int, int> IdToEntryMapping;
    public List<DictionaryDataEntry> Entries { get; set; }
    public List<AuxiliaryDictionaryDataEntry> AuxiliaryData { get; set; }

    public DictionaryData() {
        Entries = new List<DictionaryDataEntry>();
        AuxiliaryData = new List<AuxiliaryDictionaryDataEntry>();
		IdToEntryMapping = new Dictionary<int, int>();
    }

    void LoadAuxiliaryData() {
        foreach (var aux in AuxiliaryData) {
            var ent = GetEntryFromID(aux.WordID);
            if (ent == null) {
                throw new UnityException("Must have entry to add aux data.");
            } else {
                ent.SetAuxiliaryData(aux);
            }
        }
    }

    DictionaryDataEntry GetLocalEntryFromID(int id) {
		return Entries[IdToEntryMapping[id]];
    }

    public DictionaryDataEntry GetLocalEntryFromKana(string kana) {
        return (from e in Entries where e.Kana == kana select e).FirstOrDefault();
    }

    DictionaryDataEntry GetLocalEntryFromKanji(string kanji) {
        return (from e in Entries where e.Kanji == kanji select e).FirstOrDefault();
    }

    public DictionaryDataEntry GetEntryFromID(int id) {
        return GetLocalEntryFromID(id);
    }

    public DictionaryDataEntry AddNewEntry() {
        var id = AdditionalEntryStartID;
        for (int i = AdditionalEntryStartID; i < AdditionalEntryStartID + 10000; i++) {
            if (GetEntryFromID(i) != null) {
                id = i;
            }
        }
        id += 10;

        var entry = new DictionaryDataEntry(id, "temp", "temp", new List<string>(), PartOfSpeech.Noun);
        Entries.Add(entry);
		IdToEntryMapping.Add(entry.ID, Entries.Count - 1);
        return entry;
    }

    public DictionaryDataEntry GetEntryFromKana(string kana) {
        var entry = GetLocalEntryFromKana(kana);
        if (entry == null) {
            var list = LoadEntriesFromKana(kana);
            if (list.Count == 1) {
                UpdateEntry(entry);
                entry = list[0];
            }
        }

        return entry;
    }

    public List<DictionaryDataEntry> GetEntriesFromKana(string kana) {
        var list = new List<DictionaryDataEntry>();
        var entry = GetLocalEntryFromKana(kana);
        if (entry == null) {
            list = LoadEntriesFromKana(kana);
            if (list.Count == 1) {
                UpdateEntry(list[0]);
            }
        } else {
            list.Add(entry);
        }

        return list;
    }

    public List<DictionaryDataEntry> FilterEntriesFromRomaji(string romaji) {
        return (from e in Entries
                where KanaConverter.Instance.ConvertToRomaji(e.Kana).StartsWith(romaji)
                orderby e.Kana.Length
                select e).ToList();
    }

    public void UpdateEntry(DictionaryDataEntry entry) {
        var prev = GetLocalEntryFromID(entry.ID);
        if (prev != null) {
            Entries.Remove(prev);
        }
        Entries.Add(entry);
		if(IdToEntryMapping.ContainsKey(entry.ID)){
			IdToEntryMapping[entry.ID] = Entries.Count - 1;
		}else{
			IdToEntryMapping.Add(entry.ID, Entries.Count - 1);
		}
    }

    AuxiliaryDictionaryDataEntry GetAuxiliaryFromID(int id) {
        return (from e in AuxiliaryData where e.WordID == id select e).FirstOrDefault();
    }

    public void RemoveAuxiliaryData(int id) {
        var aux = GetAuxiliaryFromID(id);
        if (aux != null) {
            AuxiliaryData.Remove(aux);
        }

        var ent = GetEntryFromID(id);
        if (ent != null) {
            ent.SetAuxiliaryData(null);
        }
    }

    public void UpdateAuxiliaryData(AuxiliaryDictionaryDataEntry auxData) {
        RemoveAuxiliaryData(auxData.WordID);
        var ent = GetEntryFromID(auxData.WordID);
        if (ent == null) {
            throw new UnityException("Must have entry to add aux data.");
        } else {
            AuxiliaryData.Add(auxData);
            ent.SetAuxiliaryData(auxData);
        }
    }


}
