using UnityEngine;
using System.Collections;
using Util.Serialization;

[System.Serializable]
public class NetworkSpeechBubbleRequestedEventArgs : System.EventArgs {

    public int PlayerID { get; set; }
    public string PhraseXml { get; set; }

    public NetworkSpeechBubbleRequestedEventArgs(int playerID, string phraseXml) {
        this.PlayerID = playerID;
        this.PhraseXml = phraseXml;
    }

    public PhraseSequence GetPhraseSequence() {
        return Serializer.LoadFromXmlString<PhraseSequence>(PhraseXml);
    }

}

[System.Serializable]
public class NetworkEmoteArgs : System.EventArgs {
    public int PlayerID { get; set; }
    public int EmoteType { get; set; }
    public NetworkEmoteArgs(int playerID, int emoteType) {
        this.PlayerID = playerID;
        this.EmoteType = emoteType;
    }
}
