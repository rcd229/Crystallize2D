using UnityEngine;
using System.Collections;

public class ProcessCallbackAttribute : System.Attribute {

    public string Key { get; set; }

    public ProcessCallbackAttribute(object obj) {
        if (obj != null) {
            Key = obj.ToString();
        } else {
            Key = "NULL";
        }
    }

}