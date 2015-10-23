using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class DialogueMap {

    static Dictionary<string, DialogueSequence> dialogueDict = new Dictionary<string, DialogueSequence>();
    static Dictionary<DialogueSequence, List<string>> promptDict = new Dictionary<DialogueSequence, List<string>>();
    static List<PromptedItem<DialogueSequence>> map = new List<PromptedItem<DialogueSequence>>();

    public static List<PromptedItem<DialogueSequence>> GetMap() {
        return map;
    }

    public static List<PromptedItem<DialogueSequence>> GetMap(List<PhraseSequence> prompts) {
        return map.GetItemsForPrompts(prompts);
    }

    public static DialogueSequence GetRandomDialogue() {
        return map.GetSafely(UnityEngine.Random.Range(0, map.Count)).Item;
    }

    public static List<PhraseSequence> GetLearnedDialoguePhrases() {
        var phrases = new List<PhraseSequence>();

        foreach (var d in map) {
            foreach (var p in d.Prompts) {
                if (phrases.ContainsEquivalentPhrase(p)) {
                    continue;
                }

                if (!PlayerDataConnector.ContainsLearnedItem(p)) {
                    continue;
                }

                phrases.Add(p);
            }
        }

        return phrases;
    }

    static DialogueMap() {
        AddItem("hello", "hello");

        AddItem("good morning", "good morning");
        AddItem("good morning", "good morning", "the weather is nice");
        AddItem("good morning", "good morning", "this is a nice place");
        AddItem("goodbye", "goodbye");
        AddItem("goodbye", "see you tomorrow");
        AddItem("good evening", "good evening");
        AddItem("good evening", "you are a nice person");

        AddItem("How do you do?", "How do you do?");
        AddItem("Nice to meet you", "The same to you.");
        AddItem("How do you do?", "How do you do?", "I'm [name].");
        AddItem("what is your hobby?", "I read books");
        AddItem("what is your hobby?", "I play sports");
        AddItem("what is your hobby?", "I play games");
        AddItem("what is your hobby?", "I do flower arranging");
        AddItem("How are you?", "I'm fine. Thanks!");
        AddItem("How are you?", "I'm a little tired.");
        AddItem("Are you a student?", "no, I'm not a student");
        AddItem("Are you a student?", "yes, I'm a student");
        AddItem("What's your name?", "I'm [name].");
        AddItem("What's your name?", "I am [name].");

        AddItem("what is this?", "it is a souvenir");
        AddItem("where is the toilet?", "over there");

        WriteMap();
    }

    //static void AddItem(string prompt, string dialogue) {
    //    if (!dialogueDict.ContainsKey(dialogue)) {
    //        dialogueDict[dialogue] = new DialogueSequence("[default]", PhraseSetCollectionGameData.Default.GetPhrase(dialogue));
    //    }
    //    AddItem(prompt, dialogueDict[dialogue]);
    //}

    static void AddItem(string prompt, params string[] dialoguePhrases) {
        var combined = "";
        foreach (var p in dialoguePhrases) {
            combined += p + "\n";
        }
        
        if (!dialogueDict.ContainsKey(combined)) {
            var d = new DialogueSequence();
            //d.Actors.Add(new SceneObjectGameData("[default]"));
            DialogueElement prev = null;
            foreach (var p in dialoguePhrases) {
                GameDataInitializer.AddPhrase("map", p);
                var ele = new LineDialogueElement(PhraseSetCollectionGameData.Default.GetPhrase(p));
                ele.DefaultNextID = -1;
                d.AddNewDialogueElement(ele);
                if (prev != null) {
                    prev.DefaultNextID = ele.ID;
                }
                prev = ele;
            }
            
            dialogueDict[combined] = d;
        }
        AddItem(prompt, dialogueDict[combined]);
    }

    static void AddItem(string prompt, DialogueSequence dialogue) {
        if (!promptDict.ContainsKey(dialogue)) {
            promptDict[dialogue] = new List<string>();
        }
        promptDict[dialogue].Add(prompt);
    }

    static void WriteMap() {
        foreach (var d in promptDict) {
            var prompts = d.Value.Select((p) => PhraseSetCollectionGameData.Default.GetPhrase(p)).ToList();
            map.Add(new PromptedItem<DialogueSequence>(prompts, d.Key));
        }
        dialogueDict = null;
        promptDict = null;
    }

}
