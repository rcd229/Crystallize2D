using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public static class PromptedItemExtensions {
    public static List<T> GetItemPrompt<T>(this IEnumerable<PromptedItem<T>> items, PhraseSequence prompt) {
        var list = new List<T>();
        foreach (var i in items) {
            if (i.ContainsPrompt(prompt)) {
                list.Add(i.Item);
            }
        }
        return list;
    }

    public static List<PromptedItem<T>> GetItemsForPrompts<T>(this IEnumerable<PromptedItem<T>> items, IEnumerable<PhraseSequence> prompts) {
        var list = new List<PromptedItem<T>>();
        foreach (var i in items) {
            foreach (var p in prompts) {
                if (i.ContainsPrompt(p)) {
                    list.Add(i);
                    break;
                }
            }
        }
        return list;
    }
}

public class PromptedItem<T> {

    public List<PhraseSequence> Prompts {get; set;}
    public T Item { get; set; }

    public PromptedItem(List<PhraseSequence> prompts, T item) {
        Prompts = prompts;
        Item = item;
    }

    public bool ContainsPrompt(PhraseSequence prompt) {
        foreach (var p in Prompts) {
            if (PhraseSequence.PhrasesEquivalent(p, prompt)) {
                return true;
            }
        }
        return false;
    }

}
