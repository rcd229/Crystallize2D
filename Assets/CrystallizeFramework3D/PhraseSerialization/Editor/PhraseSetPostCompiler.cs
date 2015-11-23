using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseSetPostCompiler : EditorPostCompiler {

    const string DefaultPhraseSet = "Default";

    bool changed = false;
    Dictionary<string, PhraseSequence> keySequences = new Dictionary<string, PhraseSequence>(new PostCompilerStringComparer());

    public override void AfterCompile() {
        CollectPhrases();
        //DialogueMap.GetMap();
        UpdateKeySequences();

        foreach (var key in GetAllCompiledPhraseSets()) {
            UpdatePhraseSet(key);
        }

        if (changed) {
            PhraseSetCollectionGameData.SaveAll();
        }

        List<PhraseSequence> words;
        List<PhraseSequence> phrases;
        PhraseWordMap.GetAllWordsAndPhrases(out words, out phrases);
        
        Debug.Log(
            string.Format("Project update! Unique learnable phrases: [{0}] Unique learnable words: [{1}]", 
            phrases.Count, words.Count));
    }

    void CollectPhrases() {
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
            var containsPhrases = from t in a.GetTypes()
                                  where typeof(IContainsStaticPhrases).IsAssignableFrom(t)
                                  select t;
            foreach(var cp in containsPhrases) {
                //Debug.Log("trying: " + cp);
                if (cp.GetConstructor(Type.EmptyTypes) != null) {
                    var cpInst = (IContainsStaticPhrases)Activator.CreateInstance(cp);
                    cpInst.Initialize();
                } else {
                    //Debug.LogWarning(cp + " does not have")
                }
            }
        }
    }

    void UpdateKeySequences() {
        PhraseSetCollectionGameData.LoadAll();
        var sets = PhraseSetCollectionGameData.GetPhraseSets();

        foreach (var ps in PhrasePipeline.PhraseSets) {
            foreach (var p in ps.Value) {
                PhraseSetCollectionGameData.Default.GetOrCreatePhrase(p);
            }
        }

        foreach (var p in PhraseSetCollectionGameData.Default.Phrases) {
            UpdateKeySequence(p.Translation, p);
        }

        foreach (var set in sets) {
            if (!ContainsCompiledPhraseSet(set.Name)) {
                continue;
            }
			if(set.Name == DefaultPhraseSet){
				continue;
			}

            var keys = GetCompiledPhraseSetKeys(set.Name);
            for (int i = 0; i < set.Phrases.Count; i++) {
                
                
                if (i >= keys.Count) {
                    break;
                }

                bool isSameAsDefault = PhraseSequence.PhrasesEquivalent(
                    set.Phrases[i], PhraseSetCollectionGameData.Default.GetPhrase(keys[i]));
                if (!isSameAsDefault) {
                    if (set.Phrases[i] == null) {
                        Debug.Log(set.Name + " has null phrase.");
                    } else if (PhraseSetCollectionGameData.Default.GetPhrase(keys[i]) == null) {
                        Debug.Log("Default has null phrase: " + keys[i]);
                    } else {
                        Debug.Log(set.Name + " has non-default keys: " + set.Phrases[i].GetText() + "; " +
                              PhraseSetCollectionGameData.Default.GetPhrase(keys[i]).GetText() + ";" + set.Phrases[i].Translation);
                    }
                }

                if (!keySequences.ContainsKey(keys[i])) {
                    UpdateKeySequence(keys[i], set.Phrases[i]);
                } 
            }
        }
    }

    void UpdateKeySequence(string key, PhraseSequence phrase) {
        if (!phrase.IsEmpty) {
            keySequences[key] = phrase;
        }
    }

    void UpdatePhraseSet(string setKey) {
        if (!PhraseSetCollectionGameData.HasItem(setKey)) {
            changed = true;
        }

        var phraseSet = PhraseSetCollectionGameData.GetOrCreateItem(setKey);
        //Debug.Log(setKey);
        //Debug.Log(phraseSet.Name);
        var phraseKeys = GetCompiledPhraseSetKeys(phraseSet.Name);
        var localMap = new Dictionary<string, PhraseSequence>(new PostCompilerStringComparer());
        if (phraseKeys.Count != phraseSet.Phrases.Count) {
            foreach (var p in phraseSet.Phrases) {
                if (p.Translation != null) {
                    localMap[p.Translation] = p;
                }
            }
            changed = true;
        }

        //for (var i = 0; i < phraseKeys.Count; i++) {
        //    if (localMap.ContainsKey(phraseKeys[i])) {
        //        phraseSet.SetPhrase(i, localMap[phraseKeys[i]]);
        //    } 
        //    var p = phraseSet.GetOrCreatePhrase(i);

        //    if (p.IsEmpty && keySequences.ContainsKey(phraseKeys[i])) {
        //        p.PhraseElements = new List<PhraseSequenceElement>(keySequences[phraseKeys[i]].PhraseElements);
        //        changed = true;
        //    }

        //    if (p.Translation == null || p.Translation == "") {
        //        p.Translation = phraseKeys[i];
        //        changed = true;
        //    }
        //}
    }

    IEnumerable<string> GetAllCompiledPhraseSets() {
        return PhrasePipeline.PhraseSets.Keys;
    }

    bool ContainsCompiledPhraseSet(string setKey) {
        return PhrasePipeline.PhraseSets.ContainsKey(setKey);
    }

    List<string> GetCompiledPhraseSetKeys(string setKey) {
        return PhrasePipeline.PhraseSets[setKey];
    }

}
