using UnityEngine;
using System.Collections;

public class StringMapItem : ISerializableDictionaryItem<string> {
    public string Key { get; set; }
    public string Value { get; set; }

    public StringMapItem(string key, string value) {
        Key = key;
        Value = value;
    }
}