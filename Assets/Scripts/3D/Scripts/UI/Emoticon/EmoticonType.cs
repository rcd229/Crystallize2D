using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EmoticonType : ResourceType<Sprite> {
    static Dictionary<string, EmoticonType> types = new Dictionary<string, EmoticonType>();

    public static EmoticonType Get(string type) {
        if (types.ContainsKey(type)) {
            return types[type];
        }
        return null;
    }

    public static readonly EmoticonType Excited = new EmoticonType("Excited", "CircularSpeechBubble_Excited");
    public static readonly EmoticonType Happy = new EmoticonType("Happy","CircularSpeechBubble_Happy");
    public static readonly EmoticonType Annoyed = new EmoticonType("Annoyed","CircularSpeechBubble_Annoyed");
    public static readonly EmoticonType Sad = new EmoticonType("Sad","CircularSpeechBubble_Sad");
    public static readonly EmoticonType Angry = new EmoticonType("Angry", "CircularSpeechBubble_Angry");

    public static EmoticonType Get(int typeID) {
        if (typeID == 0) return Excited;
        if (typeID == 1) return Happy;
        if (typeID == 2) return Annoyed;
        if (typeID == 3) return Sad;
        if (typeID == 4) return Angry;
        return Annoyed;
    }

    public Sprite Image {
        get {
            return Resources.Load<Sprite>(ResourcePath);
        }
    }

    protected override string ResourceDirectory {
        get {
            return "Images/Emoticon/";
        }
    }

    public readonly string TypeKey;

    EmoticonType(string type, string name)
        : base(name) {
            TypeKey = type;
            types[type] = this;
    }
}
