using UnityEngine;
using System.Collections;
using System;

public class CustomConversation {

	public CustomConversation(ConversationID id, string t, string des){
		cid = id;
		title = t;
		description = des;
	}

	public CustomConversation(string id, string t, string des){
		cid = new ConversationID(id);
		title = t;
		description = des;
	}
	public ConversationID cid{get;private set;}
	public string title{get; private set;}
	public string description{get; private set;}
	//TODO write method to retrieve data from sql and create dialoguesequence
}
