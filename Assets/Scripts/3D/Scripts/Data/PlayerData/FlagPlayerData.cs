using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlagPlayerData {

    public const string DictionaryUnlocked = "DictionaryUnlocked";
    public const string ClickWordSlotMessage = "ClickWordSlotMessage";
    public const string IsMultiplayer = "IsMultiplayer";
    public const string PlayersSynced = "PlayersSynced";

    public HashSet<string> Flags = new HashSet<string>();
    public SerializableDictionary<string, FlagSetPlayerData> FlagSets = new SerializableDictionary<string, FlagSetPlayerData>();

    public void SetFlag(string flag, bool value) {
        if (value) {
            if (!Flags.Contains(flag)) {
                Flags.Add(flag);
            }
        } else {
            if (Flags.Contains(flag)) {
                Flags.Remove(flag);
            }
        }
    }

    public bool GetFlag(string flag) {
        return Flags.Contains(flag);
    }

    public FlagSetPlayerData GetOrCreateFlagSet(string key){
        if (!FlagSets.ContainsKey(key)) {
            var item = new FlagSetPlayerData();
            item.Name = key;
            FlagSets.Add(item);
        }
        return FlagSets.Get(key);
    }

}
