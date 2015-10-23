using UnityEngine;
using System.Collections;
using Util.Serialization;

[System.Serializable]
public class DialogueSequenceUnityXml {

    [SerializeField]
    public string xmlString = "";

    public DialogueSequence GetObject()
    {
        return Serializer.LoadFromXmlString<DialogueSequence>(xmlString);
    }

}
