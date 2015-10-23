using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[XmlInclude(typeof(NPCActorLine))]
[XmlInclude(typeof(PlayerActorLine))]
public class DialogueActorLine {

    public PhraseSequence Phrase { get; set; }
    public int AudioClipID { get; set; }

    public DialogueActorLine() {
        Phrase = new PhraseSequence();
        AudioClipID = -1;
    }

    public DialogueActorLine(PhraseSequence phrase) : this() {
        Phrase = phrase;
    }

    public AudioClip GetAudioClip() {
        if (!ScriptableObjectDictionaries.main) {
            return null;
        }
        return ScriptableObjectDictionaries.main.audioResources.GetAudioResource(AudioClipID);
    }

}
