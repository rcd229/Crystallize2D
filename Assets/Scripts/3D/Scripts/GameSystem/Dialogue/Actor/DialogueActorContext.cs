using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;
using System.Linq;

public static class DialogueActorContextExtensions {

    public static DialogueActorContext GetOrCreateRandomContext(this DialogueActor actor) {
        return DialogueActorContext.GetOrCreateRandomContext(actor.gameObject);
    }

}

public class DialogueActorContext : MonoBehaviour {

    public static DialogueActorContext GetOrCreateRandomContext(GameObject target) {
        var c = target.GetComponent<DialogueActorContext>();
        if (c) {
            return c;
        } else {
            return target.AddComponent<DialogueActorContext>();
        }
    }

    public ContextData Context { get; set; }

    void Awake() {
        Context = new ContextData();
        var phrase = new PhraseSequence();
        string name = "";
        if (IsMale()) {
            name = RandomNameGenerator.GetRandomCommonMaleName();
            phrase.Translation = "male name";
        } else {
            name = RandomNameGenerator.GetRandomCommonFemaleName();
            phrase.Translation = "female name";
        }
        name = KanaConverter.Instance.ConvertToHiragana(name.ToLower());
        name = name.Replace("'", "");
        phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, name));
        Context.Set("name", phrase);
        Context.Set("playername", new PhraseSequence(PlayerData.Instance.PersonalData.Name));

        foreach (var c in ContextPhraseResources.GetRepeatableContext()) {
            var p = ContextPhraseResources.GetAvailableForContext(c).GetRandomFromEnumerable();
            Context.Set(c, p);

            if (c == ContextPhraseResources.Hobby) {
                Context.Set("doinghobby", HobbyPhraseResources.GetPhraseForHobby(p.Translation));
            }
        }
    }

    bool IsMale() {
        bool isMale = true;
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).name.Contains("Female")) {
                isMale = false;
                break;
            }
        }
        return isMale;
    }

    public void Add(ContextData context) {
        Context = Context.OverrideWith(context);
    }

}
