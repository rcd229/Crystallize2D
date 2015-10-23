using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceLearnEventHandler : MonoBehaviour {

    //static ResourceLearnEventHandler _instance;
    //public static void GetInstance() {
    //    _instance = new GameObject("CollectEventHandler").AddComponent<ResourceLearnEventHandler>();
    //}

    //public static int GetWords(){
    //    if (_instance) {
    //        return _instance.CollectedWordCount;
    //    }
    //    return 0;
    //}

    //public static int GetPhrases(){
    //    if (_instance) {
    //        return _instance.CollectedPhraseCount;
    //    }
    //    return 0;
    //}

    

    

    //void PlayerState_OnCollectPhraseRequested(object sender, PhraseEventArgs e) {
    //    if (CollectedPhraseCount < PlayerData.Instance.Proficiency.Phrases) {
    //        if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(e.Phrase)) {
    //            PlayerDataConnector.CollectPhrase(e.Phrase);
    //            //collectedPhrases.Add(e.Phrase);
    //        }
    //    } else {
    //        UILibrary.MessageBox.Get("You can't collect more phrases today. You can collect more phrases in a session by reviewing phrases to level up.");
    //    }
    //}

    //void PlayerState_OnCollectWordRequested(object sender, PhraseEventArgs e) {
    //    if (CollectedWordCount < PlayerData.Instance.Proficiency.Words) {
    //        if (!CanLearn(e.Word)) {
    //            return;
    //        }

    //        if (!PlayerData.Instance.WordStorage.ContainsFoundWord(e.Word)) {
    //            PlayerDataConnector.CollectWord(e.Word);
    //        }
    //    } else {
    //        UILibrary.MessageBox.Get("You can't collect more words today. You can collect more words in a session by reviewing words to level up.");
    //    }
    //}

    //void OnDestroy() {
    //    CrystallizeEventManager.PlayerState.OnCollectWordRequested -= PlayerState_OnCollectWordRequested;
    //    CrystallizeEventManager.PlayerState.OnCollectPhraseRequested -= PlayerState_OnCollectPhraseRequested;
    //}

    //bool CanLearn(PhraseSequenceElement word) {
    //    return word.ElementType == PhraseSequenceElementType.FixedWord;
    //}

}