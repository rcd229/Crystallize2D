using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util.Serialization;
using System;

/**
 * Abstraction: A database for conversations
 * Schema: 
 * 1. unique identifier. int
 * 2. short title. varchar/ string
 * 3. long description: text/ string
 * 4. dialogue sequence : text/ string (prefer json string)
 * 
 * Implementation:
 * Current: Xml serialized as a list of models which satisfy the schema. PhraseSequence in each model also xml serialized.
 * 			Unique identifier is now Guid
 * Prefer: Relational database table, with dialogue sequence json serialized
 */
public class ConversationTable  {
	static ConversationTable _instance;
	public static ConversationTable Instance{
		get{
			if(_instance == null){
				Load ();
			}
			return _instance;
		}
	}

	const string FileName = "CrystallizeConversationTable";
	const string FileExtension = ".txt";
	const string EditorFilePath = "/crystallize/Resources/";

	static string GetPath(){
		return Application.dataPath + EditorFilePath + FileName + FileExtension;
	}

//	public List<ConversationCreationLog> Table{get; private set;}
	public GuidSerializableDictionary<ConversationID, ConversationLogCollection> Table{get; private set;}

	//unnecessary if relational table is used.
	public static void Save(){
		Serializer.SaveToXml<ConversationTable>(GetPath(), _instance);
	}
	public static void Load(){
		_instance = Serializer.LoadFromXml<ConversationTable>(GetPath(), false);
		if (_instance == null) {
			Debug.Log("empty xml");
			_instance = new ConversationTable();
			_instance.Table = new GuidSerializableDictionary<ConversationID, ConversationLogCollection>();
		}
	}
}

public class ConversationID : UniqueID{
	public ConversationID():base(){}
	public ConversationID(Guid id) : base(id){}
	public ConversationID(string id) : base(id){}
	public override string ToString ()
	{
		return guid.ToString();
	}
}

public class ConversationModel{
	public ConversationID cid{get;set;}
	public string title{get;set;}
	public string description{get;set;}
	public SequentialConveration dialogue{get;set;}
	public ConversationModel(){
		cid = new ConversationID();
		dialogue = new SequentialConveration();
	}
	public ConversationModel(ConversationID id, string title, string descr, SequentialConveration dialogue){
		cid = id;
		this.title = title;
		this.description = descr;
		this.dialogue = dialogue;
	}

}

public class ConversationCreationLog {


	public ConversationModel Data{get;set;}
	public string TimeStamp{get;set;}
	public virtual string User{get;set;}
	public LogStatus status{get;set;}

	public ConversationCreationLog Copy(){
		ConversationCreationLog copy = new ConversationCreationLog();
		copy.Data = new ConversationModel(Data.cid, Data.title, Data.description, Data.dialogue);
		copy.TimeStamp = TimeStamp;
		copy.status = status;
		copy.User = User;
		return copy;
	}
	public ConversationCreationArg CreateArg(UserInfo user){
		ConversationCreationArg copy = new ConversationCreationArg();
		copy.Data = new ConversationModel(Data.cid, Data.title, Data.description, Data.dialogue);
		copy.TimeStamp = TimeStamp;
		copy.status = status;
		copy.UserInfo = user;
		return copy;
	}

}

public class ConversationCreationArg: ConversationCreationLog{
	public UserInfo UserInfo{get;set;}
	public override string User {
		get {
			return UserInfo.UserName;
		}

	}
	public ConversationCreationArg ArgCopy(){
		ConversationCreationArg copy = new ConversationCreationArg();
		copy.Data = new ConversationModel(Data.cid, Data.title, Data.description, Data.dialogue);
		copy.TimeStamp = TimeStamp;
		copy.status = status;
		copy.UserInfo = UserInfo;
		return copy;
	}
	
}

public class ConversationLogCollection: ISerializableDictionaryItem<ConversationID>, ISetableKey<ConversationID>{

	#region ISetableKey implementation
	
	public void SetKey (ConversationID key)
	{
		ID = key;
	}
	
	#endregion
	
	#region ISerializableDictionaryItem implementation
	
	public ConversationID Key {
		get {
			return ID;
		}
	}
	
	#endregion
	public List<ConversationCreationLog> Log {get;set;}
	public ConversationID ID{get; set;}
	public ConversationLogCollection(){
		ID = new ConversationID();
		Log = new List<ConversationCreationLog>();
	}
}

public enum LogStatus{Creation, Edit}
