using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlInclude(typeof(LineDialogueElement))]
[XmlInclude(typeof(BranchDialogueElement))]
[XmlInclude(typeof(AnimationDialogueElement))]
[XmlInclude(typeof(MessageDialogueElement))]
public abstract class DialogueElement : ISerializableDictionaryItem<int> {

    public int ID { get; set; }
    [HideEditorProperty]
    public int ActorIndex { get; set; }
    public int DefaultNextID { get; set; }

    public abstract ProcessFactoryRef<DialogueState, DialogueState> Factory { get; }

    public int Key
    {
        get
        {
            return ID;
        }
    }

    public DialogueElement()
    {
        ID = -1;
        DefaultNextID = -1;
        ActorIndex = 0;
    }

}
