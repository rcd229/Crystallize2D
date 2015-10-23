using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeGameData : ISerializableDictionaryItem<string> {

	public string Key {
		get {
			return ChallengeID;
        }
	}

	public string ChallengeID { 
		get {
			var givenString = "";
			foreach(var s in MissingWordIDs){
				givenString += "_" + s;
			}
			return string.Format("{0}{1}", ConversationID, givenString);
		}
	}

	public int ConversationID { get; set; }
	public List<string> MissingWordIDs { get; set; }

	public ChallengeGameData(){
		ConversationID = -1;
		MissingWordIDs = new List<string> ();
	}

	public ChallengeGameData(int conversationID, List<string> missingWordIDs){
		this.ConversationID = conversationID;
		this.MissingWordIDs = missingWordIDs;
	}

    //public string GetChallengeName(){
    //    return ScriptableObjectDictionaries.main.conversationDictionary.GetConversationForID(ConversationID).dialogPhrases[1].Translation;
    //}

}
