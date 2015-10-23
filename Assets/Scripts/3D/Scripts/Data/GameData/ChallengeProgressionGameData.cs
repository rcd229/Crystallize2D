using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeProgressionGameData {

    public SerializableDictionary<string, ChallengeGameData> Challenges { get; set; }
    public List<string> ChallengeOrder { get; set; }
    public PhraseProgressionGameData PhraseProgression { get; set; }

    public ChallengeProgressionGameData() {
        Challenges = new SerializableDictionary<string, ChallengeGameData>();
        ChallengeOrder = new List<string>();
        PhraseProgression = new PhraseProgressionGameData();
    }

}
